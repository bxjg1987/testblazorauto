using Abp.Localization;
using Abp.Localization.Sources;
using BXJG.Utils.Share;
using System;

namespace BXJG.PSI.MasterData.Localization
{
    public static class PSIMasterDataLocalizationExt
    {
        #region 本地化字符串获取扩展方法
        /// <summary>
        /// 获取主数据模块中的指定键的本地化字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string PSIMasterDataL(this string key, params object[] args)
        {
            return PSIMasterDataL(LocalizationHelper.Manager, key, args);
        }
        /// <summary>
        /// 获取主数据模块中的指定键的本地化ILocalizableString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ILocalizableString PSIMasterDataLI(this string key)
        {
            return new LocalizableString(key, BXJGPSIMasterDataCoreConsts.LocalizationSourceName);
        }
        /// <summary>
        /// 获取主数据模块中的本地化字符串
        /// </summary>
        /// <param name="localizationManager"></param>
        /// <param name="name"></param>
        /// <param name="args">注意不要传递数组</param>
        /// <returns></returns>
        public static string PSIMasterDataL(this ILocalizationManager localizationManager, string name, params object[] args)
        {
            return string.Format(localizationManager.GetString(BXJGPSIMasterDataCoreConsts.LocalizationSourceName, name), args);
        }
        #endregion
    }
}