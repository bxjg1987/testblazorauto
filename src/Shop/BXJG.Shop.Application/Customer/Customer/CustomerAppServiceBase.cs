using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using BXJG.Common;
using BXJG.GeneralTree;
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
    public abstract class CustomerAppServiceBase : AppServiceBase, IApplicationService
    {
        /// <summary>
        /// 顾客实体仓储
        /// </summary>
        protected readonly IRepository<CustomerEntity, long> customerRepository;
        /// <summary>
        /// 顾客领域服务
        /// </summary>
        protected readonly CustomerManager customerManager;
        /// <summary>
        /// 顾客session
        /// </summary>
        protected readonly ICustomerSession customerSession;

        /// <summary>
        /// 实例化登录的顾客的应用服务基类
        /// </summary>
        /// <param name="customerRepository">顾客实体仓储</param>
        /// <param name="customerManager">顾客领域服务</param>
        /// <param name="customerSession">顾客session</param>
        public CustomerAppServiceBase(IRepository<CustomerEntity, long> customerRepository, CustomerManager customerManager, ICustomerSession customerSession)
        {
            this.customerRepository = customerRepository;
            this.customerManager = customerManager;
            this.customerSession = customerSession;
        }
        /// <summary>
        /// 获取当前登录用户关联的顾客信息
        /// </summary>
        /// <returns></returns>
        protected virtual Task<CustomerEntity> GetCurrentCustomerAsync()
        {
            return customerRepository.SingleByUserIdAsync(AbpSession.UserId.Value);
        }
        /// <summary>
        /// 获取当前登录用户关联的顾客id
        /// </summary>
        /// <returns></returns>
        protected virtual ValueTask<long?> GetCurrentCustomerIdAsync()
        {
            return customerSession.GetCurrentCustomerIdAsync();
        }
    }
}
