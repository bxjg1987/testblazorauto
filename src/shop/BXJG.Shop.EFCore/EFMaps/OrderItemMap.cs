using Abp.Authorization.Users;
using BXJG.Common;
using BXJG.GeneralTree;
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
    public class OrderItemMap : IEntityTypeConfiguration<OrderItemEntity>
    {
        public virtual void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.ToTable("BXJGShopOrderItems");
            builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}
