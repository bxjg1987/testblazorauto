using BXJG.Shop.Catalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public class ProductCategoryMap : IEntityTypeConfiguration<ProductCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryEntity> builder)
        {
            builder.Property(c => c.Icon).HasColumnType($"varchar({BXJGShopConsts.ItemCategoryIconMaxLength})");
            builder.Property(c => c.Image1).HasColumnType($"varchar({BXJGShopConsts.ItemCategoryImage1MaxLength})");
            builder.Property(c => c.Image2).HasColumnType($"varchar({BXJGShopConsts.ItemCategoryImage2MaxLength})");
        }
    }
}
