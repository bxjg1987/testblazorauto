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
using System.Threading;
using Abp.Threading;
using Abp.Configuration;
using BXJG.Shop.Localization;
using Abp.Events.Bus.Entities;

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
        protected readonly ISettingManager settingManager;
        //领域层不应该访问session  protected readonly IAbpSession session;

        public OrderManager(
            IRepository<OrderEntity<TUser>, long> repository,
            IRepository<CustomerEntity<TUser>, long> customerRepository,
            ISettingManager settingManager,
            CustomerManager<TUser> customerManager)
        {
            this.settingManager = settingManager;
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
            //InvoiceType invoiceType = default,
            //string invoiceTitle = "",
            //string taxId = "",
            params OrderItemInput[] items)
        {
            #region 各参数基本验证
            //领域服务的方法有必要验证入参来保证业务正确性，它不能祈求调用方或底层的数据库一定会做完整性约束。
            //但是，通常有前端验证、Action模型绑定验证、Application验证、数据库约束等验证。
            //在应用层或UI的Action可能会调用多个api时传入相同参数，那么此时参数验证一次就够了
            //一旦采用多层次都验证，那么整个项目会有大量这种情况存在，会导致性能下降，高并发时可能更严重些。
            //还是做正确的事吧，领域层本就是独立的层，保证业务数据的约束也算是业务逻辑的一部分
            customer.RequiredValidate(nameof(customer));//领域层不应访问session，所以不要在customer为空时自动获取当前登录用户所关联的顾客
            items.RequiredValidate(nameof(items));
            consignee.RequiredValidate(nameof(consignee));
            consigneePhoneNumber.RequiredValidate(nameof(consigneePhoneNumber));
            receivingAddress.RequiredValidate(nameof(receivingAddress));
            #endregion

            #region 针对顾客的各种业务判断和处理
            //比如判断是否是黑名单客户
            #endregion

            #region 开票业务
            //暂时忽略开票
            //if (invoiceType != InvoiceType.None && invoiceTitle.IsNullOrWhiteSpace())
            //    throw new UserFriendlyException(L("需要开票时，请提供发票抬头！"));
            //if (invoiceType == InvoiceType.Business && taxId.IsNullOrWhiteSpace())
            //    throw new UserFriendlyException(L("需要开企业发票时，请提供税号！"));
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
                //暂时忽略开票
                //InvoiceType = invoiceType,
                //InvoiceTitle = invoiceTitle,
                //TaxId = taxId,

                PaymentStatus = PaymentStatus.WaitingForPayment,

                Consignee = consignee,
                ConsigneePhoneNumber = consigneePhoneNumber,
                ReceivingAddress = receivingAddress,
            };

            #region 订单明细
            foreach (var item in items)
            {
                var product = new OrderItemEntity<TUser>
                {
                    Amount = item.CalculationAmount(),
                    Image = item.Item.GetImages()[0],
                    Integral = item.Item.Integral,
                    Item = item.Item,
                    ItemId = item.Item.Id,
                    Order = order,
                    Price = item.Item.Price,
                    Quantity = item.Quantity,
                    Title = item.Item.Title,
                    TotalIntegral = Convert.ToInt32(item.Item.Integral * item.Quantity)
                };
                order.Items.Add(product);
            }
            #endregion
            order.CalculationMerchandiseSubtotal();//计算并设置商品小计

            //暂时忽略配送费
            //DistributionFee 配送费 简单的情况 可以让 后台管理员确认订单时录入配送费；合理的情况是根据购买的商品自动计算

            //InvoiceTax 发票税金 https://gitee.com/bxjg1987/abp/wikis/%E9%85%8D%E7%BD%AE%E5%8A%9F%E8%83%BD?sort_id=2127088

            await repository.InsertAsync(order);
            //即使调用了，后续事件处理异常了一样会回滚，参考 https://aspnetboilerplate.com/Pages/Documents/Unit-Of-Work#savechanges
            //文档是这么说的，没有试验过
            //但是这里为了获得新的id，调用保存下
            await CurrentUnitOfWork.SaveChangesAsync();
            //使用事件总线 减小业务之间的相互依赖 参考CustomerManager中对订单创建事件的处理
            //好像这个abp自动触发了 https://aspnetboilerplate.com/Pages/Documents/EventBus-Domain-Events#entity-changes
            //await EventBus.TriggerAsync(new EntityCreatedEventData<OrderEntity<TUser>>(order));//
            return order;
        }
    }
}
