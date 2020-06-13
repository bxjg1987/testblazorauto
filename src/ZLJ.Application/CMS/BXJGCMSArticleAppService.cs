using Abp.Domain.Repositories;
using BXJG.CMS.Article;
using BXJG.CMS.Column;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.CMS
{
    /// <summary>
    /// 后台管理CMS文章的应用服务
    /// </summary>
    public class BXJGCMSArticleAppService : BXJGCMSArticleAppService<GeneralTreeEntity>
    {
        public BXJGCMSArticleAppService(IRepository<ArticleEntity<GeneralTreeEntity>, long> repository, IRepository<ColumnEntity<GeneralTreeEntity>, long> columnRepository) : base(repository, columnRepository)
        {
        }
    }
}
