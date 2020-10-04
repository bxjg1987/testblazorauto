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
        }
    }
}
