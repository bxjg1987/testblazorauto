using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.ComponentModel.DataAnnotations;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 商品分类编辑DTO
    /// </summary>
    public class ProductCategoryEditDto : GeneralTreeNodeEditBaseDto
    {
        /// <summary>
        /// 分类编码
        /// </summary>
        [Required(ErrorMessage = "请输入分类编码")]
        public string Code { get; set; }
        
        // 以下字段已在基类中定义，不需要重复定义
        // - DisplayName
        // - Name
        // - ExtData（扩展字段，对应ExtensionData）
    }
}
