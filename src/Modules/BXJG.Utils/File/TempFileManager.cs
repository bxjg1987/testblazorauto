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
namespace BXJG.Utils.File
{
    /*
     * 参考文档：https://gitee.com/bxjg1987_admin/abp/wikis/文件管理?sort_id=2946614
     * 
     * 这里文件类型、大小限制是通过Utils模块提供的settings来控制的，是全局的
     * 正常情况下模块调用方可能需要提供自己的限制，但是目前没有提供这样的扩展点
     * 
     * 将文件管理看成一个独立的，通用的功能，那么文件上传的请求方应该提供一个标识来识别是哪各模块进行上传的。然后通过此标识找到对应模块提供的处理器对文件管理实施额外逻辑
     * 或模块调用方通过集成的方式提供自己的controller和fileManager
     * 以上两种方式都可以实现扩展，推荐前一种方式
     * 
     * 利用abp提供的后台任务删除temp目录中长时间未被移走的文件
     * 
     * 这是一个临时性的实现，基于服务器本地文件存储
     */

    /// <summary>
    /// 这是一个临时性的实现，基于服务器本地文件存储的文件管理领域服务
    /// mvc或web api层的controller接收上传的文件，并通过此类进行文件的验证和保存
    /// 参考文档：<see href="https://gitee.com/bxjg1987_admin/abp/wikis/文件管理?sort_id=2946614"/> 
    /// 将来迁移到vNext后将弃用此功能，使用vNext自带的文件管理模块
    /// </summary>
    public class TempFileManager : DomainService //, ITransientDependency
    {
        #region 字段和属性

        /// <summary>
        /// 根url地址
        /// http://xxx.xxx.xx:port/
        /// </summary>
        readonly string _serverRootUrl;

        /// <summary>
        /// web根目录
        /// window中为d:\app\wwwroot
        /// </summary>
        readonly string _webRootDir;

        /// <summary>
        /// 文件上传的根目录
        /// window中d:\app\wwwroot\upload 
        /// </summary>
        readonly string _uploadDir;

        /// <summary>
        /// 文件上传的临时目录
        /// window中d:\app\wwwroot\upload\temp
        /// </summary>
        readonly string _tempDir;

        /// <summary>
        /// abp提供的设置管理器
        /// </summary>
        readonly ISettingManager _settingManager;

        /// <summary>
        /// abp提供的异步取消
        /// </summary>
        public ICancellationTokenProvider cancellationTokenProvider { get; set; } = NullCancellationTokenProvider.Instance; //空模式

        /// <summary>
        /// 用来匹配字符串文本中包含的图片地址列表
        /// 在构造函数中被初始化
        /// </summary>
        readonly Regex _regex;

        #endregion

        /// <summary>
        /// 一个简单的文件管理领域服务
        /// mvc或web api层的controller接收上传的文件，并通过此类进行文件的验证和保存
        /// 参考文档：https://gitee.com/bxjg1987_admin/abp/wikis/文件管理?sort_id=2946614
        /// </summary>
        /// <param name="env">获取当前应用的相对路径</param>
        /// <param name="settingManager">abp提供的settings系统</param>
        public TempFileManager(IEnv env, ISettingManager settingManager)
        {
            _serverRootUrl = env.RootUrl;// configuration["App:ServerRootAddress"];
            _regex = new Regex($@"<img.+?src=['""]{_serverRootUrl}(\S+)['""]",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);
            _settingManager = settingManager;

            _webRootDir = env.WebRoot; // d:\app\wwwroot
            //未考虑并发冲突，也基本不需要
            _uploadDir = Path.Combine(env.WebRoot, Consts.UploadDir); // d:\app\wwwroot\upload 
            //Directory.CreateDirectory是递归的，这里的处理省了
            //if (!Directory.Exists(_uploadDir))
            //    Directory.CreateDirectory(_uploadDir);
            _tempDir = Path.Combine(env.WebRoot, Consts.UploadTemp); // d:\app\wwwroot\upload\temp
            //if (!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);
        }

