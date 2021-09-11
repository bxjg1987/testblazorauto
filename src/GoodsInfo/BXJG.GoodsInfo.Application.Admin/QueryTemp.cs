using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Admin
{
    /// <summary>
    /// ef查询时返回的临时类型
    /// 之所以在Entity基础上包一层是为了便于自定义物品类型查询时使用join查询
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class QueryTemp<TEntity>
    {
        //public virtual GoodsInfoCategoryEntity Category { get; set; }//物品导航属性已经存在此属性
        public virtual TEntity Entity { get; set; }
    }
}
