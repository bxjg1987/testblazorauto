using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.CMS.Article
{
    /// <summary>
    /// 后台管理员对文章进行管理的应用服务接口
    /// </summary>
    public interface IArticleAppService : IAsyncCrudAppService<ArticleDto, long, GetAllArticleInput, ArticleEditDto>
    {
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BulkDelete(DeleteInput input);
    }
}
