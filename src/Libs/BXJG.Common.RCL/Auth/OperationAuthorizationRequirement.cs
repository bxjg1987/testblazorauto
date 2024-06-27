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


        public Func< ValueTask< IEnumerable<string>> > grantedPermissionNameProvoer;

        public OperationAuthorizationRequirement1([FromKeyedServices(OperationAuthorizationRequirement1.GrantedPermissionNamesProvider)] Func<ValueTask<IEnumerable<string>>> serviceProvider/*, params string[] permissionNames*/)
        {
            this.grantedPermissionNameProvoer = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            var names = await grantedPermissionNameProvoer.Invoke();

            if( names.Contains(requirement.Name, StringComparer.OrdinalIgnoreCase))
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
