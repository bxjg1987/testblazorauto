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
        public IScopedIocResolver ScopedIocResolver { get; set; }
        public Castle.Core.Logging.ILogger Logger { get; set; }
       
        public override void InterceptSynchronous(IInvocation invocation)
        {
            var old = AbpDIStaticAccessor._resolver.Value;
            AbpDIStaticAccessor._resolver.Value = ScopedIocResolver;
            //Logger.Debug($"abp拦截器查看当前IocResolver：{AbpDIStaticAccessor._resolver.Value.GetHashCode()}");
            invocation.Proceed();
            AbpDIStaticAccessor._resolver.Value = old;
        }

        protected override async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            var proceedInfo = invocation.CaptureProceedInfo();
            var old = AbpDIStaticAccessor._resolver.Value;
            AbpDIStaticAccessor._resolver.Value = ScopedIocResolver;
            proceedInfo.Invoke();
            //Logger.Debug($"abp拦截器查看当前IocResolver：{AbpDIStaticAccessor._resolver.Value?.GetHashCode()}");
            await (Task)invocation.ReturnValue;
            AbpDIStaticAccessor._resolver.Value = old;
        }

        protected override async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var proceedInfo = invocation.CaptureProceedInfo();
            var old = AbpDIStaticAccessor._resolver.Value;
            AbpDIStaticAccessor._resolver.Value = ScopedIocResolver;
            proceedInfo.Invoke();
            //Logger.Debug($"abp拦截器查看当前IocResolver：{AbpDIStaticAccessor._resolver.Value?.GetHashCode()}");
            var r = await (Task<TResult>)invocation.ReturnValue;
            AbpDIStaticAccessor._resolver.Value = old;
            return r;
        }
    }
}