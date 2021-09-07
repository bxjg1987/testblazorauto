using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.GoodsInfo
{
    /// <summary>
    /// 物品模块相关扩展方法
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 通常在你的模块PreInit阶段调用此方注册你自定义的物品类型
        /// </summary>
        /// <param name="config"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Configuration AddGoodsInfoType(this Configuration config, Func<GoodsInfoTypeDefineAddContex, GoodsInfoTypeDefine> func)
        {
            config.GoodsInfoTypeProviders.Add(func);
            return config;
        }
        /// <summary>
        /// 通常在你的模块PreInit阶段调用此方注册你自定义的物品类型
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static Configuration AddGoodsInfoType(this Configuration config, GoodsInfoTypeDefine func)
        {
            return config.AddGoodsInfoType(c => func);
        }
        /// <summary>
        /// 获取物品模块配置对象
        /// </summary>
        public static Configuration BXJGGoodsInfo(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<Configuration>();
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
        public static ILocalizableString BXJGGoodsInfoLI(this string str)
        {
            return new LocalizableString(str, BXJGGoodsInfoCoreConsts.LocalizationSourceName);
        }
        /// <summary>
        /// 注册物品分类权限
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static Permission AddGoodsInfoCategoryPermission(this Permission root)
        {
            var category = root.CreateChildPermission(BXJGGoodsInfoCoreConsts.GoodsInfoCategoryManager, BXJGGoodsInfoCoreConsts.GoodsInfoCategoryManager.BXJGGoodsInfoLI(), multiTenancySides: MultiTenancySides.Tenant);
            category.CreateChildPermission(BXJGGoodsInfoCoreConsts.GoodsInfoCategoryCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            category.CreateChildPermission(BXJGGoodsInfoCoreConsts.GoodsInfoCategoryUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            category.CreateChildPermission(BXJGGoodsInfoCoreConsts.GoodsInfoCategoryDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            return category;
        }
    }
}
