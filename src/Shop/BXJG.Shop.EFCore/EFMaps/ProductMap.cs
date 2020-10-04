using BXJG.Shop.Catalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public class ProductMap : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            //builder.Property(c => c.Title).HasColumnType($"varchar({BXJGShopDictionaryEntity.IconMaxLength})");
            builder.Property(c => c.Title).HasMaxLength(CoreConsts.ItemTitleMaxLength).IsRequired();
            //builder.Property(c => c.Sku).HasMaxLength(BXJGShopConsts.ItemSkuMaxLength);
            builder.Property(c => c.DescriptionShort).HasMaxLength(CoreConsts.ItemDescriptionShortMaxLength);
            builder.Property(c => c.Specification).HasMaxLength(CoreConsts.ItemSpecificationMaxLength);
            builder.Property(c => c.Images).HasColumnType($"varchar({CoreConsts.ItemImagesMaxLength})");
            builder.Ignore(c => c.DomainEvents);
            //builder.Property(c => c.DescriptionShort).HasMaxLength(BXJGShopConsts.ItemDescriptionShortMaxLength);
            //builder.Property(c => c.DescriptionShort).HasMaxLength(BXJGShopConsts.ItemDescriptionShortMaxLength);
            //builder.Property(c => c.DescriptionShort).HasMaxLength(BXJGShopConsts.ItemDescriptionShortMaxLength);
        }
    }
}
