using Abp.Localization;
using BXJG.Utils.Localization;
using System;

namespace ZLJ.Localization
{
   public static class LocalizationExtensions
    {
        public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum(ZLJConsts.LocalizationSourceName, val);
        }

        public static ILocalizableString GetLocalizableString(this object key) => new LocalizableString(key.ToString(), ZLJConsts.LocalizationSourceName);

        public static string L(this object key) => LocalizationHelper.GetString(ZLJConsts.LocalizationSourceName, key.ToString());
    }
}
