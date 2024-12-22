using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Abp.Runtime.Session;
using BXJG.Common.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Abp.ObjectMapping;
using BXJG.Utils.RCL;
using BXJG.Utils.RCL.Helpers;
using BXJG.Utils.Application.Share.Session;
using ZLJ.Application.Common.ClientProxy;
using Abp.Web.Models.AbpUserConfiguration;
using Microsoft.AspNetCore.SignalR.Client;
using System.Runtime.Intrinsics.X86;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using BXJG.Utils.Application.ClientProxy.Http;
using ZLJ.RCL.Exceptions;
#if DEBUG
[assembly: Rougamo.IgnoreMo]
#endif
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExt
    {
        /// <summary>
        /// 本项目，所有应用，前后端要注册的
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfg">通用signalR配置，通常只需要withurl配置地址即可</param>
        /// <returns></returns>
        public static IServiceCollection AddZLJRCL(this IServiceCollection services, Action<IServiceProvider, HubConnectionBuilder> cfg = default)
        {
            services.TryAddScoped( s =>
            {
                var AuthenticationState = s.GetRequiredService<AuthenticationStateProvider>();
                var sessionAppService = s.GetRequiredService<ZLJ.Application.Common.ClientProxy.SessionAppService>();
                return AuthenticationState.GetAuthenticationStateAsync().ContinueWith(async t =>
                  {
                      var r = t.Result;
                      if (r.User?.Identity != default && r.User.Identity.IsAuthenticated)
                          return await sessionAppService.GetCurrentLoginInformations();
                      return await Task.FromResult<GetCurrentLoginInformationsOutput>(null);
                  }).Unwrap();


            });

            services.TryAddScoped( s => s.GetRequiredService<AbpUserConfigurationService>().GetAll());

            if (cfg == default)
            {
                cfg = (s, b) =>
                {
                    var url = s.GetRequiredService<IConfiguration>()["App:ServerRootAddress"].TrimEnd('/')+ "/signalr";
                    b.WithUrl(url);

                };
            }
            //不好实现，所以不要使用多语言
            //services.TryAddSingleton<ILocalizationManager, NullLocalizationManager>();
            return services.UseBXJGUtilsRCL(cfg).AddAntDesign().Replace(ServiceDescriptor.Transient < IErrorCallback, ErrorCallback >()); ;
        }
        //public static Task<AbpUserConfigurationDto> AbpUserConfigurationDto(this ServiceProvider s)
        //{
        //    return s.GetKeyedService<Task<AbpUserConfigurationDto>>(Consts.AbpUserConfigurationDto)!;
        //}
        //public static Task<AbpUserConfigurationDto> AbpUserConfigurationDto(this ServiceProvider s) {
        //    return s.GetKeyedService<Task<AbpUserConfigurationDto>>(Consts.AbpUserConfigurationDto)!;
        //}

        ///// <summary>
        ///// 本项目，所有应用，前后端都要注册的
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //static IServiceCollection AddZLJBlazor(this IServiceCollection services)
        //{
        //    services.AddAntDesign()
        //            .AddCascadingAuthenticationState()
        //            .AddTransient<ZLJ.RCL.Files.Helper>();
        //    return services;
        //}
        ///// <summary>
        ///// 本项目，所有应用，后端都要注册的
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddZLJBlazorServer(this IServiceCollection services)
        //{
        //    services.AddZLJBlazor()
        //            .AddCommonRCLServer();
        //    return services;
        //}
    }
}