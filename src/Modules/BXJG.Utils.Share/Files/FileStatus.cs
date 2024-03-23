using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Share.Files
{
    [Flags]
    public enum FileStatus
    {
        /// <summary>
        /// 正在移动
        /// </summary>
        Moving = 1 << 0,
        /// <summary>
        /// 移动完成
        /// </summary>
        Moved = 1 << 1,

    }
}
