using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
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
    public class CustomerManager<TUser,TArea> : BXJGShopDomainServiceBase//, IAsyncEventHandler<EntityCreatedEventData<OrderEntity<TUser>>>
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IShopAdministrative
    {

        protected readonly IRepository<CustomerEntity<TUser,TArea>, long> repository;
        //领域层 不应该访问Session
        //protected readonly IAbpSession session;

        public CustomerManager(IRepository<CustomerEntity<TUser, TArea>, long> repository, IAbpSession session)
        {
            this.repository = repository;
            
        }
        /// <summary>
        /// 根据用户id获取关联的顾客实体，同时加载用户实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<CustomerEntity<TUser, TArea>> SingleByUserIdWithUserAsync(long userId)
        {
            return repository.SingleByUserIdWithUserAsync<TUser,TArea>(userId);
        }
        /// <summary>
        /// 根据用户id获取关联的顾客实体，不加载用户实体
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<CustomerEntity<TUser, TArea>> SingleByUserIdWithoutUserAsync(long userId)
        {
            return repository.SingleByUserIdWithoutUserAsync<TUser,TArea>(userId);
        }
        ///// <summary>
        ///// 获取当前登录用户关联的顾客实体,不加载用户实体
        ///// 若是匿名用户则报异常
        ///// </summary>
        ///// <returns></returns>
        //public Task<CustomerEntity<TUser>> GetCurrentWithoutUserAsync()
        //{
        //    return SingleByUserIdWithoutUserAsync(session.GetUserId());
        //}
        ///// <summary>
        ///// 获取当前登录用户关联的顾客实体,同时加载用户实体
        ///// 若是匿名用户则报异常
        ///// </summary>
        ///// <returns></returns>
        //public Task<CustomerEntity<TUser>> GetCurrentWithUserAsync()
        //{
        //    return SingleByUserIdWithUserAsync(session.GetUserId());
        //}

        /// <summary>
        /// 增减顾客积分
        /// 有乐观并发不用怕
        /// 内部触发CustomerIntegralChangedEventData事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="integral">负数则为减积分</param>
        /// <returns></returns>
        public async Task ChangeIntegral(CustomerEntity<TUser, TArea> entity, long integral)
        {
            entity.Integral += integral;
            //即使调用了，后续事件处理异常了一样会回滚，参考 https://aspnetboilerplate.com/Pages/Documents/Unit-Of-Work#savechanges
            //文档是这么说的，没有试验过
            //await CurrentUnitOfWork.SaveChangesAsync(); 

            //单独弄了个事件 而不是使用abp提供的EntityChanged事件，这样保证只有在积分变动时才触发这个事件
            await EventBus.TriggerAsync(new CustomerIntegralChangedEventData<TUser,TArea>(entity));
        }
        ///// <summary>
        ///// 订单创建成功的事件处理
        ///// 增减积分
        ///// </summary>
        ///// <param name="eventData"></param>
        ///// <returns></returns>
        //public Task HandleEventAsync(EntityCreatedEventData<OrderEntity<TUser>> eventData)
        //{
        //    //var order = eventData.Entity;
        //    //return ChangeIntegral(order.Customer, order.Integral);

        //    //订单完成才能加积分 而不是订单创建时
        //    return Task.CompletedTask;
        //}
    }
}
