using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 产品查询条件
    /// </summary>
    public class ProductCondition : IHaveKeywords
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keywords { get; set; }
        
        /// <summary>
        /// 品牌ID
        /// </summary>
        public long? BrandId { get; set; }
        
        /// <summary>
        /// 商品类别ID
        /// </summary>
        public long? CategoryId { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// 是否是虚拟产品
        /// </summary>
        public bool? IsVirtual { get; set; }
    }
}
