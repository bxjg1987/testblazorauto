using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Article
{
    /// <summary>
    /// 后台管理文章时 列表页面获取文章列表时提供的参数模型
    /// </summary>
    public class GetAllArticleInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        /// <summary>
        /// 关键字
        /// Title/DescriptionShort/SKU等..模糊查询
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 所属栏目Id。栏目id和code二选一，推荐提供code
        /// </summary>
        public long? ColumnId { get; set; }
        /// <summary>
        /// 所属栏目的code。栏目id和code二选一，推荐提供code
        /// </summary>
        public string ColumnCode { get; set; }
        /// <summary>
        /// 是否已发布
        /// </summary>
        public bool? Published { get; set; }
        /// <summary>
        /// 发布开始时间，若为空则不限
        /// Published为true时才有效
        /// </summary>
        public DateTimeOffset? PublishStartTime { get; set; }
        /// <summary>
        /// 发布结束时间，若为空则不限
        /// Published为true时才有效
        /// </summary>
        public DateTimeOffset? PublishEndTime { get; set; }

        public void Normalize()
        {
            if (this.Sorting.IsNullOrEmpty())
                this.Sorting = "creationtime desc";
        }
    }
}
