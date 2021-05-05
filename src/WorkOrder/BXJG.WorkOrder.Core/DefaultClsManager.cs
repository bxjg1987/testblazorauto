using Abp.Domain.Repositories;
using BXJG.WorkOrder.WorkOrderCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder
{
    public class DefaultClsManager : DomainServiceBase
    {
        private readonly IRepository<CategoryEntity, long> clsRepository;
        private readonly BXJGWorkOrderConfig config;

        public DefaultClsManager(IRepository<CategoryEntity, long> clsRepository, BXJGWorkOrderConfig config)
        {
            this.clsRepository = clsRepository;
            this.config = config;
        }

        public async Task<CategoryEntity> GetDefaultAsync(string workOrderType)
        {
            if (!config.WorkOrderTypes.ContainsKey(workOrderType))
                throw new ApplicationException("无效的工单类型");

            var query = clsRepository.GetAll()
                                     .Where(c => c.IsDefault)
                                     .Where(c => c.WorkOrderType == workOrderType || string.IsNullOrWhiteSpace(c.WorkOrderType));

            var list = await AsyncQueryableExecuter.ToListAsync(query);
            if (list.Count > 1)
                return list.Single(c => c.WorkOrderType != null);
            return list[0];
        }
    }
}
