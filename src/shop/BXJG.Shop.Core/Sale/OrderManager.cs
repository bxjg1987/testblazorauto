using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 订单管理领域逻辑
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class OrderManager<TUser> : BXJGShopDomainServiceBase
        where TUser : AbpUserBase
    {
        protected readonly IRepository<OrderEntity<TUser>, long> repository;

        public OrderManager(IRepository<OrderEntity<TUser>, long> repository)
        {
            this.repository = repository;
        }
       //好好考虑下 是否为订单定义一个Builder对象
        public async Task<OrderEntity<TUser>> CreateAsync(
            CustomerEntity<TUser> customer, 
            (ItemEntity item, decimal count)[] items, 
            DateTimeOffset? orderTime=null,
            bool invoiceRequired=false, 
            string customerRemark=null)
        {
            var order = new OrderEntity<TUser>();


            return order;
        }
    }
}
