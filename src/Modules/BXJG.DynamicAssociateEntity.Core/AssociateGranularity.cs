using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.DynamicAssociateEntity
{
    /// <summary>
    /// 关联粒度
    /// </summary>
    public enum AssociateGranularity
    {
        /// <summary>
        /// 所有行都关联到相同类型的目标实体
        /// </summary>
        Table,
        /// <summary>
        /// 每行数据都可能关联到不同类型的实体
        /// </summary>
        Row
    }
}
