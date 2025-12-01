using Abp.Domain.Entities.Caching;
using System;

namespace BXJG.Utils.Share.Files
{
    /// <summary>
    /// 下载文件的缓存，参看abp官方文档的租户实体缓存
    /// </summary>
    public interface IDownloadFileCache : IMultiTenancyEntityCache<DownloadFileResult, Guid>
    {
    }
}
