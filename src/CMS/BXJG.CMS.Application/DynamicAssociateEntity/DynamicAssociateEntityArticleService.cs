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
using Abp.Domain.Uow;

namespace BXJG.CMS.DynamicAssociateEntity
{
    [UnitOfWork(false)]
    public class DynamicAssociateEntityArticleService : IDynamicAssociateEntityService
    {
        private readonly IRepository<ArticleEntity, long> repository;
        private readonly IRepository<Column.ColumnEntity, long> columnRepository;
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
            var total = await query.CountAsync();
            if (!sorting.IsNullOrWhiteSpace())
                query = query.OrderBy(sorting);
            query = query.PageBy(skip, maxcount);
            var listEntity = await query.ToListAsync();
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

        public async Task<IEnumerable<IdSortDto>> GetIdsAndSortValuesAsync(string sort = default, string keyword = default, params IEnumerable<object>[] ids)
        {
            var qry = from p in repository.GetAll()
                      join c in columnRepository.GetAll() on p.ColumnId equals c.Id into g
                      from pc in g.DefaultIfEmpty()
                      select new { p, pc };

            qry = qry.WhereIf(!keyword.IsNullOrWhiteSpace(), c => c.p.Title.Contains(keyword) || c.pc.DisplayName.Contains(keyword));
            if (ids != null)
            {
                if (ids.Length >0)
                {
                    var xz = ids[0];
                    qry.Where(c => xz.Contains(c.p.ColumnId));
                }
                if (ids.Length >1)
                {
                    var xz = ids[1];
                    qry.Where(c => xz.Contains(c.p.Id));
                }
            }
            return sort switch
            {
                "title" => await qry.Select(c => new IdSortDto { Id = c.p.Id, SortValue = c.p.Title }).ToListAsync(),
                "columnName" => await qry.Select(c => new IdSortDto { Id = c.p.Id, SortValue = c.pc.DisplayName }).ToListAsync(),
                _ => await qry.Select(c => new IdSortDto { Id = c.p.Id }).ToListAsync(),
            };
        }
        public async Task<IEnumerable<object>> GetAllByIdsAsync(IEnumerable<object> ids)
        {
            var qry = from p in repository.GetAll()
                      where ids.Contains(p.Id)
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
            return await qry.ToListAsync();
        }
    }
}