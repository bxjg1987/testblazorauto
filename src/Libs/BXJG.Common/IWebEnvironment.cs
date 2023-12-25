using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /// <summary>
    /// 应用程序安全目录提供程序
    /// 用来存储证书、私钥等重要信息
    /// 如：
    /// 在asp.net中的app_data可以作为安全目录，它在应用程序根目录，但是不能被普通用户访问到；
    /// 在asp.net core中在www目录外建立的目录可以作为安全目录
    /// </summary>
    public interface IWebEnvironment
    {
        string CurrUrl { get; }
    }
}
