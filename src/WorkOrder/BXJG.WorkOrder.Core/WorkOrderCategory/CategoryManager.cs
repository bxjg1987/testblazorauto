using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    /// <summary>
    /// 工单分类领域服务
    /// </summary>
    public class CategoryManager : GeneralTreeManager<CategoryEntity>
    {
        public CategoryManager(IRepository<CategoryEntity, long> repository) : base(repository)
        {
        }
    }
}
