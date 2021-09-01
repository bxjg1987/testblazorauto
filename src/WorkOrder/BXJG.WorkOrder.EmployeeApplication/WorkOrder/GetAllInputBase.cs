using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using BXJG.WorkOrder.WorkOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.EmployeeApplication.WorkOrder
{
    public class GetAllInputBase<TGetTotal> : PagedAndSortedResultRequestDto, IGetTotalInputBase, IShouldNormalize
        where TGetTotal : IGetTotalInputBase, new()
    {
        protected readonly TGetTotal GetTotalInput = new TGetTotal();

        /// <summary>
        /// 只查询属于这几种类别的工单
        /// </summary>
        public virtual string[] CategoryCodes { get => ((IGetTotalInputBase)GetTotalInput).CategoryCodes; set => ((IGetTotalInputBase)GetTotalInput).CategoryCodes = value; }
        /// <summary>
        /// 完成时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeEnd { get => ((IGetTotalInputBase)GetTotalInput).CompletionTimeEnd; set => ((IGetTotalInputBase)GetTotalInput).CompletionTimeEnd = value; }
        /// <summary>
        /// 完成时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeStart { get => ((IGetTotalInputBase)GetTotalInput).CompletionTimeStart; set => ((IGetTotalInputBase)GetTotalInput).CompletionTimeStart = value; }
        /// <summary>
        /// 预计开始时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTimeEnd { get => ((IGetTotalInputBase)GetTotalInput).EstimatedCompletionTimeEnd; set => ((IGetTotalInputBase)GetTotalInput).EstimatedCompletionTimeEnd = value; }
        /// <summary>
        /// 预计结束时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTimeStart { get => ((IGetTotalInputBase)GetTotalInput).EstimatedCompletionTimeStart; set => ((IGetTotalInputBase)GetTotalInput).EstimatedCompletionTimeStart = value; }
        /// <summary>
        /// 预计完成时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? EstimatedExecutionTimeEnd { get => ((IGetTotalInputBase)GetTotalInput).EstimatedExecutionTimeEnd; set => ((IGetTotalInputBase)GetTotalInput).EstimatedExecutionTimeEnd = value; }
        /// <summary>
        /// 预计完成时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? EstimatedExecutionTimeStart { get => ((IGetTotalInputBase)GetTotalInput).EstimatedExecutionTimeStart; set => ((IGetTotalInputBase)GetTotalInput).EstimatedExecutionTimeStart = value; }
        /// <summary>
        /// 实际开始时间-结束
        /// </summary>
        public virtual DateTimeOffset? ExecutionTimeEnd { get => ((IGetTotalInputBase)GetTotalInput).ExecutionTimeEnd; set => ((IGetTotalInputBase)GetTotalInput).ExecutionTimeEnd = value; }
        /// <summary>
        /// 实际开始时间-开始
        /// </summary>
        public virtual DateTimeOffset? ExecutionTimeStart { get => ((IGetTotalInputBase)GetTotalInput).ExecutionTimeStart; set => ((IGetTotalInputBase)GetTotalInput).ExecutionTimeStart = value; }
        /// <summary>
        /// 关键字，模糊匹配处理人名称、电话、工单标题等
        /// </summary>
        public virtual string Keyword { get => ((IGetTotalInputBase)GetTotalInput).Keyword; set => ((IGetTotalInputBase)GetTotalInput).Keyword = value; }
        /// <summary>
        /// 只查询包含这几种状态的工单
        /// </summary>
        public virtual Status[] Statuses { get => ((IGetTotalInputBase)GetTotalInput).Statuses; set => ((IGetTotalInputBase)GetTotalInput).Statuses = value; }
        /// <summary>
        /// 只查询包含这几种紧急程度的工单
        /// </summary>
        public virtual UrgencyDegree[] UrgencyDegrees { get => ((IGetTotalInputBase)GetTotalInput).UrgencyDegrees; set => ((IGetTotalInputBase)GetTotalInput).UrgencyDegrees = value; }

        public virtual void Normalize()
        {
            //if (GetTotalInput == null)
            //    GetTotalInput 

            if (Sorting.IsNullOrEmpty())
                Sorting = "order.creationtime desc"; //默认最后更新的用户倒叙，因为它可能发生了业务。或者最后登录用户也行

            //if (!Sorting.StartsWith("category", StringComparison.OrdinalIgnoreCase))
            //    Sorting = "order." + Sorting;
            //else
            //    Sorting = Sorting.Replace("category", "category.", StringComparison.OrdinalIgnoreCase);
        }
    }
}
