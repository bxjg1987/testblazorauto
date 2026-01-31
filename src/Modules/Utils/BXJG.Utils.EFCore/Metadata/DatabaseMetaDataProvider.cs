using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.Threading;
using Abp.Timing;
using BXJG.Utils.Metadata;
using BXJG.Utils.Share.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.Metadata
{
    /// <summary>
    /// 基于数据库查询的元数据提供者
    /// 默认实现，从数据库中根据Name查询已启用的元数据，并缓存结果
    /// 缓存过期时间：绝对过期30分钟
    /// </summary>
    public class DatabaseMetaDataProvider : IMetaDataProvider, ITransientDependency
    {
        private readonly IRepository<MetadataEntity, long> _metadataRepository;
        private readonly ICacheManager _cacheManager;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DatabaseMetaDataProvider(
            IRepository<MetadataEntity, long> metadataRepository,
            ICacheManager cacheManager,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            _metadataRepository = metadataRepository;
            _cacheManager = cacheManager;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        /// <summary>
        /// 构建缓存键（列表查询）
        /// 格式：MetaData:{name}
        /// </summary>
        private string BuildListCacheKey(string name)
        {
            return $"{MetaDataConsts.MetaDataCacheName}:{name}";
        }

        /// <summary>
        /// 构建缓存键（单个实体查询）
        /// 格式：MetaData:{name}:{entityTypeFullName}
        /// </summary>
        private string BuildSingleCacheKey(string name, string entityTypeFullName)
        {
            return $"{MetaDataConsts.MetaDataCacheName}:{name}:{entityTypeFullName}";
        }

        /// <summary>
        /// 根据Name获取已启用的元数据列表（同步版本）
        /// 缓存键格式：MetaData:{name}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        public List<MetaDataDto> GetMetaData(string name)
        {
            // 获取缓存实例
            var cache = _cacheManager.GetCache<string, List<MetaDataDto>>(MetaDataConsts.MetaDataCacheName);

            // 构建缓存键
            var cacheKey = BuildListCacheKey(name);

            // 尝试从缓存获取（同步版本）
            var cachedResult = cache.GetOrDefault(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            // 从数据库查询已启用的元数据（同步版本）
            var result = LoadMetaDataFromDatabase(name);

            // 写入缓存，绝对过期30分钟（同步版本）
            cache.Set(cacheKey, result, absoluteExpireTime: Clock.Now.AddMinutes(30));

            return result;
        }

        /// <summary>
        /// 异步根据Name获取已启用的元数据列表
        /// 缓存键格式：MetaData:{name}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        public async Task<List<MetaDataDto>> GetMetaDataAsync(string name)
        {
            // 获取缓存实例
            var cache = _cacheManager.GetCache<string, List<MetaDataDto>>(MetaDataConsts.MetaDataCacheName);

            // 构建缓存键
            var cacheKey = BuildListCacheKey(name);

            // 尝试从缓存获取
            var cachedResult = await cache.GetOrDefaultAsync(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            // 从数据库查询已启用的元数据
            var result = await LoadMetaDataFromDatabaseAsync(name);

            // 写入缓存，绝对过期30分钟
            await cache.SetAsync(cacheKey, result, absoluteExpireTime: Clock.Now.AddMinutes(30));

            return result;
        }

        /// <summary>
        /// 根据Name和实体类型全名获取单个已启用的元数据（同步版本）
        /// 缓存键格式：MetaData:{name}:{entityTypeFullName}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        public MetaDataDto? GetMetaDataByEntityType(string name, string entityTypeFullName)
        {
            // 获取缓存实例
            var cache = _cacheManager.GetCache<string, MetaDataDto>(MetaDataConsts.MetaDataCacheName);

            // 构建缓存键
            var cacheKey = BuildSingleCacheKey(name, entityTypeFullName);

            // 尝试从缓存获取（同步版本）
            var cachedResult = cache.GetOrDefault(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            // 从数据库查询
            var result = LoadSingleMetaDataFromDatabase(name, entityTypeFullName);

            // 写入缓存，绝对过期30分钟（同步版本）
            if (result != null)
            {
                cache.Set(cacheKey, result, absoluteExpireTime: Clock.Now.AddMinutes(30));
            }

            return result;
        }

        /// <summary>
        /// 根据Name和实体类型全名获取单个已启用的元数据（异步版本）
        /// 缓存键格式：MetaData:{name}:{entityTypeFullName}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        public async Task<MetaDataDto?> GetMetaDataByEntityTypeAsync(string name, string entityTypeFullName)
        {
            // 获取缓存实例
            var cache = _cacheManager.GetCache<string, MetaDataDto>(MetaDataConsts.MetaDataCacheName);

            // 构建缓存键
            var cacheKey = BuildSingleCacheKey(name, entityTypeFullName);

            // 尝试从缓存获取
            var cachedResult = await cache.GetOrDefaultAsync(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }

            // 从数据库查询
            var result = await LoadSingleMetaDataFromDatabaseAsync(name, entityTypeFullName);

            // 写入缓存，绝对过期30分钟
            if (result != null)
            {
                await cache.SetAsync(cacheKey, result, absoluteExpireTime: Clock.Now.AddMinutes(30));
            }

            return result;
        }

        /// <summary>
        /// 从数据库加载指定Name的已启用元数据列表（同步版本）
        /// 仅查询IsActive为true的记录
        /// </summary>
        private List<MetaDataDto> LoadMetaDataFromDatabase(string name)
        {
            var query = _metadataRepository.GetAllReadonly();

            var result = query
                .Where(m => m.Name == name && m.IsActive)
                .OrderBy(m => m.Code)
                .Select(m => new MetaDataDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    DisplayName = m.DisplayName,
                    Code = m.Code,
                    ParentId = m.ParentId,
                    ChildrenCount = m.ChildrenCount,
                    EntityTypeFullName = m.EntityTypeFullName
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// 从数据库加载指定Name的已启用元数据列表（异步版本）
        /// 仅查询IsActive为true的记录
        /// </summary>
        private async Task<List<MetaDataDto>> LoadMetaDataFromDatabaseAsync(string name)
        {
            var query = await _metadataRepository.GetAllReadonlyAsync();

            var result = await query
                .Where(m => m.Name == name && m.IsActive)
                .OrderBy(m => m.Code)
                .Select(m => new MetaDataDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    DisplayName = m.DisplayName,
                    Code = m.Code,
                    ParentId = m.ParentId,
                    ChildrenCount = m.ChildrenCount,
                    EntityTypeFullName = m.EntityTypeFullName
                })
                .ToListAsync(_cancellationTokenProvider.Token);

            return result;
        }

        /// <summary>
        /// 从数据库加载单个元数据（同步版本）
        /// </summary>
        private MetaDataDto? LoadSingleMetaDataFromDatabase(string name, string entityTypeFullName)
        {
            var query = _metadataRepository.GetAllReadonly();

            var result = query
                .Where(m => m.Name == name && m.IsActive && m.EntityTypeFullName == entityTypeFullName)
                .Select(m => new MetaDataDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    DisplayName = m.DisplayName,
                    Code = m.Code,
                    ParentId = m.ParentId,
                    ChildrenCount = m.ChildrenCount,
                    EntityTypeFullName = m.EntityTypeFullName
                })
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 从数据库加载单个元数据（异步版本）
        /// </summary>
        private async Task<MetaDataDto?> LoadSingleMetaDataFromDatabaseAsync(string name, string entityTypeFullName)
        {
            var query = await _metadataRepository.GetAllReadonlyAsync();

            var result = await query
                .Where(m => m.Name == name && m.IsActive && m.EntityTypeFullName == entityTypeFullName)
                .Select(m => new MetaDataDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    DisplayName = m.DisplayName,
                    Code = m.Code,
                    ParentId = m.ParentId,
                    ChildrenCount = m.ChildrenCount,
                    EntityTypeFullName = m.EntityTypeFullName
                })
                .FirstOrDefaultAsync(_cancellationTokenProvider.Token);

            return result;
        }
    }
}
