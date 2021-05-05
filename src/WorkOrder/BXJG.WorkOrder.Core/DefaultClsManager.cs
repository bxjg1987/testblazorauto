using Abp.Domain.Repositories;
using BXJG.WorkOrder.WorkOrderCategory;
using BXJG.WorkOrder.WorkOrderType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder
{
    /// <summary>
    /// 根据类型获取默认工单类别
    /// </summary>
    public class DefaultClsManager : DomainServiceBase
    {
        private readonly IRepository<CategoryEntity, long> clsRepository;
        private readonly WorkOrderTypeManager config;

        public DefaultClsManager(IRepository<CategoryEntity, long> clsRepository, WorkOrderTypeManager config)
        {
            this.clsRepository = clsRepository;
            this.config = config;
        }

        public async Task<CategoryEntity> GetDefaultAsync(string workOrderType)
        {
            config.Check(workOrderType);

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
