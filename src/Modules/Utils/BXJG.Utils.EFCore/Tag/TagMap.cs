using BXJG.Utils.Files;
using BXJG.Utils.Share;
using BXJG.Utils.Tag;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.Tag
{
    public class TagMap : IEntityTypeConfiguration<TagEntity>
    {
        public void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder.ToTable("BXJGTag",tb=>tb.HasComment("通用实体标签"));
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.EntityType).IsRequired().IsUnicode(false).HasMaxLength(BXJGUtilsConsts.EntityTypeMaxLength).HasComment("关联实体类型，可以是任意唯一字符串，通常是实体类型.FullTypeName");
            builder.Property(c => c.EntityId).IsRequired().IsUnicode(false).HasMaxLength(BXJGUtilsConsts.AttachmentEntityIdMaxLength).HasComment("关联实体id");
            builder.Property(c => c.PropertyName).IsRequired().IsUnicode(false).HasMaxLength(BXJGUtilsConsts.AttachmentEntityPropertyNameMaxLength).HasComment("属性名，可空 比如工单：字段A表示要处理的问题相关tag，字段B表示处理完成时拍摄的tag，它们都使用tag表，当通过此字段来表示关联的不同的属性"); ;
            builder.Property(c => c.PropertyDisplayName).IsUnicode(true).HasMaxLength(BXJGUtilsConsts.MaxDisplayNameLength).HasComment("属性显示名，在存储时若为空则复制PropertyName"); ;

            //查询时，由于实体id多数是guid，根本就不需要实体类型这个参数，所以联合索引不合理
            //builder.HasIndex(c => new { c.EntityType, c.EntityId, c.PropertyName });
            builder.HasIndex(c => c.EntityType);
            builder.HasIndex(c => c.EntityId);
            builder.HasIndex(c => c.PropertyName);

            builder.Property(c => c.TagName).IsRequired().IsUnicode(false).HasMaxLength(BXJGUtilsConsts.TagNameMaxLength).HasComment("标签名称、同一个实体的同一个属性下必须唯一");
            builder.Property(c => c.TagDisplayName).IsUnicode(true).HasMaxLength(BXJGUtilsConsts.TagDisplayNameMaxLength).HasComment("标签显示名称");
           
            builder.Property(c => c.ExtensionData).IsUnicode(true).HasMaxLength(BXJGUtilsConsts.ExtDataMaxLength).HasComment("json格式的扩展数据");

            //builder.Property(c => c.ExtField1).IsUnicode(true).HasMaxLength(BXJGUtilsConsts.ExtFieldldMaxLength).HasComment("普通的扩展字段1");
            //builder.Property(c => c.ExtField2).IsUnicode(true).HasMaxLength(BXJGUtilsConsts.ExtField2dMaxLength).HasComment("普通的扩展字段2");

            //builder.HasOne(x => x.File).WithMany().HasForeignKey(x => x.FileId);
        }
    }
}
