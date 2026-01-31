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
        /// 根据Name获取已启用的元数据列表（同步版本）
        /// 缓存键格式：MetaData:{name}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        public List<MetaDataDto> GetEnabledMetaData(string name)
        {
            return AsyncHelper.RunSync(() => GetEnabledMetaDataAsync(name));
        }

        /// <summary>
        /// 异步根据Name获取已启用的元数据列表
        /// 缓存键格式：MetaData:{name}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        public async Task<List<MetaDataDto>> GetEnabledMetaDataAsync(string name)
        {
            // 获取缓存实例
            var cache = _cacheManager.GetCache<string, List<MetaDataDto>>(MetaDataConsts.MetaDataCacheName);

            // 构建缓存键：MetaData:{name}
            var cacheKey = $"{MetaDataConsts.MetaDataCacheName}:{name}";

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
        /// 从数据库加载指定Name的已启用元数据列表
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
    }
}
