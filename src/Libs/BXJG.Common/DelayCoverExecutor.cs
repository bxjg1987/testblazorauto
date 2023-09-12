using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BXJG.Common
{
    /*
     * 定时任务 是被动的，每个一段时间必须会执行某个逻辑，若此逻辑是个耗时任务，频率过高越浪费性能，频率过低实时性又无法保证。
     * 
     * 此延迟执行是被主动调用的，所以实时性更高，需要时才调用的，无浪费。
     * 使用请求的源自己的线程，就不会有浪费空线程。
     * （队列执行，必定会有个死循环线程，若没有请求时，死线程就是浪费。）
     * 注意：被执行的逻辑任然在各自的线程中，这个类仅仅是减小执行的几率
     * 
     */

    /// <summary>
    /// 延时覆盖执行器
    /// </summary>
    public class DelayCoverExecutor
    {
        /// <summary>
        /// 需要延迟处理的任务
        /// 耗费资源的处理才需要做这种处理，所以它一定是异步
        /// 注意，其内部不应该抛出异常。
        /// </summary>
        public Func<Task> Job { get; set; }
        /// <summary>
        /// 延迟多久，单位毫秒
        /// </summary>
        public int Delay { get; set; } = 2000;
        /// <summary>
        /// 若任务一直被延迟，超过此时间，则强制执行。单位毫秒。
        /// 注意，它必须大于Delay
        /// </summary>
        public int Timeout { get; set; } = 4000;
        /// <summary>
        /// 最后执行时间
        /// </summary>
        private DateTime lastExecuteTime = DateTime.MinValue;
        /// <summary>
        /// 执行标记
        /// </summary>
        private Guid executeTag = Guid.Empty;

        public ILogger Logger { get; set; } = NullLogger.Instance;

        //超时执行，或 执行时长大于   延迟  都可能造成并发调用
        private Task Execute()
        {
            lastExecuteTime = DateTime.Now;
            return Job();
        }

        /// <summary>
        /// 请求执行
        /// </summary>
        /// <param name="yanchi"></param>
        /// <param name="chaoshi"></param>
        /// <returns>每次请求的唯一id</returns>
        public Task Request()
        {
            var zxbj = Guid.NewGuid();
            executeTag = zxbj;
        
            return Task.Run(async () =>
            {
                int tempI = 0;
                while (executeTag == zxbj)
                {
                    Thread.Sleep(1);
                    tempI++;
                    if (tempI > Delay)
                        break;
                }

                if (executeTag != zxbj)
                {
                    if ((DateTime.Now - lastExecuteTime).TotalMilliseconds > Timeout)
                    {
                        this.Logger.LogDebug($"延迟处理已超时，强制执行。");
                        await Execute();
                        //return;
                    }
                    else
                    {
                        this.Logger.LogDebug($"延迟处理已被覆盖。");
                        //return;
                    }
                }
                else
                {
                    this.Logger.LogDebug($"延迟处理开始执行。");
                    await Execute();
                }
            });
        }

        /// <summary>
        /// 延时覆盖执行器
        /// </summary>
        /// <param name="job"></param>
        /// <param name="yanchi"></param>
        /// <param name="chaoshi"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static DelayCoverExecutor Create(Func<Task> job, int yanchi = 2000, int chaoshi = 4000, ILogger logger = default)
        {
            if (logger == default)
            {
                logger = NullLogger.Instance;
            }
            return new DelayCoverExecutor { Job = job, Delay = yanchi, Timeout = chaoshi, Logger = logger };
        }
    }
}