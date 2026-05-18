using Abp.Application.Services;
using Abp.Dependency;
using Abp.Domain.Uow;
using BXJG.Common;
using BXJG.Utils.DI;
using Castle.Core;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Interceptor
{
    /// <summary>
    /// 启用数据过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EnableDataFilterAttribute : Attribute
    {
        /// <summary>
        /// 过滤器名称列表
        /// </summary>
        public string[] FilterNames { get; set; }
        /// <summary>
        /// 过滤器名称列表
        /// </summary>
        /// <param name="filters"></param>
        public EnableDataFilterAttribute(params string[] filters)
        {
            this.FilterNames = filters;
        }
    }
    /// <summary>
    /// 禁用数据过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DisableDataFilterAttribute : Attribute
    {
        /// <summary>
        /// 过滤器名称列表
        /// </summary>
        public string[] FilterNames { get; set; }
        /// <summary>
        /// 过滤器名称列表
        /// </summary>
        /// <param name="filters"></param>
        public DisableDataFilterAttribute(params string[] filters)
        {
            this.FilterNames = filters;
        }
    }
    /// <summary>
    /// 用来启用禁用abp全局数据拦截器的拦截器
    /// </summary>
    public class DataFilterInterceptor : AbpInterceptorBase, ITransientDependency
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }
        public ILogger Logger { get; set; }
        public override void InterceptSynchronous(IInvocation invocation)
        {
            IDisposable sddf = NoDisposable.Instance, sddf2 = NoDisposable.Instance;
            var sdfdf = invocation.Method.GetCustomAttribute<EnableDataFilterAttribute>()
                ?? invocation.TargetType.GetCustomAttribute<EnableDataFilterAttribute>();
            if (sdfdf != default)
                sddf = UnitOfWorkManager?.Current?.EnableFilter(sdfdf.FilterNames);

            var sddf22 = invocation.Method.GetCustomAttribute<DisableDataFilterAttribute>()
                ?? invocation.TargetType.GetCustomAttribute<DisableDataFilterAttribute>();
            if (sddf22 != default)
                sddf2 = UnitOfWorkManager?.Current?.DisableFilter(sddf22.FilterNames);

            using (sddf)
            {
                using (sddf2)
                {
                    invocation.Proceed();
                }
            }
        }

        protected override async Task InternalInterceptAsynchronous(IInvocation invocation)
        {

            IDisposable sddf = NoDisposable.Instance, sddf2 = NoDisposable.Instance;
            var sdfdf = invocation.Method.GetCustomAttribute<EnableDataFilterAttribute>()
                ?? invocation.TargetType.GetCustomAttribute<EnableDataFilterAttribute>();
            if (sdfdf != default)
                sddf = UnitOfWorkManager?.Current?.EnableFilter(sdfdf.FilterNames);

            var sddf22 = invocation.Method.GetCustomAttribute<DisableDataFilterAttribute>()
                ?? invocation.TargetType.GetCustomAttribute<DisableDataFilterAttribute>();
            if (sddf22 != default)
                sddf2 = UnitOfWorkManager?.Current?.DisableFilter(sddf22.FilterNames);

            var proceedInfo = invocation.CaptureProceedInfo();
            using (sddf)
            {
                using (sddf2)
                {
                    proceedInfo.Invoke();
                    await (Task)invocation.ReturnValue;
                }
            }

        }

        protected override async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            IDisposable sddf = NoDisposable.Instance, sddf2 = NoDisposable.Instance;
            var sdfdf = invocation.Method.GetCustomAttribute<EnableDataFilterAttribute>()
                ?? invocation.TargetType.GetCustomAttribute<EnableDataFilterAttribute>();
            Logger.Debug($"拦截器{GetType().FullName} uowm：{UnitOfWorkManager == null} 当前uow：{UnitOfWorkManager.Current == null}");
            if (sdfdf != default)
            {
                Logger.Debug($"启用拦截器：{string.Join(',', sdfdf.FilterNames)}");
                sddf = UnitOfWorkManager.Current?.EnableFilter(sdfdf.FilterNames);
            }
            var sddf22 = invocation.Method.GetCustomAttribute<DisableDataFilterAttribute>()
                ?? invocation.TargetType.GetCustomAttribute<DisableDataFilterAttribute>();
            if (sddf22 != default)
            {
                Logger.Debug($"禁用拦截器：{string.Join(',', sddf22.FilterNames)}");
                sddf2 = UnitOfWorkManager.Current?.DisableFilter(sddf22.FilterNames);
            }
            var proceedInfo = invocation.CaptureProceedInfo();
            using (sddf)
            {
                using (sddf2)
                {
                    proceedInfo.Invoke();
                    var r = await (Task<TResult>)invocation.ReturnValue;
                    Logger.Debug($"返回值：{r}");
                    return r;
                }
            }
        }

        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                if (handler.ComponentModel.Implementation.IsDefined(typeof(EnableDataFilterAttribute), true) ||
                    handler.ComponentModel.Implementation.IsDefined(typeof(DisableDataFilterAttribute), true) ||
                    handler.ComponentModel.Implementation.GetMethods().Any(x => x.IsDefined(typeof(DisableDataFilterAttribute), true) || x.IsDefined(typeof(EnableDataFilterAttribute), true)))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AbpAsyncDeterminationInterceptor<DataFilterInterceptor>)));
                }
            };
        }
    }
}