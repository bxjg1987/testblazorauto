using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder
{
    /// <summary>
    /// 工单模块配置对象
    /// </summary>
    public class BXJGWorkOrderConfig
    {
        /// <summary>
        /// 所有工单类型
        /// </summary>
        public IDictionary<string, string> WorkOrderTypes { get; set; } = new Dictionary<string, string>();
    }
}
