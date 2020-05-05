using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 登录的顾客的应用服务基类
    /// 比如顾客下订单 支付订单 或其它顾客操作可能会定义不同的应用服务，这些服务都可以继承此类来简化代码
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TTenantManager"></typeparam>
    /// <typeparam name="TUserManager"></typeparam>
    /// <typeparam name="TCustomerManager"></typeparam>
    public abstract class BXJGShopCustomerAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager, TCustomerManager>
        : BXJGShopAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager>, IApplicationService
        where TUser : AbpUser<TUser>, new()
        where TRole : AbpRole<TUser>, new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager : AbpTenantManager<TTenant, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
        where TCustomerManager : CustomerManager<TUser>
    {
        protected readonly IRepository<CustomerEntity<TUser>, long> customerRepository;
        protected readonly TCustomerManager customerManager;
        protected readonly BXJGShopCustomerSession<TUser> customerSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerRepository"></param>
        /// <param name="customerManager"></param>
        /// <param name="customerSession"></param>
        public BXJGShopCustomerAppServiceBase(IRepository<CustomerEntity<TUser>, long> customerRepository, TCustomerManager customerManager, BXJGShopCustomerSession<TUser> customerSession)
        {
            this.customerRepository = customerRepository;
            this.customerManager = customerManager;
            this.customerSession = customerSession;
        }
        /// <summary>
        /// 获取当前登录用户关联的顾客信息
        /// </summary>
        /// <returns></returns>
        protected virtual Task<CustomerEntity<TUser>> GetCurrentCustomerAsync()
        {
            return customerRepository.SingleByUserIdWithoutUserAsync(base.AbpSession.UserId.Value);
        }
        /// <summary>
        /// 获取当前登录用户关联的顾客id
        /// </summary>
        /// <returns></returns>
        protected virtual Task<long> GetCurrentCustomerIdAsync()
        {
            return customerSession.GetCurrentCustomerIdAsync();
        }
    }
}