        /// <summary>
        /// 验证并将文件存储到temp目录
        /// </summary>
        /// <param name="createThum">上传是图片且此值为true时将生成缩略图</param>
        /// <param name="thumSize">需要创建缩略图时，缩略图的宽度，高度自动等比例缩放</param>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public async Task<List<FileResult>> UploadAsync(bool createThum = false, int thumSize = 500, params FileInput[] inputs)
        {
            var outputs = new List<FileResult>(); //准备返回值

            var ts = await _settingManager.GetSettingValueAsync(Consts.SettingKeyUploadType); //设置中允许的后缀名 pdf,jpg...
            var aryts = ts.Split(','); //设置中允许的后缀名 ["pdf","jpg"...]
            var sz = await _settingManager.GetSettingValueAsync<int>(Consts.SettingKeyUploadSize); //设置中的大小限制

            foreach (var item in inputs)
            {
                #region 各种限制

                var hzm = MimeTypesMap.GetExtension(item.ContentType); //当前文件的后缀名 .jpg
                if (!aryts.Contains(hzm, StringComparer.OrdinalIgnoreCase)) //类型判断，这种方式只能一定成都起到限制作用，因为前端上传的文件后缀可能被恶意更改
                    throw new UserFriendlyException($"不允许上传此类型的文件，仅允许{ts}");

                if (item.Length > sz * 1024) //大小限制判断    
                    throw new UserFriendlyException($"上传的文件大小超过限制，最大为{sz}Kb");

                #endregion

                var output = new FileResult(); //准备返回值

                #region 保存文件

                var hz = Path.GetExtension(item.FileName); //文件后缀.jpg
                var wjm = Guid.NewGuid().ToString("n") + hz; //xxx.jpg  xxx=guid
                var dateDir = Path.Combine(_tempDir, DateTime.Now.ToString("yyyyMMdd")); //d:\app\wwwroot\upload\temp\20201003
                //if (!Directory.Exists(dateDir))
                    Directory.CreateDirectory(dateDir);
                output.FileAbsolutePath = Path.Combine(dateDir, wjm); //d:\app\wwwroot\upload\temp\20201003\xxx.jpg
                output.FileRelativePath = Absolute2RelativePath(output.FileAbsolutePath); //\upload\temp\20201003\xxx.jpg
                output.FileUrl = Relative2AbsoluteUrl(output.FileRelativePath.DirectorySeparatorChar2UrlSeparatorChar());
                using (var fs = System.IO.File.Create(output.FileAbsolutePath))
                {
                    await item.Stream.CopyToAsync(fs, cancellationTokenProvider.Token);
                }

                #endregion

                #region 生成缩略图

                if (createThum && item.ContentType.Contains("image"))
                {
                    output.ThumAbsolutePath = ConvertToThumPath(output.FileAbsolutePath);
                    output.ThumRelativePath = Absolute2RelativePath(output.ThumAbsolutePath);
                    output.ThumUrl = ConvertToThumPath(output.FileUrl);
                    //参考：https://docs.sixlabors.com/articles/imagesharp/resize.html
                    //经过测试这里load item.Stream会报错
                    using var img = await Image.LoadAsync(output.FileAbsolutePath, cancellationTokenProvider.Token);
                    img.Mutate(x => x.Resize(thumSize, 0)); //按给定的宽度缩放
                    await img.SaveAsync(output.ThumAbsolutePath);
                }

                #endregion

                outputs.Add(output);
            }

            return outputs;
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

        /// <summary>
        /// 将temp目录中的文件和缩略图(如果有)移动到正式目录，
        /// 若是非temp目录的文件则不移动，但依然会处理返回值。
        /// 注意它不是事务性的
        /// </summary>
        /// <param name="urls">相对或绝对url</param>
        /// <returns></returns>
        public ValueTask<List<FileResult>> MoveAsync(params string[] urls)
        {
            var list = new List<FileResult>();
            foreach (var file in urls)
            {
                string item = file;

                item = Absolute2RelativeUrl(item);

                item = item.UrlSeparatorChar2DirectorySeparatorChar();

                //修改时，有部分文件是不需要移动的
                if (!item.Contains(Consts.UploadTemp, StringComparison.OrdinalIgnoreCase))
                {
                    var rr = new FileResult
                    {
                        FileAbsolutePath = Relative2AbsolutePath(item),
                        FileRelativePath = item,
                        FileUrl = Relative2AbsoluteUrl(item.DirectorySeparatorChar2UrlSeparatorChar())
                    };
                    rr.ThumAbsolutePath = ConvertToThumPath(rr.FileAbsolutePath);
                    if (System.IO.File.Exists(rr.ThumAbsolutePath))
                    {
                        rr.ThumRelativePath = Absolute2RelativePath(rr.ThumAbsolutePath);
                        rr.ThumUrl = ConvertToThumPath(rr.FileUrl);
                    }
                    else
                        rr.ThumAbsolutePath = default;

                    list.Add(rr);
                    continue;
                }

                var moveResult = new FileResult();

                //移动文件
                var sourceAbsolutePath = Relative2AbsolutePath(item); //temp绝对路径
                moveResult.FileRelativePath = TempToOkPath(item); //正式目录相对路径
                moveResult.FileUrl = Relative2AbsoluteUrl(moveResult.FileRelativePath.DirectorySeparatorChar2UrlSeparatorChar());
                moveResult.FileAbsolutePath = Relative2AbsolutePath(moveResult.FileRelativePath); //正式目录绝对路径
                if (!System.IO.File.Exists(moveResult.FileAbsolutePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(moveResult.FileAbsolutePath));
                    System.IO.File.Move(sourceAbsolutePath, moveResult.FileAbsolutePath);
                }

                //移动缩略图
                var thumItem = ConvertToThumPath(item); //temp缩略图相对路径
                var sourceThumAbsolutePath = Relative2AbsolutePath(thumItem); //temp缩略图绝对路径
                if (System.IO.File.Exists(sourceThumAbsolutePath))
                {
                    moveResult.ThumRelativePath = TempToOkPath(thumItem); //缩略图正式目录相对路径
                    moveResult.ThumAbsolutePath = Relative2AbsolutePath(moveResult.ThumRelativePath); //缩略图正式目录绝对路径
                    moveResult.ThumUrl = ConvertToThumPath(moveResult.FileUrl);
                    System.IO.File.Move(sourceThumAbsolutePath, moveResult.ThumAbsolutePath);
                }

                list.Add(moveResult);
            }

            return new ValueTask<List<FileResult>>(list);
        }

        /// <summary>
        /// 删除图像
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public ValueTask RemoveAsync(params string[] inputs)
        {
            foreach (var item1 in inputs)
            {
                string item = item1;
                //if (item.StartsWith(_serverRootUrl))
                item = Absolute2RelativeUrl(item);
                item = item.UrlSeparatorChar2DirectorySeparatorChar();
                item = Relative2AbsolutePath(item);
                try
                {
                    //  var f = Relative2AbsolutePath(item);
                    System.IO.File.Delete(item);
                }
                catch (Exception ex)
                {
                    Logger.Warn("文件删除失败", ex);
                }

                try
                {
                    //  var f = Relative2AbsolutePath(item);
                    var d = ConvertToThumPath(item);
                    System.IO.File.Delete(d);
                }
                catch (Exception ex)
                {
                    Logger.Warn("缩略图删除失败", ex);
                }
            }

            return new ValueTask();
        }

        #region 文章中的图片处理

        /// <summary>
        /// 获取指定内容中所包含的图片的相对路径
        /// /aaa/bbb/cc.jpg
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string[] GetMatchedImagePath(string content)
        {
            //Regex rg = new Regex($@"<img.+src=('|""){server}/(\S+)('|"")", RegexOptions.Multiline);
            return _regex.Matches(content).Select(c => c.Groups[1].Value).ToArray();
        }

        /// <summary>
        /// 去掉文本中图片地址的temp路径片段
        /// 建议在保存时调用，而不是每次查询时替换
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string ReplaceImagePath(string content)
        {
            return _regex.Replace(content, c => c.Value.Replace("temp" + Path.DirectorySeparatorChar, ""));
        }

        #endregion

        /// <summary>
        /// 将不带host的文件相对路径转换为完整路径
        /// /upload/20201101/a.jpg -> http://www.xx.com/upload/20201101/a.jpg
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string Relative2AbsoluteUrl(string p)
        {
            if (!p.StartsWith(_serverRootUrl))
                return _serverRootUrl + p.TrimStart('/');
            return p;
        }

        /// <summary>
        /// 将带host的完整文件路径转换为相对路径
        /// http://www.xx.com/upload/20201101/a.jpg -> /upload/20201101/a.jpg
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string Absolute2RelativeUrl(string p)
        {
            if (p.StartsWith(_serverRootUrl))
                return p.Substring(_serverRootUrl.Length);
            return p;
        }

        /// <summary>
        /// 转换为缩略图路径
        /// ...xx.jpg -> ...xxthum.jpg
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ConvertToThumPath(string path)
        {
            if (!path.Contains("thum."))
                return path.Insert(path.LastIndexOf("."), "thum");
            return path;
        }

        /// <summary>
        /// 绝对路径转相对路径
        /// d:\app\wwwroot\upload\20201022\a.jpg -> \upload\20201022\a.jpg  
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string Absolute2RelativePath(string path)
        {
            if (path.StartsWith(_webRootDir))
                return path.Substring(_webRootDir.Length);
            return path;
        }

        /// <summary>
        /// 相对路径转绝对路径
        /// \wwwroot\upload\20201022\a.jpg -> d:\app\wwwroot\upload\20201022\a.jpg
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string Relative2AbsolutePath(string path)
        {
            if (!path.StartsWith(_webRootDir))
                return Path.Combine(_webRootDir, path.TrimStart(Path.DirectorySeparatorChar));
            return path;
        }

        /// <summary>
        /// 临时目录转换为正式目录
        /// 去掉temp
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string TempToOkPath(string path)
        {
            return path.Replace(Consts.UploadTemp, Consts.UploadDir).Replace(Consts.UploadTempUrl, Consts.UploadDir);
        }

        /// <summary>
        /// 尝试获取缩略图的相对url
        /// </summary>
        /// <param name="absoluteUrl">绝对url</param>
        /// <returns></returns>
        public string TryGetThumRelativeUrl(string absoluteUrl)
        {
            absoluteUrl = Absolute2RelativeUrl(absoluteUrl);
            var url = absoluteUrl = ConvertToThumPath(absoluteUrl);
            absoluteUrl = absoluteUrl.UrlSeparatorChar2DirectorySeparatorChar();
            absoluteUrl = Relative2AbsolutePath(absoluteUrl);
            if (System.IO.File.Exists(absoluteUrl))
                return url;
            return default;
        }
    }
}