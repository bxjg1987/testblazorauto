using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 顾客收获地址
    /// 没想到合适的命名，它主要是定义一个顾客的多个收货地址及相应的联系人、联系方式等
    /// </summary>
    public class ShippingAddressEntity : Entity<long>, IMustHaveTenant, IExtendableObject
    {
        //虽然不是聚合根没必要添加这个字段，但如果有这个字段将来做数据处理可能更方便
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 所属顾客id
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 所属顾客
        /// </summary>
        public virtual CustomerEntity Customer { get; set; }
        /// <summary>
        /// 联系人名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long AreaId { get; set; }
        ///// <summary>
        ///// 所属区域
        ///// </summary>
        //public virtual AdministrativeEntity Area { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }

        //另一种方式是在CustomerEntity上定义默认收货地址的id，这样收货地址这个表可以少个字段，但是相比之下目前的方式实现起来更简单直观
        /// <summary>
        /// 是否为默认收货信息
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 扩展字段
        /// 目前没有想到它的使用场景，只是考虑其它系统引入商城模块时可以用这种简单的扩展方式扩展
        /// </summary>
        public string ExtensionData { get; set; }

        //收货时段 略
    }
}
