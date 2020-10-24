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
            builder.Property(c => c.OrderNo).IsRequired().HasColumnType($"varchar({CoreConsts.OrderNoMaxLength})");
            builder.HasIndex(g => g.OrderNo).IsUnique();
            builder.Property(c => c.CustomerRemark).HasMaxLength(CoreConsts.CustomerRemarkMaxLength);
            builder.Property(c => c.Consignee).IsRequired().HasMaxLength(CoreConsts.ConsigneeMaxLength);
            builder.Property(c => c.ConsigneePhoneNumber).IsRequired().HasColumnType($"varchar({CoreConsts.ConsigneePhoneNumberMaxLength})");
            builder.Property(c => c.ReceivingAddress).IsRequired().HasMaxLength(CoreConsts.ReceivingAddressMaxLength);
            builder.Property(c => c.LogisticsNumber).HasColumnType($"varchar({CoreConsts.LogisticsNumberMaxLength})");
            builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}
