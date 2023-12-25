using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.GeneralTree
{
    /// <summary>
    /// 移动节点的类型
    /// </summary>
    [Flags]
    public enum GeneralTreeMoveType
    {
        /// <summary>
        /// 移动到目标节点前面
        /// </summary>
        Front,
        /// <summary>
        /// 移动到目标节点后面
        /// </summary>
        After,
        /// <summary>
        /// 追加到目标节点的子节点中
        /// </summary>
        Append
    }
}
