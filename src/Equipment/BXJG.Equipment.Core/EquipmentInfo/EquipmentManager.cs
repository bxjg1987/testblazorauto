using Abp.Domain.Services;
using SuperSocket.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Equipment.EquipmentInfo
{
    /*
     * 目前不考虑过多设计，因为对硬件控制不是特别熟悉
     * 现在只针对现有场景，控制氧舱进行实现
     * 
     * 阔以考虑为每个设备创建一个EquipmentManager实例，来负责单个设备的控制
     * 由于这种情况无法使用依赖注入，因此需要定义个工厂来负责创建EquipmentManager实例
     * 这样EquipmentManager的方法都不需要EquipmentInfoEntity参数，而是在构造函数中传入
     * 目前不这样做
     * 
     * 
     * 
     */
    public class EquipmentManager : DomainService
    {
        //IEasyClient easyClient;

        /// <summary>
        /// 改变设备状态
        /// </summary>
        /// <param name="entity">设备实体，之所以传实体是领域服务中可能会做更多处理</param>
        /// <param name="type">对应协议的命令字段，1开关门 2开关电。。。</param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async ValueTask ChangeStateAsync(EquipmentInfoEntity entity, byte type, bool state)
        {

        }
        /// <summary>
        /// 控制舱压
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="add"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async ValueTask ChangePressureAsync(EquipmentInfoEntity entity, bool add, byte value)
        {

        }

        //private Span<byte> BuildCommand() {
        //    var rcmd = new byte[10];
        //    var j = 0;
        //    rcmd[j++] = 0xbb;
        //    rcmd[j++] = cmd;
        //    rcmd[j++] = 0x07;
        //    var eid = BitConverter.GetBytes(equipmentId);
        //    rcmd[j++] = eid[3];
        //    rcmd[j++] = eid[2];
        //    rcmd[j++] = eid[1];
        //    rcmd[j++] = eid[0];
        //    rcmd[j++] = state.ToByte();//状态
        //    rcmd[j++] = new ReadOnlySpan<byte>(rcmd, 1, 8).CalculateAdd();
        //    rcmd[j++] = 0xed;
        //}
    }
}
