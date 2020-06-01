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


        public void Normalize()
        {
            if (this.Sorting.IsNullOrEmpty())
                this.Sorting = "creationtime desc";
        }
    }
}
