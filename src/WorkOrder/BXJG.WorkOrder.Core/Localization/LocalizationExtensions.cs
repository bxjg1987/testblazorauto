using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WorkOrder
{
    public static class LocalizationExtensions
    {
        public static ILocalizableString BXJGWorkOrderLI(this string key)
        {
            return new LocalizableString(key, CoreConsts.LocalizationSourceName);
        }
        public static string BXJGWorkOrderEnum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum(CoreConsts.LocalizationSourceName, val);
        }
        /// <summary>
        /// 转换为工单模块本地化文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BXJGWorkOrderL(this string key, params string[] args)
        {
            return string.Format( LocalizationHelper.Manager.GetString(CoreConsts.LocalizationSourceName, key),args);
        }
    }
}
