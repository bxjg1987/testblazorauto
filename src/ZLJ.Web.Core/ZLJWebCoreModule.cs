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
using BXJG.Shop;
using BXJG.CMS;
using BXJG.Common;
using Abp.Dependency;
using BXJG.Equipment;
using BXJG.BaseInfo;
using BXJG.Equipment.EFCore;
using Abp.Configuration.Startup;
using BXJG.WorkOrder.EmployeeApplication;
//using BXJG.DynamicAssociateEntity;

namespace ZLJ
{
    [DependsOn(typeof(BXJGUtilsWebModule),
               typeof(ZLJApplicationModule),
               typeof(ZLJEntityFrameworkModule),
               typeof(AbpAspNetCoreModule),
               typeof(AbpAspNetCoreSignalRModule),
               typeof(ApplicationModule),
               typeof(BXJGCMSApplicationModule),
               typeof(BXJGEquipmentEFCoreModule),
               typeof(BXJGEquipmentApplicationModule),
               typeof(BXJG.WorkOrder.ApplicationModule),
               typeof(BXJGWorkOrderEmployeeApplicationModule))]
    public class ZLJWebCoreModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        ZLJEntityFrameworkModule abpProjectNameEntityFrameworkModule;

        public ZLJWebCoreModule(IWebHostEnvironment env, ZLJEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
            this.abpProjectNameEntityFrameworkModule = abpProjectNameEntityFrameworkModule;
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
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(ApplicationModule).Assembly, "bxjgshop");
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJGCMSApplicationModule).Assembly);
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJGEquipmentApplicationModule).Assembly);
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJGBaseInfoApplicationModule).Assembly);
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJG.WorkOrder.ApplicationModule).Assembly,"bxjgworkorder");
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJG.WorkOrder.BXJGCommonApplicationModule).Assembly, "bxjgworkorder");
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(BXJGWorkOrderEmployeeApplicationModule).Assembly, "bxjgworkorderemployee");
            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(DynamicAssociateEntityApplicationModule).Assembly, "bxjgDynamicAssociateEntity");

            ConfigureTokenAuth();

            //默认每次启动都会尝试数据库迁移，这里禁用它提高系统启动速度
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            //Configuration.ReplaceService<IEnv, NetCoreEnv>(DependencyLifeStyle.Singleton);//经过测试没什么卵用
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            //IocManager.Register<IWeChatMiniProgramLoginHandler, WeChatMiniProgramLoginHandler>(DependencyLifeStyle.Transient);
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
            //IocManager.Register<IEnv, NetCoreEnv>(DependencyLifeStyle.Singleton);//utils已经注册了个 这里可以替换
            //Configuration.ReplaceService<IEnv, NetCoreEnv>(DependencyLifeStyle.Singleton);//经过测试必须在PreInitialize
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ZLJWebCoreModule).Assembly);
            // Configuration.ReplaceService<IEnv, NetCoreEnv>(DependencyLifeStyle.Singleton);//经过测试没什么卵用
        }
    }
}
