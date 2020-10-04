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
    /// 订单的ef映射类，请在主程序的DBContext.OnModelCreating注册
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class OrderMap : IEntityTypeConfiguration<OrderEntity>
    {
        public virtual void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.Property(c => c.OrderNo).IsRequired(true).HasColumnType($"varchar({CoreConsts.OrderNoMaxLength})");
            builder.Property(c => c.CustomerRemark).HasMaxLength(CoreConsts.CustomerRemarkMaxLength);
            builder.Property(c => c.Consignee).IsRequired(true).HasMaxLength(CoreConsts.ConsigneeMaxLength);
            builder.Property(c => c.ConsigneePhoneNumber).IsRequired(true).HasColumnType($"varchar({CoreConsts.ConsigneePhoneNumberMaxLength})");
            builder.Property(c => c.ReceivingAddress).IsRequired(true).HasMaxLength(CoreConsts.ReceivingAddressMaxLength);
            builder.Property(c => c.LogisticsNumber).HasColumnType($"varchar({CoreConsts.LogisticsNumberMaxLength})");
            builder.Property(c => c.RowVersion).IsRowVersion();
            builder.HasIndex(g => g.OrderNo).IsUnique();
        }
    }
}
