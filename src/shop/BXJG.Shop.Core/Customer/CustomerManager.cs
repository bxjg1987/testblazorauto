using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Session;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using BXJG.Shop.Sale;
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
    public class CustomerManager<TUser,TArea> : BXJGShopDomainServiceBase//, ITransientDependency
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
    {
        protected readonly IRepository<CustomerEntity<TUser,TArea>, long> repository;
        //领域层 不应该访问Session
        //protected readonly IAbpSession session;

        public CustomerManager(IRepository<CustomerEntity<TUser,TArea>, long> repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// 根据用户id获取关联的顾客实体，同时加载用户实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<CustomerEntity<TUser,TArea>> SingleByUserIdWithUserAsync(long userId)
        {
            return repository.SingleByUserIdWithUserAsync<TUser,TArea>(userId);
        }
        /// <summary>
        /// 根据用户id获取关联的顾客实体，不加载用户实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<CustomerEntity<TUser,TArea>> SingleByUserIdWithoutUserAsync(long userId)
        {
            return repository.SingleByUserIdWithoutUserAsync<TUser,TArea>(userId);
        }
    }

}
