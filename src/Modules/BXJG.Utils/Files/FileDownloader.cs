using BXJG.Utils.Share.Files;
using Masuit.Tools;
using System;

namespace BXJG.Utils.Files
{
    /// <summary>
    /// 下载器
    /// </summary>
    public class FileDownloader : FileManagerBase
    {
        public virtual IDownloadFileCache DownloadFileCache { get; set; }
        /// <summary>
        /// 获取指定文件的绝对路径
        /// </summary>
        /// <param name="id">文件id</param>
        /// <returns>文件的物理路径和content-type</returns>
        public virtual DownloadFileResult GetAbsolutePath(Guid id)
        {
            var r = DownloadFileCache[id];// await Repository.GetAll().Where(x => x.Id == id).Select(x => new DownloadFileResult { Path = x.RelativePath, ContentType = x.ResponseContentType, Name = x.RealFullName }).SingleAsync(CancellationTokenProvider.Token);

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
