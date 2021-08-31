using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.GoodsInfo
{
   public static class Extensions
    {
        public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum(BXJGGoodsInfoCoreConsts.LocalizationSourceName, val);
        }

        public static ILocalizableString ZLJLI(this string str) {
            return new LocalizableString(str, BXJGGoodsInfoCoreConsts.LocalizationSourceName);
        }
    }
}
