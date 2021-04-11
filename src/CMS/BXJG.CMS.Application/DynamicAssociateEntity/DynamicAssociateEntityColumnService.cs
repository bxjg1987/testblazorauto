using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using BXJG.CMS.Column;
using BXJG.CMS.Localization;
using BXJG.DynamicAssociateEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.CMS.DynamicAssociateEntity
{
    public class DynamicAssociateEntityColumnService : IDynamicAssociateEntityService
    {
        private readonly IRepository<ColumnEntity, long> repository;
        protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        public DynamicAssociateEntityColumnService(IRepository<ColumnEntity, long> repository)
        {
            this.repository = repository;
        }

        public async Task<PagedResultDto<object>> GetAllAsync(string parentId, string keyword, string sorting, int skip, int maxcount)
        {
            var query = repository.GetAll()
                                  .WhereIf(!keyword.IsNullOrWhiteSpace(), c => c.DisplayName.Contains(keyword))
                                  .Select(c => new
                                  {
                                      columnId = c.Id,
                                      columnName = c.DisplayName,
                                      columnType = c.ColumnType.BXJGCMSEnum()
                                  } as object);
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
                                  .WhereIf(!keyword.IsNullOrWhiteSpace(), c => c.DisplayName.Contains(keyword))
                                  .Select(c => new
                                  {
                                      columnId = c.Id,
                                      columnName = c.DisplayName,
                                      columnType = c.ColumnType.BXJGCMSEnum()
                                  } as object);
            if (!sorting.IsNullOrWhiteSpace())
                query = query.OrderBy(sorting);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<IdSortDto>> GetIdsAndSortValuesAsync(string sort = default, string keyword = default, params IEnumerable<object>[] ids)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<object>> GetAllByIdsAsync(IEnumerable<object> ids)
        {
            var qry = from p in repository.GetAll()
                      where ids.Contains(p.Id)
                      select new
                      {
                          ColumnId = p.Id,
                          ColumnName = p.DisplayName,
                          columnType = p.ColumnType.BXJGCMSEnum()
                      };
            return await qry.ToListAsync();
        }
    }
}
