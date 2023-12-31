using BXJG.Common.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Admin.ClientProxy;
using ZLJ.Application.Common.ClientProxy.Extensions;
using ZLJ.Application.Share.Auditing;
using ZLJ.Application.Share.Post;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExt
    {
        /// <summary>
        /// 添加代理admin的api客户端代理服务，记得注册IAccessTokenProvider的实现
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddAdminApiClientProxy(this IServiceCollection services, Action<HttpClient> act = default) //where T : class
        {
            services.AddTransient<IAuditLogAppService, AuditingAppService>()
                    .AddTransient<IPostAppService, PostAppService>();
            return services.AddApiClientProxy(act);
        }
    }
}