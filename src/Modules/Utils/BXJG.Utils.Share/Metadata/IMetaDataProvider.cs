using System.Collections.Generic;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Metadata
{
    /// <summary>
    /// 元数据提供者接口
    /// 用于根据Name获取已启用的元数据列表
    /// </summary>
    public interface IMetaDataProvider
    {
        /// <summary>
        /// 根据Name获取已启用的元数据列表
        /// 缓存键格式：Name:{name}
        /// 缓存过期时间：绝对过期5分钟
        /// </summary>
        /// <param name="name">元数据Name属性值</param>
        /// <returns>元数据DTO列表</returns>
        List<MetaDataDto> GetEnabledMetaData(string name);

        /// <summary>
        /// 异步根据Name获取已启用的元数据列表
        /// 缓存键格式：Name:{name}
        /// 缓存过期时间：绝对过期5分钟
        /// </summary>
        /// <param name="name">元数据Name属性值</param>
        /// <returns>元数据DTO列表</returns>
        Task<List<MetaDataDto>> GetEnabledMetaDataAsync(string name);
    }
}
