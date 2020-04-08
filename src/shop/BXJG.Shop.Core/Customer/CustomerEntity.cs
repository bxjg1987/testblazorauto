using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer
{
    /*
     * 商城用户（顾客）有很多额外字段，比如积分啥的 也可能包含多个收获地址
     * 因此建立顾客与abp用户的一对一关系
     * 其它实体在引用 顾客实体时 最好连abp用户一起关联，这样将来查询更容易
     * 或许可以尝试不用泛型，而直接连到AbpUserBase，这样比较冒险。再着将来主程序需要引用CustomerEntity时也很难关联查询到泛型实体的额外属性
     * 但是如果用泛型 将来很多直接或间接依赖CustomerEntity的类都需要泛型参数 非常麻烦，因此决定冒险一试
     * 
     * 经过测试发现不用泛型行不通
     */
    public class CustomerEntity<TUser> : FullAuditedEntity<long>, IMustHaveTenant
        where TUser:AbpUserBase //因为内部可能包含领域逻辑，因此加约束更方便
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
        public TUser User { get; set; }
        /// <summary>
        /// 顾客的积分
        /// </summary>
        public long Integral { get; set; }
        /// <summary>
        /// 总消费金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
