using BXJG.Equipment;
using BXJG.Equipment.EquipmentInfo;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment
{
    public class EquipmentInfoMap<TEntity> : IEntityTypeConfiguration<EquipmentInfoEntity>
    {
        public void Configure(EntityTypeBuilder<EquipmentInfoEntity> builder)
        {
            builder.Property(c => c.HardwareCode).HasColumnType($"varchar({BXJGEquipmentConst.EquipmentInfoHardwareCodeMaxLength})");
            builder.Property(c => c.Name).HasMaxLength(BXJGEquipmentConst.EquipmentInfoNameMaxLength).IsRequired();

            builder.Property(c => c.Longitude).IsRequired();
            builder.Property(c => c.Latitude).IsRequired();
        }
    }

}
