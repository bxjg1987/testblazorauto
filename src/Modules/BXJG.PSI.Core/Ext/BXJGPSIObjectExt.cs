using BXJG.PSI;
using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class BXJGPSIObjectExt
    {
        #region MyRegion
        /// <summary>
        /// 获取Utils模块中的指定键的本地化字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string PSILString(this string key, params object[] args)
        {
            //return string.Format(LocalizationHelper.GetString(UtilsConsts.LocalizationSourceName, key), args);
            //return Abp.Localization.LocalizationHelper.GetString(UtilsConsts.LocalizationSourceName, key);
            return BXJGPSILocalizationExt.PSILString(LocalizationHelper.Manager, key, args);
        }
        /// <summary>
        /// 获取Utils模块中的指定键的本地化ILocalizableString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ILocalizableString PSILS(this string key)
        {
            return new LocalizableString(key, BXJGPSICoreConsts.LocalizationSourceName);
        }
        #endregion
    }
}
