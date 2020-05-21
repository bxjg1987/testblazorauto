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
    /// 订单产品明细ef映射
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class OrderItemMap<TUser, TArea,TEntity> : IEntityTypeConfiguration<TEntity>
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
        where TEntity: OrderItemEntity<TUser, TArea>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable("BXJGShopOrderItems");
            builder.Property(c => c.RowVersion).IsRowVersion();
        }
    }
}
