using System.Threading.Tasks;

namespace BXJG.Utils.Share.Metadata
{
    /// <summary>
    /// IMetaDataProvider 扩展方法
    /// 提供基于泛型类型参数获取单个元数据的便捷方法
    /// </summary>
    public static class MetaDataProviderExtensions
    {
        /// <summary>
        /// 根据泛型类型参数获取单个已启用的元数据（同步版本）
        /// 自动从类型参数获取类型全名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="provider">元数据提供者</param>
        /// <param name="name">对应元数据树的Name属性</param>
        /// <returns>元数据DTO，如果不存在则返回null</returns>
        public static MetaDataDto? GetMetaDataByEntityType<TEntity>(this IMetaDataProvider provider, string name)
        {
            var entityTypeFullName = typeof(TEntity).FullName;
            return provider.GetMetaDataByEntityType(name, entityTypeFullName);
        }

        /// <summary>
        /// 异步根据泛型类型参数获取单个已启用的元数据
        /// 自动从类型参数获取类型全名
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="provider">元数据提供者</param>
        /// <param name="name">对应元数据树的Name属性</param>
        /// <returns>元数据DTO，如果不存在则返回null</returns>
        public static async Task<MetaDataDto?> GetMetaDataByEntityTypeAsync<TEntity>(this IMetaDataProvider provider, string name)
        {
            var entityTypeFullName = typeof(TEntity).FullName;
            return await provider.GetMetaDataByEntityTypeAsync(name, entityTypeFullName);
        }
    }
}
