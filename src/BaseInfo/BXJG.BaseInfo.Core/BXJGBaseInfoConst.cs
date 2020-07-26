using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.BaseInfo
{
    public class BXJGBaseInfoConst
    {
        public const string LocalizationSourceName = "BXJGBaseInfo";

        #region 实体映射常量
        //当实体是泛型时，在其中定义常量不太容易被调用，因此实体中的常量定义在这里
        #region 设备信息
        public const int AdministrativeNameMaxLength = 200;
        //public const int AdministrativeMnemonicCodeMaxLength = 200;
        //public const int AdministrativeSizeMaxLength = 200;
        #endregion
        #endregion

    }
}
