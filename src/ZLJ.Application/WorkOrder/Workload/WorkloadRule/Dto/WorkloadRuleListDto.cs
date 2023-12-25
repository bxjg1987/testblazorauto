using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Admin.BaseInfo.StaffInfo;
using ZLJ.WorkOrder.Workload;

namespace ZLJ.App.Admin.WorkOrder.Workload.Dto
{
    public class WorkloadRuleListDto : EntityDto, IHasCreationTime
    {
        public WorkloadType WorkloadType { get; set; }
        public WorkloadRuleType WorkloadRuleType { get; set; }
        public string RuleParams { get; set; }
        public string RuleParamsFormat { get; set; }
        public string RuleDesc { get; set; }
        public int Points { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
