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
using Abp.Threading;
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
        public ICacheManager CacheManager { get; set; }
        public ICancellationTokenProvider CancellationTokenProvider { get; set; }

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

        /// <summary>
        /// 应用数据权限过滤（同步版本）
        /// 拒绝优先原则：如果元数据要求该实体需要数据权限且过滤器已启用，则必须进行数据权限过滤
        /// - 未登录用户拒绝访问
        /// - 没有获取到权限配置，拒绝访问
        /// </summary>
        /// <param name="query">原始查询</param>
        /// <returns>应用过滤后的查询</returns>
        protected virtual IQueryable<TEntity> ApplyDataPermissionFilter(IQueryable<TEntity> query)
        {
            // 检查是否需要应用数据权限过滤
            if (!ShouldApplyDataPermission())
                return query;

            var entityTypeFullName = typeof(TEntity).FullName;

            // 拒绝优先：元数据要求数据权限，但未登录用户拒绝访问
            if (!AbpSession.UserId.HasValue)
                return query.Where(e => false);

            // 从缓存或数据库加载数据权限（同步版本）
            var permission = GetDataPermissionFromCacheOrLoad();
            if (permission == null || !permission.TryGetValue(entityTypeFullName, out var dto))
            {
                // 拒绝优先：要求数据权限但没有获取到权限配置，拒绝访问
                return query.Where(e => false);
            }

            return ApplyPermissionFilter(query, dto);
        }

        /// <summary>
        /// 应用数据权限过滤（异步版本）
        /// 拒绝优先原则：如果元数据要求该实体需要数据权限且过滤器已启用，则必须进行数据权限过滤
        /// - 未登录用户拒绝访问
        /// - 没有获取到权限配置，拒绝访问
        /// </summary>
        /// <param name="query">原始查询</param>
        /// <returns>应用过滤后的查询</returns>
        protected virtual async Task<IQueryable<TEntity>> ApplyDataPermissionFilterAsync(IQueryable<TEntity> query)
        {
            // 检查是否需要应用数据权限过滤
            if (!await ShouldApplyDataPermissionAsync())
                return query;

            var entityTypeFullName = typeof(TEntity).FullName;

            // 拒绝优先：元数据要求数据权限，但未登录用户拒绝访问
            if (!AbpSession.UserId.HasValue)
                return query.Where(e => false);

            // 从缓存或数据库加载数据权限（异步版本）
            var permission = await GetDataPermissionFromCacheOrLoadAsync();
            if (permission == null || !permission.TryGetValue(entityTypeFullName, out var dto))
            {
                // 拒绝优先：要求数据权限但没有获取到权限配置，拒绝访问
                return query.Where(e => false);
            }

            return ApplyPermissionFilter(query, dto);
        }

        /// <summary>
        /// 从缓存获取数据权限，如果没有则同步加载
        /// </summary>
        protected virtual Dictionary<string, DataPermissionDto> GetDataPermissionFromCacheOrLoad()
        {
            if (!AbpSession.UserId.HasValue)
                return null;

            // 获取缓存实例
            var cache = CacheManager.GetCache<string, Dictionary<string, DataPermissionDto>>(DataPermissionConsts.DataPermission);
            var ck = AbpSession.UserId.ToString() + "@" + AbpSession.TenantId;
            
            var dtos = cache.GetOrDefault(ck);
            if (dtos == null)
            {
                dtos = LoadDataPermissionCore();
                // 缓存3分钟，人员角色、组织单位变化时应该删除缓存
                cache.Set(ck, dtos, absoluteExpireTime: Clock.Now.AddMinutes(3));
            }

            return dtos;
        }

        /// <summary>
        /// 从缓存获取数据权限，如果没有则异步加载
        /// </summary>
        protected virtual async Task<Dictionary<string, DataPermissionDto>> GetDataPermissionFromCacheOrLoadAsync()
        {
            if (!AbpSession.UserId.HasValue)
                return null;

            // 获取缓存实例
            var cache = CacheManager.GetCache<string, Dictionary<string, DataPermissionDto>>(DataPermissionConsts.DataPermission);
            var ck = AbpSession.UserId.ToString() + "@" + AbpSession.TenantId;
            
            var dtos = await cache.GetOrDefaultAsync(ck);
            if (dtos == null)
            {
                dtos = await LoadDataPermissionCoreAsync();
                // 缓存3分钟，人员角色、组织单位变化时应该删除缓存
                await cache.SetAsync(ck, dtos, absoluteExpireTime: Clock.Now.AddMinutes(3));
            }

            return dtos;
        }

        /// <summary>
        /// 核心加载数据权限逻辑（同步版本）
        /// </summary>
        protected virtual Dictionary<string, DataPermissionDto> LoadDataPermissionCore()
        {
            var uid = AbpSession.UserId;
            var context = GetContext();
            
            // 查询数据权限
            var dpq = context.Set<DataPermissionEntity>().AsNoTracking();

            // 查询当前用户的数据权限（直接对用户、角色、组织单位授权）
            var query = dpq.Where(dp => 
                dp.UserId == uid || // 直接对用户授权
                context.Set<UserRole>().Any(ur => ur.UserId == uid && ur.RoleId == dp.RoleId) || // 通过角色授权
                context.Set<UserOrganizationUnit>().Any(uo => uo.UserId == uid && uo.OrganizationUnitId == dp.UserOrganizationUnit) // 通过组织单位授权
            ).Select(dp => new
            {
                dp.EntityTypeFullName,
                dp.DataOrganizationUnit,
                dp.GrantType
            }).Distinct();

            var result = query.ToList();

            var dtos = new Dictionary<string, DataPermissionDto>();
            var gp = result.GroupBy(x => x.EntityTypeFullName);
            
            // 拒绝优先原则
            foreach (var item in gp)
            {
                var dto = new DataPermissionDto();
                if (item.Any(x => x.GrantType == DataPermissionGrantType.Rejected))
                {
                    dto.GrantType = DataPermissionGrantType.Rejected;
                }
                else if (item.Any(x => x.GrantType == DataPermissionGrantType.OnlyMe))
                {
                    dto.GrantType = DataPermissionGrantType.OnlyMe;
                }
                else if (item.Any(x => x.GrantType == DataPermissionGrantType.OrganizationUnit))
                {
                    dto.GrantType = DataPermissionGrantType.OrganizationUnit;
                    dto.OrganizationUnitIds = item.Where(x => x.GrantType == DataPermissionGrantType.OrganizationUnit && x.DataOrganizationUnit.HasValue)
                        .Select(x => x.DataOrganizationUnit.Value).Distinct();
                }
                else if (item.Any(x => x.GrantType == DataPermissionGrantType.OrganizationUnitRecursive))
                {
                    dto.GrantType = DataPermissionGrantType.OrganizationUnitRecursive;
                    var qt = item.Where(x => x.GrantType == DataPermissionGrantType.OrganizationUnitRecursive && x.DataOrganizationUnit.HasValue)
                        .Select(x => x.DataOrganizationUnit.Value).Distinct();
                    if (qt.Any())
                    {
                        var ouq = context.Set<OrganizationUnit>().AsNoTracking();
                        dto.OrganizationUnitCodes = ouq.Where(x => qt.Contains(x.Id))
                            .Select(x => x.Code).ToList();
                    }
                }
                else
                {
                    dto.GrantType = DataPermissionGrantType.All;
                }
                
                dtos.Add(item.Key, dto);
            }

            return dtos;
        }

        /// <summary>
        /// 核心加载数据权限逻辑（异步版本）
        /// </summary>
        protected virtual async Task<Dictionary<string, DataPermissionDto>> LoadDataPermissionCoreAsync()
        {
            var uid = AbpSession.UserId;
            var context = await GetContextAsync();
            
            // 查询数据权限
            var dpq = context.Set<DataPermissionEntity>().AsNoTracking();

            // 查询当前用户的数据权限（直接对用户、角色、组织单位授权）
            var query = dpq.Where(dp => 
                dp.UserId == uid || // 直接对用户授权
                context.Set<UserRole>().Any(ur => ur.UserId == uid && ur.RoleId == dp.RoleId) || // 通过角色授权
                context.Set<UserOrganizationUnit>().Any(uo => uo.UserId == uid && uo.OrganizationUnitId == dp.UserOrganizationUnit) // 通过组织单位授权
            ).Select(dp => new
            {
                dp.EntityTypeFullName,
                dp.DataOrganizationUnit,
                dp.GrantType
            }).Distinct();

            var result = await query.ToListAsync(CancellationTokenProvider.Token);

            var dtos = new Dictionary<string, DataPermissionDto>();
            var gp = result.GroupBy(x => x.EntityTypeFullName);
            
            // 拒绝优先原则
            foreach (var item in gp)
            {
                var dto = new DataPermissionDto();
                if (item.Any(x => x.GrantType == DataPermissionGrantType.Rejected))
                {
                    dto.GrantType = DataPermissionGrantType.Rejected;
                }
                else if (item.Any(x => x.GrantType == DataPermissionGrantType.OnlyMe))
                {
                    dto.GrantType = DataPermissionGrantType.OnlyMe;
                }
                else if (item.Any(x => x.GrantType == DataPermissionGrantType.OrganizationUnit))
                {
                    dto.GrantType = DataPermissionGrantType.OrganizationUnit;
                    dto.OrganizationUnitIds = item.Where(x => x.GrantType == DataPermissionGrantType.OrganizationUnit && x.DataOrganizationUnit.HasValue)
                        .Select(x => x.DataOrganizationUnit.Value).Distinct();
                }
                else if (item.Any(x => x.GrantType == DataPermissionGrantType.OrganizationUnitRecursive))
                {
                    dto.GrantType = DataPermissionGrantType.OrganizationUnitRecursive;
                    var qt = item.Where(x => x.GrantType == DataPermissionGrantType.OrganizationUnitRecursive && x.DataOrganizationUnit.HasValue)
                        .Select(x => x.DataOrganizationUnit.Value).Distinct();
                    if (qt.Any())
                    {
                        var ouq = context.Set<OrganizationUnit>().AsNoTracking();
                        dto.OrganizationUnitCodes = await ouq.Where(x => qt.Contains(x.Id))
                            .Select(x => x.Code).ToListAsync(CancellationTokenProvider.Token);
                    }
                }
                else
                {
                    dto.GrantType = DataPermissionGrantType.All;
                }
                
                dtos.Add(item.Key, dto);
            }

            return dtos;
        }

        /// <summary>
        /// 判断是否应该应用数据权限过滤（同步版本）
        /// </summary>
        private bool ShouldApplyDataPermission()
        {
            // 检查当前 UnitOfWork 是否启用了数据权限过滤器
            var currentUow = CurrentUnitOfWorkProvider?.Current;
            if (currentUow == null || !currentUow.IsFilterEnabled(DataPermissionConsts.DataPermission))
                return false;

            // 查询元数据，判断该实体类型是否需要进行数据权限控制
            var entityTypeFullName = typeof(TEntity).FullName;
            var context = GetContext();
            return context.Set<MetadataEntity>()
                .Any(m => m.Name == DataPermissionConsts.DataPermission && m.EntityTypeFullName == entityTypeFullName);
        }

        /// <summary>
        /// 判断是否应该应用数据权限过滤（异步版本）
        /// </summary>
        private async Task<bool> ShouldApplyDataPermissionAsync()
        {
            // 检查当前 UnitOfWork 是否启用了数据权限过滤器
            var currentUow = CurrentUnitOfWorkProvider?.Current;
            if (currentUow == null || !currentUow.IsFilterEnabled(DataPermissionConsts.DataPermission))
                return false;

            // 查询元数据，判断该实体类型是否需要进行数据权限控制
            var entityTypeFullName = typeof(TEntity).FullName;
            var context = await GetContextAsync();
            return await context.Set<MetadataEntity>()
                .AnyAsync(m => m.Name == DataPermissionConsts.DataPermission && m.EntityTypeFullName == entityTypeFullName);
        }

        /// <summary>
        /// 根据权限类型应用过滤条件
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyPermissionFilter(IQueryable<TEntity> query, DataPermissionDto permission)
        {
            switch (permission.GrantType)
            {
                case DataPermissionGrantType.Rejected:
                    // 拒绝访问，返回空结果
                    return query.Where(e => false);
                case DataPermissionGrantType.All:
                    // 允许查看全部，不添加过滤
                    return query;
                case DataPermissionGrantType.OnlyMe:
                    // 仅查看自己的数据
                    return ApplyOnlyMeFilter(query);
                case DataPermissionGrantType.OrganizationUnit:
                case DataPermissionGrantType.OrganizationUnitRecursive:
                    // 按组织单位过滤
                    return ApplyOrganizationUnitFilter(query, permission);
                default:
                    // 未知类型，拒绝访问（安全优先）
                    return query.Where(e => false);
            }
        }

        /// <summary>
        /// 应用"仅自己"过滤
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyOnlyMeFilter(IQueryable<TEntity> query)
        {
            // 检查实体是否实现了 ICreationAudited
            if (typeof(ICreationAudited).IsAssignableFrom(typeof(TEntity)))
            {
                var userId = AbpSession.UserId.Value;
                return query.Where(e => ((ICreationAudited)e).CreatorUserId == userId);
            }

            // 如果实体没有创建人字段，则无法过滤，返回空结果
            return query.Where(e => false);
        }

        /// <summary>
        /// 应用组织单位过滤
        /// OrganizationUnitIds 和 OrganizationUnitCodes 之间是 OR 关系
        /// </summary>
        protected virtual IQueryable<TEntity> ApplyOrganizationUnitFilter(IQueryable<TEntity> query, DataPermissionDto permission)
        {
            var entityType = typeof(TEntity);
            var hasIds = permission.OrganizationUnitIds?.Any() == true;
            var hasCodes = permission.OrganizationUnitCodes?.Any() == true;

            // 如果没有组织单位条件，直接返回
            if (!hasIds && !hasCodes)
            {
                return query;
            }

            // 构建总的 OR 条件
            Expression<Func<TEntity, bool>> predicate = PredicateBuilder.New<TEntity>(false);

            // 1. 处理 OrganizationUnitIds（精确匹配）
            if (hasIds)
            {
                var ouIds = permission.OrganizationUnitIds.ToList();

                // 处理 IMayHaveOrganizationUnit
                if (typeof(IMayHaveOrganizationUnit).IsAssignableFrom(entityType))
                {
                    predicate = predicate.Or(e => ouIds.Contains(((IMayHaveOrganizationUnit)e).OrganizationUnitId.Value));
                }

                // 处理 IMustHaveOrganizationUnit
                if (typeof(IMustHaveOrganizationUnit).IsAssignableFrom(entityType))
                {
                    predicate = predicate.Or(e => ouIds.Contains(((IMustHaveOrganizationUnit)e).OrganizationUnitId));
                }
            }

            // 2. 处理 OrganizationUnitCodes（递归，StartWith 匹配）
            if (hasCodes && typeof(IOU).IsAssignableFrom(entityType))
            {
                var codes = permission.OrganizationUnitCodes.Where(c => !string.IsNullOrEmpty(c)).ToList();
                foreach (var code in codes)
                {
                    var codeValue = code; // 捕获变量
                    predicate = predicate.Or(e => ((IOU)e).OrganizationUnit.Code.StartsWith(codeValue));
                }
            }

            return query.Where(predicate);
        }
    }
}
