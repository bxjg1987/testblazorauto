using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using BXJG.Utils.Share.Files;
using Masuit.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.Utils.Files
{
    /// <summary>
    /// 下载器
    /// </summary>
    public class FileDownloader : FileManagerBase
    {
      public IDownloadFileCache DownloadFileCache { get; set; }

        //public FileDownloader(IDownloadFileCache downloadFileCache)
        //{
        //    DownloadFileCache = downloadFileCache;
        //}
        public IRepository<FileEntity,Guid > Repository { get; set; }

        public IAbpSession AbpSession { get; set; }
        /// <summary>
        /// 获取指定文件的绝对路径
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns>文件的物理路径和content-type</returns>
        public virtual async Task< DownloadFileResult> GetAbsolutePath(Guid id)
        {
            base.Logger.Debug($"正在获取文件的物理路径，文件id：{id} 缓存器：{DownloadFileCache==default} 租户：{AbpSession.TenantId}");

            // var r = DownloadFileCache[id];
            var r = await DownloadFileCache.GetAsync(id);

           // var r1 = await Repository.GetAll().Where(x => x.Id == id).SingleAsync();
           // base.ObjectMapper.Map< DownloadFileResult >(r1 );


            //base.Logger.Debug("从缓存获取的对象："+System.Text.Json.JsonSerializer.Serialize(r));
            //r = r.DeepClone(); 路径转换内部有判读，所以这里无需克隆

            r.RelativePath = RelativeToAbsolutePath(r.RelativePath);
            r.RelativePathThumbnail = RelativeToAbsolutePath(r.RelativePathThumbnail);
            return r;
        }
        ///// <summary>
        ///// 获取指定文件的缩略图的绝对路径
        ///// </summary>
        ///// <param name="id">文件id</param>
        ///// <returns>文件缩略图的物理路径</returns>
        //public virtual DownloadFileResult GetAbsolutePathThum(Guid id)
        //{
        //    var r = DownloadFileCache[id];//await Repository.GetAll().Where(x => x.Id == id).Select(x => new DownloadFileResult { Path = x.RelativePathThumbnail, Name = x.RealFullName }).SingleAsync(CancellationTokenProvider.Token);
        //    r.RelativePath = RelativeToAbsolutePath(r.RelativePath);
        //    r.ResponseContentType = "image/jpeg";
        //    return r;
        //}
    }
}
