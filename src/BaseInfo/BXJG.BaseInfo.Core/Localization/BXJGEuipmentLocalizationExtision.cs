using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.BaseInfo.Localization
{
    /// <summary>
    /// 针对当前模块的本地化扩展
    /// </summary>
    public static class BXJGEuipmentLocalizationExtision
    {
        static string LSN => BXJGBaseInfoConst.LocalizationSourceName;
        /// <summary>
        /// 获取基础信息模块中对应键的ILocalizableString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ILocalizableString BXJGBaseInfoL(this string key)
        {
            return new LocalizableString(key, LSN);
        }
        /// <summary>
        /// 获取基础信息模块中对应键的本地化字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BXJGBaseInfoLS(this string key)
        {
            return LocalizationHelper.Manager.GetString(LSN, key);
        }
        /// <summary>
        /// 获取基础信息模块中枚举字符串对应的本地化文本
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string BXJGBaseInfoLS<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum<TEnum>(LSN, val);
        }
        /// <summary>
        /// 获取基础信息模块中枚举对应的本地化数据字典
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static IDictionary<int, string> BXJGBaseInfoLDic<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum<TEnum>(LSN);
        }
        ///// <summary>
        ///// 获取订单状态的本地化文本
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string BXJGBaseInfoOrderStatus(this OrderStatus status)
        //{
        //    return status.BXJGBaseInfoEnum<OrderStatus>();
        //}
        ///// <summary>
        ///// 获取订单的物流状态的本地化文本
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string BXJGBaseInfoLogisticsStatus(this LogisticsStatus status)
        //{
        //    return status.BXJGBaseInfoEnum<LogisticsStatus>();
        //}
        ///// <summary>
        ///// 获取订单支付状态本地化文本
        ///// </summary>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //public static string BXJGBaseInfoPaymentStatus(this PaymentStatus status)
        //{
        //    return status.BXJGBaseInfoEnum<PaymentStatus>();
        //}
    }
}
