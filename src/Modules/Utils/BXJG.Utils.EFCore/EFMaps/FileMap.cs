using BXJG.Utils.Files;
using BXJG.Utils.Share;
using BXJG.Utils.Share.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.EFMaps
{
    public class FileMap : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.ToTable("BXJGFiles",x=>x.HasComment("通用的文件表"));
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.ExtensionData).IsUnicode().HasMaxLength(BXJGUtilsConsts.ExtDataMaxLength).HasComment("扩展数据");
            builder.Property(c => c.RealName).IsRequired().IsUnicode().HasMaxLength(BXJGUtilsConsts.FileRealNameMaxLength).HasComment("真实的文件名 c#高级编程");
            builder.Property(c => c.Ext).IsUnicode(false).HasMaxLength(BXJGUtilsConsts.FileExtMaxLength).HasComment("文件扩展名，如：.jpg");
            builder.Property(c => c.ResponseContentType).IsRequired().IsUnicode(false).HasMaxLength(BXJGUtilsConsts.FileContentTypeMaxLength).HasComment("响应的文件类型，mime");
            builder.Property(c => c.RelativePath).IsRequired().IsUnicode(false).HasMaxLength(BXJGUtilsConsts.FileRelativePathMaxLength).HasComment("相对于文件存储目录的 相对路径");
            builder.Property(c => c.RelativePathThumbnail).IsUnicode(false).HasMaxLength(BXJGUtilsConsts.FileThumbnailRelativePathMaxLength).HasComment("缩略图相对路径");

            builder.HasIndex(c => c.RealName);
            builder.HasIndex(c => c.Size);
            builder.HasIndex(c => c.ResponseContentType);
        }
    }
}
