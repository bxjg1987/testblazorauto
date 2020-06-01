using BXJG.CMS.Article;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.EFCore.EFMaps
{
    public class ArticleMap : IEntityTypeConfiguration<ArticleEntity>
    {
        public void Configure(EntityTypeBuilder<ArticleEntity> builder)
        {
            //builder.Property(c => c.Title).HasArticleType($"varchar({ArticleEntity.IconMaxLength})");
            builder.Property(c => c.Title).HasMaxLength(ArticleEntity.TitleMaxLength).IsRequired(true);
            builder.Property(c => c.Title).IsRequired(true);
        }
    }
}
