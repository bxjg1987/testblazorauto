using Abp.Authorization.Users;
using BXJG.Common;
using BXJG.GoodsInfo.GoodsInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.GoodsInfo.EFCore.Map
{

    /// <summary>
    /// 基础物品信息ef映射
    /// </summary>
    public class GoodsInfoMap : IEntityTypeConfiguration<GoodsInfoEntity>
    {
        public void Configure(EntityTypeBuilder<GoodsInfoEntity> builder)
        {
            builder.Property(c => c.Name).HasMaxLength(BXJGGoodsInfoCoreConsts.GoodsInfoNameMaxLength);
            builder.Property(c => c.MnemonicCode).HasMaxLength(BXJGGoodsInfoCoreConsts.GoodsInfoMnemonicCodeMaxLength);
            builder.Property(c => c.UnitId).HasMaxLength(BXJGGoodsInfoCoreConsts.GoodsInfoUnitIdMaxLength);
            builder.Property(c => c.GoodsInfoExtensionType).HasMaxLength(BXJGGoodsInfoCoreConsts.GoodsInfoExtensionTypeMaxLength);
        }
    }

    /// <summary>
    /// 具体的物品类型的ef映射基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GoodsInfoExtension<T> : IEntityTypeConfiguration<T>
        where T : class, IGoodsInfoExtensionEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            //虽然目前是空的，将来可能增加属性时需要映射

            //builder.Property(c => c.RowVersion).IsRowVersion();
            ////builder.Property(c => c.ExtensionData).HasMaxLength(int.MaxValue);
            //builder.Property(c => c.Title).HasMaxLength(CoreConsts.OrderTitleMaxLength).IsRequired();
            //builder.Property(c => c.Description).HasMaxLength(CoreConsts.OrderDescriptionMaxLength);
            //builder.Property(c => c.StatusChangedDescription).HasMaxLength(CoreConsts.OrderStatusChangedDescriptionMaxLength);
            //builder.Property(c => c.EmployeeId).HasColumnType($"varchar({CoreConsts.OrderEmployeeIdMaxLength})");
            ////builder.Property(c => c.ContactName).HasMaxLength(CoreConsts.OrderContactNameMaxLength);
            ////builder.Property(c => c.ContactPhone).HasColumnType($"varchar({CoreConsts.OrderContactPhoneMaxLength})");
            ////外键好像默认会建立索引，但这里没有使用外键
            //builder.HasIndex(p => new { p.CategoryId, p.EmployeeId });
        }
    }
}