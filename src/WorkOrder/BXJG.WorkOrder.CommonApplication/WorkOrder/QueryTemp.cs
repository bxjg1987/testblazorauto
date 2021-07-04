using BXJG.WorkOrder.WorkOrderCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    //用元祖的话目前不支持动态排序
    public class QueryTemp<TEntity>
    {
        public TEntity Order { get; set; }
        public CategoryEntity Category { get; set; }
    }
}
