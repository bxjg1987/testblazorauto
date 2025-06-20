using Abp.Localization;
using Abp.Localization.Sources;
using BXJG.Utils.Localization;
using System;

namespace ZLJ.Core.Localization
{
   public static class LocalizationExtensions
    {
        public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum(ZLJ.Core.Share.ZLJConsts.LocalizationSourceName, val);
        }

        public static ILocalizableString GetLocalizableString(this object key) => new LocalizableString(key.ToString(), ZLJ.Core.Share.ZLJConsts.LocalizationSourceName);

        public static string L(this object key) => LocalizationHelper.GetString(ZLJ.Core.Share.ZLJConsts.LocalizationSourceName, key.ToString());

        public static ILocalizationSource GetZLJCoreLocalizationSource()
        {
            return LocalizationHelper.Manager.GetSource(ZLJ.Core.Share.ZLJConsts.LocalizationSourceName);
        }
    }
}
