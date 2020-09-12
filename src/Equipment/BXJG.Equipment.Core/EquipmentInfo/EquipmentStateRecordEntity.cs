using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Equipment.EquipmentInfo;
using Castle.MicroKernel.Registration.Interceptor;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment
{
    /// <summary>
    /// 设备运行过程中采集的状态记录
    /// </summary>
    public class EquipmentStateRecordEntity : CreationAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 所属设备id
        /// </summary>
        public long EquipmentInfoId { get; set; }
        /// <summary>
        /// 所属设备导航属性
        /// </summary>
        public virtual EquipmentInfoEntity EquipmentInfo { get; set; }
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
