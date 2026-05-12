using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    /// <summary>
    /// Web环境信息提供程序，用于获取当前请求的URL等信息
    /// </summary>
    public interface IWebEnvironment
    {
        string CurrUrl { get; }
    }
}
