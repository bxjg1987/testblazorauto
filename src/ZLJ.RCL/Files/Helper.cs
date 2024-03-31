using Abp.Dependency;
using BXJG.Common.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.RCL.Files
{
    public class Helper : ITransientDependency
    {
        IConfiguration Configuration;
        IAccessTokenProvider  accessTokenProvider;

        public Helper(IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
        {
            Configuration = configuration;
            this.accessTokenProvider = accessTokenProvider;
        }

        /// <summary>
        /// 生成文件下载路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string BuildDownloadUrl(Guid id) {
            return Configuration["App:ServerRootAddress"].TrimEnd('/') + "/api/bxjgfile/Download/" + id+"?at="+accessTokenProvider.GetAccessToken();
        }
        /// <summary>
        /// 生成文件的缩略图url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string BuildDownloadUrlThum(Guid id)
        {
            return Configuration["App:ServerRootAddress"].TrimEnd('/') + "/api/bxjgfile/DownloadThum/" + id + "?at=" + accessTokenProvider.GetAccessToken();
        }
    }
}
