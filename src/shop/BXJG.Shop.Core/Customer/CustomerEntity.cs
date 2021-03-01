using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Sale;
using BXJG.Utils.BusinessUser;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 商城系统中的顾客，与用户一对一关联
    /// </summary>
    public class CustomerEntity : FullAuditedAggregateRoot<long>, IBusinessUserEntity, IMustHaveTenant, IExtendableObject
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 关联到abp用户的id
        /// </summary>
        public long UserId { get; private set; }
        private long integral;
        /// <summary>
        /// 顾客的积分
        /// </summary>
        public long Integral
        {
            get
            {
                return integral;
            }
            set
            {
                var t = integral;
                integral = value;
                //怀疑ef是给变量integral赋值的，应该不会查询时触发这个事件
                if (t != value)
                {
                    DomainEvents.Add(new CustomerIntegralChangedEventData(this, t));
                }
            }
        }
        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal Amount { get; private set; }
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
        ///// <summary>
        ///// 所属区域
        ///// </summary>
        //public virtual AdministrativeEntity Area { get; set; }
        ///// <summary>
        ///// 顾客的订单列表
        ///// 不要加这个属性，会导致顾客变得复杂。如果加了这个 那将来有更多概念需要与顾客关联时，顾客实体会变得越来越复杂
        ///// </summary>
        //public virtual List<OrderEntity<TUser>> Orders { get; set; }
        /// <summary>
        /// 积分、余额等处理时可能存在并发处理
        /// 最好的办法是在要处理的字段上加并发控制，以减小并发冲突的几率，但是目前一切从简先用行并发控制
        /// </summary>
        public byte[] RowVersion { get; private set; }
        public string ExtensionData { get; set; }
        public virtual List<ShippingAddressEntity> ShippingAddresses { get; set; }
        /// <summary>
        /// 给ef用的，它可以放回私有成员
        /// </summary>
        private CustomerEntity() { }
        /// <summary>
        /// 实例化顾客，AutoMapper可以使用构造函数映射
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="integral"></param>
        /// <param name="amount"></param>
        /// <param name="id"></param>
        public CustomerEntity(long userId, long integral = default, decimal amount = default, long id = default, List<ShippingAddressEntity> addresses=default)
        {
            Id = id;
            UserId = userId;
            Integral = integral;
            Amount = amount;
            ShippingAddresses = addresses ?? new List<ShippingAddressEntity>();
        }

        #region 积分处理

        ///// <summary>
        ///// 调整积分
        ///// </summary>
        ///// <param name="value">增加或减少的积分数量</param>
        ///// <param name="increase">true增加，false减少</param>
        ///// <returns></returns>
        //public void ChangeIntegral(long value)
        //{
        //    SetIntegral(Integral + value);
        //}
        ///// <summary>
        ///// 设置积分，若值有变化将触发事件
        ///// </summary>
        ///// <param name="value"></param>
        //public void SetIntegral(long value)
        //{
        //    if (Integral != value)
        //    {
        //        var t = Integral;
        //        Integral = value;
        //        DomainEvents.Add(new CustomerIntegralChangedEventData(this, t));
        //    }
        //}

        #endregion
        // 使用方法而不是属性，这样调用方明确知晓此调用将进行计算，而不是直接从变量中获取
        /// <summary>
        /// 获取默认收货地址
        /// </summary>
        /// <returns></returns>
        public ShippingAddressEntity GetDefaltShippingAddress()
        {
            return ShippingAddresses.Single(c => c.IsDefault);
        }
    }
}
