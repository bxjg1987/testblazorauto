using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Timing;
using BXJG.Utils.DataPermission;
using BXJG.Utils.Share.DataPermission;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.DataPermission
{
    /// <summary>
    /// 基于数据库查询的数据权限规则提供者
    /// 默认实现，从数据库中查询用户对指定实体类型的数据权限规则，并缓存结果
    /// 缓存键格式：租户ID + ":" + 用户ID + ":" + 实体类型全名
    /// 缓存过期时间：绝对过期5分钟
    /// </summary>
    public class DatabaseDataPermissionRuleProvider : IDataPermissionRuleProvider, ITransientDependency
    {
        private readonly IRepository<DataPermissionEntity, Guid> _dataPermissionRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly ICacheManager _cacheManager;
        private readonly IAbpSession _abpSession;


        /// <summary>
        /// 构造函数
        /// </summary>
        public DatabaseDataPermissionRuleProvider(
            IRepository<DataPermissionEntity, Guid> dataPermissionRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            ICancellationTokenProvider cancellationTokenProvider,
            ICacheManager cacheManager,
            IAbpSession abpSession)
        {
            _dataPermissionRepository = dataPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _cancellationTokenProvider = cancellationTokenProvider;
            _cacheManager = cacheManager;
            _abpSession = abpSession;
        }

        /// <summary>
        /// 获取当前用户对指定实体类型的数据权限规则（同步版本）
        /// 内部自动解析IAbpSession获取用户ID和租户ID
        /// </summary>
        public DataPermissionDto GetRules(string entityTypeFullName)
        {
            return AsyncHelper.RunSync(() => GetRulesAsync(entityTypeFullName));
        }

        /// <summary>
        /// 异步获取当前用户对指定实体类型的数据权限规则
        /// 内部自动解析IAbpSession获取用户ID和租户ID
        /// </summary>
        public async Task<DataPermissionDto> GetRulesAsync(string entityTypeFullName)
        {
            var userId = _abpSession.UserId.Value;
            var tenantId = _abpSession.TenantId;

            // 获取缓存实例
            var cache = _cacheManager.GetCache<string, DataPermissionDto>(DataPermissionConsts.DataPermission);

            // 构建缓存键：租户ID + 用户ID + 实体类型全名
            var cacheKey = $"{tenantId?.ToString() ?? "null"}:{userId}:{entityTypeFullName}";

            // 尝试从缓存获取
            var cachedResult = await cache.GetOrDefaultAsync(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            // 从数据库查询
            var result = await LoadRulesFromDatabaseAsync(userId, entityTypeFullName);

            // 写入缓存，绝对过期5分钟
            await cache.SetAsync(cacheKey, result, absoluteExpireTime: Clock.Now.AddMinutes(5));

            return result;
        }

        /// <summary>
        /// 从数据库加载指定实体类型的数据权限规则
        /// </summary>
        private async Task<DataPermissionDto> LoadRulesFromDatabaseAsync(long userId, string entityTypeFullName)
        {
            var dpq = await _dataPermissionRepository.GetAllReadonlyAsync();
            var urq = await _userRoleRepository.GetAllReadonlyAsync();
            var uoq = await _userOrganizationUnitRepository.GetAllReadonlyAsync();

            // 左连接用户角色和用户组织单位查询数据权限
            var query = (from dp in dpq
                         join ur in urq on dp.RoleId equals ur.RoleId into urGroup
                         from ur in urGroup.DefaultIfEmpty()
                         join uo in uoq on dp.UserOrganizationUnit equals uo.UserId into uoGroup
                         from uo in uoGroup.DefaultIfEmpty()
                         where dp.EntityTypeFullName == entityTypeFullName &&
                               (dp.UserId == userId || ur.UserId == userId || uo.UserId == userId)
                         select new
                         {
                             dp.EntityTypeFullName,
                             dp.DataOrganizationUnit,
                             dp.GrantType
                         }).Distinct();

            var result = await query.ToListAsync(_cancellationTokenProvider.Token);

            var dtos = new DataPermissionDto();
            //只有整体规则，但是可能多个规则冲突，按理说这种冲突应该在保存规则时阻止保存
            //同一个人对同一个数据类型，不按部门设置规则时，应该只有一条规则
            if (result.All(x => !x.DataOrganizationUnit.HasValue) && result.Any())
            {
                dtos.GrantType = result.First().GrantType;
            }
            else if (result.Count >= 1)
            {
                dtos.OrganizationUnits = new List<DataPermissionOrganizationUnitDto>();
                // 将查询结果转换为DTO列表
                foreach (var item in result.Where(x => x.DataOrganizationUnit.HasValue))
                {
                    var dto = new DataPermissionOrganizationUnitDto
                    {
                        GrantType = item.GrantType
                    };
                    if (item.GrantType == DataPermissionGrantType.OrganizationUnit)
                    {
                        dto.OrganizationUnitId = item.DataOrganizationUnit;
                    }
                    else if (item.GrantType == DataPermissionGrantType.All)
                    {
                        var ouq = await _organizationUnitRepository.GetAllReadonlyAsync();
                        var code = await ouq.Where(x => x.Id == item.DataOrganizationUnit.Value)
                            .Select(x => x.Code)
                            .FirstOrDefaultAsync(_cancellationTokenProvider.Token);
                            dto.OrganizationUnitCode = code;
                    }
                    dtos.OrganizationUnits.Add(dto);
                }
            }
            else
                return null;
            return dtos;
        }
    }
}
