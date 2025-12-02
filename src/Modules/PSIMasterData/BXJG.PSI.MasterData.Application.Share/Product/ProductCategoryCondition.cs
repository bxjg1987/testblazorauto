using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI.MasterData.Application.Share.Product
{
    /// <summary>
    /// 产品分类查询条件
    /// </summary>
    public class ProductCategoryCondition : GeneralTreeGetTreeInput, IHaveKeywords
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keywords { get; set; }
    }
}
