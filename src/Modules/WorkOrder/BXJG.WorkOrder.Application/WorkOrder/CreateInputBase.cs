using Abp.Application.Services.Dto;
using BXJG.Utils.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 后台管理工单新增模型基类
    /// </summary>
    public class CreateInputBase : UpdateInputBase
    {
        /// <summary>
        /// 状态
        /// </summary>
        public Status? Status { get; set; }
        /// <summary>
        /// 调整状态的时间
        /// </summary>
        public DateTimeOffset? StatusChangedTime { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTimeOffset? ExecutionTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTimeOffset? CompletionTime { get; set; }
    }
    /// <summary>
    /// 工单后台管理编辑模型基类
    /// </summary>
    public class WorkOrderCreateInput : CreateInputBase
    {
        /// <summary>
        /// 扩展字段
        /// </summary>
        public IDictionary<string, object> ExtensionData { get; set; }
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
