using BXJG.Common.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Admin.ClientProxy;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExt
    {
        /// <summary>
        /// 添加代理admin的api客户端代理服务，记得注册IAccessTokenProvider的实现
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddApiClientProxy(this IServiceCollection services, Action<HttpClient> act = default) //where T : class
        {
            return services.AddApiClientProxy<AdminAppClientProxy>(act);
        }
    }
}