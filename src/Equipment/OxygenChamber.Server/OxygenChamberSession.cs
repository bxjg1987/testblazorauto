using SuperSocket.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OxygenChamber.Server
{
    /// <summary>
    /// 设备通讯的自定义Session
    /// </summary>
    public class OxygenChamberSession : AppSession
    {
        ///// <summary>
        ///// 每个命令一个队列
        ///// </summary>
        //public readonly IDictionary<byte, BlockingCollection<OxygenChamberPackage>> dic = new Dictionary<byte, BlockingCollection<OxygenChamberPackage>>();

        //默认使用的线程安全的队列new ConcurrentQueue<OxygenChamberPackage>()
        //public readonly BlockingCollection<OxygenChamberPackage> bc = new BlockingCollection<OxygenChamberPackage>( 10);

        public OxygenChamberSession()
        {
            //for (int i = 0; i < 4; i++)
            //{

            //}
            //cq = new ConcurrentQueue<OxygenChamberPackage>();
           // cq.
        }

        /// <summary>
        /// 获取设备id
        /// </summary>
        public int EquipmentId { get; set; }
        //protected override ValueTask OnSessionConnectedAsync()
        //{
        //    return base.OnSessionConnectedAsync();
        //}
    }
}
