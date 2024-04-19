using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Core.Share.Enums
{
    /// <summary>
    /// 行政区级别
    /// </summary>
    [Flags]
    public enum AdministrativeLevel
    {
        /// <summary>
        /// 省、直辖市
        /// </summary>
        Province,
        /// <summary>
        /// 市
        /// </summary>
        City,
        /// <summary>
        /// 区县
        /// </summary>
        County
    }
}
