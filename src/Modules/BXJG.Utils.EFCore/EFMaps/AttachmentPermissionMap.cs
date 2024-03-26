using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Utils.Files;
using BXJG.Utils.Share;
using BXJG.Utils.Share.Files;

namespace BXJG.Utils.EFCore.EFMaps
{
    public class AttachmentPermissionMap : IEntityTypeConfiguration<AttachmentPermissionEntity>
    {
        public void Configure(EntityTypeBuilder<AttachmentPermissionEntity> builder)
        {
            builder.ToTable("BXJGUtilsAttchmentPermissions");
            builder.Property(c => c.EntityType).IsRequired().HasColumnType($"varchar({BXJGUtilsConsts.EntityFileEntityTypeMaxLength})").HasComment("关联实体类型，可以是任意唯一字符串，通常是实体类型.FullTypeName");
            builder.Property(c => c.EntityId).HasColumnType($"varchar({BXJGUtilsConsts.EntityFileEntityIdMaxLength})").HasComment("关联实体id");
            builder.Property(c => c.DownloadPermissionName).HasColumnType($"varchar({BXJGUtilsConsts.AttachmentPermissionNameMaxLength})").HasComment("允许下载的权限名称");
            builder.Property(c => c.DeletePermissionName).HasColumnType($"varchar({BXJGUtilsConsts.AttachmentPermissionNameMaxLength})").HasComment("允许删除的权限名称");

        }
    }
}
