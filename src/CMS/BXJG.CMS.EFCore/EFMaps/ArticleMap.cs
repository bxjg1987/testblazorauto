using BXJG.CMS.Article;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.EFCore.EFMaps
{
    public class ArticleMap<TDataDictionary> : IEntityTypeConfiguration<ArticleEntity<TDataDictionary>>
    {
        public void Configure(EntityTypeBuilder<ArticleEntity<TDataDictionary>> builder)
        {
            builder.Property(c => c.Title).HasMaxLength(BXJGCMSConsts.ArticleTitleMaxLength).IsRequired(true);
            builder.Property(c => c.SeoTitle).HasMaxLength(BXJGCMSConsts.ArticleSeoTitleMaxLength);
            builder.Property(c => c.SeoKeyword).HasMaxLength(BXJGCMSConsts.ArticleSeoKeywordMaxLength);
            builder.Property(c => c.SeoDescription).HasMaxLength(BXJGCMSConsts.ArticleSeoDescriptionMaxLength);
            builder.Property(c => c.Summary).HasMaxLength(BXJGCMSConsts.ArticleSummaryMaxLength);
        }
    }
}
