using Abp;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.MultiTenancy;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.RCL
{
    /*
     * 基本上把抽象的应用服务中的abp相关玩意搞过来
     * 注意只有scope tran服务才需要特殊处理？ abp是做了这个处理的，但blazor框架如果够聪明的话，单例对象它应该自己处理
     * 
     * UOW分析
     * 组件调用应用服务，应用服务的uow范围应该是不依赖外部的，而是通过proxy自动实现的
     * 所以组件中通常是不需要开启uow的，所以不需要做任何uow相关的封装，此时一个应用服务的一个方法对应一个uow
     * 如果有必要，可以在组件中开启uow，这样多个应用服务通过环境uow共享要给组件中的uow范围
     * 
     * 也不可能在所有事件处理程序中去自动开启uow，可能很多操作仅仅是界面交互，不需要uow
     * 
     * 既然blazor组件是注册到ioc的，那么使用动态代理应该也可以做aop，没测试过
     * 如果可行，可以定义Attribute，用户在指定方法上应用Attribute即可自动处理uow
     * 目前不考虑，以后再说吧
     * 
     * 总的来说，通常是不需要的，应用用例基本都是对应到应用服务的
     */

    public class AbpComponentBase<TUser, TUserManager, TRole> : OwningComponentBase
        where TUser : AbpUser<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        Lazy<IAbpSession> abpSession;
        protected IAbpSession AbpSession => abpSession.Value;

        //Lazy<TTenantManager> tenantManager;
        //protected TTenantManager TenantManager => tenantManager.Value;

        //Lazy<TRoleManager> roleManager;
        //protected TRoleManager RoleManager => roleManager.Value;

        Lazy<TUserManager> userManager;
        protected TUserManager UserManager => userManager.Value;

        Lazy<IUnitOfWorkManager> unitOfWorkManager;
        protected IUnitOfWorkManager UnitOfWorkManager => unitOfWorkManager.Value;

        //这个是跟线程相关的，在blazor中不能用
        //protected IActiveUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;

        Lazy<ILocalizationManager> localizationManager;
        protected ILocalizationManager LocalizationManager => localizationManager.Value;
        ILocalizationSource _localizationSource;

        protected string LocalizationSourceName { get; set; }
        protected ILocalizationSource LocalizationSource
        {
            get
            {
                if (LocalizationSourceName == null)
                {
                    throw new AbpException("Must set LocalizationSourceName before, in order to get LocalizationSource");
                }

                if (_localizationSource == null || _localizationSource.Name != LocalizationSourceName)
                {
                    _localizationSource = LocalizationManager.GetSource(LocalizationSourceName);
                }

                return _localizationSource;
            }
        }

        Lazy<ISettingManager> settingManager;
        protected ISettingManager SettingManager => settingManager.Value;

        Lazy<ILoggerFactory> loggerFactory;

        ILogger _logger;
        protected ILogger Logger
        {
            get
            {
                if (_logger == default)
                    _logger = loggerFactory.Value.Create(this.GetType());
                return _logger;
            }
        }

        Lazy<IObjectMapper> objectMapper;
        protected IObjectMapper ObjectMapper => objectMapper.Value;

        Lazy<IPermissionManager> permissionManager;
        public IPermissionManager PermissionManager => permissionManager.Value;

        Lazy<IPermissionChecker> permissionChecker;
        public IPermissionChecker PermissionChecker => permissionChecker.Value;

        Lazy<IFeatureManager> featureManager;
        public IFeatureManager FeatureManager => featureManager.Value;

        Lazy<IFeatureChecker> featureChecker;
        public IFeatureChecker FeatureChecker => featureChecker.Value;

        //生命周期方法没有原生的aop支持，参考：https://github.com/dotnet/aspnetcore/issues/20986

        //参考：https://learn.microsoft.com/zh-cn/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-7.0#utility-base-component-classes-to-manage-a-di-scope
        //不确定构造函数注入有问题没，保险起见不用吧，参考abpVnext中也没有在构造函数注入
        //public AbpComponentBase(Lazy<IAbpSession> abpSession, Lazy<TTenantManager> tenantManager, Lazy<TRoleManager> roleManager, Lazy<TUserManager> userManager)
        //{
        //    this.abpSession = abpSession;
        //    this.tenantManager = tenantManager;
        //    this.userManager = userManager;
        //    this.roleManager = roleManager;
        //}

      

        //不要用异步方法做服务注入
        protected override void OnInitialized()
        {
            abpSession = ScopedServices.GetRequiredService<Lazy<IAbpSession>>();
            //tenantManager = ScopedServices.GetRequiredService<Lazy<TTenantManager>>();
            //roleManager = ScopedServices.GetRequiredService<Lazy<TRoleManager>>();
            userManager = ScopedServices.GetRequiredService<Lazy<TUserManager>>();
            unitOfWorkManager = ScopedServices.GetRequiredService<Lazy<IUnitOfWorkManager>>();
            localizationManager = ScopedServices.GetRequiredService<Lazy<ILocalizationManager>>();
            settingManager = ScopedServices.GetRequiredService<Lazy<ISettingManager>>();
            objectMapper = ScopedServices.GetRequiredService<Lazy<IObjectMapper>>();
            loggerFactory = ScopedServices.GetRequiredService<Lazy<ILoggerFactory>>();
            permissionChecker = ScopedServices.GetRequiredService<Lazy<IPermissionChecker>>();
            permissionManager = ScopedServices.GetRequiredService<Lazy<IPermissionManager>>();
            featureChecker = ScopedServices.GetRequiredService<Lazy<IFeatureChecker>>();
            featureManager = ScopedServices.GetRequiredService<Lazy<IFeatureManager>>();
        }

        protected virtual string L(string name)
        {
            return LocalizationSource.GetString(name);
        }
        protected virtual string L(string name, params object[] args)
        {
            return LocalizationSource.GetString(name, args);
        }
        protected virtual string L(string name, CultureInfo culture)
        {
            return LocalizationSource.GetString(name, culture);
        }
        protected virtual string L(string name, CultureInfo culture, params object[] args)
        {
            return LocalizationSource.GetString(name, culture, args);
        }
    }
}
