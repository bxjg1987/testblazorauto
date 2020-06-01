using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

namespace BXJG.CMS.Article
{
    public class ArticleAppService : AsyncCrudAppService<ArticleEntity, ArticleDto, long, GetAllArticleInput, ArticleEditDto>, IArticleAppService
    {
        public ArticleAppService(IRepository<ArticleEntity, long> repository) : base(repository)
        {
        }

        public Task BulkDelete(DeleteInput input)
        {
            throw new NotImplementedException();
        }
    }
}
