using Abp.Dependency;
using BXJG.Common.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BXJG.Utils.RCL.Helpers
{
    //由于是前后端共用的，即便是在后端，也不能使用abp的依赖注入特征
    public class FileHelper //: ITransientDependency
    {
        IConfiguration Configuration;
        IAccessTokenProvider accessTokenProvider;

        public FileHelper(IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
        {
            Configuration = configuration;
            this.accessTokenProvider = accessTokenProvider;
        }

        /// <summary>
        /// 生成文件下载路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string BuildDownloadUrl(Guid id)
        {
            return Configuration["App:ServerRootAddress"].TrimEnd('/') + "/api/bxjgfile/Download/" + id + "?access_token=" + HttpUtility.UrlEncode(accessTokenProvider.GetEncryptedAccessToken());
        }
        /// <summary>
        /// 生成文件的缩略图url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string BuildDownloadUrlThum(Guid id)
        {
            return Configuration["App:ServerRootAddress"].TrimEnd('/') + "/api/bxjgfile/DownloadThum/" + id + "?access_token=" + HttpUtility.UrlEncode(accessTokenProvider.GetEncryptedAccessToken());
        }
        /// <summary>
        /// 生成临时文件的访问URL
        /// </summary>
        /// <param name="encryptedPath">加密的临时文件相对路径</param>
        /// <returns>临时文件的完整访问URL</returns>
        public string BuildTempFileUrl(string encryptedPath)
        {
            return Configuration["App:ServerRootAddress"].TrimEnd('/') + "/api/BXJGFile/GetTempFile/" + encryptedPath+ "?access_token=" + WebUtility.UrlEncode(accessTokenProvider.GetEncryptedAccessToken());
        }
    }
}
