using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Localization
{
   public static class LocatizationExtensions
    {
        public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum<TEnum>(ZLJConsts.LocalizationSourceName, val);
        }
    }
}
