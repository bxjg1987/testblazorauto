using Abp.Dependency;
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
        public IConfiguration Configuration { get; set; }
        /// <summary>
        /// 生成文件下载路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string BuildDownloadUrl(Guid id) {
            return Configuration["App:ServerRootAddress"].TrimEnd('/') + "/bxjgfile/Download/" + id;
        }
        /// <summary>
        /// 生成文件的缩略图url
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string BuildDownloadUrlThum(Guid id)
        {
            return Configuration["App:ServerRootAddress"].TrimEnd('/') + "/bxjgfile/DownloadThum/" + id;
        }
    }
}
