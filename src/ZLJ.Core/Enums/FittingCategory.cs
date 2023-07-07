using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Enums
{
    [Flags]
    public enum FittingCategory
    {
        /// <summary>
        /// 硒鼓
        /// </summary>
        TonerCartridge = 1,

        /// <summary>
        /// 打印头
        /// </summary>
        PrintHead = 2,
    }
}
