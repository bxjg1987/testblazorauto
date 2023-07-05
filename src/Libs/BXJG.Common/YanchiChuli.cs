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
        public int Yanchi { get; set; } = 2000;
        /// <summary>
        /// 若任务一直被延迟，超过此时间，则强制执行。单位毫秒。
        /// 注意，它必须大于Yanchi
        /// </summary>
        public int Chaoshi { get; set; } = 4000;
        /// <summary>
        /// 最后执行时间
        /// </summary>
        private DateTime zuihouZhixingShijian = DateTime.MinValue;
        /// <summary>
        /// 执行标记
        /// </summary>
        private Guid zhixingBiaoji = Guid.Empty;

        public ILogger Logger { get; set; } = NullLogger.Instance;

        private Task Zhixing()
        {
            zuihouZhixingShijian = DateTime.Now;
            return Job();
        }

        public void Qingqiu(int yanchi = 0, int chaoshi = 0)
        {
            var zxbj = Guid.NewGuid();
            zhixingBiaoji = zxbj;
            Task.Run(async () =>
            {
                if (yanchi == 0)
                    yanchi = Yanchi;


                //await Task.Delay(yanchi == 0 ? Yanchi : yanchi);
                int tempI = 0;
                while (zhixingBiaoji == zxbj)
                {
                    Thread.Sleep(1);
                    tempI++;
                    if (tempI > yanchi)
                        break;
                }

                if ((DateTime.Now - zuihouZhixingShijian).TotalMilliseconds >= (chaoshi == 0 ? Chaoshi : chaoshi))
                {
                    this.Logger.LogDebug($"延迟处理已超时，强制执行。");
                    await Zhixing();
                    return;
                }

                //注意与上面的处理顺序不要动，否则上面的逻辑不符合要求。
                if (zhixingBiaoji != zxbj)
                {
                    this.Logger.LogDebug($"延迟处理已被覆盖。");
                    return;
                }

                this.Logger.LogDebug($"延迟处理开始执行。");
                await Zhixing();
            });
        }

        public static YanchiChuli Create(Func<Task> job, int yanchi = 2000, int chaoshi = 4000, ILogger logger = default)
        {
            if (logger == default)
            {
                logger = NullLogger.Instance;
            }
            return new YanchiChuli { Job = job, Yanchi = yanchi, Chaoshi = chaoshi, Logger = logger };
        }
    }
}