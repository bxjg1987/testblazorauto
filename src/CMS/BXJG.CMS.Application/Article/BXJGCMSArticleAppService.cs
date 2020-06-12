using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using BXJG.CMS.Authorization;
using BXJG.GeneralTree;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.Extensions;

namespace BXJG.CMS.Article
{
    /// <summary>
    /// 后台管理文章的应用服务
    /// </summary>
    public class BXJGCMSArticleAppService<TDataDictionary> : AsyncCrudAppService<ArticleEntity<TDataDictionary>,
                                                                                 ArticleDto,
                                                                                 long,
                                                                                 GetAllArticleInput,
                                                                                 ArticleEditDto>, IBXJGCMSArticleAppService
        where TDataDictionary : GeneralTreeEntity<TDataDictionary>
    {
        public BXJGCMSArticleAppService(IRepository<ArticleEntity<TDataDictionary>, long> repository) : base(repository)
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

        protected override IQueryable<ArticleEntity<TDataDictionary>> CreateFilteredQuery(GetAllArticleInput input)
        {
            return base.CreateFilteredQuery(input)
                       .Include(c => c.Column)
                       .AsNoTracking()
                       .WhereIf(input.Published.HasValue, c => c.Published == input.Published.Value)
                       .WhereIf(input.PublishStartTime.HasValue, c =>  c.PublishStartTime >= input.PublishStartTime.Value)
                       .WhereIf(input.PublishEndTime.HasValue, c =>  c.PublishEndTime < input.PublishEndTime.Value)
                       .WhereIf(!input.Keywords.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Keywords)
                                                                           || c.SeoTitle.Contains(input.Keywords)
                                                                           || c.SeoDescription.Contains(input.Keywords)
                                                                           || c.SeoKeyword.Contains(input.Keywords)
                                                                           || c.Summary.Contains(input.Keywords));
        }


    }
}
