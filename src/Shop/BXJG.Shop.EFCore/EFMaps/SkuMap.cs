using BXJG.Shop.Catalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public class SkuMap : IEntityTypeConfiguration<SkuEntity>
    {
        public void Configure(EntityTypeBuilder<SkuEntity> builder)
        {
            builder.ToTable("BXJGShopSku");

            builder.Property(c => c.DynamicProperty1Name).HasMaxLength(CoreConsts.SkuDynamicPropertyNameMaxLength);
            builder.Property(c => c.DynamicProperty1Value).HasMaxLength(CoreConsts.DynamicPropertyValueMaxLength);
            builder.Property(c => c.DynamicProperty1Text).HasMaxLength(CoreConsts.DynamicPropertyTextMaxLength);

            builder.Property(c => c.DynamicProperty2Name).HasMaxLength(CoreConsts.SkuDynamicPropertyNameMaxLength);
            builder.Property(c => c.DynamicProperty2Value).HasMaxLength(CoreConsts.DynamicPropertyValueMaxLength);
            builder.Property(c => c.DynamicProperty2Text).HasMaxLength(CoreConsts.DynamicPropertyTextMaxLength);

            builder.Property(c => c.DynamicProperty3Name).HasMaxLength(CoreConsts.SkuDynamicPropertyNameMaxLength);
            builder.Property(c => c.DynamicProperty3Value).HasMaxLength(CoreConsts.DynamicPropertyValueMaxLength);
            builder.Property(c => c.DynamicProperty3Text).HasMaxLength(CoreConsts.DynamicPropertyTextMaxLength);

            builder.Property(c => c.DynamicProperty4Name).HasMaxLength(CoreConsts.SkuDynamicPropertyNameMaxLength);
            builder.Property(c => c.DynamicProperty4Value).HasMaxLength(CoreConsts.DynamicPropertyValueMaxLength);
            builder.Property(c => c.DynamicProperty4Text).HasMaxLength(CoreConsts.DynamicPropertyTextMaxLength);

            builder.Property(c => c.DynamicProperty5Name).HasMaxLength(CoreConsts.SkuDynamicPropertyNameMaxLength);
            builder.Property(c => c.DynamicProperty5Value).HasMaxLength(CoreConsts.DynamicPropertyValueMaxLength);
            builder.Property(c => c.DynamicProperty5Text).HasMaxLength(CoreConsts.DynamicPropertyTextMaxLength);

        }
    }
}
