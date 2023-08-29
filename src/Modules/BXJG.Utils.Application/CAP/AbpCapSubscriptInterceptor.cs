using Abp.Dependency;
using Abp.Domain.Uow;
using Castle.DynamicProxy;
//using DotNetCore.CAP.Transport;
//using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BXJG.Utils.CAP
{
    /// <summary>
    /// 为cap订阅者自己动开启事务的拦截器
    /// </summary>
    public class AbpCapSubscriptInterceptor : AbpInterceptorBase, ITransientDependency
    {
        private readonly IUnitOfWorkManager unitOfWorkManager;
        public AbpCapSubscriptInterceptor(IUnitOfWorkManager unitOfWorkManager)
        {
            this.unitOfWorkManager = unitOfWorkManager;
        }

        public override void InterceptSynchronous(IInvocation invocation)
        {
            if (invocation.MethodInvocationTarget.GetCustomAttribute<CapSubscribeAttribute>() != default && unitOfWorkManager.Current == default)
            {
                using (var scope = this.unitOfWorkManager.Begin())
                {
                    //invocation.MethodInvocationTarget.GetParameters()
                    //using var d =  this.unitOfWorkManager.Current.SetTenantId();
                    //自动设置租户id
                    invocation.Proceed();
                    scope.Complete();
                }
            }
            else
                invocation.Proceed();
        }

        protected override async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            if (invocation.MethodInvocationTarget.GetCustomAttribute<CapSubscribeAttribute>() != default && unitOfWorkManager.Current == default)
            {
                using (var scope = this.unitOfWorkManager.Begin())
                {
                    var proceedInfo = invocation.CaptureProceedInfo();
                    proceedInfo.Invoke();
                    var task = (Task)invocation.ReturnValue;
                    await task;
                    await scope.CompleteAsync();
                    return;
                }
            }

            var proceedInfo1 = invocation.CaptureProceedInfo();
            proceedInfo1.Invoke();
            var task1 = (Task)invocation.ReturnValue;
            await task1;
        }

        protected override async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            if (invocation.MethodInvocationTarget.GetCustomAttribute<CapSubscribeAttribute>() != default && unitOfWorkManager.Current == default)
            {
                using (var scope = this.unitOfWorkManager.Begin())
                {
                    var proceedInfo = invocation.CaptureProceedInfo();
                    proceedInfo.Invoke();
                    var taskResult = (Task<TResult>)invocation.ReturnValue;
                    var r = await taskResult;
                    await scope.CompleteAsync();
                    return r;
                }
            }
            var proceedInfo1 = invocation.CaptureProceedInfo();
            proceedInfo1.Invoke();
            var taskResult1 = (Task<TResult>)invocation.ReturnValue;
            return await taskResult1;
        }
    }
}
