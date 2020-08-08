using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace OxygenChamber.Server.Protocol
{
    /// <summary>
    /// 设备发送过来的消息
    /// 可能是服务器发送控制命令后的返回信息
    /// 也可能是设备主动发送过来的数据
    /// </summary>
    public class OxygenChamberPackage : IKeyedPackageInfo<byte>
    {
        /// <summary>
        /// 对应协议中的命令
        /// 1开关门的返回
        /// 2断电的返回
        /// 3气阀的返回
        /// 4仓压的返回
        /// 5设备上报状态
        /// 
        /// 101(0x65)控制开关门
        /// ...
        /// 104(0x68)控制仓压
        /// </summary>
        public byte Key { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 门的开关状态
        /// </summary>
        public bool DoorState { get; set; }
        /// <summary>
        /// 通电状态
        /// </summary>
        public bool ElectricState { get; set; }
        /// <summary>
        /// 气阀状态
        /// </summary>
        public bool ValveState { get; set; }
        /// <summary>
        /// 仓压
        /// </summary>
        public short Pressure { get; set; }
        /// <summary>
        /// 压力控制 true增加 false减少
        /// </summary>
        public bool PressureControl { get; set; }
        /// <summary>
        /// 控制仓压的返回状态
        /// </summary>
        public bool PressureState { get; set; }
        /// <summary>
        /// 氧气浓度
        /// </summary>
        public short OxygenConcentration { get; set; }
        /// <summary>
        /// 备用8字节
        /// </summary>
        public byte[] Spare { get; set; }
        ///// <summary>
        ///// 控制方发来的原始命令，直接下发给设备用的
        ///// </summary>
        //public byte[] OriginalCMD { get; set; }
    }
}
