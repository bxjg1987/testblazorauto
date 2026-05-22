using Abp;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Organizations;
using Abp.RealTime;
using Abp.Reflection.Extensions;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Abp.Timing;
using Abp.Zero.Configuration;
using AutoMapper;
using BXJG.Common;
using BXJG.Common.Contracts;
using BXJG.Utils.DI;
using BXJG.Utils.DynamicProperty;
using BXJG.Utils.Enums;
using BXJG.Utils.Extensions;
using BXJG.Utils.Feedback;
using BXJG.Utils.Files;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.Interceptor;
using BXJG.Utils.Localization;
using BXJG.Utils.OU;
using BXJG.Utils.Settings;
using BXJG.Utils.Share;
using BXJG.Utils.Share.DataPermission;
using BXJG.Utils.Tag;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BXJG.Utils
{
    /*
     * 通用公共功能模块
     */
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class BXJGUtilsModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.RegisterFilter(DataPermissionConsts.DataPermission, true);

            Configuration.ReplaceService<IAbpSession, AbpSessionWithHttpContext>();  
            DataFilterInterceptor.Initialize(IocManager);
            IocManager.Register<BXJGUtilsModuleConfig>();
            //Configuration.Modules.BXJGUtils().AddEnum(typeof(Gender), "gender", UtilsConsts.LocalizationSourceName);
            Configuration.Modules.BXJGUtils().EnumLocalizationProviders.Add(() => new[] {
                new EnumLocalizationDefine(typeof(Gender), "gender"),
                //new EnumLocalizationDefine(typeof(bool), "bool"),
            });

            Configuration.Modules.AbpAutoMapper().Configurators
                .Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            BXJGUtilsLocalizationConfigurer.Configure(Configuration.Localization);
            Configuration.Settings.Providers.Add<BXJGUtilsSettingProvider>();
            //查看abp源码 uow拦截器调用manager.begin，内部从ioc获取uow对象efuow 然后设置到asynclocal上的
            IocManager.IocContainer.Kernel.ComponentCreated += Kernel_ComponentCreated;

            Configuration.ReplaceService<IOrganizationUnitManager, BXJGOrganizationUnitManager>(DependencyLifeStyle.Transient);
        
        }

        private void Kernel_ComponentCreated(Castle.Core.ComponentModel model, object instance)
        {
            if (instance is IActiveUnitOfWork uow)
            {
                uow.Disposed -= Uow_Disposed;//保险起见
                uow.Disposed += Uow_Disposed;
            }
        }

        private static void Uow_Disposed(object sender, EventArgs e)
        {
            ////参考ActiveUnitOfWorkExtensions
            //var uow = sender as IActiveUnitOfWork;
            //if (uow.Items.TryGetValue(ActiveUnitOfWorkExtensions.__disposeableObject, out var t))
            //{
            //    foreach (var item in (t as HashSet<object>))
            //    {
            //        if (item is IAsyncDisposable d)
            //            AsyncHelper.RunSync(() => d.DisposeAsync().AsTask());

            //        if (item is IDisposable ee)
            //            ee.Dispose();
            //    }
            //}
        }

        public override void Initialize()
        {
            //注册非abp依赖的公共库Common中的服务
            //base.IocManager.RegisterIfNot<IClock, LocalClock>(DependencyLifeStyle.Singleton);
            //base.IocManager.RegisterIfNot<IEnv,BXJG.Common.DefaultWebEnv>(DependencyLifeStyle.Singleton);
           

            IocManager.RegisterAssemblyByConvention(typeof(BXJGUtilsModule).GetAssembly());
            //Lazy<TService>注入
            IocManager.IocContainer.Register(
               Castle.MicroKernel.Registration.Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>()
            );
            //IocManager.Register<IClock, AbpClock>();
            IocManager.RegService(services =>
            {
                services.AddBXJGCommon();
                //services.AddScoped<DistributedLockHelper>();
            });
            IocManager.Register(typeof(DynamicPropertyManager<>), DependencyLifeStyle.Singleton);
            IocManager.Register(typeof(FeedbackManager<,>), DependencyLifeStyle.Transient);

            // ISubDependencyResolver
            // Castle.MicroKernel.resolver
            // ScopedIocResolver
            // IocResolverExtensions
            // IIocResolver sdf;
            // sdf.Resolve()
            // IocManager.IocContainer.Resolve()
            // IDependencyResolver sdf;
            // Castle.Windsor.MsDependencyInjection.WindsorServiceScopeFactory
            // sdf.Resolve()
            // IIocResolver sdf;
            // IDependencyResolver
            // iresolver
            // Abp.Dependency.ScopedIocResolver sdf;
            // sdf.ResolveAsDisposable
            // sdf.

            //通用附件管理器
            IocManager.Register(typeof(AttachmentManager<>), DependencyLifeStyle.Transient);
            //通用tag管理器
            IocManager.Register(typeof(TagManager<>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(AttachmentManager<>), DependencyLifeStyle.Transient);
            //调试模式时默认实现获取的路径是 ..\bin\debug\wwwroot
            //而asp.net core默认读取是在ZLJ.Web.Host\wwwroot 导致上传的文件看不到效果
            //发布到服务器后不存在这个问题，调试时需要在web.core模块PreInitialize中替换服务，注意经过测试一定要在PreInitialize中替换
            //IocManager.Register<IEnv, Utils.File.DefaultEnv>(Abp.Dependency.DependencyLifeStyle.Singleton);

            // IocManager.Register<IEnv, NullEnv>();
            IocManager.Register(typeof(GeneralTreeManager<>), DependencyLifeStyle.Transient);
            IocManager.Register(typeof(AbpAsyncDeterminationInterceptor<DataFilterInterceptor>), DependencyLifeStyle.Transient);
            //IocManager.ser
            //IocManager.IocContainer.rep
        }

        public override void PostInitialize()
        {
            #region 本地化枚举系统
            var utilsCfg = Configuration.Modules.BXJGUtils();
            var list = new List<EnumLocalizationDefine>();
            foreach (var item in utilsCfg.EnumLocalizationProviders)
            {
                var items = item();

                foreach (var item2 in items)
                {
                    var temp = list.SingleOrDefault(c => c.Name == item2.Name);
                    if (temp != null)
                    {
                        list.Remove(temp);
                    }
                    list.Add(item2);
                }
            }
            utilsCfg.EnumLocalizationProviders = null;
            var sdf = new EnumLocalizationContainer(list);
            base.IocManager.RegService(c => c.TryAddSingleton(sdf));
            #endregion

            AdjustBuiltInSettingDefinitions();
        }

        /// <summary>
        /// 调整ABP内置设置项的分组和显示名。
        /// 用户管理设置项的DisplayName需替换：ABP源码中使用FixedLocalizableString（硬编码英文），无法通过扩展本地化源实现中文，故替换为UtilsLI()。
        /// 邮件设置项的DisplayName无需替换：ABP源码中使用LocalizableString引用"Abp"本地化源，该源已内置中文翻译，仅需调整Group即可。
        /// </summary>
        private void AdjustBuiltInSettingDefinitions()
        {
            var settingDefinitionManager = IocManager.Resolve<ISettingDefinitionManager>();
            var allSettings = settingDefinitionManager.GetAllSettingDefinitions();

            var userMgmt = new SettingDefinitionGroup("UserManagement", "用户管理".UtilsLI());
            var emailSetting = new SettingDefinitionGroup("EmailSetting", "邮件设置".UtilsLI());

            foreach (var s in allSettings)
            {
                switch (s.Name)
                {
                    #region 用户管理
                    case AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin:
                        s.Group = userMgmt;
                        s.DisplayName = "登录需要邮箱确认".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled:
                        s.Group = userMgmt;
                        s.DisplayName = "启用账户锁定".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout:
                        s.Group = userMgmt;
                        s.DisplayName = "锁定前最大失败尝试次数".UtilsLI();
                        s.CustomData = new { csharpType = "int", min = 1, max = 100 };
                        break;
                    case AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds:
                        s.Group = userMgmt;
                        s.DisplayName = "默认账户锁定时间(秒)".UtilsLI();
                        s.CustomData = new { csharpType = "int", min = 1 };
                        break;
                    #endregion

                    #region 密码策略
                    case AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength:
                        s.Group = userMgmt;
                        s.DisplayName = "密码最小长度".UtilsLI();
                        s.CustomData = new { csharpType = "int", min = 1, max = 100 };
                        break;
                    case AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric:
                        s.Group = userMgmt;
                        s.DisplayName = "要求非字母数字字符".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase:
                        s.Group = userMgmt;
                        s.DisplayName = "要求小写字母".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase:
                        s.Group = userMgmt;
                        s.DisplayName = "要求大写字母".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit:
                        s.Group = userMgmt;
                        s.DisplayName = "要求数字".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    #endregion

                    #region 双因素认证
                    case AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled:
                        s.Group = userMgmt;
                        s.DisplayName = "启用双因素登录".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled:
                        s.Group = userMgmt;
                        s.DisplayName = "启用邮箱验证".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled:
                        s.Group = userMgmt;
                        s.DisplayName = "启用短信验证".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled:
                        s.Group = userMgmt;
                        s.DisplayName = "启用记住浏览器".UtilsLI();
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    #endregion

                    #region 邮件设置
                    case EmailSettingNames.Smtp.Host:
                        s.Group = emailSetting;
                        break;
                    case EmailSettingNames.Smtp.Port:
                        s.Group = emailSetting;
                        s.CustomData = new { csharpType = "int", min = 1, max = 65535 };
                        break;
                    case EmailSettingNames.Smtp.UserName:
                        s.Group = emailSetting;
                        break;
                    case EmailSettingNames.Smtp.Password:
                        s.Group = emailSetting;
                        break;
                    case EmailSettingNames.Smtp.Domain:
                        s.Group = emailSetting;
                        break;
                    case EmailSettingNames.Smtp.EnableSsl:
                        s.Group = emailSetting;
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case EmailSettingNames.Smtp.UseDefaultCredentials:
                        s.Group = emailSetting;
                        s.CustomData = new { csharpType = "bool" };
                        break;
                    case EmailSettingNames.DefaultFromAddress:
                        s.Group = emailSetting;
                        break;
                    case EmailSettingNames.DefaultFromDisplayName:
                        s.Group = emailSetting;
                        break;
                    #endregion
                }
            }
        }
    }
}
