using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WorkOrder
{
    public static class LocalizationExtensions
    {
        public static ILocalizableString BXJGWorkOrderL(this string key)
        {
            return new LocalizableString(key, CoreConsts.LocalizationSourceName);
        }
        public static string BXJGWorkOrderEnum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum(CoreConsts.LocalizationSourceName, val);
        }
    }
}
