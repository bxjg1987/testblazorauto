using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Linq.Expressions;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Timing;
using BXJG.Utils.DataPermission;
using BXJG.Utils.Metadata;
using Abp.Authorization.Users;
using BXJG.Utils.OU;
using BXJG.Utils.Share.DataPermission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.Repositories
{
    /// <summary>
    /// 支持数据权限的仓储基类（带主键）
    /// </summary>
    /// <typeparam name="TDbContext">DbContext类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class BXJGUtilsEfCoreRepositoryBase<TDbContext, TEntity, TPrimaryKey>
        : EfCoreRepositoryBase<TDbContext, TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public IAbpSession AbpSession { get; set; }
        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }
        public IDataPermissionRuleProvider DataPermissionRuleProvider { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextProvider">DbContext提供者</param>
        public BXJGUtilsEfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// 获取所有实体（应用数据权限过滤）
        /// </summary>
        public override IQueryable<TEntity> GetAll()
        {
            var query = base.GetAll();
            return ApplyDataPermissionFilter(query);
        }

        /// <summary>
        /// 获取所有实体（只读，应用数据权限过滤）
        /// </summary>
        public override IQueryable<TEntity> GetAllReadonly()
        {
            var query = base.GetAllReadonly();
            return ApplyDataPermissionFilter(query);
        }

        /// <summary>
        /// 获取所有实体（包含导航属性，应用数据权限过滤）
        /// </summary>
        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = base.GetAllIncluding(propertySelectors);
            return ApplyDataPermissionFilter(query);
        }

        /// <summary>
        /// 异步获取所有实体（应用数据权限过滤）
        /// </summary>
        public override async Task<IQueryable<TEntity>> GetAllAsync()
        {
            var query = await base.GetAllAsync();
            return await ApplyDataPermissionFilterAsync(query);
        }

        /// <summary>
        /// 异步获取所有实体（只读，应用数据权限过滤）
        /// </summary>
        public override async Task<IQueryable<TEntity>> GetAllReadonlyAsync()
        {
            var query = await base.GetAllReadonlyAsync();
            return await ApplyDataPermissionFilterAsync(query);
        }

        /// <summary>
        /// 异步获取所有实体（包含导航属性，应用数据权限过滤）
        /// </summary>
        public override async Task<IQueryable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = await base.GetAllIncludingAsync(propertySelectors);
            return await ApplyDataPermissionFilterAsync(query);
        }



     }
}
