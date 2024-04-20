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
using Abp.Configuration;
using ZLJ.Application.Common.Share.OU;
using BXJG.Utils.Application.Share.GeneralTree;
using ZLJ.Application.Common.Share.Auth;
using ZLJ.Application.Common.Share.Administrative;
using BXJG.Utils.Application.ClientProxy;

namespace ZLJ.Application.Common.ClientProxy.Extensions
{
    public static class DIExt
    {
        /// <summary>
        /// 添加代理api客户端代理服务，记得注册IAccessTokenProvider的实现
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiClientProxy(this IServiceCollection services, Action<HttpClient> act = default,Action<IHttpClientBuilder> act2=default)
        {
            services.AddBXJGUtilsMessageHandler(act,act2);

            services.AddTransient<AbpUserConfigurationService>();
            services.AddTransient<SessionAppService>();
            services.AddTransient<IAdministrativeProviderAppService, AdministrativeProviderAppService>();

            services.AddTransient<IOuProviderAppService, OuProviderAppService>();
            services.AddTransient<IDataDictionaryProviderAppService, DataDictionaryProviderAppService>();
            services.AddTransient<ITokenAuthAppService, TokenAuthAppService>();
            return services;
        }
    }
}
