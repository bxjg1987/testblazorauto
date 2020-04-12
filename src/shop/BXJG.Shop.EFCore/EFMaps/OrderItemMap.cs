using Abp.Authorization.Users;
using BXJG.Shop.Sale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
namespace BXJG.Shop.EFMaps
{
    /// <summary>
    /// 订单产品明细ef映射
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class OrderItemMap<TUser> : IEntityTypeConfiguration<OrderItemEntity<TUser>>
        where TUser : AbpUserBase
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity<TUser>> builder)
        {
           builder.ToTable("BXJGShopOrderItems");
           builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}
