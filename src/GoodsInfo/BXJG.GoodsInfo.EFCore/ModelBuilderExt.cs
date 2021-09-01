using Abp.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BXJG.GoodsInfo.EFCore
{
    public static class ModelBuilderExt
    {
        /// <summary>
        /// 注册商城系统中的ef映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyConfigurationBXJGGoodsInfo(this ModelBuilder modelBuilder)
        {
            return modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
