using Abp.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Metadata
{
    /// <summary>
    /// IMetaDataProvider 扩展方法
    /// 提供基于实体类型名称获取单个元数据的便捷方法
    /// </summary>
    public static class MetaDataProviderExtensions
    {
        /// <summary>
        /// 根据实体类型名称获取单个已启用的元数据（同步版本）
        /// 内部调用 GetEnabledMetaData 方法并筛选匹配 EntityTypeFullName 的第一条记录
        /// </summary>
        /// <param name="provider">元数据提供者</param>
        /// <param name="name">对应元数据树的Name属性</param>
        /// <param name="entityTypeFullName">实体类型全名</param>
        /// <returns>元数据DTO，如果不存在则返回null</returns>
        public static MetaDataDto? GetEnabledMetaDataByEntityType(this IMetaDataProvider provider, string name, string entityTypeFullName)
        {
            return AsyncHelper.RunSync(() => GetEnabledMetaDataByEntityTypeAsync(provider, name, entityTypeFullName));
        }

        /// <summary>
        /// 根据泛型类型参数获取单个已启用的元数据（同步版本）
        /// 自动从类型参数获取类型全名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="provider">元数据提供者</param>
        /// <param name="name">对应元数据树的Name属性</param>
        /// <returns>元数据DTO，如果不存在则返回null</returns>
        public static MetaDataDto? GetEnabledMetaDataByEntityType<TEntity>(this IMetaDataProvider provider, string name)
        {
            return AsyncHelper.RunSync(() => GetEnabledMetaDataByEntityTypeAsync<TEntity>(provider, name));
        }

        /// <summary>
        /// 异步根据实体类型名称获取单个已启用的元数据
        /// 内部调用 GetEnabledMetaDataAsync 方法并筛选匹配 EntityTypeFullName 的第一条记录
        /// </summary>
        /// <param name="provider">元数据提供者</param>
        /// <param name="name">对应元数据树的Name属性</param>
        /// <param name="entityTypeFullName">实体类型全名</param>
        /// <returns>元数据DTO，如果不存在则返回null</returns>
        public static async Task<MetaDataDto?> GetEnabledMetaDataByEntityTypeAsync(this IMetaDataProvider provider, string name, string entityTypeFullName)
        {
            // 使用完整的实体类型名称查询元数据
            var list = await provider.GetEnabledMetaDataAsync(name);

            // 在内存中筛选匹配 EntityTypeFullName 的第一条记录
            return list.FirstOrDefault(m => m.EntityTypeFullName == entityTypeFullName);
        }

        /// <summary>
        /// 异步根据泛型类型参数获取单个已启用的元数据
        /// 自动从类型参数获取类型全名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="provider">元数据提供者</param>
        /// <param name="name">对应元数据树的Name属性</param>
        /// <returns>元数据DTO，如果不存在则返回null</returns>
        public static async Task<MetaDataDto?> GetEnabledMetaDataByEntityTypeAsync<TEntity>(this IMetaDataProvider provider, string name)
        {
            var entityTypeFullName = typeof(TEntity).FullName;
            return await GetEnabledMetaDataByEntityTypeAsync(provider, name, entityTypeFullName);
        }
    }
}
