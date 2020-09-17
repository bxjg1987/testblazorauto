using Abp.Dependency;
using Abp.Domain.Services;
using BXJG.Equipment.Protocol;
using Microsoft.Extensions.Logging;
using SuperSocket.Channel;
using SuperSocket.Client;
using SuperSocket.ProtoBase;
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
     * 设备控制通常分两种，开关某个状态，配合用命令类型可以区分开关哪个状态
     * 另一个是设置某个值，可以直接设置、增加、减小值
     * 在进一步抽象可以将状态的开关也看做是设置值
     * 目前的舱压控制的设计并不通用，后期再调整
     * 
     */

    /// <summary>
    /// 设备控制领域服务，目前是开关设备状态、调整舱压
    /// 设备状态上报的数据可以直接在应用服务中通过仓储查询
    /// </summary>
    public class EquipmentManager : DomainService
    {
        /// <summary>
        /// 负责与设备中控服务端通信
        /// </summary>
        IEasyClient<OxygenChamberPackage> easyClient;
        /// <summary>
        /// 负责将强类型的控制设备状态的数据包转换成通信协议规定的命令
        /// </summary>
        IPackageEncoder<OxygenChamberStatePackage> ztkz;
        /// <summary>
        /// 负责将强类型的调整设备舱压的数据包转换成通信协议规定的命令
        /// </summary>
        IPackageEncoder<cykz> cykz;
        /// <summary>
        /// 实例化设备带有设备控制功能的领域服务实力
        /// </summary>
        /// <param name="easyClient">负责与设备中控服务端通信</param>
        /// <param name="ztkz">负责将强类型的控制设备状态的数据包转换成通信协议规定的命令</param>
        /// <param name="cyzk">负责将强类型的调整设备舱压的数据包转换成通信协议规定的命令</param>
        public EquipmentManager(IEasyClient<OxygenChamberPackage> easyClient, IPackageEncoder<OxygenChamberStatePackage> ztkz, IPackageEncoder<cykz> cyzk)
        {
            this.easyClient = easyClient;
            this.ztkz = ztkz;
            this.cykz = cyzk;
        }
        /// <summary>
        /// 改变设备状态
        /// </summary>
        /// <param name="entity">设备实体，之所以传实体是领域服务中可能会做更多处理</param>
        /// <param name="type">对应协议的命令字段，1开关门 2开关电。。。</param>
        /// <param name="state">true开 false关</param>
        /// <returns></returns>
        public async ValueTask ChangeStateAsync(EquipmentInfoEntity entity, byte type, bool state)
        {
            await easyClient.SendAsync(ztkz, new OxygenChamberStatePackage
            {
                EquipmentId = int.Parse(entity.HardwareCode),
                Key = type,
                State = state
            });
        }
        /// <summary>
        /// 控制舱压
        /// </summary>
        /// <param name="entity">设备实体对象</param>
        /// <param name="add">true增加 false减小</param>
        /// <param name="value">增加或减小的舱压值</param>
        /// <returns></returns>
        public async ValueTask ChangePressureAsync(EquipmentInfoEntity entity, bool add, short value)
        {
            await easyClient.SendAsync(cykz, new cykz
            {
                EquipmentId = int.Parse(entity.HardwareCode),
                Key = 104,
                Add = add,
                Value = value
            });
        }
    }
}
