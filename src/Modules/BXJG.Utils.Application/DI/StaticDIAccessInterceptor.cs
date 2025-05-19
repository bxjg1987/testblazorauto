using Abp.Application.Services;
using Abp.Dependency;
using BXJG.Utils.DI;
using Castle.Core;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application
{
    public class StaticDIAccessInterceptor : AbpInterceptorBase, ITransientDependency
    {
        //AbpKernelModule IocManager.Register<IScopedIocResolver, ScopedIocResolver>(DependencyLifeStyle.Transient);

        public IScopedIocResolver ScopedIocResolver { get; set; }
        public Castle.Core.Logging.ILogger Logger { get; set; }

        public override void InterceptSynchronous(IInvocation invocation)
        {
            var old = AbpDIStaticAccessor._resolver.Value;
            AbpDIStaticAccessor._resolver.Value = ScopedIocResolver;
            using (ScopedIocResolver)
            {
                //Logger.Debug($"abp拦截器查看当前IocResolver：{AbpDIStaticAccessor._resolver.Value.GetHashCode()}");
                try
                {
                    invocation.Proceed();
                }
                finally
                {
                    AbpDIStaticAccessor._resolver.Value = old;
                }
            }
        }

        protected override async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            var old = AbpDIStaticAccessor._resolver.Value;
            AbpDIStaticAccessor._resolver.Value = ScopedIocResolver;
            using (ScopedIocResolver)
            {
                try
                {
                    var proceedInfo = invocation.CaptureProceedInfo();
                    proceedInfo.Invoke();
                    //Logger.Debug($"abp拦截器查看当前IocResolver：{AbpDIStaticAccessor._resolver.Value?.GetHashCode()}");
                    await (Task)invocation.ReturnValue;
                }
                finally
                {
                    AbpDIStaticAccessor._resolver.Value = old;
                }
            }
        }
        protected override async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var old = AbpDIStaticAccessor._resolver.Value;
            AbpDIStaticAccessor._resolver.Value = ScopedIocResolver;
            using (ScopedIocResolver)
            {
                try
                {
                    var proceedInfo = invocation.CaptureProceedInfo();
                    proceedInfo.Invoke();
                    //Logger.Debug($"abp拦截器查看当前IocResolver：{AbpDIStaticAccessor._resolver.Value?.GetHashCode()}");
                    return await (Task<TResult>)invocation.ReturnValue;
                }
                finally
                {
                    AbpDIStaticAccessor._resolver.Value = old;
                }
            }
        }

        //这个是全局注册的，所有应用服务都会应用这个拦截器，比较浪费
        //另外还有个中间件，按理说应该二选一，那个也比较浪费
        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                if (typeof(IApplicationService).IsAssignableFrom(handler.ComponentModel.Implementation))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AbpAsyncDeterminationInterceptor<StaticDIAccessInterceptor>)));
                }
            };
        }
    }
}