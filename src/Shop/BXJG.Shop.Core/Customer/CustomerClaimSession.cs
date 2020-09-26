using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 基于asp.net claim方式的顾客session实现
    /// 参考：https://github.com/aspnetboilerplate/aspnetboilerplate/blob/98df3d15a1409e9823f11330c0d6d6d0b3118d26/src/Abp/Runtime/Session/ClaimsAbpSession.cs
    /// 如果需要使用它请在在BXJGShopCoreModule中注册，建议用单例
    /// 推荐配合CustomerLoginManager<TTenant, TRole, TUser, TUserManager>，在LoginManager中将当前登陆的顾客id存储到claim中
    /// 参考ZZLJ.Authorization.LoginManager.CreateLoginResultAsync
    /// </summary>
    public class CustomerClaimSession : ICustomerSession
    {
        public ValueTask<long> GetCurrentCustomerIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
