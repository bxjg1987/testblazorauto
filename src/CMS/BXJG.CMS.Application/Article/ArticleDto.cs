using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.CMS.Article
{
    /// <summary>
    /// 后台对文章进行管理时的列表页面用的模型
    /// </summary>
    public class ArticleDto : FullAuditedEntityDto<long>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 系统预设文章 不允许删除
        /// </summary>
        public bool IsSysDefine { get; set; }
        /// <summary>
        /// 所属栏目的Id
        /// </summary>
        public long ColumnId { get; set; }
        /// <summary>
        /// 所属栏目的名称
        /// </summary>
        public string ColumnDisplayName { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        public string SeoTitle { get; set; }
        /// <summary>
        /// SEO描述信息
        /// </summary>
        public string SeoDescription { get; set; }
        /// <summary>
        /// seo关键字
        /// </summary>
        public string SeoKeyword { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 是否已发布
        /// </summary>
        public bool Published { get; set; }
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
    }
}
