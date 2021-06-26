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
    /// 维修人员工单获取列表页数据时的输入模型<br />
    /// 不同类型的工单可以提供相应子类，以便增加更多搜索条件
    /// </summary>
    public class GetAllWorkOrderBase1Input : GetTotalInput, IPagedAndSortedResultRequest, /*PagedAndSortedResultRequestDto,*/ IShouldNormalize
    {
        ///// <summary>
        ///// 获取数量时，控制查询处理人范围的方式
        ///// </summary>
        //public EmpType EmployeeType { get; set; }
        ///// <summary>
        ///// 处理人Id集合，参考<see cref="GetTotalInput.EmployeeIds"/>
        ///// </summary>
        //public IEnumerable<string> EmployeeIds { get; set; }
        ///// <summary>
        ///// 只包含在这几种状态内的工单
        ///// </summary>
        //public Status[] Statuses { get; set; }
        ///// <summary>
        ///// 只包含在这几种紧急程度内的工单
        ///// </summary>
        //public UrgencyDegree[] UrgencyDegrees { get; set; }
        ///// <summary>
        ///// 这包含这几种工单类别的
        ///// </summary>
        //public string[] CategoryCodes { get; set; }
        ///// <summary>
        ///// 预计开始时间范围-开始
        ///// </summary>
        //public DateTimeOffset? EstimatedExecutionTimeStart { get; set; }
        ///// <summary>
        ///// 预计结束时间范围-结束
        ///// </summary>
        //public DateTimeOffset? EstimatedExecutionTimeEnd { get; set; }
        ///// <summary>
        ///// 预计完成时间范围-开始
        ///// </summary>
        //public DateTimeOffset? EstimatedCompletionTimeStart { get; set; }
        ///// <summary>
        ///// 预计完成时间范围-结束
        ///// </summary>
        //public DateTimeOffset? EstimatedCompletionTimeEnd { get; set; }
        ///// <summary>
        ///// 实际开始时间-开始
        ///// </summary>
        //public DateTimeOffset? ExecutionTimeStart { get; set; }
        ///// <summary>
        ///// 实际开始时间-结束
        ///// </summary>
        //public DateTimeOffset? ExecutionTimeEnd { get; set; }
        ///// <summary>
        ///// 实际完成时间-开始
        ///// </summary>
        //public DateTimeOffset? CompletionTimeStart { get; set; }
        ///// <summary>
        ///// 实际完成实际-结束
        ///// </summary>
        //public DateTimeOffset? CompletionTimeEnd { get; set; }
        ///// <summary>
        ///// 关键字，模糊匹配处理人名称、电话、工单标题等
        ///// </summary>
        //public string Keyword { get; set; }
        public int SkipCount { get; set; }
        public int MaxResultCount { get; set; }
        public string Sorting { get; set; }

        /// <summary>
        /// 模型绑定后，abp会调用此方法来进一步初始化
        /// </summary>
        public virtual void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "Id desc"; //默认最后更新的用户倒叙，因为它可能发生了业务。或者最后登录用户也行
        }
    }
    /// <summary>
    /// 后台管理普通工单获取列表时的输入模型
    /// </summary>
    public class GetAllWorkOrderInput : GetAllWorkOrderBase1Input
    {
        ///// <summary>
        ///// 动态关联实体的限制条件<br />
        ///// key：动态关联数据类型，比如column对应栏目，value：对应的值。目前只考虑每种数据选一个
        ///// </summary>
        //public IDictionary<string, object> DynamicAssociateData { get; set; }
    }
}
