using BXJG.WorkOrder.WorkOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 获取工单数量时的输入模型
    /// </summary>
    public class GetTotalInputBase : IGetTotalInputBase
    {
        /// <summary>
        /// 处理人Id
        /// </summary>
        public virtual string EmployeeId { get; set; }
        /// <summary>
        /// 只包含在这几种状态内的工单
        /// </summary>
        public virtual Status[] Statuses { get; set; }
        /// <summary>
        /// 只包含在这几种紧急程度内的工单
        /// </summary>
        public virtual UrgencyDegree[] UrgencyDegrees { get; set; }
        /// <summary>
        /// 这包含这几种工单类别的
        /// </summary>
        public virtual string[] CategoryCodes { get; set; }
        /// <summary>
        /// 预计开始时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? EstimatedExecutionTimeStart { get; set; }
        /// <summary>
        /// 预计结束时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? EstimatedExecutionTimeEnd { get; set; }
        /// <summary>
        /// 预计完成时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTimeStart { get; set; }
        /// <summary>
        /// 预计完成时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTimeEnd { get; set; }
        /// <summary>
        /// 实际开始时间-开始
        /// </summary>
        public virtual DateTimeOffset? ExecutionTimeStart { get; set; }
        /// <summary>
        /// 实际开始时间-结束
        /// </summary>
        public virtual DateTimeOffset? ExecutionTimeEnd { get; set; }
        /// <summary>
        /// 实际完成时间-开始
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeStart { get; set; }
        /// <summary>
        /// 实际完成实际-结束
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeEnd { get; set; }
        /// <summary>
        /// 关键字，模糊匹配处理人名称、电话、工单标题等
        /// </summary>
        public virtual string Keyword { get; set; }
    }


}
