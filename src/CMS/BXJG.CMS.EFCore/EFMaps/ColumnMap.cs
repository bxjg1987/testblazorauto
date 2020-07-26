using BXJG.CMS.Column;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.EFCore.EFMaps
{
    public class ColumnMap : IEntityTypeConfiguration<ColumnEntity>
    {
        public void Configure(EntityTypeBuilder<ColumnEntity> builder)
        {
            builder.Property(c => c.Icon).HasColumnType($"varchar({BXJGCMSConsts.ColumnIconMaxLength})");
            builder.Property(c => c.SeoTitle).HasMaxLength(BXJGCMSConsts.ColumnSeoTitleMaxLength);
            builder.Property(c => c.SeoDescription).HasMaxLength(BXJGCMSConsts.ColumnSeoDescriptionMaxLength);
            builder.Property(c => c.SeoKeyword).HasMaxLength(BXJGCMSConsts.ColumnSeoKeywordMaxLength);
            builder.Property(c => c.ListTemplate).HasColumnType($"varchar({BXJGCMSConsts.ColumnListTemplateMaxLength})");
            builder.Property(c => c.DetailTemplate).HasColumnType($"varchar({BXJGCMSConsts.ColumnDetailTemplateMaxLength})");
        }
    }
}
