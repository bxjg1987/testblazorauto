using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using BXJG.CMS.Article;
using BXJG.DynamicAssociateEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BXJG.CMS.Localization;

namespace BXJG.CMS.DynamicAssociateEntity
{
    public class DynamicAssociateEntityArticleService : IDynamicAssociateEntityService
    {
        private readonly IRepository<ArticleEntity, long> repository;
        private readonly IRepository<Column.ColumnEntity, long> columnRepository;
        protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        public DynamicAssociateEntityArticleService(IRepository<ArticleEntity, long> repository, IRepository<Column.ColumnEntity, long> columnRepository)
        {
            this.repository = repository;
            this.columnRepository = columnRepository;
        }
        public async Task<PagedResultDto<object>> GetAllAsync(string parentId, string keyword, string sorting, int skip, int maxcount)
        {
            var query = repository.GetAll()
                                  .WhereIf(!parentId.IsNullOrWhiteSpace(), c => c.ColumnId.ToString() == parentId)
                                  .WhereIf(!keyword.IsNullOrWhiteSpace(), c => c.Title.Contains(keyword))
                                  .Select(c => new
                                  {
                                      c.Id,
                                      c.Title,
                                      c.Published
                                  });
            var total = await AsyncQueryableExecuter.CountAsync(query);
            if (!sorting.IsNullOrWhiteSpace())
                query = query.OrderBy(sorting);
            query = query.PageBy(skip, maxcount);
            var listEntity = await AsyncQueryableExecuter.ToListAsync(query);
            return new PagedResultDto<object>(total, listEntity);
        }
        public async Task<IList<object>> GetAllNoPageAsync(string parentId, string keyword, string sorting)
        {
            var query = repository.GetAll()
                                  .WhereIf(!parentId.IsNullOrWhiteSpace(), c => c.ColumnId.ToString() == parentId)
                                  .WhereIf(!keyword.IsNullOrWhiteSpace(), c => c.Title.Contains(keyword))
                                  .Select(c => new
                                  {
                                      c.Id,
                                      c.Title,
                                      c.Published
                                  } as object);
            if (!sorting.IsNullOrWhiteSpace())
                query = query.OrderBy(sorting);
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<object>> GetAllByIdsAsync(params IEnumerable<object>[] ids)
        {
            var qry = from p in repository.GetAll()
                      join c in columnRepository.GetAll() on p.ColumnId equals c.Id into g
                      from pc in g.DefaultIfEmpty()
                      select new
                      {
                          p.Id,
                          p.Title,
                          p.ColumnId,
                          ColumnName = pc.DisplayName,
                          p.Published,
                          ColumnType = pc.ColumnType.BXJGCMSEnum()
                      };

            if (ids != null)
            {
                if (ids.Length == 1)
                {
                    var columnIds = ids[0];
                    qry = qry.Where(c => columnIds.Contains(c.ColumnId));
                }
                else if (ids.Length == 2)
                {
                    var columnIds = ids[1];
                    qry = qry.Where(c => columnIds.Contains(c.Id));
                }
            }

            return await qry.ToListAsync();
        }
    }
}
