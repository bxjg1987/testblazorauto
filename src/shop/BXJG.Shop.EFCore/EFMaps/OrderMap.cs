using Abp.Authorization.Users;
using BXJG.Common;
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
    public class OrderMap<TUser, TArea,TEntity, TDataDictionary> : IEntityTypeConfiguration<TEntity>
        //where TUser : AbpUserBase
        //where TArea : GeneralTreeEntity<TArea>, IAdministrative
        where TEntity: OrderEntity<TUser, TArea, TDataDictionary>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(c => c.OrderNo).IsRequired(true).HasColumnType($"varchar({BXJGShopConsts.OrderNoMaxLength})");
            builder.Property(c => c.CustomerRemark).HasMaxLength(BXJGShopConsts.CustomerRemarkMaxLength);
            builder.Property(c => c.Consignee).IsRequired(true).HasMaxLength(BXJGShopConsts.ConsigneeMaxLength);
            builder.Property(c => c.ConsigneePhoneNumber).IsRequired(true).HasColumnType($"varchar({BXJGShopConsts.ConsigneePhoneNumberMaxLength})");
            builder.Property(c => c.ReceivingAddress).IsRequired(true).HasMaxLength(BXJGShopConsts.ReceivingAddressMaxLength);
            builder.Property(c => c.LogisticsNumber).HasColumnType($"varchar({BXJGShopConsts.LogisticsNumberMaxLength})");
            builder.Property(c => c.RowVersion).IsRowVersion();
            builder.HasIndex(g => g.OrderNo).IsUnique();
        }
    }
}
