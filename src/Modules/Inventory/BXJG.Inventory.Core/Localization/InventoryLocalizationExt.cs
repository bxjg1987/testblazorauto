using Abp.Localization;
using Abp.Localization.Sources;
using BXJG.Utils.Share;
using System;

namespace BXJG.Inventory.Localization
{
    public static class InventoryLocalizationExt
    {
        #region 本地化字符串获取扩展方法
        /// <summary>
        /// 获取库存模块中的指定键的本地化字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string InventoryL(this string key, params object[] args)
        {
            return InventoryL(LocalizationHelper.Manager, key, args);
        }
        /// <summary>
        /// 获取库存模块中的指定键的本地化ILocalizableString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ILocalizableString InventoryLI(this string key)
        {
            return new LocalizableString(key, BXJGInventoryCoreConsts.LocalizationSourceName);
        }
        /// <summary>
        /// 获取库存模块中的本地化字符串
        /// </summary>
        /// <param name="localizationManager"></param>
        /// <param name="name"></param>
        /// <param name="args">注意不要传递数组</param>
        /// <returns></returns>
        public static string InventoryL(this ILocalizationManager localizationManager, string name, params object[] args)
        {
            return string.Format(localizationManager.GetString(BXJGInventoryCoreConsts.LocalizationSourceName, name), args);
        }
        #endregion
    }
}