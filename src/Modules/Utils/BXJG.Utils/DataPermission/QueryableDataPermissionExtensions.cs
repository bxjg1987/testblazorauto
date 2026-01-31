using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Uow;
using Abp.Linq.Expressions;
using Abp.Organizations;
using Abp.Runtime.Session;
using BXJG.Common.DI;
using BXJG.Utils.DI;
using BXJG.Utils.Metadata;
using BXJG.Utils.OU;
using BXJG.Utils.Share.DataPermission;
using BXJG.Utils.Share.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BXJG.Utils.DataPermission
{
    /// <summary>
    /// IQueryable数据权限过滤扩展方法
    /// </summary>
    public static class QueryableDataPermissionExtensions
    {
        /// <summary>
        /// 检查数据权限过滤器是否开启，并验证实体类型是否在元数据中定义（同步版本）
        /// </summary>
        /// <param name="entityTypeFullName">实体类型全名</param>
        /// <returns>如果过滤器未开启或实体类型不在元数据中定义，则返回false；否则返回true</returns>
        static bool IsDataPermissionFilterEnabled(string entityTypeFullName)
        {
            if (!IsDataPermissionFilterEnable(entityTypeFullName))
            {
                return false;
            }

            // 通过DI解析元数据提供器，检查实体类型是否在元数据中定义
            var metadataProvider = AbpDIStaticAccessor.IocResolver?.Resolve<IMetaDataProvider>();
            if (metadataProvider != null)
            {
                var metadata = metadataProvider.GetMetaDataByEntityType(DataPermissionConsts.DataPermission, entityTypeFullName);
                if (metadata == null)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 检查数据权限过滤器是否开启，并验证实体类型是否在元数据中定义（异步版本）
        /// </summary>
        /// <param name="entityTypeFullName">实体类型全名</param>
        /// <returns>如果过滤器未开启或实体类型不在元数据中定义，则返回false；否则返回true</returns>
        static async Task<bool> IsDataPermissionFilterEnabledAsync(string entityTypeFullName)
        {
            if (!IsDataPermissionFilterEnable(entityTypeFullName))
            {
                return false;
            }

            // 通过DI解析元数据提供器，检查实体类型是否在元数据中定义
            var metadataProvider = AbpDIStaticAccessor.IocResolver?.Resolve<IMetaDataProvider>();
            if (metadataProvider != null)
            {
                var metadata = await metadataProvider.GetMetaDataByEntityTypeAsync(DataPermissionConsts.DataPermission, entityTypeFullName);
                if (metadata == null)
                    return false;
            }

            return true;
        }

        private static bool IsDataPermissionFilterEnable(string entityTypeFullName)
        {
            var uowProvider = AbpDIStaticAccessor.IocResolver?.Resolve<ICurrentUnitOfWorkProvider>();
            if (uowProvider?.Current == null)
                return false;
            if (!uowProvider.Current.IsFilterEnabled(DataPermissionConsts.DataPermission))
                return false;

            if (entityTypeFullName == typeof(MetadataEntity).FullName)
                return false;
            return true;
        }

        /// <summary>
        /// 用于数据权限规则
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> WhereDataPermission<TEntity>(this IQueryable<TEntity> query)
            where TEntity : class
        {
            var entityTypeFullName = typeof(TEntity).FullName;

            if (!IsDataPermissionFilterEnabled(entityTypeFullName))
                return query;

            // 通过DI解析数据权限规则提供器
            var ruleProvider = AbpDIStaticAccessor.IocResolver?.Resolve<IDataPermissionRuleProvider>();

            var rules = ruleProvider.GetRules(entityTypeFullName);

            if (rules == null)
                return query.Where(e => false);


            return query.WherePermission(rules);
        }

        /// <summary>
        /// 用于数据权限规则
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryTask"></param>
        /// <returns></returns>
        public static async Task<IQueryable<TEntity>> WhereDataPermission<TEntity>(this Task<IQueryable<TEntity>> queryTask)
            where TEntity : class
        {
            var query = await queryTask;

            var entityTypeFullName = typeof(TEntity).FullName;

            if (!await IsDataPermissionFilterEnabledAsync(entityTypeFullName))
                return query;

            // 通过DI解析数据权限规则提供器
            var ruleProvider = AbpDIStaticAccessor.IocResolver?.Resolve<IDataPermissionRuleProvider>();

            var rules = await ruleProvider.GetRulesAsync(entityTypeFullName);

            if (rules == null)
                return query.Where(e => false);


            return query.WherePermission(rules);
        }
    }
}
