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
        /// <summary>
        /// 处理人Id
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 状态
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
        /// <summary>
        /// 预计开始时间范围-开始
        /// </summary>
        public DateTimeOffset? EstimatedExecutionTimeStart { get; set; }
        /// <summary>
        /// 预计结束时间范围-结束
        /// </summary>
        public DateTimeOffset? EstimatedExecutionTimeEnd { get; set; }
        /// <summary>
        /// 预计完成时间范围-开始
        /// </summary>
        public DateTimeOffset? EstimatedCompletionTimeStart { get; set; }
        /// <summary>
        /// 预计完成时间范围-结束
        /// </summary>
        public DateTimeOffset? EstimatedCompletionTimeEnd { get; set; }
        /// <summary>
        /// 实际开始时间-开始
        /// </summary>
        public DateTimeOffset? ExecutionTimeStart { get; set; }
        /// <summary>
        /// 实际开始时间-结束
        /// </summary>
        public DateTimeOffset? ExecutionTimeEnd { get; set; }
        /// <summary>
        /// 实际完成时间-开始
        /// </summary>
        public DateTimeOffset? CompletionTimeStart { get; set; }
        /// <summary>
        /// 实际完成实际-结束
        /// </summary>
        public DateTimeOffset? CompletionTimeEnd { get; set; }
        /// <summary>
        /// 关键字，模糊匹配处理人名称、电话、工单标题等
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
    /// <summary>
    /// 后台管理普通工单获取列表时的输入模型
    /// </summary>
    public class GetAllWorkOrderInput : GetAllWorkOrderBaseInput
    {
        ///// <summary>
        ///// 动态关联实体的限制条件<br />
        ///// key：动态关联数据类型，比如column对应栏目，value：对应的值。目前只考虑每种数据选一个
        ///// </summary>
        //public IDictionary<string, object> DynamicAssociateData { get; set; }
    }
}
