using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Uow;
using Abp.Linq.Expressions;
using Abp.Organizations;
using Abp.Runtime.Session;
using BXJG.Common.DI;
using BXJG.Utils.DI;
using BXJG.Utils.OU;
using BXJG.Utils.Share.DataPermission;
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
        static bool sfkq() {
            var uowProvider = AbpDIStaticAccessor.IocResolver?.Resolve<ICurrentUnitOfWorkProvider>();
            if (!uowProvider.Current.IsFilterEnabled(DataPermissionConsts.DataPermission))
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
            where TEntity : class, IEntity
        {
            if (!sfkq())
                return query;

            // 通过DI解析数据权限规则提供器
            var ruleProvider = AbpDIStaticAccessor.IocResolver?.Resolve<IDataPermissionRuleProvider>();

            var entityTypeFullName = typeof(TEntity).FullName;
            var rules = ruleProvider.GetRules(entityTypeFullName);

            if (rules == null)
                return query.Where(e => false);


            return query.WherePermission( rules);
        }

        /// <summary>
        /// 用于数据权限规则
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryTask"></param>
        /// <returns></returns>
        public static async Task<IQueryable<TEntity>> WhereDataPermission<TEntity>(this Task<IQueryable<TEntity>> queryTask)
            where TEntity : class, IEntity
        {
            var query = await queryTask;

            // 通过DI解析数据权限规则提供器
            var ruleProvider = AbpDIStaticAccessor.IocResolver?.Resolve<IDataPermissionRuleProvider>();

            var entityTypeFullName = typeof(TEntity).FullName;
            var rules = await ruleProvider.GetRulesAsync(entityTypeFullName);

            if (rules == null)
                return query.Where(e => false);


            return query.WherePermission(rules);
        }
    }
}
