using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 提供与顾客登陆相关功能
    /// </summary>
    public class CustomerLoginManager<TTenant, TRole, TUser, TUserManager>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        private readonly IRepository<CustomerEntity, long> repository;
        private readonly TUserManager userManager;
        //protected readonly IAbpSession session;//领域层 不应该访问Session

        public CustomerLoginManager(IRepository<CustomerEntity, long> repository, TUserManager userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }
        /// <summary>
        /// 尝试以顾客身份登陆
        /// 若当前用户属于Customer角色则认为是顾客，则将顾客id存储的claim中
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public async Task TryLoginAsync(AbpLoginResult<TTenant, TUser> r)
        {
            var isCustomer = await userManager.IsCustomerAsync(r.User);
            if (!isCustomer)
                return;
            var custId = await repository.GetCustomerIdByUserIdAsync(r.User.Id);
            r.Identity.AddClaim(new System.Security.Claims.Claim(BXJGShopConsts.CustomerIdClaim, custId.ToString()));
        }
    }
}
