using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
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
    /// 延迟处理
    /// </summary>
    public class YanchiChuli
    {/// <summary>
     /// 需要延迟处理的任务
     /// 耗费资源的处理才需要做这种处理，所以它一定是异步
     /// 注意，其内部不应该抛出异常。
     /// </summary>
        Func<Task> job;// { get; set; }
        /// <summary>
        /// 延迟任务执行后触发
        /// </summary>
        public event Func<YanchiChuli, Task, ValueTask> OnExecuted;
        /// <summary>
        /// 延迟执行报错时触发
        /// </summary>
        public event Func<YanchiChuli, Exception, Task, ValueTask> OnError;
        /// <summary>
        /// 延迟多久，单位毫秒
        /// </summary>
        int delay;//{ get; set; } = 2000;
        ///// <summary>
        ///// 最后执行时间
        ///// </summary>
        //public DateTime lastExecuteTime { get; private set; } = DateTime.MinValue;

        private readonly object locker = new object();

        private bool executing = false;

        ILogger logger { get; set; } = NullLogger.Instance;

        public YanchiChuli(Func<Task> job, int delay = 2000, ILogger logger = default)
        {
            this.job = job;
            this.delay = delay;
            if (logger == default)
                this.logger = NullLogger.Instance;
            else
                this.logger = logger;
        }
        // //超时执行，或 执行时长大于   延迟  都可能造成并发调用
        //private Task Execute()
        //{
        //  //  lastExecuteTime = DateTime.Now;
        //    return Job();
        //}

        /// <summary>
        /// 请求执行
        /// </summary>
        /// <param name="yanchi"></param>
        /// <param name="chaoshi"></param>
        /// <returns>每次请求的唯一id</returns>
        public void Request()
        {
            if (executing)
                return;

            lock (locker)
            {
                if (executing)
                    return;

                executing = true;

                Task.Run(async () =>
                {
                    Thread.Sleep(delay);
                    var t = job();
                    try
                    {
                        await t;
                        if (OnExecuted != default)
                            await OnExecuted(this, t);
                    }
                    catch (Exception ex)
                    {
                        if (OnError != default)
                            await OnError(this, ex, t);
                    }
                    finally
                    {
                        executing = false;
                    }
                });
            }
        }

        ///// <summary>
        ///// 延时覆盖执行器
        ///// </summary>
        ///// <param name="job"></param>
        ///// <param name="yanchi"></param>
        ///// <param name="chaoshi"></param>
        ///// <param name="logger"></param>
        ///// <returns></returns>
        //public static DelayCoverExecutor Create(Func<Task> job, int yanchi = 2000, int chaoshi = 4000, ILogger logger = default)
        //{
        //    if (logger == default)
        //    {
        //        logger = NullLogger.Instance;
        //    }
        //    return new DelayCoverExecutor { job = job, Delay = yanchi, Timeout = chaoshi, Logger = logger };
        //}
    }
}