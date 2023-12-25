using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.WorkOrder.Workload;

namespace ZLJ.App.Admin.WorkOrder.Workload.Dto
{
    public class WorkloadUpdateDto : EntityDto
    {
        public WorkloadRuleType WorkloadRuleType { get; set; }
        public string RuleParams { get; set; }
        public string RuleDesc { get; set; }
        public int Points { get; set; }
    }
}
