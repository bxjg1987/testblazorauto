using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using BXJG.CMS.Authorization;

namespace BXJG.CMS.Article
{
    /// <summary>
    /// 后台管理文章的应用服务
    /// </summary>
    public class BXJGCMSArticleAppService : AsyncCrudAppService<ArticleEntity, ArticleDto, long, GetAllArticleInput, ArticleEditDto>, IBXJGCMSArticleAppService
    {
        public BXJGCMSArticleAppService(IRepository<ArticleEntity, long> repository) : base(repository)
        {
            LocalizationSourceName = BXJGCMSConsts.LocalizationSourceName;

            GetAllPermissionName = BXJGCMSPermissions.Article;
            GetPermissionName = BXJGCMSPermissions.Article;
            CreatePermissionName = BXJGCMSPermissions.ArticleCreate;
            DeletePermissionName = BXJGCMSPermissions.ArticleDelete;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task BulkDeleteAsync(DeleteInput input)
        {
            return Repository.DeleteAsync(c => input.Ids.Contains(c.Id));
        }
    }
}
