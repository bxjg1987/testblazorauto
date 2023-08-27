using Abp;
using Abp.Application.Features;
using Abp.AspNetCore.Configuration;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.MultiTenancy;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.UI;
using BXJG.Common;
using BXJG.Common.Dto;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BXJG.Utils
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

    public class AbpBaseComponent/*<TUser, TUserManager, TRole>*/ : OwningComponentBase
    //where TUser : AbpUser<TUser>
    //where TRole : AbpRole<TUser>, new()
    //where TUserManager : AbpUserManager<TRole, TUser>
    {
        private IAbpSession abpSession;
        /// <summary>
        /// 获取当前session
        /// </summary>
        protected IAbpSession AbpSession => abpSession ??= ScopedServices.GetRequiredService<IAbpSession>();

        private IWebHostEnvironment webHostEnvironment;
        /// <summary>
        /// 获取当前环境
        /// </summary>
        protected IWebHostEnvironment WebHostEnvironment => webHostEnvironment ??= ScopedServices.GetRequiredService<IWebHostEnvironment>();

        private IEventBus eventBus;
        /// <summary>
        /// 获取abp事件总线
        /// </summary>
        protected IEventBus EventBus => eventBus ??= ScopedServices.GetRequiredService<IEventBus>();

        private Zhongjie zhongjie;
        /// <summary>
        /// 获取变形精怪中介
        /// </summary>
        protected Zhongjie Zhongjie => zhongjie ??= ScopedServices.GetRequiredService<Zhongjie>();


        private IUnitOfWorkDefaultOptions unitOfWorkDefaultOptions;
        /// <summary>
        /// 获取abp工作单元配置对象
        /// </summary>
        protected IUnitOfWorkDefaultOptions UnitOfWorkDefaultOptions => unitOfWorkDefaultOptions ??= ScopedServices.GetRequiredService<IUnitOfWorkDefaultOptions>();

        private ICancellationTokenProvider cancellationTokenProvider;
        /// <summary>
        /// 获取abp取消令牌提供者
        /// </summary>
        public ICancellationTokenProvider CancellationTokenProvider => cancellationTokenProvider ??= ScopedServices.GetRequiredService<ICancellationTokenProvider>();

        private IAbpAspNetCoreConfiguration aspnetCoreConfiguration;
        /// <summary>
        /// 获取abp aspnetcore配置对象
        /// </summary>
        public IAbpAspNetCoreConfiguration AspNetCoreConfiguration => aspnetCoreConfiguration ??= ScopedServices.GetRequiredService<IAbpAspNetCoreConfiguration>();

        /// <summary>
        /// 获取当前组件只读的全局取消令牌源
        /// </summary>
        protected readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        protected override void Dispose(bool disposing)
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource?.Dispose();

            base.Dispose(disposing);
        }

        //Lazy<TTenantManager> tenantManager;
        //protected TTenantManager TenantManager => tenantManager.Value;

        //Lazy<TRoleManager> roleManager;
        //protected TRoleManager RoleManager => roleManager.Value;

        //Lazy<TUserManager> userManager;
        //protected TUserManager UserManager => userManager.Value;

        private IUnitOfWorkManager unitOfWorkManager;
        /// <summary>
        /// 获取abp工作单元管理器
        /// </summary>
        protected IUnitOfWorkManager UnitOfWorkManager => unitOfWorkManager ??= ScopedServices.GetRequiredService<IUnitOfWorkManager>();

        //这个是跟线程相关的，在blazor中不能用
        //protected IActiveUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;

        ILocalizationManager localizationManager;
        /// <summary>
        /// 获取abp本地化管理器
        /// </summary>
        protected ILocalizationManager LocalizationManager => localizationManager ??= ScopedServices.GetRequiredService<ILocalizationManager>();


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

        private ISettingManager settingManager;
        protected ISettingManager SettingManager => settingManager ??= ScopedServices.GetRequiredService<ISettingManager>();

        //   ILoggerFactory loggerFactory;

        private ILogger _logger;
        protected ILogger Logger
        {
            get
            {
                if (_logger == default)
                    _logger = ScopedServices.GetRequiredService<ILoggerFactory>().Create(this.GetType());
                return _logger;
            }
        }

        private IObjectMapper objectMapper;
        protected IObjectMapper ObjectMapper => objectMapper ??= ScopedServices.GetRequiredService<IObjectMapper>();

        private IPermissionManager permissionManager;
        protected IPermissionManager PermissionManager => permissionManager ??= ScopedServices.GetRequiredService<IPermissionManager>();

        private IPermissionChecker permissionChecker;
        protected IPermissionChecker PermissionChecker => permissionChecker ??= ScopedServices.GetRequiredService<IPermissionChecker>();

        private IFeatureManager featureManager;
        protected IFeatureManager FeatureManager => featureManager ??= ScopedServices.GetRequiredService<IFeatureManager>();

        private IFeatureChecker featureChecker;
        protected IFeatureChecker FeatureChecker => featureChecker ??= ScopedServices.GetRequiredService<IFeatureChecker>();

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

        protected int? TenantId;
        protected long? UserId;

        //不要用异步方法做服务注入
        protected override void OnInitialized()
        {
            //AbpSession = ScopedServices.GetRequiredService<IAbpSession>();

            TenantId = AbpSession.TenantId;
            UserId = AbpSession.UserId;
            //EventBus = ScopedServices.GetRequiredService<IEventBus>();
            //WebHostEnvironment = ScopedServices.GetRequiredService<IWebHostEnvironment>();
            //Zhongjie = ScopedServices.GetRequiredService<Zhongjie>();
            ////tenantManager = ScopedServices.GetRequiredService<Lazy<TTenantManager>>();
            ////roleManager = ScopedServices.GetRequiredService<Lazy<TRoleManager>>();
            ////userManager = ScopedServices.GetRequiredService<Lazy<TUserManager>>();
            //unitOfWorkManager = ScopedServices.GetRequiredService<Lazy<IUnitOfWorkManager>>();
            //localizationManager = ScopedServices.GetRequiredService<Lazy<ILocalizationManager>>();
            //settingManager = ScopedServices.GetRequiredService<Lazy<ISettingManager>>();
            //objectMapper = ScopedServices.GetRequiredService<Lazy<IObjectMapper>>();
            //loggerFactory = ScopedServices.GetRequiredService<Lazy<ILoggerFactory>>();
            //permissionChecker = ScopedServices.GetRequiredService<Lazy<IPermissionChecker>>();
            //permissionManager = ScopedServices.GetRequiredService<Lazy<IPermissionManager>>();
            //featureChecker = ScopedServices.GetRequiredService<Lazy<IFeatureChecker>>();
            //featureManager = ScopedServices.GetRequiredService<Lazy<IFeatureManager>>();
            //unitOfWorkDefaultOptions = ScopedServices.GetRequiredService<IUnitOfWorkDefaultOptions>();
            //aspnetCoreConfiguration = ScopedServices.GetRequiredService<IAbpAspNetCoreConfiguration>();
            //cancellationTokenProvider = ScopedServices.GetRequiredService<ICancellationTokenProvider>();

            SafeExecute(OnInitialized2);
        }

        /// <summary>
        /// 子类重新次方法，可用避免手动调用SafeExecute
        /// </summary>
        protected virtual void OnInitialized2() { }

        protected override Task OnInitializedAsync()
        {
            return SafeExecuteAsync(OnInitialized2Async);
            //  return base.OnInitializedAsync();
        }
        /// <summary>
        /// 子类重新次方法，可用避免手动调用SafeExecute
        /// </summary>
        protected virtual Task OnInitialized2Async() => Task.CompletedTask;

        protected override void OnAfterRender(bool firstRender)
        {
            SafeExecute(() => OnAfterRender2(firstRender));
            //base.OnAfterRender(firstRender);
        }
        protected virtual void OnAfterRender2(bool firstRender) { }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return SafeExecuteAsync(() => OnAfterRender2Async(firstRender));
        }
        protected virtual Task OnAfterRender2Async(bool firstRender) => Task.CompletedTask;


        protected override void OnParametersSet()
        {
            SafeExecute(OnParametersSet2);
            //base.OnParametersSet();
        }
        protected virtual void OnParametersSet2() { }

        protected override async Task OnParametersSetAsync()
        {
            await SafeExecuteAsync(OnParametersSet2Async);
            //return base.OnParametersSetAsync();
        }
        protected virtual Task OnParametersSet2Async() => Task.CompletedTask;

        //这里不要搞，它比init先执行
        //public override async Task SetParametersAsync(ParameterView parameters)
        //{
        //    await SafeExecuteAsync(()=>SetParameters2Async(parameters));
        //    await base.SetParametersAsync(parameters);
        //}
        //public virtual Task SetParameters2Async(ParameterView parameters)=> Task.CompletedTask;


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


        ///// <summary>
        ///// 批量操作检查
        ///// </summary>
        //protected virtual void BatchOperationCheck(BatchOperationOutputBase output, string operationName = "操作", Func<BatchOperationOutputBase, string> errorItemFormatter = default)
        //{
        //    if (output.ErrorMessage.Any())
        //    {
        //        string str;
        //        if (errorItemFormatter != default)
        //            str = errorItemFormatter(output);
        //        else
        //        {
        //            var sb = new StringBuilder();
        //            foreach (var item in output.ErrorMessage)
        //            {
        //                sb.Append("[");
        //                sb.Append(item.Id);
        //                sb.Append("]");
        //                sb.AppendLine(item.Message);
        //            }
        //            str = sb.ToString();
        //        }
        //        throw new UserFriendlyException($"{operationName}时，部分成功！失败项：{str}");
        //    }
        //}

        /// <summary>
        /// 执行委托，用户友好异常时直接显示错误消息（记得重写ShowErrorAsync），否则记录日志并显示服务端错误消息。
        /// 默认情况下自动处理取消问题，特殊情况修改cts或替换canceltokenprovider
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected virtual async Task SafeExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            // Logger.Debug("aaa");
            // Logger.Debug(action.Method.Name);
            try
            {
                /*
                 * 主cts = 连接 参数的ct？
                 * 不好，因为主的可能被其它地方调用，当前调用取消是并不一定希望其它地方取消
                 * 
                 * 参数的ct引用主的?
                 * 可以的，不过这不应该在抽象中来决定
                 * 
                 */

                var ct1 = cancellationToken == default ? CancellationTokenSource.Token : cancellationToken;
                //   Logger.Debug("bbbb");
                using (var ct = CancellationTokenProvider.Use(ct1))
                {
                    //     Logger.Debug("ccc");
                    await action();
                    //   Logger.Debug("dddd");
                    //查看集成blazor文档，已全局开启按约定的拦截器
                    ////https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp.AspNetCore/AspNetCore/Mvc/Uow/AbpUowActionFilter.cs#L14
                    //var unitOfWorkAttr = unitOfWorkDefaultOptions
                    //    .GetUnitOfWorkAttributeOrNull(action.Method) ??
                    //    aspnetCoreConfiguration.DefaultUnitOfWorkAttribute;

                    //if (!unitOfWorkAttr.IsDisabled)
                    //{
                    //    //https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp/Domain/Uow/UnitOfWorkAttribute.cs#L189
                    //    var opt = new UnitOfWorkOptions
                    //    {
                    //        IsTransactional = unitOfWorkAttr.IsTransactional,
                    //        IsolationLevel = unitOfWorkAttr.IsolationLevel,
                    //        Timeout = unitOfWorkAttr.Timeout,
                    //        Scope = unitOfWorkAttr.Scope
                    //    };
                    //    using (var uow = UnitOfWorkManager.Begin(opt))
                    //    {
                    //        await action();
                    //        if (opt.IsTransactional == true)
                    //            await uow.CompleteAsync();
                    //    }
                    //}
                    //else
                    //{
                    //    await action();
                    //}
                }
            }
            catch (UserFriendlyException ex)
            {
                await ShowErrorAsync(ex.Message);
            }
            catch (Exception ex)
            {
                if (WebHostEnvironment.IsDevelopment())
                    throw;

                Logger.Error(ex.ToString(), ex);
                await ShowErrorAsync(L("InternalServerError"));
            }
        }
        /// <summary>
        /// 执行委托，用户友好异常时直接显示错误消息（记得重写ShowErrorAsync），否则记录日志并显示服务端错误消息。
        /// 默认情况下自动处理取消问题，特殊情况修改cts或替换canceltokenprovider
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        protected virtual async Task<T> SafeExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
        {
            try
            {
                /*
                  * 主cts = 连接 参数的ct？
                  * 不好，因为主的可能被其它地方调用，当前调用取消是并不一定希望其它地方取消
                  * 
                  * 参数的ct引用主的?
                  * 可以的，不过这不应该在抽象中来决定
                  * 
                  */
                var ct1 = cancellationToken == default ? CancellationTokenSource.Token : cancellationToken;
                using (var ct = CancellationTokenProvider.Use(ct1))
                {

                    return await action();

                    //已在全局开启按约定的拦截器，参考blazor集成文档
                    ////https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp.AspNetCore/AspNetCore/Mvc/Uow/AbpUowActionFilter.cs#L14
                    //var unitOfWorkAttr = unitOfWorkDefaultOptions
                    //    .GetUnitOfWorkAttributeOrNull(action.Method) ??
                    //    aspnetCoreConfiguration.DefaultUnitOfWorkAttribute;

                    //if (!unitOfWorkAttr.IsDisabled)
                    //{
                    //    //https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp/Domain/Uow/UnitOfWorkAttribute.cs#L189
                    //    var opt = new UnitOfWorkOptions
                    //    {
                    //        IsTransactional = unitOfWorkAttr.IsTransactional,
                    //        IsolationLevel = unitOfWorkAttr.IsolationLevel,
                    //        Timeout = unitOfWorkAttr.Timeout,
                    //        Scope = unitOfWorkAttr.Scope
                    //    };
                    //    using (var uow = UnitOfWorkManager.Begin(opt))
                    //    {
                    //        var r = await action();
                    //        if (opt.IsTransactional == true)
                    //            await uow.CompleteAsync();
                    //        return r;
                    //    }
                    //}
                    //else
                    //{
                    //    return await action();
                    //}
                }
            }
            catch (UserFriendlyException ex)
            {
                await ShowErrorAsync(ex.Message);
            }
            catch (Exception ex)
            {
                if (WebHostEnvironment.IsDevelopment())
                    throw;

                Logger.Error(ex.ToString(), ex);
                await ShowErrorAsync(L("InternalServerError"));
            }
            return default;
        }
        public virtual ValueTask ShowErrorAsync(string msg) => ValueTask.CompletedTask;

        public virtual void ShowError(string msg) { }
        /// <summary>
        /// 执行委托，用户友好异常时直接显示错误消息（记得重写ShowError），否则记录日志并显示服务端错误消息。
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected virtual void SafeExecute(Action action)
        {
            try
            {
                /*
                   * 主cts = 连接 参数的ct？
                   * 不好，因为主的可能被其它地方调用，当前调用取消是并不一定希望其它地方取消
                   * 
                   * 参数的ct引用主的?
                   * 可以的，不过这不应该在抽象中来决定
                   * 
                   */

                action();

                //已在全局开启按约定的拦截器，参考blazor集成文档
                ////https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp.AspNetCore/AspNetCore/Mvc/Uow/AbpUowActionFilter.cs#L14
                //var unitOfWorkAttr = unitOfWorkDefaultOptions
                //    .GetUnitOfWorkAttributeOrNull(action.Method) ??
                //    aspnetCoreConfiguration.DefaultUnitOfWorkAttribute;

                //if (!unitOfWorkAttr.IsDisabled)
                //{
                //    //https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp/Domain/Uow/UnitOfWorkAttribute.cs#L189
                //    var opt = new UnitOfWorkOptions
                //    {
                //        IsTransactional = unitOfWorkAttr.IsTransactional,
                //        IsolationLevel = unitOfWorkAttr.IsolationLevel,
                //        Timeout = unitOfWorkAttr.Timeout,
                //        Scope = unitOfWorkAttr.Scope
                //    };
                //    using (var uow = UnitOfWorkManager.Begin(opt))
                //    {
                //        action();
                //        if (opt.IsTransactional == true)
                //            uow.Complete();
                //    }
                //}
                //else
                //{
                //    action();
                //}
            }
            catch (UserFriendlyException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                if (WebHostEnvironment.IsDevelopment())
                    throw;

                Logger.Error(ex.ToString(), ex);
                ShowError(L("InternalServerError"));
            }
        }
        protected virtual T SafeExecute<T>(Func<T> action)
        {
            try
            {
                /*
                    * 主cts = 连接 参数的ct？
                    * 不好，因为主的可能被其它地方调用，当前调用取消是并不一定希望其它地方取消
                    * 
                    * 参数的ct引用主的?
                    * 可以的，不过这不应该在抽象中来决定
                    * 
                    */

                return action();

                //已在全局开启按约定的拦截器，参考blazor集成文档
                ////https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp.AspNetCore/AspNetCore/Mvc/Uow/AbpUowActionFilter.cs#L14
                //var unitOfWorkAttr = unitOfWorkDefaultOptions
                //    .GetUnitOfWorkAttributeOrNull(action.Method) ??
                //    aspnetCoreConfiguration.DefaultUnitOfWorkAttribute;

                //if (!unitOfWorkAttr.IsDisabled)
                //{
                //    //https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp/Domain/Uow/UnitOfWorkAttribute.cs#L189
                //    var opt = new UnitOfWorkOptions
                //    {
                //        IsTransactional = unitOfWorkAttr.IsTransactional,
                //        IsolationLevel = unitOfWorkAttr.IsolationLevel,
                //        Timeout = unitOfWorkAttr.Timeout,
                //        Scope = unitOfWorkAttr.Scope
                //    };
                //    using (var uow = UnitOfWorkManager.Begin(opt))
                //    {
                //        var r = action();
                //        if (opt.IsTransactional == true)
                //            uow.Complete();
                //        return r;
                //    }
                //}
                //else
                //{
                //    return action();
                //}
            }
            catch (UserFriendlyException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                if (WebHostEnvironment.IsDevelopment())
                    throw;

                Logger.Error(ex.ToString(), ex);
                ShowError(L("InternalServerError"));
            }
            return default;
        }

        protected IDisposable ResumeSession()
        {
            //if(AbpSession.TenantId.HasValue )
            return AbpSession.Use(TenantId, UserId);
        }
    }

    //已在全局开启按约定的拦截器，参考blazor集成文档
    ////https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp/Domain/Uow/UnitOfWorkDefaultOptionsExtensions.cs#L9
    //internal static class UnitOfWorkDefaultOptionsExtensions
    //{
    //    public static UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(this IUnitOfWorkDefaultOptions unitOfWorkDefaultOptions, MethodInfo methodInfo)
    //    {
    //        var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
    //        if (attrs.Length > 0)
    //        {
    //            return attrs[0];
    //        }

    //        attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
    //        if (attrs.Length > 0)
    //        {
    //            return attrs[0];
    //        }

    //        if (unitOfWorkDefaultOptions.IsConventionalUowClass(methodInfo.DeclaringType))
    //        {
    //            return new UnitOfWorkAttribute(); //Default
    //        }

    //        return null;
    //    }

    //    public static bool IsConventionalUowClass(this IUnitOfWorkDefaultOptions unitOfWorkDefaultOptions, Type type)
    //    {
    //        return unitOfWorkDefaultOptions.ConventionalUowSelectors.Any(selector => selector(type));
    //    }
    //}
}
