using System;

namespace BXJG.Common
{   /// <summary>
    /// 区域
    /// </summary>
    public interface IAdministrative
    { /// <summary>
      /// 省？市、区、县
      /// </summary>
        AdministrativeLevel Level { get; }
    }
}
