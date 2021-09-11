using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Admin
{
    /// <summary>
    /// 后台管理物品的条件对象
    /// 获取数量或分页数据时的输入模型
    /// </summary>
    public class GoodsInfoGetTotalInput
    {
        /// <summary>
        /// 类别code
        /// 将查询此类别及其后台类别下的所有物品
        /// </summary>
        public string CategoryCode { get; set; }
        /// <summary>
        /// 品牌id
        /// </summary>
        public string BrandId { get; set; }
        /// <summary>
        /// 关键字
        /// 模糊匹配：物品名、助记码、所属类别名称等
        /// </summary>
        public string Keywords { get; set; }
    }
}
