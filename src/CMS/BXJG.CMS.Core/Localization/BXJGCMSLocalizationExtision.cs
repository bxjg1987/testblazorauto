using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Localization
{
    public static class BXJGCMSLocalizationExtision
    {
        public static ILocalizableString BXJGCMSL(this string key)
        {
            return new LocalizableString(key, BXJGCMSConsts.LocalizationSourceName);
        }
        public static string BXJGCMSEnum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum<TEnum>(BXJGCMSConsts.LocalizationSourceName, val);
        }
    }
}
