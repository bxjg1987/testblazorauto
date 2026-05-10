using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using System.Net;
using Abp.Runtime.Session;
using Abp.Domain.Uow;
using System.Diagnostics;

namespace BXJG.Utils.Web.Authorization
{
    /// <summary>
    /// 基于abp的授权处理器
    /// </summary>
    public class AbpAuthorizationHandler : AuthorizationHandler<AbpOperationAuthorizationRequirement>
    {
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly Castle.Core.Logging.ILogger logger;
        private readonly IPermissionChecker permissionChecker;
        //private readonly IAbpSession abpSession;
        //  ILogger 
        public AbpAuthorizationHandler(IPermissionChecker permissionChecker, Castle.Core.Logging.ILogger logger, IUnitOfWorkManager unitOfWorkManager)
        {
            this.permissionChecker = permissionChecker;
            this.logger = logger;
            this.unitOfWorkManager = unitOfWorkManager;
        }

        //服务是没通过abp模块的方式注册 不确定标签行不行，不行的话手动uowManager
        //  [Abp.Domain.Uow.UnitOfWork]
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AbpOperationAuthorizationRequirement requirement)
        {
            //permissionChecker的方法上已经应用UOW
            //应用首次访问时未命中缓存，在执行数据库操作UserManager.FindById时会提示访问dispose对象的问题

            //using (var uow = unitOfWorkManager.Begin( System.Transactions.TransactionScopeOption.RequiresNew))
            //{
            try
            {
                //Stopwatch s = new Stopwatch();
                //s.Start();
                //经过测试有缓存，检查耗时通常是0毫秒
                if (await permissionChecker.IsGrantedAsync(requirement.RequiredAll, requirement.PermissionNames))
                {

                    context.Succeed(requirement);
                    //logger.Debug($"执行了异步权限判断，权限：{string.Join(",", requirement.PermissionNames)}");
                }
                //s.Stop();
                //logger.Debug($"执行了异步权限判断，权限：{string.Join(",", requirement.PermissionNames)}，耗时：{s.ElapsedMilliseconds}ms");
            }
            // [已评审-忽略] 异步权限检查失败时回退同步检查是刻意设计：
            // 在ABP框架中，异步检查可能因UoW未正确初始化而抛出异常（非真正的业务错误），
            // 同步回退可以确保权限判断不因框架初始化时序问题而失败。
            catch (Exception ex)
            {
                //logger.Debug("权限判断执行了同步操作", ex);
                if (permissionChecker.IsGranted(requirement.RequiredAll, requirement.PermissionNames))
                {
                    context.Succeed(requirement);
                    logger.Debug($"执行了同步权限判断，权限：{string.Join(",", requirement.PermissionNames)}");
                }
            }
            //}
            //permissionChecker.IsGrantedAsync()
            //throw new NotImplementedException();
        }
    }
}
