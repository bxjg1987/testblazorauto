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
        /// 是否开启普通工单功能，默认true
        /// </summary>
        public bool EnableDefaultWorkOrder { get; set; } = true;
    }
}
