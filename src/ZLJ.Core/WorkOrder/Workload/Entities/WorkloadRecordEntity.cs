using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo.StaffInfo;

namespace ZLJ.WorkOrder.Workload
{
    public class WorkloadRecordEntity : FullAuditedEntity<Guid>, IMustHaveTenant
    {
        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime StatisticsTime { get; set; }
        /// <summary>
        /// 规则积分
        /// </summary>
        public int RulePoints { get; set; }
        /// <summary>
        /// 实际积分
        /// </summary>
        public int ActualPoints { get; set; }

        /// <summary>
        /// 关联员工Id
        /// </summary>
        public virtual long? StaffInfoId { get; set; }
        [ForeignKey("StaffInfoId")]
        public virtual StaffInfoEntity StaffInfo { get; set; }
        public int TenantId { get; set; }
    }
}
