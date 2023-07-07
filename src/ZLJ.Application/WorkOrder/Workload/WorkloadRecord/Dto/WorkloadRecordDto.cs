using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Admin.WorkOrder.Workload.WorkloadRecord.Dto
{
    public class WorkloadRecordDto : FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 规则积分
        /// </summary>
        public int RulePoints { get; set; }
    }
}
