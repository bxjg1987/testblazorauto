using BXJG.Shop.Catalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public class ItemMap<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity: ItemEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            //builder.Property(c => c.Icon).HasColumnType($"varchar({BXJGShopDictionaryEntity.IconMaxLength})");
        }
    }
}
