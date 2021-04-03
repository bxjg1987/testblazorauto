using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 工单后台管理更新模型<br />
    /// 不同工单类型有相应子类
    /// </summary>
    public class WorkOrderUpdateBaseInput : EntityDto<long>
    {
        /// <summary>
        /// 所属分类id
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public Status? Status { get; set; }
        /// <summary>
        /// 紧急程度
        /// </summary>
        public UrgencyDegree? UrgencyDegree { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(CoreConsts.OrderTitleMaxLength)]
        public string Title { get; set; }
        /// <summary>
        /// 内容描述
        /// </summary>
        [StringLength(CoreConsts.OrderDescriptionMaxLength)]
        public string Description { get; set; }
        /// <summary>
        /// 当前状态情况说明
        /// </summary>
        [StringLength(CoreConsts.OrderStatusChangedDescriptionMaxLength)]
        public string StatusChangedDescription { get; set; }
        /// <summary>
        /// 预计开始时间
        /// </summary>
        public DateTimeOffset? EstimatedExecutionTime { get; set; }
        /// <summary>
        /// 预计结束时间
        /// </summary>
        public DateTimeOffset? EstimatedCompletionTime { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTimeOffset? ExecutionTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTimeOffset? CompletionTime { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public string EmployeeId { get; set; }
    }
    /// <summary>
    /// 工单后台管理普通工单更新模型
    /// </summary>
    public class WorkOrderUpdateInput : WorkOrderUpdateBaseInput
    {
        ///// <summary>
        ///// 实体Id
        ///// </summary>
        //public string EntityId { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public Dictionary<string, object> ExtensionData { get; set; }
        /// <summary>
        /// 预留字段1
        /// </summary>
        public string ExtendedField1 { get; set; }
        /// <summary>
        /// 预留字段2
        /// </summary>
        public string ExtendedField2 { get; set; }
        /// <summary>
        /// 预留字段3
        /// </summary>
        public string ExtendedField3 { get; set; }
        /// <summary>
        /// 预留字段4
        /// </summary>
        public string ExtendedField4 { get; set; }
        /// <summary>
        /// 预留字段5
        /// </summary>
        public string ExtendedField5 { get; set; }
    }
}
