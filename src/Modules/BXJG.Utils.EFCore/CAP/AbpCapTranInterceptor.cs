using Abp.Dependency;
using Abp.Domain.Uow;
using Castle.DynamicProxy;
using DotNetCore.CAP;
using DotNetCore.CAP.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.CAP
{
    /// <summary>
    /// 在方法执行前为ICapPublisher.Transaction赋值
    /// 注意ICapPublisher.Transaction是AsyncLocal类型的，且ICapPublisher本身是单例的
    /// 此拦截器在当前模块中注册的
    /// </summary>
    public class AbpCapTranInterceptor : AbpInterceptorBase, ITransientDependency
    {
        private readonly ICapPublisher capPublisher;
        private readonly BXJGUtilsModuleConfig abpCapTransaction;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IDispatcher dispatcher;
        public AbpCapTranInterceptor(ICapPublisher capPublisher, BXJGUtilsModuleConfig abpCapTransaction, IUnitOfWorkManager unitOfWorkManager, IDispatcher dispatcher)
        {
            this.capPublisher = capPublisher;
            this.abpCapTransaction = abpCapTransaction;
            this.unitOfWorkManager = unitOfWorkManager;
            this.dispatcher = dispatcher;
        }

        public override void InterceptSynchronous(IInvocation invocation)
        {
            Intercept();
            invocation.Proceed();
        }

        protected override async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            Intercept();
            var proceedInfo = invocation.CaptureProceedInfo();
            proceedInfo.Invoke();
            var task = (Task)invocation.ReturnValue;
            await task;
        }

        protected override async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            Intercept();
            var proceedInfo = invocation.CaptureProceedInfo();
            proceedInfo.Invoke();
            var taskResult = (Task<TResult>)invocation.ReturnValue;
            return await taskResult;
        }

        private void Intercept()
        {
            if (unitOfWorkManager.Current != null && unitOfWorkManager.Current.Options.IsTransactional == true)
            {
                capPublisher.Transaction.Value = abpCapTransaction.wt(dispatcher, unitOfWorkManager.Current);
                capPublisher.Transaction.Value.AutoCommit = false;
            }
        }
    }
}
