using BXJG.Common.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Admin.CoreRCL.Extensions
{
    public static class DependencyInjectionExt
    {
        /// <summary>
        /// 服务端和客户端共同需要注册的服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="_appConfiguration"></param>
        /// <returns></returns>
        public static IServiceCollection AddBlazorClientCore(this IServiceCollection services)
        {
            //注意，客户端单独注册的东东不要定义在这里


            //服务端要注册，否则报错，不过可以不用
            services.AddAntDesign()
                   
                    .AddCascadingAuthenticationState();

            //services.Configure<ProSettings>(_appConfiguration.GetSection("ProSettings"));
            return services;
        }
    }
}
