using SuperSocket.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace OxygenChamber.Server
{
    /// <summary>
    /// 设备通讯的自定义Session
    /// </summary>
    public class OxygenChamberSession : AppSession
    {
        /// <summary>
        /// 获取设备id
        /// </summary>
        public int EquipmentId { get; set; }
    }
}
