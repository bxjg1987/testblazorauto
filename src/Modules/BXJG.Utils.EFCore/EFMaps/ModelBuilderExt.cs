using Abp.Authorization.Users;
using BXJG.Utils.Files;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelBuilderExt
    {
        /// <summary>
        /// 注册utils中的ef映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyConfigurationBXJGUtils(this ModelBuilder modelBuilder)
        {
            return modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModelBuilderExt).Assembly);
        }

        /// <summary>
        /// 应用通用树的配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityTypeBuilder"></param>
        /// <returns></returns>
        public static EntityTypeBuilder<T> MapGeneralTree<T>(this EntityTypeBuilder<T> entityTypeBuilder)
            where T : GeneralTreeEntity<T>
        {
            entityTypeBuilder.HasIndex(c => c.Code).IsUnique();

            return entityTypeBuilder;
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
