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
    //blazor客户端 基于功能的权限处理器，同时也是权限一句，有自定义policprovider创建

    public class OperationAuthorizationRequirement1 : AuthorizationHandler<OperationAuthorizationRequirement>//, IAuthorizationRequirement
    {
        public const string GrantedPermissionNamesProvider = "GrantedPermissionNamesProvider";

      

        //string[] PermissionNames;// { get; set; } //= new string[0];
        //public bool RequiredAll { get; set; } = false;

        public Func<   IEnumerable<string>   > serviceProvider;

        public OperationAuthorizationRequirement1([FromKeyedServices(OperationAuthorizationRequirement1.GrantedPermissionNamesProvider)] Func<ValueTask< IEnumerable<string>> > serviceProvider/*, params string[] permissionNames*/)
        {
            this.serviceProvider = serviceProvider;
         //   PermissionNames = permissionNames;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            var names = serviceProvider.Invoke();
            if (RequiredAll)
            {
                if (!PermissionNames.Except(names).Any())
                    context.Succeed(requirement);
            }
            else
            {
                if (PermissionNames.Any(d => names.Contains(d)))
                    context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class PermissionNameAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        Func<IEnumerable<string>> permissionNameProvider;

        public PermissionNameAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options,
                                             [FromKeyedServices(OperationAuthorizationRequirement1.GrantedPermissionNamesProvider)] Func<IEnumerable<string>> permissionNameProvider) : base(options)
        {
            this.permissionNameProvider = permissionNameProvider;
        }

        //这里是高频访问，后期仔细考虑是否需要new对象出来，是否可以缓存或对象池方式来提高性能
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // var scheme = await authenticationSchemeProvider.GetAllSchemesAsync();
            //  var ss = authenticationOptions.SchemeMap.Keys; // scheme.Select(c => c.Name);

            var r = await base.GetPolicyAsync(policyName);//若是
            if (r != null)
                return r;

            //Console.WriteLine("找到没有？"+ (r==null));

            //blazor的授权组件会做AuthorizationPolicy.Combi，会合并schemes合并的，这里保持null就行了
            //权限1,权限2 true
            //不太有必要使用AuthorizationPolicy，因为它无非是对授权依据和身份验证方案的简化，少new个Builder，性能更高
            if (policyName.Contains(','))
            {
                var ary = policyName.Split(' ');
                var names = ary[0].Split(',');
                var all = false;
                if (ary.Length > 1)
                {
                    all = bool.Parse(ary[1]);
                }
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new OperationAuthorizationRequirement1(permissionNameProvider, names) { RequiredAll = all });
                return policy.Build();

                //return new AuthorizationPolicy(new IAuthorizationRequirement[] { new OperationAuthorizationRequirement(permissionNameProvider, names) { RequiredAll = all } }, new string[0]);
            }
            else
            {
                var names1 = permissionNameProvider.Invoke();
                if (names1.Contains(policyName))
                {
                    var policy = new AuthorizationPolicyBuilder();
                    policy.AddRequirements(new OperationAuthorizationRequirement1(permissionNameProvider, policyName));
                    return policy.Build();
                }
                //return new AuthorizationPolicy(new IAuthorizationRequirement[] { new OperationAuthorizationRequirement(permissionNameProvider, policyName) }, new string[0]);
            }
            //Console.WriteLine("木有找到2");
            //return await base.GetPolicyAsync(policyName);
            return await base.GetDefaultPolicyAsync();
        }
    }
}
