using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Core.Share
{
    [Flags]
    public enum EquipmentComponentCategory
    {
        /// <summary>
        /// 碳粉盒
        /// </summary>
        TFH = 1,
        /// <summary>
        /// 定影组件
        /// </summary>
        DYZJ,
        /// <summary>
        /// 鼓组件
        /// </summary>
        GZJ,
        /// <summary>
        /// 显影组件
        /// </summary>
        XYZJ,
        /// <summary>
        /// 鼓刮板
        /// </summary>
        GGB,
        /// <summary>
        /// 转印组件
        /// </summary>
        ZYZJ,
        /// <summary>
        /// 废碳粉盒
        /// </summary>
        FTFH,
    }
}
