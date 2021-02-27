using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
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
        protected readonly IRepository<CustomerEntity, long> repository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        /// <summary>
        /// 实例化登录的顾客的应用服务基类
        /// </summary>
        /// <param name="customerSession">顾客session</param>
        public CustomerAppServiceBase(ICustomerSession customerSession, IRepository<CustomerEntity, long> repository)
        {
            this.customerSession = customerSession;
            this.repository = repository;
        }

        /// <summary>
        /// 获取当前登录用户关联的顾客
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<CustomerEntity> GetCurrentCustomerAsync()
        {
            return await repository.SingleAsync(c => c.Id == customerSession.BusinessUserId);
        }
    }
}
