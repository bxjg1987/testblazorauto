using Abp.Application.Services.Dto;
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
    public class DynamicAssociateEntityEquipmentInfoService : IDynamicAssociateEntityService, IDynamicAssociateEntityService2
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
            query = query.OrderBy(sorting).PageBy(skip, maxcount);
            var listEntity = await AsyncQueryableExecuter.ToListAsync(query);

            //var list = new List<Dictionary<string, object>>();

            //listEntity.ForEach(c =>
            //{
            //    var dic = new Dictionary<string, object>();
            //    foreach (var item in defines.Fields)
            //    {
            //        dic[item.Name] = t.GetProperty(item.Name).GetValue(c);
            //    }
            //    list.Add(dic);
            //});
            //return new PagedResultDto<IDictionary<string, object>>(total, list);
            return new PagedResultDto<object>(total, listEntity);
        }

        public async Task<IEnumerable<object>> GetAllByIdsAsync(string parentId, params string[] ids)
        {
            var query = repository.GetAllIncluding(c => c.Area).Where(c => ids.Contains(c.Id.ToString()));
            var listEntity = await AsyncQueryableExecuter.ToListAsync(query);
            return listEntity;
            //  var list = new List<Dictionary<string, object>>();

            //listEntity.ForEach(c =>
            //{
            //    var dic = new Dictionary<string, object>();
            //    foreach (var item in defines.Fields)
            //    {
            //        dic[item.Name] = t.GetProperty(item.Name).GetValue(c);
            //    }
            //    list.Add(dic);
            //});
            //return list;
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
