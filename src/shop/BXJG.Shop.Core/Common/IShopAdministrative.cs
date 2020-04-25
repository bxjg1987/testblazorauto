using BXJG.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Common
{
    /// <summary>
    /// 区域
    /// </summary>
    public interface IShopAdministrative
    {
        /// <summary>
        /// 省？市、区、县
        /// </summary>
        AdministrativeLevel Level { get;  }
    }
}
