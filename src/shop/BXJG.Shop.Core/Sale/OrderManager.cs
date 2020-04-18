using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using BXJG.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BXJG.Utils.Extensions;
using System.Threading;
using Abp.Threading;

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
        //领域层不应该访问session  protected readonly IAbpSession session;

        public OrderManager(
            IRepository<OrderEntity<TUser>, long> repository,
            IRepository<CustomerEntity<TUser>, long> customerRepository,
            CustomerManager<TUser> customerManager)
        {
            this.repository = repository;
            this.customerRepository = customerRepository;
            this.customerManager = customerManager;
        }

        public async Task<OrderEntity<TUser>> CreateAsync(
            CustomerEntity<TUser> customer,
            string consignee,
            string consigneePhoneNumber,
            string receivingAddress,
            DateTimeOffset? orderTime = null,
            string customerRemark = null,
            bool invoiceRequired = false,
            params OrderItemInput[] items)
        {
            #region 各参数基本验证
            //前端、Controller都验证过了，反反复复验证没必要，依赖数据库的非空就够了
            //consignee.RequiredValidate(nameof(consignee));
            //consigneePhoneNumber.RequiredValidate(nameof(consigneePhoneNumber));
            //receivingAddress.RequiredValidate(nameof(receivingAddress));
            if (items == null || items.Length == 0)
                throw new ArgumentNullException(nameof(items));
            #endregion

            #region 顾客处理
            //领域层不应付访问session
            //if (customer == null)
            //    await customerManager.GetCurrentWithoutUserAsync(); //session.GetUserId()会报异常的
            //顾客的各种业务逻辑判断，比如是否是黑名单顾客
            #endregion

            #region 订单时间
            //所在服务器的当前时间未必准确
            //abp官方文档 提供了时间处理方案 https://aspnetboilerplate.com/Pages/Documents/Timing
            //.net后来提供的DateTimeOffset也许已经处理了这个问题
            if (!orderTime.HasValue)
                orderTime = DateTimeOffset.Now;
            #endregion

            //所有业务判断都成功时才创建订单对象
            var order = new OrderEntity<TUser>
            {
                Customer = customer,
                CustomerId = customer.Id,
                OrderNo = Guid.NewGuid().ToString("N"),//将来再考虑用个专门的组件生产简单、不重复的订单号
                OrderTime = orderTime.Value,
                Status = OrderStatus.Created,
                CustomerRemark = customerRemark,

                InvoiceRequired = invoiceRequired,
                PaymentStatus = PaymentStatus.WaitingForPayment,

                Consignee = consignee,
                ConsigneePhoneNumber = consigneePhoneNumber,
                ReceivingAddress = receivingAddress,
            };

            #region 订单明细
           
            foreach (var item in items)
            {


            }
            #endregion

            //MerchandiseSubtotal 商品小计
            //DistributionFee 配送费 简单的情况 可以让 后台管理员确认订单时录入配送费；合理的情况是根据购买的商品自动计算
            //InvoiceTax 发票税金

            await repository.InsertAsync(order);
            await CurrentUnitOfWork.SaveChangesAsync();//保存，以获得新的自增id
            return order;
        }
    }
}
