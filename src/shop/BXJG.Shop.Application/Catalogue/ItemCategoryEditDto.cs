using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 商品分类的后台管理的编辑
    /// </summary>
    public class ItemCategoryEditDto : GeneralTreeNodeEditBaseDto
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 图片1
        /// </summary>
        public string Image1 { get; set; }
        /// <summary>
        /// 图片2
        /// </summary>
        public string Image2 { get; set; }
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        public bool ShowInHome { get; set; }
    }

}
