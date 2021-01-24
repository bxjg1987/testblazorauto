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
        /// 顾客session
        /// </summary>
        protected readonly ICustomerSession customerSession;

        /// <summary>
        /// 实例化登录的顾客的应用服务基类
        /// </summary>
        /// <param name="customerSession">顾客session</param>
        public CustomerAppServiceBase(ICustomerSession customerSession)
        {
            this.customerSession = customerSession;
        }

        /// <summary>
        /// 获取当前登录用户关联的顾客id
        /// </summary>
        /// <returns></returns>
        protected virtual ValueTask<long?> GetCurrentCustomerIdOrNullAsync()
        {
            return customerSession.GetCurrentCustomerIdAsync();
        }
        /// <summary>
        /// 获取当前登录用户关联的顾客id
        /// </summary>
        /// <returns></returns>
        protected virtual async ValueTask<long> GetCurrentCustomerIdAsync()
        {
            return (await GetCurrentCustomerIdOrNullAsync()).Value;
        }
    }
    /// <summary>
    /// 登录的顾客的应用服务基类，可以获取当前顾客实体
    /// </summary>
    public abstract class CustomerAppServiceWithCustomerBase : CustomerAppServiceBase
    {
        public readonly IRepository<CustomerEntity, long> repository;


        protected CustomerAppServiceWithCustomerBase(ICustomerSession customerSession, IRepository<CustomerEntity, long> repository) : base(customerSession)
        {
            this.repository = repository;
        }
        /// <summary>
        /// 获取当前登录用户关联的顾客
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<CustomerEntity> GetCurrentCustomerOrNullAsync()
        {
            var id = await base.GetCurrentCustomerIdOrNullAsync();
            if (id.HasValue)
                return await repository.SingleAsync(c => c.Id == id.Value);
            return null;
        }
        /// <summary>
        /// 获取当前登录用户关联的顾客
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<CustomerEntity> GetCurrentCustomerAsync()
        {
            var id = await base.GetCurrentCustomerIdAsync();
            return await repository.SingleAsync(c => c.Id == id);
        }
    }
}
