using BXJG.Shop.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public class DictionaryMap : IEntityTypeConfiguration<BXJGShopDictionaryEntity>
    {
        public void Configure(EntityTypeBuilder<BXJGShopDictionaryEntity> builder)
        {
            builder.Property(c => c.Icon).HasColumnType($"varchar({BXJGShopDictionaryEntity.IconMaxLength})");
        }
    }
}
