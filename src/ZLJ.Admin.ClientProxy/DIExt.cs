using BXJG.Common.Http;
using BXJG.Utils.Application.ClientProxy;
using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Admin.ClientProxy;
using ZLJ.Application.Common.ClientProxy;
using ZLJ.Application.Common.ClientProxy.Extensions;
using ZLJ.Application.Share.Administrative;
using ZLJ.Application.Share.Auditing;
using ZLJ.Application.Share.MultiTenancy;
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
        public static IServiceCollection AddAdminApiClientProxy(this IServiceCollection services, Action<HttpClient> act = default, Action<IHttpClientBuilder> act2 = default) //where T : class
        {
            services.AddTransient<IAuditLogAppService, AuditingAppService>()
                    .AddTransient<IPostAppService, PostAppService>()
                    .AddTransient<IBXJGBaseInfoAdministrativeAppService, AdministrativeAppService>()
                    .AddTransient<IDataDictionaryAppService, DataDictionaryAppService>()
                    .AddTransient<ITenantAppService, TenantAppService>();
            //BXJGBaseClient.HttpClientName = Consts.ZLJ_ADMIN_HTTP_CLIENT_NAME;
            return services.AddApiClientProxy(act, act2);
        }
    }
}