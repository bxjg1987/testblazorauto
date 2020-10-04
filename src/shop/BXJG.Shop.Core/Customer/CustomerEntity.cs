using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Sale;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 商城系统中的顾客，与用户一对一关联
    /// </summary>
    public class CustomerEntity : FullAuditedEntity<long>, IMustHaveTenant, IExtendableObject
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 关联到abp用户的id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 顾客的积分
        /// </summary>
        public long Integral { get; set; }
        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTimeOffset? Birthday { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public virtual AdministrativeEntity Area { get; set; }
        ///// <summary>
        ///// 顾客的订单列表
        ///// 不要加这个属性，会导致顾客变得复杂。如果加了这个 那将来有更多概念需要与顾客关联时，顾客实体会变得越来越复杂
        ///// </summary>
        //public virtual List<OrderEntity<TUser>> Orders { get; set; }
        /// <summary>
        /// 积分、余额等处理时可能存在并发处理
        /// 最好的办法是在要处理的字段上加并发控制，以减小并发冲突的几率，但是目前一切从简先用行并发控制
        /// </summary>
        public byte[] RowVersion { get; set; }
        public string ExtensionData { get; set; }
    }
}
