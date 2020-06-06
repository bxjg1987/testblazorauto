using BXJG.CMS.Column;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.EFCore.EFMaps
{
    public class ColumnMap<TDataDictionary> : IEntityTypeConfiguration<ColumnEntity<TDataDictionary>>
    {
        public void Configure(EntityTypeBuilder<ColumnEntity<TDataDictionary>> builder)
        {
            builder.Property(c => c.Icon).HasColumnType($"varchar({BXJGCMSConsts.IconMaxLength})");
            builder.Property(c => c.SeoTitle).HasMaxLength(BXJGCMSConsts.SeoTitleMaxLength);
            builder.Property(c => c.SeoDescription).HasMaxLength(BXJGCMSConsts.SeoDescriptionMaxLength);
            builder.Property(c => c.SeoKeyword).HasMaxLength(BXJGCMSConsts.SeoKeywordMaxLength);
            builder.Property(c => c.ListTemplate).HasColumnType($"varchar({BXJGCMSConsts.ListTemplateMaxLength})");
            builder.Property(c => c.DetailTemplate).HasColumnType($"varchar({BXJGCMSConsts.DetailTemplateMaxLength})");
        }
    }
}
