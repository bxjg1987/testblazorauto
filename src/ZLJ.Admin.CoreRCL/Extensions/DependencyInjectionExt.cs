using BXJG.Common.Http;
using BXJG.Utils.RCL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExt
    {
        /// <summary>
        /// 后台管理应用，服务端和客户端共同需要注册的服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdminBlazor(this IServiceCollection services)
        {
         
            BXJGHttpClientExt.DefaultFctory = f => f.CreateHttpClientAdmin();
            return services.AddZLJRCL().AddAutoMapper(typeof(DependencyInjectionExt));
        }
    }
}
