using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Abp.UI;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{
    /*
     * 订单创建过程分析
     * -----------------------------------------------------------------
     * 流程：   
     *      前端登录的会员下单，指明购买哪些商品 数量是多少 和其它辅助信息...
     *      后端对提交来的数据做基本验证（由asp.net core的模型绑定完成）：比如各种必填项...数据长度之类的...
     *      检查用户状态是否正常，如：是否是黑名单用户...
     *      检查用户要购买的商品的状态是否正常
     *          商品目前是否处于发布状态，是否在发布时间范围内
     *          库存是否充足（这个问题有点复杂，因为多个用户可能同时发起请求购买同一个商品，需要考虑并发问题）
     *          待补充....
     *      检查系统配置：比如店铺目前是否关闭
     *      在数据库中创建订单
     *      其它后续处理
     *      返回结果
     *      
     *      流程总结：主要是围绕订单做各种判断、处理
     * 
     * 哪些场景会创建订单？
     *      前台顾客购买
     *      后台管理员可以直接新增订单吗？
     *      从其它地方导入订单？
     *      
     * 
     */

    /// <summary>
    /// 订单管理领域逻辑
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class OrderManager<TUser> : BXJGShopDomainServiceBase
        where TUser : AbpUserBase
    {
        protected readonly IRepository<OrderEntity<TUser>, long> repository;
        protected readonly IRepository<CustomerEntity<TUser>, long> customerRepository;
        protected readonly CustomerManager<TUser> customerManager;
        protected readonly IAbpSession session;

        public OrderManager(
            IRepository<OrderEntity<TUser>, long> repository,
            IRepository<CustomerEntity<TUser>, long> customerRepository,
            CustomerManager<TUser> customerManager,
            IAbpSession session
            )
        {
            this.repository = repository;
            this.customerRepository = customerRepository;
            this.customerManager = customerManager;
            this.session = session;
        }

        public async Task<OrderEntity<TUser>> CreateAsync(
            CustomerEntity<TUser> customer = null,
            DateTimeOffset? orderTime = null,
            string customerRemark = null,
            bool invoiceRequired = false,
            string consignee = "",
            params OrderItemInput[] items)
        {
            if (customer == null)
            {
                //if (!session.UserId.HasValue)
                //    throw new ApplicationException("创建订单时无法确定购买客户");
                //else
                //   await customerRepository.SingleByUserIdWithoutUserAsync(session.UserId.Value);

                //session.GetUserId()会报异常的
                await customerManager.GetCurrentWithoutUserAsync();
            }
            //顾客的各种业务逻辑判断，比如是否是黑名单顾客


            var order = new OrderEntity<TUser>
            {
                Customer = customer,
                CustomerId = customer.Id,
                OrderNo = Guid.NewGuid().ToString("N"),//将来再考虑用个专门的组件生产简单、不重复的订单号
                Status = OrderStatus.Created,
                CustomerRemark = customerRemark,
                InvoiceRequired = invoiceRequired,
                PaymentStatus = PaymentStatus.WaitingForPayment
            };
            //MerchandiseSubtotal 商品小计
            //DistributionFee 配送费 简单的情况 可以让 后台管理员确认订单时录入配送费；合理的情况是根据购买的商品自动计算
            //InvoiceTax 发票税金

            await repository.InsertAsync(order);
            await CurrentUnitOfWork.SaveChangesAsync();//保存，以获得新的自增id
            return order;
        }
    }
}
