using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.Localization
{
    /// <summary>
    /// 设备管理模块针对模块内的本地化的扩展
    /// 内部方法均使用本模块的本地化资源文件
    /// </summary>
    public static class BXJGEuipmentLocalizationExtision
    {
        static string LSN => BXJGEquipmentConst.LocalizationSourceName;
        /// <summary>
        /// 获取本模块中指定键的ILocalizableString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ILocalizableString BXJGEquipmentL(this string key)
        {
            return new LocalizableString(key, LSN);
        }
        /// <summary>
        /// 获取设备管理模块中指定键的本地化字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BXJGEquipmentLS(this string key)
        {
            return LocalizationHelper.Manager.GetString(LSN, key);
        }
        public static string BXJGEquipmentLS<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum(LSN, val);
        }
        public static IDictionary<int, string> BXJGEquipmentLDic<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum<TEnum>(LSN);
        }
    }
}
