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

namespace BXJG.Utils.Authorization
{

    /// <summary>
    /// 基于abp的授权处理器
    /// </summary>
    public class AbpAuthorizationHandler : AuthorizationHandler<AbpOperationAuthorizationRequirement>
    {
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        private readonly IPermissionChecker permissionChecker;
        //private readonly IAbpSession abpSession;

        public AbpAuthorizationHandler(IPermissionChecker permissionChecker, ILogger<AbpAuthorizationHandler> logger)
        {
            this.permissionChecker = permissionChecker;
            this.logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AbpOperationAuthorizationRequirement requirement)
        {
            if (await permissionChecker.IsGrantedAsync(requirement.RequiredAll, requirement.PermissionNames))
                context.Succeed(requirement);
          
            //permissionChecker.IsGrantedAsync()
            //throw new NotImplementedException();
        }
    }
}
