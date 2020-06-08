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
        }
    }
}
