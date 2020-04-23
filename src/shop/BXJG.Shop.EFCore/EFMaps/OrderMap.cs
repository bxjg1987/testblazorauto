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
    /// 订单的ef映射类，请在主程序的DBContext.OnModelCreating注册
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class OrderMap<TUser> : IEntityTypeConfiguration<OrderEntity<TUser>>
        where TUser : AbpUserBase
    {
        public void Configure(EntityTypeBuilder<OrderEntity<TUser>> builder)
        {
            builder.Property(c => c.OrderNo).IsRequired(true).HasColumnType($"varchar({OrderEntity<TUser>.OrderNoMaxLength})");
            builder.Property(c => c.CustomerRemark).HasMaxLength(OrderEntity<TUser>.CustomerRemarkMaxLength);
            builder.Property(c => c.Consignee).IsRequired(true).HasMaxLength(OrderEntity<TUser>.ConsigneeMaxLength);
            builder.Property(c => c.ConsigneePhoneNumber).IsRequired(true).HasColumnType($"varchar({OrderEntity<TUser>.ConsigneePhoneNumberMaxLength})");
            builder.Property(c => c.ReceivingAddress).IsRequired(true).HasMaxLength(OrderEntity<TUser>.ReceivingAddressMaxLength);
            builder.Property(c => c.LogisticsNumber).HasColumnType($"varchar({OrderEntity<TUser>.LogisticsNumberMaxLength})");
            builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}
