using Abp.Application.Services;
using Abp.Authorization.Users;
using Abp.CachedUniqueKeys;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Timing;
using BXJG.Utils.Share.DataPermission;
using Castle.Core;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.DataPermission
{
    /// <summary>
    /// 数据权限拦截器
    /// 它为dbcontext中的全局数据拦截器准备数据
    /// </summary>
    public class DataPermissionInterceptor : AbpInterceptorBase, ITransientDependency
    {
        public ILogger Logger { get; set; }
        public IAbpSession AbpSession { get; set; }

        public DataPermissionInterceptor()
        {
            Logger = NullLogger.Instance;
        }


        protected override async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            var proceedInfo = invocation.CaptureProceedInfo();

            // 1. 最高优先级：检查方法上是否有 Disable 属性
            if (invocation.Method.IsDefined(typeof(DisableDataPermissionAttribute), true))
            {
                proceedInfo.Invoke();
                await (Task)invocation.ReturnValue;
            }

            await LoadDataPermission();
            using var df = CurrentUnitOfWorkProvider.Current.EnableFilter(DataPermissionConsts.DataPermission);
            proceedInfo.Invoke();
            await (Task)invocation.ReturnValue;


        }
        protected override async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var proceedInfo = invocation.CaptureProceedInfo();

            // 1. 最高优先级：检查方法上是否有 Disable 属性
            if (invocation.Method.IsDefined(typeof(DisableDataPermissionAttribute), true))
            {
                proceedInfo.Invoke();
                return await (Task<TResult>)invocation.ReturnValue; ;
            }

            //Before method execution
            //var stopwatch = Stopwatch.StartNew();
            await LoadDataPermission();
            using var df = CurrentUnitOfWorkProvider.Current.EnableFilter(DataPermissionConsts.DataPermission);

            proceedInfo.Invoke();

            return await (Task<TResult>)invocation.ReturnValue;

        }

        public override void InterceptSynchronous(IInvocation invocation)
        {
            // base.InterceptSynchronous(invocation);
            invocation.Proceed();
            //throw new NotImplementedException();
        }

        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }
        public ICacheManager CacheManager { get; set; }

        /// <summary>
        /// 准备数据权限相关数据
        /// 在UnitOfWork中标记数据权限过滤器已启用
        /// </summary>
        /// <returns></returns>
        async Task LoadDataPermission()
        {
            if (!AbpSession.UserId.HasValue)
                return;

            // 数据权限规则现在由 IDataPermissionRuleProvider 提供
            // 缓存逻辑已在 DatabaseDataPermissionRuleProvider 中实现
            // 这里只需要启用过滤器，实际规则会在仓储中按需获取
            await Task.CompletedTask;
        }

        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                if (ShouldIntercept(handler.ComponentModel.Implementation))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AbpAsyncDeterminationInterceptor<DataPermissionInterceptor>)));
                }
            };
        }
        private static bool ShouldIntercept(Type type)
        {
            //// 只拦截应用服务
            //if (!typeof(IApplicationService).IsAssignableFrom(type))
            //{
            //    return false;
            //}

            // 1. 检查类上是否有 DataPermissionAttribute
            if (type.GetTypeInfo().IsDefined(typeof(DataPermissionAttribute), true))
            {
                return true;
            }

            // 2. 检查是否有任何方法上有 DataPermissionAttribute
            if (type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Any(m => m.IsDefined(typeof(DataPermissionAttribute), true)))
            {
                return true;
            }

            return false;
        }

    }
}
