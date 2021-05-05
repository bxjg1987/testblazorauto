using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderType
{
    /// <summary>
    /// 应用启动阶段将调用多个模块的IWorkOrderTypeProvider以初始化工单类型定义，此上下文对象在多个提供器执行期间共享数据
    /// </summary>
    public class WorkOrderTypeProviderContext
    {
        /// <summary>
        /// 工单类型定义集合
        /// </summary>
        public readonly IList<WorkOrderTypeDefine> Defines  = new List<WorkOrderTypeDefine>();
    }
}
