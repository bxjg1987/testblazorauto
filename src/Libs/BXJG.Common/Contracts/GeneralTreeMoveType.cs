using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Contracts
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
        [Description("之前")]
        Front = 1<<0,
        /// <summary>
        /// 移动到目标节点后面
        /// </summary>
        [Description("之后")]
        After =1<<1,
        /// <summary>
        /// 追加到目标节点的子节点中
        /// </summary>
        [Description("内部")]
        Append =1<<2,
    }
}
