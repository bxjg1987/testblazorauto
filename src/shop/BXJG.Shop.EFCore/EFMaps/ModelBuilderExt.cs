using Abp.Authorization.Users;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
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
        /// <typeparam name="TArea"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyConfigurationBXJGShop<TUser, TArea>(this ModelBuilder modelBuilder)
            where TUser : AbpUserBase
            where TArea : GeneralTreeEntity<TArea>, IShopAdministrative
        {
            return modelBuilder
                .ApplyConfigurationsFromAssembly(BXJGShopEFCoreModule.GetAssembly())
                .ApplyConfiguration(new OrderItemMap<TUser, TArea>())
                .ApplyConfiguration(new OrderMap<TUser, TArea>());//上面扫描程序集的方式无法注册带泛型的，所以下面单独加

        }
    }
}
