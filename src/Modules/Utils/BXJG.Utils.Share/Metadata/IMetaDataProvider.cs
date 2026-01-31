using System.Collections.Generic;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Metadata
{
    /// <summary>
    /// 元数据提供者接口
    /// 用于根据Name获取已启用的元数据列表或单个元数据
    /// </summary>
    public interface IMetaDataProvider
    {
        /// <summary>
        /// 根据Name获取已启用的元数据列表
        /// 缓存键格式：MetaData:{name}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        /// <param name="name">元数据Name属性值</param>
        /// <returns>元数据DTO列表</returns>
        List<MetaDataDto> GetMetaData(string name);

        /// <summary>
        /// 异步根据Name获取已启用的元数据列表
        /// 缓存键格式：MetaData:{name}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        /// <param name="name">元数据Name属性值</param>
        /// <returns>元数据DTO列表</returns>
        Task<List<MetaDataDto>> GetMetaDataAsync(string name);

        /// <summary>
        /// 根据Name和实体类型全名获取单个已启用的元数据（同步版本）
        /// 缓存键格式：MetaData:{name}:{entityTypeFullName}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        /// <param name="name">对应元数据树的Name属性</param>
        /// <param name="entityTypeFullName">实体类型全名</param>
        /// <returns>元数据DTO，如果不存在则返回null</returns>
        MetaDataDto? GetMetaDataByEntityType(string name, string entityTypeFullName);

        /// <summary>
        /// 根据Name和实体类型全名获取单个已启用的元数据（异步版本）
        /// 缓存键格式：MetaData:{name}:{entityTypeFullName}
        /// 缓存过期时间：绝对过期30分钟
        /// </summary>
        /// <param name="name">对应元数据树的Name属性</param>
        /// <param name="entityTypeFullName">实体类型全名</param>
        /// <returns>元数据DTO，如果不存在则返回null</returns>
        Task<MetaDataDto?> GetMetaDataByEntityTypeAsync(string name, string entityTypeFullName);
    }
}
