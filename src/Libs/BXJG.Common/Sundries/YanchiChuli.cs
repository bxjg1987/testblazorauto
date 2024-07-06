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
        /// 注意，其内部不应该抛出异常。
        /// </summary>
        Func<CancellationToken, Task> job;// { get; set; }
        /// <summary>
        /// 延迟任务执行后触发
        /// </summary>
        public event Func<YanchiChuli, Task, CancellationToken, ValueTask> OnExecuted;
        /// <summary>
        /// 延迟执行报错时触发
        /// </summary>
        public event Func<YanchiChuli, Task, CancellationToken, Exception, ValueTask> OnError;
        /// <summary>
        /// 延迟多久，单位毫秒
        /// </summary>
        public int Delay { get; private set; }
        
        ///// <summary>
        ///// 最后执行时间
        ///// </summary>
        //public DateTime lastExecuteTime { get; private set; } = DateTime.MinValue;

        // private readonly object locker = new object();

        public bool Executing => cts != default && !cts.IsCancellationRequested;

        ILogger logger = SimpleLogger.Instance;

        public YanchiChuli(Func<CancellationToken, Task> job, int delay = 5000, ILoggerFactory loggerFactory = null)
        {
            this.job = job;
            this.Delay = delay;
            if (loggerFactory != default)
                logger = loggerFactory.CreateLogger<YanchiChuli>();
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
        public void Request()
        {
            //由于后执行的要覆盖先执行的，所以请求时直接覆盖
            try
            {
                cts?.Cancel();
            }
            catch { }
            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var tempcts = cts;// CancellationTokenSource.CreateLinkedTokenSource(cts.Token);//闭包，确保对老对象有引用
                int lssc = 0;//已等待的毫秒数
                while (true)
                {
                    //经过测试，这里等1毫秒有问题，起码本地测试是有问题的
                    await Task.Delay(10);
                    lssc += 10;
                    //若后执行覆盖了当前执行，则放弃执行
                    if (tempcts.IsCancellationRequested)
                    {
                        logger.LogDebug($"延时覆盖{GetHashCode()} tempcts{tempcts.GetHashCode()} 任务放弃了！");
                        try
                        {
                            tempcts.Dispose();//早点释放也无所谓
                        }
                        catch { }
                        return;
                    }

                    if (lssc >= Delay)
                        break;
                }

                //若这里又来个新请求，顶部的 cts?.Cancel();会生效
                logger.LogDebug($"延时覆盖执行器{GetHashCode()} tempcts{tempcts.GetHashCode()} 任务开始执行...");
                var t = job(tempcts.Token);
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