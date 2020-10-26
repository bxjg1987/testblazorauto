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
            builder.Property(c => c.DynamicEntityProperty1Value).HasMaxLength(CoreConsts.DynamicEntityPropertyValueMaxLength);
            builder.Property(c => c.DynamicEntityProperty1Text).HasMaxLength(CoreConsts.DynamicEntityPropertyTextMaxLength);

            builder.Property(c => c.DynamicProperty2Name).HasMaxLength(CoreConsts.SkuDynamicPropertyNameMaxLength);
            builder.Property(c => c.DynamicEntityProperty2Value).HasMaxLength(CoreConsts.DynamicEntityPropertyValueMaxLength);
            builder.Property(c => c.DynamicEntityProperty2Text).HasMaxLength(CoreConsts.DynamicEntityPropertyTextMaxLength);

            builder.Property(c => c.DynamicProperty3Name).HasMaxLength(CoreConsts.SkuDynamicPropertyNameMaxLength);
            builder.Property(c => c.DynamicEntityProperty3Value).HasMaxLength(CoreConsts.DynamicEntityPropertyValueMaxLength);
            builder.Property(c => c.DynamicEntityProperty3Text).HasMaxLength(CoreConsts.DynamicEntityPropertyTextMaxLength);

            builder.Property(c => c.DynamicProperty4Name).HasMaxLength(CoreConsts.SkuDynamicPropertyNameMaxLength);
            builder.Property(c => c.DynamicEntityProperty4Value).HasMaxLength(CoreConsts.DynamicEntityPropertyValueMaxLength);
            builder.Property(c => c.DynamicEntityProperty4Text).HasMaxLength(CoreConsts.DynamicEntityPropertyTextMaxLength);

            builder.Property(c => c.DynamicProperty5Name).HasMaxLength(CoreConsts.SkuDynamicPropertyNameMaxLength);
            builder.Property(c => c.DynamicEntityProperty5Value).HasMaxLength(CoreConsts.DynamicEntityPropertyValueMaxLength);
            builder.Property(c => c.DynamicEntityProperty5Text).HasMaxLength(CoreConsts.DynamicEntityPropertyTextMaxLength);

        }
    }
}
