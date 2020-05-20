using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.GeneralTree
{
    /// <summary>
    /// 区域
    /// </summary>
    public interface IAdministrative
    {
        /// <summary>
        /// 省？市、区、县
        /// </summary>
        AdministrativeLevel Level { get;  }
    }
}
