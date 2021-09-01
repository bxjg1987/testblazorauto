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
    /*
     * c#中GetAllInputBase<TGetTotal>无法继承TGetTotal
     * 但有希望保证输入参数模型扁平化，使用目前的方式实现
     * 使用泛型的GetTotalInput属性实现IGetTotalInputBase接口，来简化GetAllInputBase<TGetTotal>对IGetTotalInputBase的实现
     */

    /// <summary>
    /// 后台管理工单获取列表页数据时的输入模型<br />
    /// 不同类型的工单可以提供相应子类
    /// </summary>
    public class GetAllInputBase<TGetTotal> : PagedAndSortedResultRequestDto, IGetTotalInputBase, IShouldNormalize
        where TGetTotal : IGetTotalInputBase, new()
    {
        protected readonly TGetTotal GetTotalInput = new TGetTotal();
        /// <summary>
        /// 这包含这几种工单类别的
        /// </summary>
        public virtual string[] CategoryCodes { get => ((IGetTotalInputBase)GetTotalInput).CategoryCodes; set => ((IGetTotalInputBase)GetTotalInput).CategoryCodes = value; }
        /// <summary>
        /// 实际完成实际-结束
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeEnd { get => ((IGetTotalInputBase)GetTotalInput).CompletionTimeEnd; set => ((IGetTotalInputBase)GetTotalInput).CompletionTimeEnd = value; }
        /// <summary>
        /// 实际完成时间-开始
        /// </summary>
        public virtual DateTimeOffset? CompletionTimeStart { get => ((IGetTotalInputBase)GetTotalInput).CompletionTimeStart; set => ((IGetTotalInputBase)GetTotalInput).CompletionTimeStart = value; }
        /// <summary>
        /// 处理人Id
        /// </summary>
        public virtual string EmployeeId { get => ((IGetTotalInputBase)GetTotalInput).EmployeeId; set => ((IGetTotalInputBase)GetTotalInput).EmployeeId = value; }
        /// <summary>
        /// 预计结束时间范围-结束
        /// </summary>
        public virtual DateTimeOffset? EstimatedCompletionTimeEnd { get => ((IGetTotalInputBase)GetTotalInput).EstimatedCompletionTimeEnd; set => ((IGetTotalInputBase)GetTotalInput).EstimatedCompletionTimeEnd = value; }
        /// <summary>
        /// 预计开始时间范围-开始
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
        /// 只包含在这几种状态内的工单
        /// </summary>
        public virtual Status[] Statuses { get => ((IGetTotalInputBase)GetTotalInput).Statuses; set => ((IGetTotalInputBase)GetTotalInput).Statuses = value; }
        /// <summary>
        /// 只包含在这几种紧急程度内的工单
        /// </summary>
        public virtual UrgencyDegree[] UrgencyDegrees { get => ((IGetTotalInputBase)GetTotalInput).UrgencyDegrees; set => ((IGetTotalInputBase)GetTotalInput).UrgencyDegrees = value; }

        /// <summary>
        /// 模型绑定后，abp会调用此方法来进一步初始化
        /// </summary>
        public virtual void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "order.creationtime desc"; //默认最后更新的用户倒叙，因为它可能发生了业务。或者最后登录用户也行
        }
    }
}
