using BXJG.Common.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Common.Extensions;
using ZLJ.Application.Common.ClientProxy;
using Abp.Application.Navigation;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.Application.Common.ClientProxy.Http;
using Abp.Configuration;
using ZLJ.Application.Common.Share.OU;
using BXJG.Utils.Application.Share.GeneralTree;

namespace ZLJ.Application.Common.ClientProxy.Extensions
{
    public static class DIExt
    {
        /// <summary>
        /// 添加代理api客户端代理服务，记得注册IAccessTokenProvider的实现
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddApiClientProxy(this IServiceCollection services, Action<HttpClient> act = default)
        {
            if (act == default)
                act = hc => { };

            services.AddTransient<AbpWraperDelegatHandler>();
            var b = services.AddAccessTokenHandler().AddHttpClient(Consts.ZLJ_ADMIN_HTTP_CLIENT_NAME, act).AddHttpMessageHandler<AbpWraperDelegatHandler>().AddHttpMessageHandler<AccessTokenHandler>();


            services.AddTransient<AbpUserConfigurationService>();
            services.AddTransient<SessionAppService>();
            services.AddTransient<IOuProviderAppService, OuProviderAppService>();
            services.AddTransient<IDataDictionaryProviderAppService, DataDictionaryProviderAppService>();
            return b;
        }
    }
}
