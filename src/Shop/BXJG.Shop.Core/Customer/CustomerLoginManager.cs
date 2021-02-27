using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using BXJG.Utils.BusinessUser;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    public interface ICustomerLoginManager<TUser> : IBusinessUserLoginManager<TUser> { }

    /// <summary>
    /// 提供与顾客登陆相关功能
    /// </summary>
    public class CustomerLoginManager<TTenant,
                                      TRole,
                                      TUser,
                                      TUserManager> : BusinessUserLoginManager<CustomerEntity,
                                                                               long,
                                                                               TTenant,
                                                                               TRole,
                                                                               TUser,
                                                                               TUserManager>, ICustomerLoginManager<TUser>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        public CustomerLoginManager(IRepository<CustomerEntity, long> repository,
                                    TUserManager userManager) : base(repository,
                                                                     userManager,
                                                                     CoreConsts.CustomerRoleName,
                                                                     CoreConsts.CustomerIdClaim)
        {

        }
    }
}
