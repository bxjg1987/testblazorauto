using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    /*
     * 虽然ICustomerRepositoryExtensions提供了扩展方法，但是需要提供泛型TUser，并且也无法（也不合理）提供session的访问
     * 因此在领域服务提供一个封装
     */
    public class CustomerManager<TUser> : BXJGShopDomainServiceBase
        where TUser : AbpUserBase
    {
        protected readonly IRepository<CustomerEntity<TUser>, long> repository;
        protected readonly IAbpSession session;

        public CustomerManager(IRepository<CustomerEntity<TUser>, long> repository, IAbpSession session)
        {
            this.repository = repository;
            this.session = session;
        }
        /// <summary>
        /// 根据用户id获取关联的顾客实体，同时加载用户实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<CustomerEntity<TUser>> SingleByUserIdWithUserAsync(long userId)
        {
            return repository.SingleByUserIdWithUserAsync<TUser>(userId);
        }
        /// <summary>
        /// 根据用户id获取关联的顾客实体，不加载用户实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<CustomerEntity<TUser>> SingleByUserIdWithoutUserAsync(long userId)
        {
            return repository.SingleByUserIdWithoutUserAsync<TUser>(userId);
        }
        /// <summary>
        /// 获取当前登录用户关联的顾客实体,不加载用户实体
        /// 若是匿名用户则报异常
        /// </summary>
        /// <returns></returns>
        public Task<CustomerEntity<TUser>> GetCurrentWithoutUserAsync()
        {
            return SingleByUserIdWithoutUserAsync(session.GetUserId());
        }
        /// <summary>
        /// 获取当前登录用户关联的顾客实体,同时加载用户实体
        /// 若是匿名用户则报异常
        /// </summary>
        /// <returns></returns>
        public Task<CustomerEntity<TUser>> GetCurrentWithUserAsync()
        {
            return SingleByUserIdWithUserAsync(session.GetUserId());
        }
    }
}
