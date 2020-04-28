using Abp.Authorization.Users;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
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
    public class OrderMap<TUser,TArea> : IEntityTypeConfiguration<OrderEntity<TUser,TArea>>
        where TUser : AbpUserBase
          where TArea : GeneralTreeEntity<TArea>, IShopAdministrative
    {
        public void Configure(EntityTypeBuilder<OrderEntity<TUser, TArea>> builder)
        {
            builder.Property(c => c.OrderNo).IsRequired(true).HasColumnType($"varchar({OrderEntity<TUser, TArea>.OrderNoMaxLength})");
            builder.Property(c => c.CustomerRemark).HasMaxLength(OrderEntity<TUser, TArea>.CustomerRemarkMaxLength);
            builder.Property(c => c.Consignee).IsRequired(true).HasMaxLength(OrderEntity<TUser, TArea>.ConsigneeMaxLength);
            builder.Property(c => c.ConsigneePhoneNumber).IsRequired(true).HasColumnType($"varchar({OrderEntity<TUser, TArea>.ConsigneePhoneNumberMaxLength})");
            builder.Property(c => c.ReceivingAddress).IsRequired(true).HasMaxLength(OrderEntity<TUser, TArea>.ReceivingAddressMaxLength);
            builder.Property(c => c.LogisticsNumber).HasColumnType($"varchar({OrderEntity<TUser, TArea>.LogisticsNumberMaxLength})");
            builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}
