using Abp.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Authorization
{
    /// <summary>
    /// 基于abp的权限点的方式动态创建授权策略的提供器
    /// </summary>
    public class AbpAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly IPermissionManager permissionManager;
        // private readonly IAuthenticationSchemeProvider authenticationSchemeProvider;
        private readonly AuthenticationOptions authenticationOptions;
        public AbpAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options,
                                              IPermissionManager permissionManager,
                                              // IAuthenticationSchemeProvider authenticationSchemeProvider,
                                              IOptions<AuthenticationOptions> authenticationOptions) : base(options)
        {
            this.permissionManager = permissionManager;
            //  this.authenticationSchemeProvider = authenticationSchemeProvider;
            this.authenticationOptions = authenticationOptions.Value;

            //  CookieAuthenticationOptions
        }

        //这里是高频访问，后期仔细考虑是否需要new对象出来，是否可以缓存或对象池方式来提高性能
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // var scheme = await authenticationSchemeProvider.GetAllSchemesAsync();
            //  var ss = authenticationOptions.SchemeMap.Keys; // scheme.Select(c => c.Name);

            //blazor的授权组件会做AuthorizationPolicy.Combi，会合并schemes合并的，这里保持null就行了
            //权限1,权限2 true
            //不太有必要使用AuthorizationPolicy，因为它无非是对授权依据和身份验证方案的简化，少new个Builder，性能更高

            var r = await base.GetPolicyAsync(policyName);//若是
            if (r != null)
                return r;

            if (policyName.Contains(","))
            {
                var ary = policyName.Split(' ');
                var names = ary[0].Split(',');
                var all = false;
                if (ary.Length > 1)
                {
                    all = bool.Parse(ary[1]);
                }

                return new AuthorizationPolicy(new IAuthorizationRequirement[] { new AbpOperationAuthorizationRequirement { PermissionNames = names, RequiredAll = all } }, authenticationOptions.SchemeMap.Keys);
            }
            else if (permissionManager.GetPermissionOrNull(policyName) != default)
            {
                //针对非复合型权限判断，这里确实可以在应用启动时 直接初始化所有单一权限策略，这里的代码就不需要了，直接最后拿到策略返回
                return new AuthorizationPolicy(new IAuthorizationRequirement[] { new AbpOperationAuthorizationRequirement { PermissionNames = new[] { policyName } } }, authenticationOptions.SchemeMap.Keys);
            }

            return await base.GetDefaultPolicyAsync();
        }
    }
}