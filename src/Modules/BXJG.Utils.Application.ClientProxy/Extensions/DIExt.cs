using BXJG.Common.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Common.Extensions;
using Abp.Application.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Abp.Configuration;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.ClientProxy.Http;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExt
    {
        /// <summary>
        /// 添加httpclient的拦截器，用于添加accessToken请求头，并在响应时解包abp ajax包
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddBXJGUtilsMessageHandler(this IHttpClientBuilder services)
        {
            services.Services.AddTransient<AbpWraperDelegatHandler>();
            return  services.AddHttpMessageHandler<AbpWraperDelegatHandler>().AddHttpMessageHandler<AccessTokenHandler>();

        }
       
    }
}
