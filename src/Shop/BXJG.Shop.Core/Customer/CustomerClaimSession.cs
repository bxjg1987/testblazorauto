using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using BXJG.Utils.BusinessUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 顾客session，用来获取当前登陆的顾客id
    /// </summary>
    public interface ICustomerSession : IBusinessUserSession<long>
    {
        ///// <summary>
        ///// 获取当前登陆用户关联的顾客信息
        ///// 若当前登陆用户不属于Customer角色则返回null
        ///// </summary>
        ///// <returns></returns>
        //ValueTask<long?> GetCurrentCustomerIdAsync();
    }
    /// <summary>
    /// 基于asp.net claim方式的顾客session实现
    /// 参考：
    /// https://github.com/aspnetboilerplate/aspnetboilerplate/blob/98df3d15a1409e9823f11330c0d6d6d0b3118d26/src/Abp/Runtime/Session/ClaimsAbpSession.cs
    /// https://aspnetboilerplate.com/Pages/Documents/Articles%5CHow-To%5Cadd-custom-session-field-aspnet-core
    /// 如果需要使用它请在在BXJGShopCoreModule中注册，建议用单例
    /// 推荐配合CustomerLoginManager<TTenant, TRole, TUser, TUserManager>，在LoginManager中将当前登陆的顾客id存储到claim中
    /// 参考ZLJ.Authorization.LoginManager.CreateLoginResultAsync
    /// 我们的文档：https://gitee.com/bxjg1987_admin/abp/wikis/多种用户类型?sort_id=3639713
    /// </summary>
    public class CustomerClaimSession : BusinessUserClaimSession<long>, ICustomerSession
    {
        public CustomerClaimSession(IPrincipalAccessor principalAccessor,
                                    IMultiTenancyConfig multiTenancy,
                                    ITenantResolver tenantResolver,
                                    IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) : base(principalAccessor,
                                                                                                                multiTenancy,
                                                                                                                tenantResolver,
                                                                                                                sessionOverrideScopeProvider,
                                                                                                                CoreConsts.CustomerIdClaim)
        {

        }
    }
}
