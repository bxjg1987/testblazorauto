using BXJG.Utils.File;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.EFCore.EFMaps
{
    public class EntityFileMap : IEntityTypeConfiguration<EntityFileEntity>
    {
        public void Configure(EntityTypeBuilder<EntityFileEntity> builder)
        {
            builder.Property(c => c.EntityType).HasColumnType($"varchar({FileConsts.EntityFileEntityTypeMaxLength})");
            builder.Property(c => c.EntityId).HasColumnType($"varchar({FileConsts.EntityFileEntityIdMaxLength})");
            builder.Property(c => c.FileUrl).HasColumnType($"varchar({FileConsts.EntityFileFileUrlMaxLength})");
            builder.Property(c => c.ThumUrl).HasColumnType($"varchar({FileConsts.EntityFileThumUrlMaxLength})");

            //builder.HasMany(c => c.WorkOrderTypes).WithOne().HasForeignKey("CategoryId");
            //builder.Property(c => c.WorkOrderTypes).HasMaxLength(CoreConsts.WorkOrderClsTypeMaxLength);
            //builder.Property(c => c.Icon).HasColumnType($"varchar({CoreConsts.ItemCategoryIconMaxLength})");
            //builder.Property(c => c.Image1).HasColumnType($"varchar({CoreConsts.ItemCategoryImage1MaxLength})");
            //builder.Property(c => c.Image2).HasColumnType($"varchar({CoreConsts.ItemCategoryImage2MaxLength})");
        }
    }

}
