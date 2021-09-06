using Abp.Configuration.Startup;
using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.GoodsInfo
{
    public static class Extensions
    {
        /// <summary>
        /// 注册具体的物品类型
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static GoodsInfoConfiguration AddGoodsInfoType(this GoodsInfoConfiguration config, Func<GoodsInfoTypeDefineAddContex, GoodsInfoTypeDefine> func)
        {
            config.GoodsInfoTypeProviders.Add(func);
            return config;
        }
        /// <summary>
        /// 注册具体的物品类型
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static GoodsInfoConfiguration AddGoodsInfoType(this GoodsInfoConfiguration config, GoodsInfoTypeDefine func)
        {
            return config.AddGoodsInfoType(c => func);
        }
        /// <summary>
        /// 获取物品模块配置对象
        /// </summary>
        public static GoodsInfoConfiguration BXJGGoodsInfo(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<GoodsInfoConfiguration>();
        }
        /// <summary>
        /// 将模块内部的枚举值转换为本地化字符串
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Enum<TEnum>(this TEnum val) where TEnum : Enum
        {
            return LocalizationHelper.Manager.GetEnum(BXJGGoodsInfoCoreConsts.LocalizationSourceName, val);
        }
        /// <summary>
        /// 获取当前模块的本地化
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ILocalizableString ZLJLI(this string str)
        {
            return new LocalizableString(str, BXJGGoodsInfoCoreConsts.LocalizationSourceName);
        }
    }
}
