using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /*
     * 在复印机项目的设备功能中，设备列表页的条件勾选了某些条件，表示仅查看同时满足这些条件的设备。
     * 当设备状态变化后，应判断此设备的状态是否符合当前条件，若全部符合，且设备目前没在当前页中，则应该刷新当前页面，
     * 类似的若设备原本就在当前页面，但更新后它不应该出现在当前页面中，此时也应该刷新当前页面。
     * 由于一页中有多个设备，可能段时间内会出现多次刷新请求，比较浪费。
     * 
     * 用户主动操作的刷新保持原来的方式，立即刷新
     * 仅在其它用户对设备操作，或设备中心的推送导致设备状态变化时，引起的刷新，需要实现延迟刷新。
     * 
     * 延迟刷新的实现方式：
     * 设备状态变化时，比对决定是否需要刷新列表，若需要，更新标记字段，并延迟调用刷新方法，注意用值传递延迟刷新标记。
     * 当延迟到期时，比对标记是否变化，若变化了，则说明有其它刷新请求，放弃本次刷新。
     * 但这样有个问题，若一直有刷新请求，导致可能很长时间才被真正执行刷新，所以这种延迟刷新得有个超时限制。
     * 
     * 另外，这种延迟刷新是个通用需求，可以抽象出来，放到BXJG.Common中，把刷新逻辑看成委托。
     * 
     * 只要耗费资源的处理才需要做这种处理，所以它一定是异步
     */

    /// <summary>
    /// 延迟处理，覆盖执行，后执行的生效，但不完全保证。
    /// 通常用于延迟重新加载数据的场景
    /// </summary>
    public class YanchiChuli : IDisposable
    {
        /// <summary>
        /// 需要延迟处理的任务
        /// 耗费资源的处理才需要做这种处理，所以它一定是异步
        /// object：请求执行时（调用request时）可以指定一个参数传递进去
        /// CancellationToken：有请求覆盖时，之前的任务可以通过这个标记取消
        /// Task：处理程序返回对象，延迟覆盖执行器不关心这个，所以就Task就行了
        /// </summary>
        Func<object, CancellationToken, Task> job;// { get; set; }
        /// <summary>
        /// 延迟任务执行后触发
        /// YanchiChuli：当前对象本身
        /// Task：job的返回任务对象
        /// CancellationToken：有请求覆盖时，之前的任务可以通过这个标记取消
        /// ValueTask：事件处理程序的返回值
        /// </summary>
        public event Func<YanchiChuli, Task, CancellationToken, ValueTask> OnExecuted;
        /// <summary>
        /// 延迟执行报错时触发
        /// YanchiChuli：当前对象本身
        /// Task：job的返回任务对象
        /// CancellationToken：有请求覆盖时，之前的任务可以通过这个标记取消
        /// Exception：错误对象
        /// ValueTask：事件处理程序的返回值
        /// </summary>
        public event Func<YanchiChuli, Task, CancellationToken, Exception, ValueTask> OnError;
        /// <summary>
        /// 延迟多久，单位毫秒
        /// </summary>
        public int Delay { get; private set; }
        /// <summary>
        /// 若大于0，则表示最多等这么久，超时后job立即强制执行
        /// 小于等于0时，则忽略此逻辑
        /// 默认为Delay*5
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// 最后一次真正执行的时间点
        /// </summary>
        DateTime lastExecuteTime = DateTime.MinValue;

        ///// <summary>
        ///// 最后执行时间
        ///// </summary>
        //public DateTime lastExecuteTime { get; private set; } =

        private readonly object locker = new object();

        public bool Executing => cts != default && !cts.IsCancellationRequested;

        ILogger logger = SimpleLogger.Instance;

        public YanchiChuli(Func<object, CancellationToken, Task> job, int delay = 5000, ILoggerFactory loggerFactory = null)
        {
            this.job = job;
            this.Delay = delay;
            if (loggerFactory != default)
                logger = loggerFactory.CreateLogger<YanchiChuli>();
            Timeout = delay * 5;
        }

        CancellationTokenSource cts;// = new CancellationTokenSource();
        public void Dispose()
        {
            logger.LogDebug($"延迟覆盖执行器{GetHashCode()}释放了");
            try
            {
                cts?.Cancel();
            }
            catch { }
        }
        /// <summary>
        /// 请求执行
        /// </summary>
        /// <param name="yanchi"></param>
        /// <param name="chaoshi"></param>
        /// <returns>每次请求的唯一id</returns>
        public void Request(object state = default)
        {
            //由于后执行的要覆盖先执行的，所以请求时直接覆盖
            CancellationTokenSource tempcts;
            //lock确保后续任务闭包引用正确的tempcts，不会产生无法释放的CancellationTokenSource
            lock (locker)
            {
                try
                {
                    cts?.Cancel();
                }
                catch { }
                tempcts = cts = new CancellationTokenSource();
            }
            Task.Run(async () =>
            {
                try
                {
                    int lssc = 0;//已等待的毫秒数
                    while (true)
                    {
                        //经过测试，这里等1毫秒有问题，起码本地测试是有问题的
                        await Task.Delay(10);
                        lssc += 10;

                        if (Timeout > 0 && (DateTime.Now - lastExecuteTime).TotalMilliseconds >= Timeout)
                        {
                            logger.LogDebug($"延时覆盖{GetHashCode()} tempcts{tempcts.GetHashCode()} 超时了{Timeout}，立即强制执行！");
                            //有序下面会new，这里需要手动释放，不能依赖外层的finally兜底
                            try
                            {
                                tempcts.Dispose();
                            }
                            catch { }
                            //new个新的，防止下个请求取消它，后续的finally会释放它
                            tempcts = new CancellationTokenSource();
                            break;
                        }

                        //若后执行覆盖了当前执行，则放弃执行，此逻辑一定放在timeout逻辑后面
                        if (tempcts.IsCancellationRequested)
                        {
                            logger.LogDebug($"延时覆盖{GetHashCode()} tempcts{tempcts.GetHashCode()} 任务放弃了！");
                            //有外层的finally兜底
                            //try
                            //{
                            //    tempcts.Dispose();//早点释放也无所谓
                            //}
                            //catch { }
                            return;
                        }

                        if (lssc >= Delay)
                            break;
                    }
                    //若这里又来个新请求，顶部的 cts?.Cancel();会生效
                    logger.LogDebug($"延时覆盖执行器{GetHashCode()} tempcts{tempcts.GetHashCode()} 任务开始执行...");
                    lastExecuteTime = DateTime.Now;
                    var t = job(state, tempcts.Token);
                    try
                    {
                        await t;
                        if (OnExecuted != default)
                            await OnExecuted(this, t, tempcts.Token);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"延迟覆盖执行器{this.GetHashCode()}，执行目标任务时发生未处理异常！");
                        if (OnError != default)
                            await OnError(this, t, tempcts.Token, ex);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"延时覆盖执行器{GetHashCode()} tempcts{tempcts.GetHashCode()} 执行内部错误...");
                }
                finally
                {
                    //若之前或新请求导致释放了，这里再释放会抛出异常
                    try
                    {
                        tempcts.Dispose();
                    }
                    catch { }
                }
            });
        }
    }
}