using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.Protocol
{
    /// <summary>
    /// 设备发送过来的消息
    /// 可能是服务器发送控制命令后的返回信息
    /// 也可能是设备主动发送过来的数据
    /// </summary>
    public class OxygenChamberPackage : IKeyedPackageInfo<byte>
    {
        /// <summary>
        /// 每条消息的唯一id
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();
        /// <summary>
        /// 产生消息的时间
        /// </summary>
        public DateTimeOffset CreateTime { get; } = DateTimeOffset.Now;
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
        public int EquipmentId { get; set; }
    }
    /// <summary>
    /// 发送或返回的设备状态的包
    /// </summary>
    public class OxygenChamberStatePackage : OxygenChamberPackage
    {
        public bool State { get; set; }
    }
    /// <summary>
    /// 发送舱压控制的包
    /// </summary>
    public class cykz : OxygenChamberPackage
    {
        public bool Add { get; set; }
        public byte Value { get; set; }
    }
    /// <summary>
    /// 状态上报
    /// </summary>
    public class ztsb : OxygenChamberPackage
    {
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
        /// 氧气浓度
        /// </summary>
        public short OxygenConcentration { get; set; }
    }
}
