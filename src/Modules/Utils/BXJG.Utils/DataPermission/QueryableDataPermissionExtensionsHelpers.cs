using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Linq.Expressions;
using Abp.Organizations;
using Abp.Runtime.Session;
using BXJG.Utils.DI;
using BXJG.Utils.OU;
using BXJG.Utils.Share.DataPermission;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// 扩展IQueryable<TEntity>以应用数据权限规则
    /// </summary>
    public static class QueryableDataPermissionExtensionsHelpers
    {
        /// <summary>
        /// 应用指定的数据权限规则
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> WherePermission<TEntity>(this IQueryable<TEntity> query, DataPermissionDto rules)
            where TEntity : class, IEntity
        {
            if (rules.OrganizationUnits == null || !rules.OrganizationUnits.Any())
            {
                switch (rules.GrantType)
                {
                    case DataPermissionGrantType.All:
                        return query;
                    case DataPermissionGrantType.OnlyMe:
                        var userId = AbpDIStaticAccessor.IocResolver.Resolve<IAbpSession>().UserId;
                        return query.Where(e => ((ICreationAudited)e).CreatorUserId == userId);
                    case DataPermissionGrantType.Rejected:
                    //    return query.Where(e => false);
                    //case DataPermissionGrantType.OrganizationUnit:
                    //    break;
                    //case DataPermissionGrantType.OrganizationUnitRecursive:
                    //    break;
                    default:
                        return query.Where(e => false);
                }
            }

            var entityType = typeof(TEntity);
            Expression<Func<TEntity, bool>> predicate = PredicateBuilder.New<TEntity>(false);

            var tmpQ = rules.OrganizationUnits.Where(x => x.OrganizationUnitId.HasValue);
            //这些组织单位的数据不能看
            var rejectIds = tmpQ.Where(x => x.GrantType == DataPermissionGrantType.Rejected).Select(x => x.OrganizationUnitId);
            //指定的组织但单位可以看
            var ouIds = tmpQ.Where(x => x.GrantType == DataPermissionGrantType.OrganizationUnit).Select(x => x.OrganizationUnitId);
            //这些组织单位仅查看自己的数据
            var onlyMeIds = tmpQ.Where(x => x.GrantType == DataPermissionGrantType.OnlyMe).Select(x => x.OrganizationUnitId);

            if (typeof(IMayHaveOrganizationUnit).IsAssignableFrom(entityType))
            {
                query = query.Where(e => !rejectIds.Contains(((IMayHaveOrganizationUnit)e).OrganizationUnitId.Value));
                if (ouIds.Any())
                {
                    predicate = predicate.Or(e => ouIds.Contains(((IMayHaveOrganizationUnit)e).OrganizationUnitId.Value));
                }
                if (onlyMeIds.Any())
                {
                    var userId = AbpDIStaticAccessor.IocResolver.Resolve<IAbpSession>().UserId;
                    predicate = predicate.Or(e => ((ICreationAudited)e).CreatorUserId == userId && ouIds.Contains(((IMayHaveOrganizationUnit)e).OrganizationUnitId.Value));
                }
            }
            else if (typeof(IMustHaveOrganizationUnit).IsAssignableFrom(entityType))
            {
                query = query.Where(e => !rejectIds.Contains(((IMustHaveOrganizationUnit)e).OrganizationUnitId));
                if (ouIds.Any())
                {
                    predicate = predicate.Or(e => ouIds.Contains(((IMustHaveOrganizationUnit)e).OrganizationUnitId));
                }
                if (onlyMeIds.Any())
                {
                    var userId = AbpDIStaticAccessor.IocResolver.Resolve<IAbpSession>().UserId;
                    predicate = predicate.Or(e => ((ICreationAudited)e).CreatorUserId == userId && ouIds.Contains(((IMustHaveOrganizationUnit)e).OrganizationUnitId));
                }
            }
            //这些组织单位仅查看自己的数据
            var allCodes = tmpQ.Where(x => x.GrantType == DataPermissionGrantType.All).Select(x => x.OrganizationUnitCode);
            foreach (var item in allCodes)
            {
                predicate = predicate.Or(x => ((IOU)x).OrganizationUnit.Code.StartsWith(item));
            }
            return query.Where(predicate);
        }
    }
}