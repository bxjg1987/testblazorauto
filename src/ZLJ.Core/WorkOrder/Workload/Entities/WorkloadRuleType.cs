using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrder.Workload
{
    /// <summary>
    /// 工作量规则类型
    /// </summary>
    public enum WorkloadRuleType
    {
        /// <summary>
        /// 按工龄
        /// </summary>
        [Description("按工龄")]
        ByWorkYears,
        /// <summary>
        /// 按个人
        /// </summary>
        [Description("按个人")]
        ByPersonal
    }
}
