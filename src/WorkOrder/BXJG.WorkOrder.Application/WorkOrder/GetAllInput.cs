using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 后台管理工单获取列表页数据时的输入模型<br />
    /// 不同类型的工单可以提供相应子类
    /// </summary>
    public class GetAllWorkOrderBaseInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string EmployeeId { get; set; }
        /// <summary>
        /// 紧急程度
        /// </summary>
        public Status? Status { get; set; }
        /// <summary>
        /// 紧急程度
        /// </summary>
        public UrgencyDegree? UrgencyDegree { get; set; }
        /// <summary>
        /// 所属分类code
        /// </summary>
        public string CategoryCode { get; set; }
        public DateTimeOffset? EstimatedExecutionTimeStart { get; set; }
        public DateTimeOffset? EstimatedExecutionTimeEnd { get; set; }
        public DateTimeOffset? EstimatedCompletionTimeStart { get; set; }
        public DateTimeOffset? EstimatedCompletionTimeEnd { get; set; }
        public DateTimeOffset? ExecutionTimeStart { get; set; }
        public DateTimeOffset? ExecutionTimeEnd { get; set; }
        public DateTimeOffset? CompletionTimeStart { get; set; }
        public DateTimeOffset? CompletionTimeEnd { get; set; }
        //public string EmployeeId { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 模型绑定后，abp会调用此方法来进一步初始化
        /// </summary>
        public virtual void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "Title desc"; //默认最后更新的用户倒叙，因为它可能发生了业务。或者最后登录用户也行
        }
    }
}
