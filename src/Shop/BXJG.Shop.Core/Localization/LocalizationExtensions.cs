using Abp.Localization;
using BXJG.Shop.Sale;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Localization
{
    public static class LocalizationExtensions
    {
        /// <summary>
        /// 获取订单系统中的指定键的ILocalizableString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ILocalizableString BXJGShopL(this string key)
        {
            return new LocalizableString(key, CoreConsts.LocalizationSourceName);
        }
        public static string BXJGShopEnum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum<TEnum>(CoreConsts.LocalizationSourceName, val);
        }
        /// <summary>
        /// 获取订单状态的本地化文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BXJGShopOrderStatus(this OrderStatus status)
        {
            return status.BXJGShopEnum<OrderStatus>();
        }
        /// <summary>
        /// 获取订单的物流状态的本地化文本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BXJGShopLogisticsStatus(this LogisticsStatus status)
        {
            return status.BXJGShopEnum<LogisticsStatus>();
        }
        /// <summary>
        /// 获取订单支付状态本地化文本
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string BXJGShopPaymentStatus(this PaymentStatus status)
        {
            return status.BXJGShopEnum<PaymentStatus>();
        }
    }
}
