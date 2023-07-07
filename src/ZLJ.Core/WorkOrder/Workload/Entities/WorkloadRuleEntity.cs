using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrder.Workload
{
    public class WorkloadRuleEntity : FullAuditedEntity, IMustHaveTenant
    {
        /// <summary>
        /// 工作量类型
        /// </summary>
        public WorkloadType WorkloadType { get; set; }
        /// <summary>
        /// 工作量规则类型
        /// </summary>
        public WorkloadRuleType WorkloadRuleType { get; set; }
        /// <summary>
        /// 规则参数
        /// </summary>
        public string RuleParams { get; set; }
        /// <summary>
        /// 规则描述
        /// </summary>
        public string RuleDesc { get; set; }
        /// <summary>
        /// 规则积分
        /// </summary>
        public int Points { get; set; }
        public int TenantId { get; set; }
    }
}
