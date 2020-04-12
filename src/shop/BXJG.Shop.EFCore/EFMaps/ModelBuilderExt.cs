using Abp.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.EFMaps
{
    public static class ModelBuilderExt
    {
        /// <summary>
        /// 注册商城系统中的ef映射
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyConfigurationBXJGShop<TUser>(this ModelBuilder modelBuilder)
            where TUser : AbpUserBase
        {
            return modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(BXJGShopEFCoreModule).Assembly)
                .ApplyConfiguration(new OrderItemMap<TUser>())
                .ApplyConfiguration(new OrderMap<TUser>());//上面扫描程序集的方式无法注册带泛型的，所以下面单独加

        }
    }
}
