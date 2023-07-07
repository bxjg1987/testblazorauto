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
    /// <summary>
    /// 查询分页数据的输入模型
    /// </summary>
    /// <typeparam name="TGetTotal"></typeparam>
    public class GetAllInputBase<TGetTotal> : PagedAndSortedResultRequestDto, /*IGetTotalInputBase,*/ IShouldNormalize
        where TGetTotal : /*IGetTotalInputBase*/GetTotalInputBase, new()
    {
        /// <summary>
        /// 包含查询条件的对象，此对象的的熟悉将映射到当前类的同名属性
        /// 目的是确保当前输入模型扁平化
        /// </summary>
        protected internal readonly TGetTotal GetTotalInput = new TGetTotal();

        /// <summary>
        /// 只查询属于这几种类别的工单
        /// </summary>
        public virtual string[] CategoryCodes { get => ((GetTotalInputBase)GetTotalInput).CategoryCodes; set => ((GetTotalInputBase)GetTotalInput).CategoryCodes = value; }
        /// <summary>
        /// 完成时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeEnd { get => ((GetTotalInputBase)GetTotalInput).CompletionTimeEnd; set => ((GetTotalInputBase)GetTotalInput).CompletionTimeEnd = value; }
        /// <summary>
        /// 完成时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeStart { get => ((GetTotalInputBase)GetTotalInput).CompletionTimeStart; set => ((GetTotalInputBase)GetTotalInput).CompletionTimeStart = value; }
        /// <summary>
        /// 预计开始时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTimeEnd { get => ((GetTotalInputBase)GetTotalInput).EstimatedCompletionTimeEnd; set => ((GetTotalInputBase)GetTotalInput).EstimatedCompletionTimeEnd = value; }
        /// <summary>
        /// 预计结束时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTimeStart { get => ((GetTotalInputBase)GetTotalInput).EstimatedCompletionTimeStart; set => ((GetTotalInputBase)GetTotalInput).EstimatedCompletionTimeStart = value; }
        /// <summary>
        /// 预计完成时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? EstimatedExecutionTimeEnd { get => ((GetTotalInputBase)GetTotalInput).EstimatedExecutionTimeEnd; set => ((GetTotalInputBase)GetTotalInput).EstimatedExecutionTimeEnd = value; }
        /// <summary>
        /// 预计完成时间范围-开始
        /// </summary>
        public virtual DateTimeOffset? EstimatedExecutionTimeStart { get => ((GetTotalInputBase)GetTotalInput).EstimatedExecutionTimeStart; set => ((GetTotalInputBase)GetTotalInput).EstimatedExecutionTimeStart = value; }
        /// <summary>
        /// 实际开始时间-结束
        /// </summary>
        public virtual DateTimeOffset? ExecutionTimeEnd { get => ((GetTotalInputBase)GetTotalInput).ExecutionTimeEnd; set => ((GetTotalInputBase)GetTotalInput).ExecutionTimeEnd = value; }
        /// <summary>
        /// 实际开始时间-开始
        /// </summary>
        public virtual DateTimeOffset? ExecutionTimeStart { get => ((GetTotalInputBase)GetTotalInput).ExecutionTimeStart; set => ((GetTotalInputBase)GetTotalInput).ExecutionTimeStart = value; }
        /// <summary>
        /// 关键字，模糊匹配处理人名称、电话、工单标题等
        /// </summary>
        public virtual string Keyword { get => ((GetTotalInputBase)GetTotalInput).Keyword; set => ((GetTotalInputBase)GetTotalInput).Keyword = value; }
        /// <summary>
        /// 只查询包含这几种状态的工单
        /// </summary>
        public virtual Status[] Statuses { get => ((GetTotalInputBase)GetTotalInput).Statuses; set => ((GetTotalInputBase)GetTotalInput).Statuses = value; }
        /// <summary>
        /// 只查询包含这几种紧急程度的工单
        /// </summary>
        public virtual UrgencyDegree[] UrgencyDegrees { get => ((GetTotalInputBase)GetTotalInput).UrgencyDegrees; set => ((GetTotalInputBase)GetTotalInput).UrgencyDegrees = value; }

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
