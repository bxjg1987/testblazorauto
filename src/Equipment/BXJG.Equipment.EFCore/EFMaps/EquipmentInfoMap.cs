using BXJG.Equipment;
using BXJG.Equipment.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment
{
    public class EquipmentInfoMap<TEntity, TDataDictionary> : IEntityTypeConfiguration<TEntity>
        where TEntity: EquipmentInfoEntity<TDataDictionary>
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            //builder.Property(c => c.Title).HasColumnType($"varchar({BXJGShopDictionaryEntity.IconMaxLength})");
            builder.Property(c => c.Name).HasMaxLength(BXJGEquipmentConst.EquipmentInfoNameMaxLength).IsRequired();
            builder.Property(c => c.Longitude).IsRequired();
            builder.Property(c => c.Latitude).IsRequired();
        }
    }
}
