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
using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.Extensions.Configuration;
using Abp.Extensions;
using System.Text.RegularExpressions;

namespace BXJG.Utils.File
{
    /*
     * 参考文档：https://gitee.com/bxjg1987/abp/wikis/文件管理?sort_id=2946614
     * 
     * 这里文件类型、大小限制是通过Utils模块提供的settings来控制的，是全局的
     * 正常情况下模块调用方可能需要提供自己的限制，但是目前没有提供这样的扩展点
     * 
     * 将文件管理看成一个独立的，通用的功能，那么文件上传的请求方应该提供一个标识来识别是哪各模块进行上传的。然后通过此标识找到对应模块提供的处理器对文件管理实施额外逻辑
     * 或模块调用方通过集成的方式提供自己的controller和fileManager
     * 以上两种方式都可以实现扩展，推荐前一种方式
     * 
     * 利用abp提供的后台任务删除temp目录中长时间未被移走的文件
     */

    /// <summary>
    /// 一个简单的文件管理领域服务
    /// mvc或web api层的controller接收上传的文件，并通过此类进行文件的验证和保存
    /// 参考文档：https://gitee.com/bxjg1987/abp/wikis/文件管理?sort_id=2946614
    /// </summary>
    public class TempFileManager : DomainService//, ITransientDependency
    {
        #region 字段和属性
        /// <summary>
        /// appsettings中的app下的ServerRootAddress
        /// http://xxx.xxx.xx:21021/
        /// </summary>
        string serverRootAddress;
        /// <summary>
        /// abp提供的设置管理器
        /// </summary>
        readonly ISettingManager settingManager;
        /// <summary>
        /// d:\app\wwwroot
        /// </summary>
        string rootDir;
        /// <summary>
        /// 文件上传的根目录
        /// d:\app\wwwroot\upload 
        /// </summary>
        string uploadDir;
        /// <summary>
        /// 文件上传的临时目录
        /// d:\app\wwwroot\upload\temp
        /// </summary>
        string tempDir;
        /// <summary>
        /// abp提供的异步取消
        /// </summary>
        public ICancellationTokenProvider cancellationTokenProvider { get; set; } = NullCancellationTokenProvider.Instance; //空模式
        /// <summary>
        /// 用来匹配字符串文本中包含的图片地址列表
        /// 在构造函数中被初始化
        /// </summary>
        Regex regex;
        #endregion
        #region 构造函数
        /// <summary>
        /// 一个简单的文件管理领域服务
        /// mvc或web api层的controller接收上传的文件，并通过此类进行文件的验证和保存
        /// 参考文档：https://gitee.com/bxjg1987/abp/wikis/文件管理?sort_id=2946614
        /// </summary>
        /// <param name="env">获取当前应用的相对路径</param>
        /// <param name="settingManager">abp提供的settings系统</param>
        public TempFileManager(IEnv env, ISettingManager settingManager, IConfiguration configuration)
        {
            serverRootAddress = configuration["App:ServerRootAddress"];
            regex = new Regex($@"<img.+?src=['""]{serverRootAddress}(\S+)['""]", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            this.settingManager = settingManager;

            rootDir = env.WebRoot;                                         // d:\app\wwwroot

            this.uploadDir = Path.Combine(env.WebRoot, Consts.UploadDir);  // d:\app\wwwroot\upload 
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            this.tempDir = Path.Combine(env.WebRoot, Consts.UploadTemp);   // d:\app\wwwroot\upload\temp
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);
        }
        #endregion
        #region 公共方法
        /// <summary>
        /// 将不带host的文件相对路径转换为完整路径
        /// /upload/20201101/a.jpg -> http://www.xx.com/upload/20201101/a.jpg
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string AddServerPath(string p)
        {
            if (p.IsNullOrWhiteSpace())
                return p;
            p = p.Replace("\\", "/");
            p = p.TrimStart('/');
            p = serverRootAddress + p;
            return p;
        }
        /// <summary>
        /// 将带host的完整文件路径转换为相对路径
        /// http://www.xx.com/upload/20201101/a.jpg -> /upload/20201101/a.jpg
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string RemoveServerPath(string p)
        {
            if (p.IsNullOrWhiteSpace())
                return p;
            p = p.Replace(serverRootAddress, "");
            if (!p.StartsWith('/'))
                p = "/" + p;
            return p;
        }
        /// <summary>
        /// 验证并将文件存储到temp目录
        /// </summary>
        /// <param name="createThum"></param>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public async Task<List<FileResult>> UploadAsync(bool createThum = false, params FileInput[] inputs)
        {
            var outputs = new List<FileResult>();   //准备返回值

            var ts = await settingManager.GetSettingValueAsync(Consts.SettingKeyUploadType);        //设置中允许的后缀名 .pdf,.jpg...
            var aryts = ts.Split(',');              //设置中允许的后缀名 [".pdf",".jpg"...]
            var sz = await settingManager.GetSettingValueAsync<int>(Consts.SettingKeyUploadSize);   //设置中的大小限制

            foreach (var item in inputs)
            {
                #region 各种限制
                var hzm = MimeTypesMap.GetExtension(item.ContentType);      //当前文件的后缀名 .jpg
                if (!aryts.Contains(hzm, StringComparer.OrdinalIgnoreCase)) //类型判断，这种方式只能一定成都起到限制作用，因为前端上传的文件后缀可能被恶意更改
                    throw new UserFriendlyException($"不允许上传此类型的文件，仅允许{ts}");

                if (item.Length > sz * 1024)        //大小限制判断    
                    throw new UserFriendlyException($"上传的文件大小超过限制，最大为{sz}Kb");
                #endregion

                var output = new FileResult();      //准备返回值

                #region 保存文件
                var hz = Path.GetExtension(item.FileName);                  //文件后缀.jpg
                var wjm = Guid.NewGuid().ToString("n") + hz;                //xxx.jpg  xxx=guid
                var dateDir = Path.Combine(tempDir, DateTime.Now.ToString("yyyyMMdd"));            //d:\app\wwwroot\upload\temp\20201003
                                                                                                   //if (!Directory.Exists(dateDir))
                Directory.CreateDirectory(dateDir);
                output.FileAbsolutePath = Path.Combine(dateDir, wjm);//d:\app\wwwroot\upload\temp\20201003\xxx.jpg
                output.FileRelativePath = Absolute2RelativePath(output.FileAbsolutePath);               //\upload\temp\20201003\xxx.jpg
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

                    //参考：https://docs.sixlabors.com/articles/imagesharp/resize.html
                    //经过测试这里load item.Stream会报错
                    using (SixLabors.ImageSharp.Image img = await SixLabors.ImageSharp.Image.LoadAsync(output.FileAbsolutePath, cancellationTokenProvider.Token))
                    {
                        img.Mutate(x => x.Resize(500, 0));  //按给定的宽度缩放
                        img.Save(output.ThumAbsolutePath);
                    }
                    //以下方式很模糊
                    //img = img.GetThumbnailImage(400, 500, () => false, IntPtr.Zero);    //暂时固定缩略图大小，后期考虑 通过settings + 当前请求提供的缩略图大小来设置
                }
                #endregion

                outputs.Add(output);
            }
            return outputs;
        }
        /// <summary>
        /// 将temp目录中的文件和缩略图(如果有)移动到正式目录
        /// </summary>
        /// <param name="inputs">文件在temp目录相对路径集合</param>
        /// <returns></returns>
        public async ValueTask<List<FileResult>> MoveAsync(params string[] inputs)
        {
            var list = new List<FileResult>();
            for (var jk = 0; jk < inputs.Length; jk++)
            {
                var item = inputs[jk].Replace("/", "\\");

                if (!item.Contains(Consts.UploadTemp, StringComparison.OrdinalIgnoreCase))
                {
                    var rr = new FileResult();
                    rr.FileAbsolutePath = Relative2AbsolutePath(item);
                    rr.FileRelativePath = item;
                    rr.ThumAbsolutePath = ConvertToThumPath(rr.FileAbsolutePath);
                    if (System.IO.File.Exists(rr.ThumAbsolutePath))
                    {
                        rr.ThumRelativePath = Absolute2RelativePath(rr.ThumAbsolutePath);
                    }
                    else
                        rr.ThumAbsolutePath = default;
                    list.Add(rr);
                    continue;
                }

                var moveResult = new FileResult();

                //移动文件
                var sourceAbsolutePath = Relative2AbsolutePath(item);                                   //temp绝对路径
                moveResult.FileRelativePath = TempToOkPath(item);                                       //正式目录相对路径
                moveResult.FileAbsolutePath = Relative2AbsolutePath(moveResult.FileRelativePath);       //正式目录绝对路径
                if (!System.IO.File.Exists(moveResult.FileAbsolutePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(moveResult.FileAbsolutePath));
                    System.IO.File.Move(sourceAbsolutePath, moveResult.FileAbsolutePath);
                }
                //移动缩略图
                var thumItem = ConvertToThumPath(item);                                                 //temp缩略图相对路径
                var sourceThumAbsolutePath = Relative2AbsolutePath(thumItem);                           //temp缩略图绝对路径
                if (System.IO.File.Exists(sourceThumAbsolutePath))
                {
                    moveResult.ThumRelativePath = TempToOkPath(thumItem);                               //缩略图正式目录相对路径
                    moveResult.ThumAbsolutePath = Relative2AbsolutePath(moveResult.ThumRelativePath);   //缩略图正式目录绝对路径
                    System.IO.File.Move(sourceThumAbsolutePath, moveResult.ThumAbsolutePath);
                }

                list.Add(moveResult);
            }
            return list;
        }
        /// <summary>
        /// 删除图像
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public ValueTask RemoveAsync(params string[] inputs)
        {
            foreach (var item in inputs)
            {
                try
                {
                    var f = this.Relative2AbsolutePath(item);
                    System.IO.File.Delete(f);
                }
                catch (Exception ex)
                {
                    base.Logger.Warn("文件删除失败", ex);
                }
                try
                {
                    var f = this.Relative2AbsolutePath(item);
                    var d = ConvertToThumPath(f);
                    System.IO.File.Delete(d);
                }
                catch (Exception ex)
                {
                    base.Logger.Warn("缩略图删除失败", ex);
                }
            }
            return new ValueTask();
        }

