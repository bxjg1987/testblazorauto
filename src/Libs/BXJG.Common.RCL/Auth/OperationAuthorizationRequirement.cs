using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.RCL.Auth
{
    public class OperationAuthorizationRequirement1 : AuthorizationHandler<OperationAuthorizationRequirement>//, IAuthorizationRequirement
    {
        public const string GrantedPermissionNamesProvider = "GrantedPermissionNamesProvider";

        private readonly ILogger logger;    
        public Func< ValueTask< IEnumerable<string>> > grantedPermissionNameProvoer;

        public OperationAuthorizationRequirement1([FromKeyedServices(OperationAuthorizationRequirement1.GrantedPermissionNamesProvider)] Func<ValueTask<IEnumerable<string>>> serviceProvider/*, params string[] permissionNames*/, ILogger<OperationAuthorizationRequirement1> logger)
        {
            this.grantedPermissionNameProvoer = serviceProvider;
            this.logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
          //  logger.LogDebug($"开始验证权限，名称：{requirement.Name}");
            var names = await grantedPermissionNameProvoer.Invoke();
            //logger.LogDebug($"已授权的列表，名称：{string.Join("，", names)}");
            if ( names.Contains(requirement.Name, StringComparer.OrdinalIgnoreCase))
                    context.Succeed(requirement);
            
        }
    }

    public class PermissionNameAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionNameAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var r = await base.GetPolicyAsync(policyName);
            if (r != null)
                return r;

            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new OperationAuthorizationRequirement { Name = policyName });
            return policy.Build();
        }
    }
}
