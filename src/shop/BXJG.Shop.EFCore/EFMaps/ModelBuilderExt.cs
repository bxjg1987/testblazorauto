using Abp.Authorization.Users;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Common;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;
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
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyConfigurationBXJGShop<TUser>(this ModelBuilder modelBuilder)
        {
            return modelBuilder
                .ApplyConfigurationsFromAssembly(BXJGShopEFCoreModule.GetAssembly())
                .ApplyConfiguration(new CustomerMap<TUser, CustomerEntity<TUser>>())
                .ApplyConfiguration(new OrderMap<TUser, OrderEntity<TUser>>())
                .ApplyConfiguration(new OrderItemMap<TUser, OrderItemEntity<TUser>>());

        }

        //public static ModelBuilder ApplyConfigurationBXJGShop<TEntity, TMap>(this ModelBuilder modelBuilder)
        //    where TEntity : class
        //    where TMap : IEntityTypeConfiguration<TEntity>, new()
        //{
        //    return modelBuilder.ApplyConfiguration(new TMap());
        //}

        //public static ModelBuilder ApplyConfigurationBXJGShop<TUser, TEntity, TMap>(this ModelBuilder modelBuilder)
        //    where TUser : AbpUserBase
        //    
        //    where TEntity : class
        //    where TMap : IEntityTypeConfiguration<TEntity>, new()
        //{
        //    return modelBuilder.ApplyConfigurationBXJGShop<TEntity, TMap>();
        //}
    }
}
