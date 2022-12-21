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

namespace BXJG.Utils.Authorization
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
                if (await permissionChecker.IsGrantedAsync(requirement.RequiredAll, requirement.PermissionNames))
                    context.Succeed(requirement);
            }
            catch (Exception ex)
            {
                logger.Debug("权限判断执行了同步操作", ex);
                if (permissionChecker.IsGranted(requirement.RequiredAll, requirement.PermissionNames))
                    context.Succeed(requirement);
            }
            //}
            //permissionChecker.IsGrantedAsync()
            //throw new NotImplementedException();
        }
    }
}
