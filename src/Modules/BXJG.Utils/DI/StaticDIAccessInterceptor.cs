using Abp.Dependency;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.DI
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
    }
}