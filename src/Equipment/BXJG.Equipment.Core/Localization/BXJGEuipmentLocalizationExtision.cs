using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.Localization
{
    public static class BXJGEuipmentLocalizationExtision
    {
        public static string LSN => BXJGEquipmentConst.LocalizationSourceName;
        public static ILocalizableString BXJGEquipmentL(this string key)
        {
            return new LocalizableString(key, LSN);
        }
        public static string BXJGEquipmentLS(this string key)
        {
            return LocalizationHelper.Manager.GetString(LSN, key);
        }
        public static string BXJGEquipmentLS<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum<TEnum>(LSN, val);
        }
        public static IDictionary<int, string> BXJGEquipmentLDic<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum<TEnum>(LSN);
        }
        ///// <summary>
        ///// 获取订单状态的本地化文本
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string BXJGEquipmentOrderStatus(this OrderStatus status)
        //{
        //    return status.BXJGEquipmentEnum<OrderStatus>();
        //}
        ///// <summary>
        ///// 获取订单的物流状态的本地化文本
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string BXJGEquipmentLogisticsStatus(this LogisticsStatus status)
        //{
        //    return status.BXJGEquipmentEnum<LogisticsStatus>();
        //}
        ///// <summary>
        ///// 获取订单支付状态本地化文本
        ///// </summary>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //public static string BXJGEquipmentPaymentStatus(this PaymentStatus status)
        //{
        //    return status.BXJGEquipmentEnum<PaymentStatus>();
        //}
    }
}
