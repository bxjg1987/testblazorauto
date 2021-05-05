using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderType
{
    /// <summary>
    /// 工单类型提供器接口
    /// </summary>
    public interface IWorkOrderTypeProvider : ITransientDependency
    {
        public void Create(WorkOrderTypeProviderContext context);
    }
    /// <summary>
    /// 默认工单的类型注册实现
    /// </summary>
    public class WorkOrderTypeProvider : IWorkOrderTypeProvider
    {
        public void Create(WorkOrderTypeProviderContext context)
        {
            context.Defines.Add(new WorkOrderTypeDefine
            {
                Name = CoreConsts.DefaultWorkOrderTypeName,
                DisplayName = "普通工单".BXJGWorkOrderLI(),
            });
        }
    }
}
