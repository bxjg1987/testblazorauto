using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using BXJG.DynamicAssociateEntity;
using BXJG.Equipment.EquipmentInfo;
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
                                  .WhereIf(!keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(keyword));
            var total = await AsyncQueryableExecuter.CountAsync(query);
            if (!sorting.IsNullOrWhiteSpace())
                query = query.OrderBy(sorting);
            query= query.PageBy(skip, maxcount);
            var listEntity = await AsyncQueryableExecuter.ToListAsync(query);
            return new PagedResultDto<object>(total, listEntity);
        }

        public async Task<IEnumerable<object>> GetAllByIdsAsync(string parentId, params string[] ids)
        {
            var query = repository.GetAllIncluding(c => c.Area).Where(c => ids.Contains(c.Id.ToString()));
            var listEntity = await AsyncQueryableExecuter.ToListAsync(query);
            return listEntity;
        }

        public async Task<IEnumerable<string>> GetIdsByKeywordAsync(string parentId, string keyword)
        {
            var query = repository.GetAllIncluding(c => c.Area)
                           .WhereIf(!keyword.IsNullOrWhiteSpace(), c => c.Name.Contains(keyword))
                           .Select(c => c.Id.ToString());
            return await AsyncQueryableExecuter.ToListAsync(query);
        }
    }
}
