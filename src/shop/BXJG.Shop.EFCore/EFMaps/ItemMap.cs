using BXJG.Shop.Catalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public class ItemMap : IEntityTypeConfiguration<ItemEntity>
    {
        public void Configure(EntityTypeBuilder<ItemEntity> builder)
        {
            //builder.Property(c => c.Title).HasColumnType($"varchar({BXJGShopDictionaryEntity.IconMaxLength})");
            builder.Property(c => c.Title).HasMaxLength(BXJGShopConsts.ItemTitleMaxLength).IsRequired();
            builder.Property(c => c.Sku).HasMaxLength(BXJGShopConsts.ItemSkuMaxLength);
            builder.Property(c => c.DescriptionShort).HasMaxLength(BXJGShopConsts.ItemDescriptionShortMaxLength);
            builder.Property(c => c.Specification).HasMaxLength(BXJGShopConsts.ItemSpecificationMaxLength);
            builder.Property(c => c.Images).HasColumnType($"varchar({BXJGShopConsts.ItemImagesMaxLength})");
            builder.Ignore(c => c.DomainEvents);
            //builder.Property(c => c.DescriptionShort).HasMaxLength(BXJGShopConsts.ItemDescriptionShortMaxLength);
            //builder.Property(c => c.DescriptionShort).HasMaxLength(BXJGShopConsts.ItemDescriptionShortMaxLength);
            //builder.Property(c => c.DescriptionShort).HasMaxLength(BXJGShopConsts.ItemDescriptionShortMaxLength);
        }
    }
}
