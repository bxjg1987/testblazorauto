using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.CMS.Column
{
    /// <summary>
    /// 后台管理CMS栏目的编辑模型
    /// </summary>
    public class ColumnEditDto : GeneralTreeNodeEditBaseDto
    {
        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(BXJGCMSConsts.ColumnIconMaxLength)]
        public string Icon { get; set; }
        /// <summary>
        /// 栏目类型
        /// </summary>
        public ColumnType ColumnType { get; set; }
        /// <summary>
        /// 内容类型Id
        /// </summary>
        public long ContentTypeId { get; set; }
        /// <summary>
        /// 是否是系统预定的栏目，这些栏目不允许被删除
        /// </summary>
        public bool IsSysDefine { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        [StringLength(BXJGCMSConsts.ColumnSeoTitleMaxLength)]
        public string SeoTitle { get; set; }
        /// <summary>
        /// SEO描述信息
        /// </summary>
        [StringLength(BXJGCMSConsts.ColumnSeoDescriptionMaxLength)]
        public string SeoDescription { get; set; }
        /// <summary>
        /// seo关键字
        /// </summary>
        [StringLength(BXJGCMSConsts.ColumnSeoKeywordMaxLength)]
        public string SeoKeyword { get; set; }
        /// <summary>
        /// 栏目描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 列表页模板
        /// </summary>
        [StringLength(BXJGCMSConsts.ColumnListTemplateMaxLength)]
        public string ListTemplate { get; set; }
        /// <summary>
        /// 详情页模板
        /// </summary>
        [StringLength(BXJGCMSConsts.ColumnDetailTemplateMaxLength)]
        public string DetailTemplate { get; set; }
    }

}
