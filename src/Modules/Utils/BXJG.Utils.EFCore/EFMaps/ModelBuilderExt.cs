using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelBuilderExt
    {
        /// <summary>
        /// 自动发现和应用所有相关的EF Core配置
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder实例</param>
        /// <returns>配置后的ModelBuilder实例</returns>
        public static ModelBuilder ApplyConfigurationBXJGUtils(this ModelBuilder modelBuilder)
        {
            // 获取当前应用域中所有加载的程序集，并去重
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Distinct().ToList();
            
            foreach (var assembly in assemblies)
            {
                // 跳过系统程序集、动态生成的程序集和常用第三方库
                if (IsSystemOrThirdPartyAssembly(assembly))
                    continue;
                
                // 优先检查程序集名称，这是快速操作
                if (assembly.FullName.Contains(".Ef", StringComparison.OrdinalIgnoreCase))
                {
                    ApplyAssemblyConfigurations(modelBuilder, assembly);
                    continue;
                }
                
                // 如果程序集名称不匹配，再检查是否包含配置类型（相对较慢的操作）
                if (ContainsEntityTypeConfiguration(assembly))
                {
                    ApplyAssemblyConfigurations(modelBuilder, assembly);
                }
            }
            
            return modelBuilder;
        }
        
        /// <summary>
        /// 检查是否为系统程序集或第三方库程序集
        /// </summary>
        /// <param name="assembly">要检查的程序集</param>
        /// <returns>如果是系统程序集或第三方库程序集则返回true，否则返回false</returns>
        private static bool IsSystemOrThirdPartyAssembly(Assembly assembly)
        {
            if (assembly.IsDynamic)
                return true;
                
            var fullName = assembly.FullName;
            return fullName.StartsWith("System.") ||
                   fullName.StartsWith("Microsoft.") ||
                   fullName.StartsWith("netstandard") ||
                   fullName.StartsWith("Newtonsoft.Json") ||
                   fullName.StartsWith("NuGet.") ||
                   fullName.StartsWith("Autofac.") ||
                   fullName.StartsWith("Castle.Core") ||
                   fullName.StartsWith("EntityFramework") ||
                   fullName.StartsWith("Serilog.") ||
                   fullName.StartsWith("NLog.") ||
                   fullName.StartsWith("log4net");
        }
        
        /// <summary>
        /// 检查程序集中是否包含实现IEntityTypeConfiguration泛型接口的类型
        /// </summary>
        /// <param name="assembly">要检查的程序集</param>
        /// <returns>如果包含则返回true，否则返回false</returns>
        private static bool ContainsEntityTypeConfiguration(Assembly assembly)
        {
            try
            {
                // 使用LINQ简化类型检查逻辑，提高可读性
                return assembly.GetTypes()
                    .Any(type => type.IsClass && !type.IsAbstract &&
                           type.GetInterfaces()
                               .Any(@interface => @interface.IsGenericType &&
                                      @interface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));
            }
            catch (ReflectionTypeLoadException)
            {
                // 忽略类型加载异常，视为不包含配置类型
                return false;
            }
        }
        
        /// <summary>
        /// 应用指定程序集的EF配置
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder实例</param>
        /// <param name="assembly">包含EF配置的程序集</param>
        private static void ApplyAssemblyConfigurations(ModelBuilder modelBuilder, Assembly assembly)
        {
            // EF Core内部会跟踪已应用的配置，不会重复应用同一个配置类型
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
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
            entityTypeBuilder.HasIndex(c => c.Code);//.IsUnique();//多租户时，唯一索引有问题

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
