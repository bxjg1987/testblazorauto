using Abp.Dependency;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using BXJG.Common;
using Abp.Configuration;
using Abp.Threading.Extensions;
using System.Linq;
using Abp.UI;
using HeyRed.Mime;
using Abp.Threading;
//using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.Extensions.Configuration;
using Abp.Extensions;
using System.Text.RegularExpressions;
using BXJG.Common.Extensions;
using BXJG.Utils.Share.Files;
using BXJG.Utils.Share;
using Abp;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Abp.Timing;
using Abp.Runtime.Session;
using Abp.BackgroundJobs;
namespace BXJG.Utils.Files
{
    /// <summary>
    /// 基于服务器本地文件存储的文件管理领域服务
    /// mvc或web api层的controller接收上传的文件，并通过此类进行文件的验证和保存
    /// 开始将文件存储在临时目录中，确认后移动到正式目录
    /// </summary>
    public class FileManager : BXJGBaseDomainService, IShouldInitialize
    {
        //api服务器根和上传目录是不允许修改的，因此不使用abp的settings系统

        #region 字段和属性
        ///// <summary>
        ///// 获取后端服务器根
        ///// </summary>
        //protected string _serverRootUrl;
        /// <summary>
        /// 获取安全的上传目录，一般是非应用程序目录 可读写 不可执行
        /// </summary>
        protected string _uploadDir;
        /// <summary>
        /// 安全上传时的临时目录
        /// </summary>
        protected string _tempDir;
        /// <summary>
        /// abp提供的异步取消
        /// </summary>
        public ICancellationTokenProvider CancellationTokenProvider { get; set; } = NullCancellationTokenProvider.Instance; //空模式

        //替换 文本中 文件引用的，改用固定字符串替换的方式更简单
        ///// <summary>
        ///// 用来匹配字符串文本中包含的图片地址列表
        ///// 在构造函数中被初始化
        ///// </summary>
        //readonly Regex _regex;
        public IGuidGenerator GuidGenerator { get; set; }
        public IRepository<FileEntity, Guid> Repository { get; set; }

        public IConfiguration Configuration { get; set; }

        public IAbpSession AbpSession { get; set; }
        public IBackgroundJobManager BackgroundJobManager { get; set; }
        #endregion

        //去掉构造函数，便于子类重写
        public void Initialize()
        {
            // _serverRootUrl = Configuration["App:ServerRootAddress"];
            //_regex = new Regex($@"<img.+?src=['""]{_serverRootUrl}(\S+)['""]", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            //_settingManager = settingManager;
            //base.LocalizationSourceName = 
            //_webRootDir = env.WebRoot; // d:\app\wwwroot
            //未考虑并发冲突，也基本不需要
            _uploadDir = Configuration["Upload:SaveDir"]; // d:\app\wwwroot\upload 
            //Directory.CreateDirectory是递归的，这里的处理省了
            if (!Directory.Exists(_uploadDir))
                Directory.CreateDirectory(_uploadDir);
            _tempDir = Path.Combine(_uploadDir, BXJGUtilsConsts.UploadTemp); // d:\app\wwwroot\upload\temp
            if (!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);
        }
        /// <summary>
        /// 验证并将文件存储到temp目录
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public virtual async Task<string> Upload2Temp(Stream stream)
        {
            var ts = await SettingManager.GetSettingValueAsync(BXJGUtilsConsts.SettingKeyUploadType); //设置中允许的后缀名 pdf,jpg...
            var aryts = ts.Split(','); //设置中允许的后缀名 ["pdf","jpg"...]

            //这里可以控制saas用户，最大文件总和
            var sz = await SettingManager.GetSettingValueAsync<int>(BXJGUtilsConsts.SettingKeyUploadSize); //设置中的大小限制

            #region 各种限制
            //var hzm = MimeTypesMap.GetExtension(item.ContentType); //当前文件的后缀名 .jpg
            //if (!aryts.Contains(hzm, StringComparer.OrdinalIgnoreCase)) //类型判断，这种方式只能一定成都起到限制作用，因为前端上传的文件后缀可能被恶意更改
            //    throw new UserFriendlyException($"不允许上传此类型的文件，仅允许{ts}");
            var hzm2 = "." + MimeGuesser.GuessExtension(stream);
            if (!aryts.Contains(hzm2, StringComparer.OrdinalIgnoreCase))
                throw new UserFriendlyException($"不允许上传此类型的文件，仅允许{ts}");
            if (stream.Length > sz * 1024) //大小限制判断    
                throw new UserFriendlyException($"上传的文件大小超过限制，最大为{sz}mb");
            #endregion

            // var output = new FileResult(); //准备返回值

            #region 保存文件

            //var hz = Path.GetExtension(item.FileName); //文件后缀.jpg
            var wjm = GuidGenerator.Create().ToString("n") + hzm2; //xxx.jpg  xxx=guid

            //var dateDir = Path.Combine(_tempDir, DateTime.Now.ToString("yyyyMMdd")); //d:\app\wwwroot\upload\temp\20201003
            //if (!Directory.Exists(dateDir))
            //    Directory.CreateDirectory(dateDir);

            var absolutePath = Path.Combine(_tempDir, wjm); //d:\upload\temp\xxx.jpg
                                                            // var rileRelativePath = Absolute2RelativePath(absolutePath); //\temp\20201003\xxx.jpg
                                                            //output.FileName = item.FileName.Split('.').First();
                                                            //output.FileUrl = Relative2AbsoluteUrl(output.FileRelativePath.DirectorySeparatorChar2UrlSeparatorChar());
            using (var fs = File.Create(absolutePath))
            {
                await stream.CopyToAsync(fs, CancellationTokenProvider.Token);
            }
            #endregion

            return Absolute2RelativePath(absolutePath);
            //  return absolutePath;
        }

