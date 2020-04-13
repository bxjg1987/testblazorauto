using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
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
        protected readonly IAbpSession session;

        public OrderManager(IRepository<OrderEntity<TUser>, long> repository,IAbpSession session)
        {
            this.repository = repository;
            this.session = session;
        }

        //好好考虑下 是否为订单定义一个Builder对象
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="orderTime"></param>
        /// <param name="orderNo"></param>
        /// <param name="customerRemark"></param>
        /// <param name="invoiceRequired"></param>
        /// <param name="consignee"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<OrderEntity<TUser>> CreateAsync(
            CustomerEntity<TUser> customer = null,
            DateTimeOffset? orderTime = null,
            string orderNo = "",
            string customerRemark = null,
            bool invoiceRequired = false,
            string consignee = "",
            params OrderItemInput[] items)
        {
            var order = new OrderEntity<TUser>();


            return order;
        }
    }
}
