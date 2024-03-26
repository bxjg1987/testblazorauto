using BXJG.Utils.Files;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.Share;
using BXJG.Utils.Share.Files;
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
            builder.ToTable("BXJGUtilsAttachments");
            builder.Property(c => c.EntityType).IsRequired().HasColumnType($"varchar({BXJGUtilsConsts.EntityFileEntityTypeMaxLength})").HasComment("关联实体类型，可以是任意唯一字符串，通常是实体类型.FullTypeName");
            builder.Property(c => c.EntityId).IsRequired().HasColumnType($"varchar({BXJGUtilsConsts.EntityFileEntityIdMaxLength})").HasComment("关联实体id");
            builder.Property(c => c.PropertyName).HasColumnType($"varchar({BXJGUtilsConsts.EntityFilePropertyNameMaxLength})").HasComment("属性名，可空 比如工单：字段A表示要处理的问题相关图片，字段B表示处理完成时拍摄的图片，它们都使用附件表，当通过此字段来表示关联的不同的属性"); ;

            builder.HasIndex(c => new { c.EntityType, c.EntityId, c.PropertyName });

            //builder.HasOne(x => x.File).WithMany().HasForeignKey(x => x.Id);
            //builder.Property(c => c.RelativeFileUrl).HasColumnType($"varchar({Consts.EntityFileFileUrlMaxLength})");
            //builder.Ignore(c => c.RelativeThumUrl);
            //builder.Ignore(c => c.AbsoluteFileUrl);
            //builder.Ignore(c => c.AbsoluteThumUrl);
            builder.Property(c => c.ExtensionData).HasColumnType($"varchar({BXJGUtilsConsts.ExtDataMaxLength})");
            //builder.HasMany(c => c.WorkOrderTypes).WithOne().HasForeignKey("CategoryId");
            //builder.Property(c => c.WorkOrderTypes).HasMaxLength(CoreConsts.WorkOrderClsTypeMaxLength);
            //builder.Property(c => c.Icon).HasColumnType($"varchar({CoreConsts.ItemCategoryIconMaxLength})");
            //builder.Property(c => c.Image1).HasColumnType($"varchar({CoreConsts.ItemCategoryImage1MaxLength})");
            //builder.Property(c => c.Image2).HasColumnType($"varchar({CoreConsts.ItemCategoryImage2MaxLength})");

        }
    }
    
}