        public virtual async Task<FileEntity> Upload(string fileName, string fileRelativePath)
        {
            var file = await AddFileRecord(fileName, fileRelativePath);
            Move(fileRelativePath, file);
            return file;
        }

        /// <summary>
        /// 生成文件的数据库记录
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="tempFileRelativePath"></param>
        /// <returns></returns>
        protected virtual async Task<FileEntity> AddFileRecord(string fileName, string tempFileRelativePath)
        {
            /*
             * dbcontext是一个请求一个实例，所以在业务系统中先开事务，然后执行此逻辑，最后提交事务
             */
            // ur.TempPath = ur.TempPath.AESDecryptUtf8String();

            //临时文件的绝对路径
            var jdlj = Relative2AbsolutePath(tempFileRelativePath);

            #region 存储数据
            var file = new FileEntity
            {
                Ext = Path.GetExtension(tempFileRelativePath),
                // FullName = ur.FileName,
                Id = Guid.Parse(Path.GetFileNameWithoutExtension(tempFileRelativePath)),
                //Name = Path.GetFileName(ur.TempPath),
                //Status = FileStatus.Moving,
                RealName = fileName,
                ResponseContentType = MimeGuesser.GuessMimeType(jdlj), //ur.ContentType,

                //RelativePath = Path.Combine( Clock.Now.ToString("yyyyMMdd"), Path.GetFileName(ur.TempPath)),
                //ThumbnailRelativePath = Path.Combine(TimingProvider.Get().ToString("yyyyMMdd"), Path.GetFileName(ur.TempPath).Replace(".", "_thum.")),

            };
            var nyr = Clock.Now.ToString("yyyyMMdd");
            file.RelativePath = Path.Combine(nyr, Path.GetFileName(tempFileRelativePath));
            if (file.ResponseContentType.StartsWith("image/"))
            {
                file.RelativePathThumbnail = Path.Combine(nyr, Path.GetFileNameWithoutExtension(tempFileRelativePath) + ".jpg");
            }

            if (AbpSession.TenantId.HasValue)
            {
                file.RelativePath = Path.Combine(AbpSession.TenantId.Value.ToString(), file.RelativePath);
                if (file.ResponseContentType.StartsWith("image/"))
                {
                    file.RelativePathThumbnail = Path.Combine(AbpSession.TenantId.Value.ToString(), file.RelativePathThumbnail);
                }
            }

            //if (permissions != default)
            //{
            //    file.Permissions = permissions.Select(c => new FilePermissionEntity
            //    {
            //        Id = file.Id,
            //        PermissionName = c
            //    }).ToList();
            //}

            await Repository.InsertAsync(file);
            //await db.Set<FileEntity>().AddAsync(file, cancellationToken);
            //await db.SaveChangesAsync(cancellationToken);
            #endregion
            return file;
        }

        //public string GetDownloadUrl(string relativeUrl)
        //{
        //    relativeUrl = relativeUrl.UrlSeparatorChar2DirectorySeparatorChar();
        //    relativeUrl = Relative2AbsolutePath(relativeUrl);
        //    using (var fs = System.IO.File.OpenRead(relativeUrl))
        //    {
        //        act(fs);
        //    }
        //}

