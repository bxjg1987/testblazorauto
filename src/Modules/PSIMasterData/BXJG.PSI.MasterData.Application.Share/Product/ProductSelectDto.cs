using Abp.Application.Services.Dto;
using BXJG.Utils.Application.Share.GeneralTree;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 产品选择DTO
    /// </summary>
    public class ProductSelectDto : EntityDto<string>
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 商品规格型号
        /// </summary>
        public string Model { get; set; }
        
        /// <summary>
        /// 计量单位
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandDisplayName { get; set; }
        
        /// <summary>
        /// 商品类别名称
        /// </summary>
        public string CategoryDisplayName { get; set; }
        
        /// <summary>
        /// 是否是虚拟产品
        /// </summary>
        public bool IsVirtual { get; set; }
        
  
    }
}
