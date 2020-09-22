using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{

    /// <summary>
    /// 顾客支付订单成功后的事件处理器
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea"></typeparam>
    public class OrderPaidEventHandler : BXJGShopDomainServiceBase, IAsyncEventHandler<OrderPaidEventData>
        
    {
        protected readonly IRepository<CustomerEntity, long> repository;


        public OrderPaidEventHandler(IRepository<CustomerEntity, long> repository)
        {
            this.repository = repository;

        }


        /// <summary>
        /// 增减顾客积分
        /// 有乐观并发不用怕
        /// 内部触发CustomerIntegralChangedEventData事件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="integral">负数则为减积分</param>
        /// <returns></returns>
        public async Task ChangeIntegralAsync(CustomerEntity entity, long integral)
        {
            entity.Integral += integral;
            //即使调用了，后续事件处理异常了一样会回滚，参考 https://aspnetboilerplate.com/Pages/Documents/Unit-Of-Work#savechanges
            //文档是这么说的，没有试验过
            //await CurrentUnitOfWork.SaveChangesAsync(); 

            //单独弄了个事件 而不是使用abp提供的EntityChanged事件，这样保证只有在积分变动时才触发这个事件
            await EventBus.TriggerAsync(new CustomerIntegralChangedEventData(entity));
        }
        /// <summary>
        /// 订单付款成功的事件处理
        /// 增加顾客积分
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        //[UnitOfWork] 不确定是否必须加
        public Task HandleEventAsync(OrderPaidEventData eventData)
        {
            return ChangeIntegralAsync(eventData.Entity.Customer, eventData.Entity.Integral);
        }
    }
}
