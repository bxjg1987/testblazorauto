using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using BXJG.Shop.Sale;
using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer
{
    /*
     * 商城用户（顾客）有很多额外字段，比如积分啥的 也可能包含多个收货地址
     * 因此建立顾客与abp用户的一对一关系
     * 其它实体在引用 顾客实体时 可以保护abp用户和顾客两个实体的引用，关联查询会更方便，但是导致那个实体更复杂，商城业务中更多情况只关注顾客属性
     * 或许可以尝试不用泛型，而直接连到AbpUserBase，这样比较冒险。再着将来主程序需要引用CustomerEntity时也很难关联查询到泛型实体的额外属性
     * 
     * 经过测试发现不用泛型行不通
     * 
     * 去掉OrderEntity的直接关联，因为顾客是个独立的概念，它不一定会有订单；按同样的思路，今后出现的更多与顾客有关的概念时 顾客实体会变得更加复杂
     * 再则订单是一个复杂的概念，将来可能有更多泛型，顾客实体会变得越来越复杂
     * 
     */
    /// <summary>
    /// 商城系统中的顾客
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class CustomerEntity<TUser, TArea> : FullAuditedEntity<long>, IMustHaveTenant
        where TUser : AbpUserBase //因为内部可能包含领域逻辑，因此加约束更方便
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
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
        /// 关联到abp用户
        /// </summary>
        public virtual TUser User { get; set; }
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
        public virtual TArea Area { get; set; }
        ///// <summary>
        ///// 顾客的订单列表
        ///// 不要加这个属性，会导致顾客变得复杂。如果加了这个 那将来有更多概念需要与顾客关联时，顾客实体会变得越来越复杂
        ///// </summary>
        //public virtual List<OrderEntity<TUser, TArea>> Orders { get; set; }
        /// <summary>
        /// 积分、余额等处理时可能存在并发处理
        /// 最好的办法是在要处理的字段上加并发控制，以减小并发冲突的几率，但是目前一切从简先用行并发控制
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
