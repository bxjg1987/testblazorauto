using BXJG.Common.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Web.Blazor.Auth;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExt
    {
        /// <summary>
        /// 后台管理应用，服务端和客户端共同需要注册的服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="_appConfiguration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdminBlazor(this IServiceCollection services)
        {
            return services;
        }
    }
}