        #region 文章中的图片处理
        //public ValueTask<(string, string[])> HandConentAsync(string str)
        //{

        //}

        string pattern;
        /// <summary>
        /// 获取指定内容中所包含的图片的相对路径
        /// /aaa/bbb/cc.jpg
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string[] GetMatchedImagePath(string content)
        {
            //Regex rg = new Regex($@"<img.+src=('|""){server}/(\S+)('|"")", RegexOptions.Multiline);
            return regex.Matches(content).Select(c => c.Groups[1].Value).ToArray();
        }

        public string ReplaceImagePath(string content)
        {
            return regex.Replace(content, c => c.Value.Replace("temp/", ""));
        }
        #endregion

        #endregion
        #region 辅助方法
        /// <summary>
        /// 转换为缩略图路径
        /// ...xx.jpg -> ...xxthum.jpg
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string ConvertToThumPath(string path)
        {
            return path.Insert(path.LastIndexOf("."), "thum");
        }
        /// <summary>
        /// 绝对路径转相对路径
        /// d:\app\wwwroot\upload\20201022\a.jpg -> \upload\20201022\a.jpg  
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string Absolute2RelativePath(string path)
        {
            return path.Substring(rootDir.Length);
        }
        /// <summary>
        /// 相对路径转绝对路径
        /// \wwwroot\upload\20201022\a.jpg -> d:\app\wwwroot\upload\20201022\a.jpg
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string Relative2AbsolutePath(string path)
        {
            return Path.Combine(rootDir, path);
        }
        /// <summary>
        /// 临时目录转换为正式目录
        /// 去掉temp
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string TempToOkPath(string path)
        {
            return path.Replace(Consts.UploadTemp, Consts.UploadDir + @"\");
        }
        #endregion
    }
}