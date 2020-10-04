using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 基于asp.net claim方式的顾客session实现
    /// 参考：
    /// https://github.com/aspnetboilerplate/aspnetboilerplate/blob/98df3d15a1409e9823f11330c0d6d6d0b3118d26/src/Abp/Runtime/Session/ClaimsAbpSession.cs
    /// https://aspnetboilerplate.com/Pages/Documents/Articles%5CHow-To%5Cadd-custom-session-field-aspnet-core
    /// 如果需要使用它请在在BXJGShopCoreModule中注册，建议用单例
    /// 推荐配合CustomerLoginManager<TTenant, TRole, TUser, TUserManager>，在LoginManager中将当前登陆的顾客id存储到claim中
    /// 参考ZZLJ.Authorization.LoginManager.CreateLoginResultAsync
    /// </summary>
    public class CustomerClaimSession : ICustomerSession
    {
        private readonly IPrincipalAccessor PrincipalAccessor;
        public CustomerClaimSession(IPrincipalAccessor principalAccessor)
        {
            this.PrincipalAccessor = principalAccessor;
        }
        public async ValueTask<long?> GetCurrentCustomerIdAsync()
        {
            var userEmailClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == CoreConsts.CustomerIdClaim);
            if (string.IsNullOrEmpty(userEmailClaim?.Value))
                return null;
            return long.Parse(userEmailClaim.Value);
        }
    }
}
