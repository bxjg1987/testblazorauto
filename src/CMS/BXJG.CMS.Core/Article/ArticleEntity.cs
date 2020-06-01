using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Article
{
    /// <summary>
    /// 文章实体类
    /// </summary>
    public class ArticleEntity : FullAuditedEntity<long>, IMustHaveTenant
    {
        public const int TitleMaxLength = 500;

        public int TenantId { get; set; }
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
        public bool SystemDefine { get; set; }
    }
}
