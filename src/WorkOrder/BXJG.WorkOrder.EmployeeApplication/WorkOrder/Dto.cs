using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 工单后台管理列表页使用的显示模型<br />
    /// 不同工单类型有相应子类
    /// </summary>
    public class WorkOrderEmployeeDtoBase : EntityDto<long>
    {
        /// <summary>
        /// 所属分类id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 所属分类名称
        /// </summary>
        public string CategoryDisplayName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusDisplayName => Status.BXJGWorkOrderEnum();
        /// <summary>
        /// 紧急程度
        /// </summary>
        public UrgencyDegree UrgencyDegree { get; set; }
        /// <summary>
        /// 紧急程度名称
        /// </summary>
        public string UrgencyDegreeDisplayName => UrgencyDegree.BXJGWorkOrderEnum();
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 当前状态情况说明
        /// </summary>
        public string StatusChangedDescription { get; set; }
        /// <summary>
        /// 变成当前状态的时间
        /// </summary>
        public DateTimeOffset StatusChangedTime { get; set; }
        /// <summary>
        /// 希望的开始时间
        /// </summary>
        public DateTimeOffset? EstimatedExecutionTime { get; set; }
        /// <summary>
        /// 希望的结束时间
        /// </summary>
        public DateTimeOffset? EstimatedCompletionTime { get; set; }
        /// <summary>
        /// 实际的执行时间
        /// </summary>
        public DateTimeOffset? ExecutionTime { get; set; }
        /// <summary>
        /// 实际的结束时间
        /// </summary>
        public DateTimeOffset? CompletionTime { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 员工手机号
        /// </summary>
        public string EmployeePhone { get; set; }
        
    }
    /// <summary>
    /// 后台管理普通工单查询模型
    /// </summary>
    public class WorkOrderEmployeeDto : WorkOrderEmployeeDtoBase
    {
        /// <summary>
        /// 扩展字段
        /// </summary>
        public dynamic ExtensionData { get; set; }
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