        protected virtual void Move(string tempRelativePath, FileEntity file, int w = 180)
        {
            var des = Relative2AbsolutePath(file.RelativePath);                     //目标文件绝对路径
            var dir = Path.GetDirectoryName(des);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.Move(Relative2AbsolutePath(tempRelativePath), des);
            if (file.RelativePathThumbnail.IsNotNullOrWhiteSpaceBXJG())
            {
                //file.RelativePathThumbnail = Path.Combine(TimingProvider.Get().ToString("yyyyMMdd"), Path.GetFileNameWithoutExtension(file.RelativePath)) + ".jpg";
                ImageHelper.MakeThumb(des, Relative2AbsolutePath(file.RelativePathThumbnail));
                //Helper.MakeThumb(des, Relative2Absolute(file.RelativePathThumbnail), w);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public virtual async Task Remove(FileEntity file)
        {
            await Repository.DeleteAsync(file);
            //BackgroundJob.Enqueue(() => File.Delete(Relative2AbsolutePath(file.RelativePath)));
            //BackgroundJob.Enqueue(() => File.Delete(Relative2AbsolutePath(file.RelativePathThumbnail)));
            await BackgroundJobManager.EnqueueAsync<DeleteFileBackgroundJob, string>(Relative2AbsolutePath(file.RelativePath));
            await BackgroundJobManager.EnqueueAsync<DeleteFileBackgroundJob, string>(Relative2AbsolutePath(file.RelativePathThumbnail));
        }

        //public void Execute(sdfsdf args)
        //{
        //    File.Delete(args.file1);
        //    File.Delete(args.file2);
        //}

        //#region 文章中的图片处理

        ///// <summary>
        ///// 获取指定内容中所包含的图片的相对路径
        ///// /aaa/bbb/cc.jpg
        ///// </summary>
        ///// <param name="content"></param>
        ///// <returns></returns>
        //public string[] GetMatchedImagePath(string content)
        //{
        //    //Regex rg = new Regex($@"<img.+src=('|""){server}/(\S+)('|"")", RegexOptions.Multiline);
        //    return _regex.Matches(content).Select(c => c.Groups[1].Value).ToArray();
        //}

        ///// <summary>
        ///// 去掉文本中图片地址的temp路径片段
        ///// 建议在保存时调用，而不是每次查询时替换
        ///// </summary>
        ///// <param name="content"></param>
        ///// <returns></returns>
        //public string ReplaceImagePath(string content)
        //{
        //    return _regex.Replace(content, c => c.Value.Replace("temp" + Path.DirectorySeparatorChar, ""));
        //}

        //#endregion

        ///// <summary>
        ///// 将不带host的文件相对路径转换为完整路径
        ///// /upload/20201101/a.jpg -> http://www.xx.com/upload/20201101/a.jpg
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public string Relative2AbsoluteUrl(string p)
        //{
        //    if (!p.StartsWith(_serverRootUrl))
        //        return _serverRootUrl + p.TrimStart('/');
        //    return p;
        //}

        ///// <summary>
        ///// 将带host的完整文件路径转换为相对路径
        ///// http://www.xx.com/upload/20201101/a.jpg -> /upload/20201101/a.jpg
        ///// </summary>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //public string Absolute2RelativeUrl(string p)
        //{
        //    if (p.StartsWith(_serverRootUrl))
        //        return p.Substring(_serverRootUrl.Length);
        //    return p;
        //}

        ///// <summary>
        ///// 转换为缩略图路径
        ///// ...xx.jpg -> ...xxthum.jpg
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public string ConvertToThumPath(string path)
        //{
        //    if (!path.Contains("thum."))
        //        return path.Insert(path.LastIndexOf("."), "thum");
        //    return path;
        //}

        /// <summary>
        /// 绝对路径转相对路径
        /// d:\upload\20201022\a.jpg -> 20201022\a.jpg  
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string Absolute2RelativePath(string path)
        {
            if (path.StartsWith(_uploadDir))
                return path.Substring(_uploadDir.Length + 1);
            return path;
        }

        /// <summary>
        /// 相对路径转绝对路径
        /// upload\20201022\a.jpg -> d:\\upload\20201022\a.jpg
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string Relative2AbsolutePath(string path)
        {
            if (!path.StartsWith(_uploadDir))
                return Path.Combine(_uploadDir, path);
            return path;
        }

        ///// <summary>
        ///// 临时目录转换为正式目录
        ///// 去掉temp
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public string TempToOkPath(string path)
        //{
        //    return path.Replace(BXJGUtilsConsts.UploadTemp, BXJGUtilsConsts.UploadDir).Replace(BXJGUtilsConsts.UploadTempUrl, BXJGUtilsConsts.UploadDir);
        //}

        ///// <summary>
        ///// 尝试获取缩略图的相对url
        ///// </summary>
        ///// <param name="absoluteUrl">绝对url</param>
        ///// <returns></returns>
        //public string TryGetThumRelativeUrl(string absoluteUrl)
        //{
        //    absoluteUrl = Absolute2RelativeUrl(absoluteUrl);
        //    var url = absoluteUrl = ConvertToThumPath(absoluteUrl);
        //    absoluteUrl = absoluteUrl.UrlSeparatorChar2DirectorySeparatorChar();
        //    absoluteUrl = Relative2AbsolutePath(absoluteUrl);
        //    if (System.IO.File.Exists(absoluteUrl))
        //        return url;
        //    return default;
        //}

        //public void Initialize()
        //{
        // // AsyncHelper.RunSync
        //}
    }
}