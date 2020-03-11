using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using ZLJ.Authentication.JwtBearer;
using ZLJ.Configuration;
using ZLJ.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using ZLJ.Navigation;
using BXJG.Utils;
using BXJG.GeneralTree;

namespace ZLJ
{
    [DependsOn(
         typeof(ZLJApplicationModule),
         typeof(ZLJEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
        ,typeof(AbpAspNetCoreSignalRModule)
     )]
    public class ZLJWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ZLJWebCoreModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<ZLJNavigationProvider>();

            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                ZLJConsts.ConnectionStringName
            );

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(ZLJApplicationModule).GetAssembly()
                 );

            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJGUtilsModule).Assembly/*, moduleName: "utils", useConventionalHttpVerbs: true*/);
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(GeneralTreeModule).Assembly);

            ConfigureTokenAuth();
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ZLJWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ZLJWebCoreModule).Assembly);
        }
    }
}
