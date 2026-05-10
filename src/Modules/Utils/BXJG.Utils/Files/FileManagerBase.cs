using System;
using System.IO;
//using System.Drawing;
using Microsoft.Extensions.Configuration;
using BXJG.Utils.Share;
using Abp;
namespace BXJG.Utils.Files
{
    /// <summary>
    /// 文件下载器和管理器的抽象类
    /// </summary>
    public abstract class FileManagerBase : BXJGBaseDomainService, IShouldInitialize 
    {
        /// <summary>
        /// 获取安全的上传目录，一般是非应用程序目录 可读写 不可执行
        /// </summary>
        protected string _uploadDir;
        /// <summary>
        /// 安全上传时的临时目录
        /// </summary>
        protected string _tempDir;
        /// <summary>
        /// 配置（上传目录相关配置是死的，所以没有用abp的settings系统）
        /// </summary>
        public IConfiguration Configuration { get; set; }
       
        //去掉构造函数，便于子类重写
        public virtual void Initialize()
        {
            // _serverRootUrl = Configuration["App:ServerRootAddress"];
            //_regex = new Regex($@"<img.+?src=['""]{_serverRootUrl}(\S+)['""]", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            //_settingManager = settingManager;
            //base.LocalizationSourceName = 
            //_webRootDir = env.WebRoot; // d:\app\wwwroot
            //未考虑并发冲突，也基本不需要
            _uploadDir = Configuration["Upload:SaveDir"];
            if (string.IsNullOrWhiteSpace(_uploadDir))
                throw new InvalidOperationException("Upload:SaveDir 配置项未设置，请检查 appsettings.json。");
            if (!Directory.Exists(_uploadDir))
                Directory.CreateDirectory(_uploadDir);
            _tempDir = Path.Combine(_uploadDir, BXJGUtilsConsts.UploadTemp); // d:\app\wwwroot\upload\temp
            if (!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);
        }

        /// <summary>
        /// 绝对路径转相对路径
        /// d:\upload\20201022\a.jpg -> 20201022\a.jpg  
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string AbsoluteToRelativePath(string path)
        {
            if (path.StartsWith(_uploadDir))
                return path.Substring(_uploadDir.Length);
            return path;
        }

        /// <summary>
        /// 相对路径转绝对路径
        /// upload\20201022\a.jpg -> d:\\upload\20201022\a.jpg
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string RelativeToAbsolutePath(string path)
        {
            if (!path.StartsWith(_uploadDir))
                return Path.Combine(_uploadDir, path);
            return path;
        }
    }
}