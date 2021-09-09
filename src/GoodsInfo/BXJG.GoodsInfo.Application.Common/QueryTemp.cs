using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Common
{
    /// <summary>
    /// 便于自定义物品类型查询时使用join查询
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class QueryTemp<TEntity>
    {
        //public virtual GoodsInfoCategoryEntity Category { get; set; }//物品导航属性已经存在此属性
        public virtual TEntity GoodsInfo { get; set; }
    }
}
