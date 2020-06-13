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
using BXJG.CMS.Column;

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
       private readonly  IRepository<ColumnEntity<TDataDictionary>, long> columnRepository;

        public BXJGCMSArticleAppService(IRepository<ArticleEntity<TDataDictionary>, long> repository, IRepository<ColumnEntity<TDataDictionary>, long> columnRepository) : base(repository)
        {
            LocalizationSourceName = BXJGCMSConsts.LocalizationSourceName;

            GetAllPermissionName = BXJGCMSPermissions.Article;
            GetPermissionName = BXJGCMSPermissions.Article;
            CreatePermissionName = BXJGCMSPermissions.ArticleCreate;
            DeletePermissionName = BXJGCMSPermissions.ArticleDelete;

            this.columnRepository = columnRepository;
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
            //这种做法不好，查两次，且为了查code 还使用了同步。最好是不要这个直接一条语句查询
            //当然最最好的办法是强制要求调用方传递code，提供columnId的唯一目的是考虑调用方有时候确实不太方便传递code
            if (input.ColumnCode.IsNullOrWhiteSpace() && input.ColumnId .HasValue&& input.ColumnId.Value>0)
                input.ColumnCode = this.columnRepository.Get(input.ColumnId.Value).Code;

            return base.CreateFilteredQuery(input)
                       .Include(c => c.Column)
                       .AsNoTracking()
                       .WhereIf(!input.ColumnCode.IsNullOrWhiteSpace(),c=>c.Column.Code.StartsWith(input.ColumnCode))
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
