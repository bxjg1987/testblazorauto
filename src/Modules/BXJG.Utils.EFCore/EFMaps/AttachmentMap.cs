using BXJG.Utils.File;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.EFCore.EFMaps
{
    public class AttachmentMap : IEntityTypeConfiguration<AttachmentEntity>
    {
        public void Configure(EntityTypeBuilder<AttachmentEntity> builder)
        {
            builder.Property(c => c.EntityType).HasColumnType($"varchar({FileConsts.EntityFileEntityTypeMaxLength})");
            builder.Property(c => c.EntityId).HasColumnType($"varchar({FileConsts.EntityFileEntityIdMaxLength})");
            builder.HasIndex(c => new { c.EntityType, c.EntityId });

            builder.Property(c => c.RelativeFileUrl).HasColumnType($"varchar({FileConsts.EntityFileFileUrlMaxLength})");
            builder.Ignore(c => c.RelativeThumUrl);
            builder.Ignore(c => c.AbsoluteFileUrl);
            builder.Ignore(c => c.AbsoluteThumUrl);
            builder.Property(c => c.PropertyName).HasColumnType($"varchar({FileConsts.EntityFilePropertyNameMaxLength})");
            //builder.HasMany(c => c.WorkOrderTypes).WithOne().HasForeignKey("CategoryId");
            //builder.Property(c => c.WorkOrderTypes).HasMaxLength(CoreConsts.WorkOrderClsTypeMaxLength);
            //builder.Property(c => c.Icon).HasColumnType($"varchar({CoreConsts.ItemCategoryIconMaxLength})");
            //builder.Property(c => c.Image1).HasColumnType($"varchar({CoreConsts.ItemCategoryImage1MaxLength})");
            //builder.Property(c => c.Image2).HasColumnType($"varchar({CoreConsts.ItemCategoryImage2MaxLength})");
        }
    }
    public class GeneralTreeMap : IEntityTypeConfiguration<DataDictionaryEntity> {
        public void Configure(EntityTypeBuilder<DataDictionaryEntity> builder)
        {
            builder.MapGeneralTree();
        }
    }
}
