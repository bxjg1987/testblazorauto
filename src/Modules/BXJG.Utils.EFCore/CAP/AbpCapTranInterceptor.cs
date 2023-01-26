using Abp.Dependency;
using Abp.Domain.Uow;
using Castle.DynamicProxy;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.CAP
{
    public class AbpCapTranInterceptor : AbpInterceptorBase, ITransientDependency
    {
        private readonly ICapPublisher capPublisher;
        private readonly AbpCapTransaction abpCapTransaction;

        public AbpCapTranInterceptor(ICapPublisher capPublisher, AbpCapTransaction abpCapTransaction)
        {
            this.capPublisher = capPublisher;
            this.abpCapTransaction = abpCapTransaction;
        }

        public override void InterceptSynchronous(IInvocation invocation)
        {
            capPublisher.Transaction.Value = abpCapTransaction;
            this.capPublisher.Transaction.Value.AutoCommit = true;
            //throw new NotImplementedException();
            invocation.Proceed();
        }

        protected override async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            capPublisher.Transaction.Value = abpCapTransaction;
            this.capPublisher.Transaction.Value.AutoCommit = true;
            var proceedInfo = invocation.CaptureProceedInfo();
            proceedInfo.Invoke();
            var task = (Task)invocation.ReturnValue;
            await task;
        }

        protected override async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            capPublisher.Transaction.Value = abpCapTransaction;
            this.capPublisher.Transaction.Value.AutoCommit = true;
            var proceedInfo = invocation.CaptureProceedInfo();
            proceedInfo.Invoke();
            var taskResult = (Task<TResult>)invocation.ReturnValue;
            return await taskResult;
        }
    }
}
