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
        public bool SystemDefine { get; set; }
    }
}
