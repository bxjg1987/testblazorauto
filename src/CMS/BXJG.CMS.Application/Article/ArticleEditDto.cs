using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.CMS.Article
{
    /// <summary>
    /// 后台新增或修改文章时前端提交的数据模型
    /// </summary>
    public class ArticleEditDto : EntityDto<long>
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(ArticleEntity.TitleMaxLength)]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        public string Content { get; set; }
    }
}
