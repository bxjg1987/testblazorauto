using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 工单操作日志
    /// </summary>
    public class OperationLogDto
    {
        /// <summary>
        /// 被操作的实体类型
        /// </summary>
        public string EntityTypeFullName { get; set; }
        /// <summary>
        /// 被操作的实体的id
        /// </summary>
        public object EntityId { get; set; }
        /// <summary>
        /// 工单处理人
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 处理人姓名
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset OperationTime { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
    }
}
