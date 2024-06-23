using BXJG.Common.Contracts;
using BXJG.Common.Events;
using BXJG.Common.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BXJGCommonDIExt
    {
        /// <summary>
        /// 注册公共服务，也适用于blazor web auto的客户端和服务端
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBXJGCommon(this IServiceCollection services)
        {
            services.TryAddSingleton<IClock, LocalClock>();
            //services.TryAddSingleton(Zhongjie.Instance);
            services.TryAddScoped<Zhongjie>();//客户端模式中是单例，server模式中是scope
            services.TryAddTransient<AccessTokenHandler>();
            return services;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddAccessTokenHandler(this IServiceCollection services)
        //{
           
        //}
    }
}
