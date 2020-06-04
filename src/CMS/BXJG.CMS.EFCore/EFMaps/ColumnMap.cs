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
            builder.Property(c => c.Title).HasMaxLength(ColumnEntity.SeoTitleMaxLength);
            builder.Property(c => c.Icon).HasColumnType($"varchar({ColumnEntity.IconMaxLength})");
            builder.Property(c => c.SeoTitle).HasMaxLength(ColumnEntity.SeoTitleMaxLength);
            builder.Property(c => c.SeoDescription).HasMaxLength(ColumnEntity.SeoDescriptionMaxLength);
            builder.Property(c => c.SeoKeyword).HasMaxLength(ColumnEntity.SeoKeywordMaxLength);
            //builder.Property(c => c.ColumnType).HasMaxLength(ColumnEntity.SeoKeywordMaxLength);
            //builder.Property(c => c.SeoKeyword).HasMaxLength(ColumnEntity.SeoKeywordMaxLength);
        }
    }
}
