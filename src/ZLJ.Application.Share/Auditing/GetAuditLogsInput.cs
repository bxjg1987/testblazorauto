using System;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Timing;

namespace ZLJ.Application.Share.Auditing.Dto
{
    /// <summary>
    /// 过滤查询审计日志时提供的条件输入模型
    /// </summary>
    public class GetAuditLogsInput : PagedAndSortedResultRequestDto, IShouldNormalize,IHaveKeywords
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        //public string? UserName { get; set; }

        //public string? ServiceName { get; set; }

        //public string? MethodName { get; set; }

        //public string? BrowserInfo { get; set; }

        public bool? HasException { get; set; }

        public int? MinExecutionDuration { get; set; }

        public int? MaxExecutionDuration { get; set; }
        public string? Keywords { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpaceBXJG())
                Sorting = "ExecutionTime desc";
        }
    }
}