using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using BXJG.DynamicAssociateEntity;
using BXJG.Equipment.EquipmentInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Equipment.DynamicAssociateEntity
{
    public class DynamicAssociateEntityEquipmentInfoService : IDynamicAssociateEntityService
    {
        private readonly IRepository<EquipmentInfoEntity, long> repository;
        protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        public DynamicAssociateEntityEquipmentInfoService(IRepository<EquipmentInfoEntity, long> repository)
        {
            this.repository = repository;
        }
        public async Task<PagedResultDto<object>> GetAllAsync(string parentId, string keyword, string sorting, int skip, int maxcount)
        {
            var query = repository.GetAllIncluding(c => c.Area)
                                  .WhereIf(!keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(keyword) || c.HardwareCode.Contains(keyword))
                                  .Select(c => new
                                  {
                                      c.Id,c.Name,c.HardwareCode
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
            var query = repository.GetAllIncluding(c => c.Area)
                                    .WhereIf(!keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(keyword) || c.HardwareCode.Contains(keyword))
                                    .Select(c => new
                                    {
                                        c.Id,
                                        c.Name,
                                        c.HardwareCode
                                    } as object);
            if (!sorting.IsNullOrWhiteSpace())
                query = query.OrderBy(sorting);
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<object>> GetAllByIdsAsync(params IEnumerable<object>[] ids)
        {
            var qry = from c in repository.GetAll()
                      select new
                      {
                          c.Id,
                          c.Name,
                          c.HardwareCode
                      };
            var columnIds = ids.SingleOrDefault();
            if (columnIds != null)
                qry = qry.Where(c => columnIds.Contains(c.Id));
            return await qry.ToListAsync();
        }
    }
}
