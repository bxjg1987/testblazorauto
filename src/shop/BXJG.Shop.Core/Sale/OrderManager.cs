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
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using Abp.Dependency;
using BXJG.Common;

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
     */

    /// <summary>
    /// 订单管理领域逻辑
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea">送货地址区域类型 参考实体类的泛型说明</typeparam>
    public class OrderManager<TUser, TArea> : BXJGShopDomainServiceBase//, ITransientDependency
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
    {
        protected readonly IRepository<OrderEntity<TUser, TArea>, long> repository;
        protected readonly IRepository<CustomerEntity<TUser,TArea>, long> customerRepository;
        //protected readonly CustomerManager<TUser,TArea> customerManager;
        protected readonly ISettingManager settingManager;
        //领域层不应该访问session  protected readonly IAbpSession session;

        public OrderManager(
            IRepository<OrderEntity<TUser, TArea>, long> repository,
            IRepository<CustomerEntity<TUser,TArea>, long> customerRepository,
            ISettingManager settingManager
            /*,CustomerManager<TUser,TArea> customerManager*/)
        {
            this.settingManager = settingManager;
            this.repository = repository;
            this.customerRepository = customerRepository;
            //this.customerManager = customerManager;
        }
        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="customer">顾客</param>
        /// <param name="area">收货人所属地区</param>
        /// <param name="consignee">收货人</param>
        /// <param name="consigneePhoneNumber">收货人电话</param>
        /// <param name="receivingAddress">收货地址</param>
        /// <param name="customerRemark"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<OrderEntity<TUser, TArea>> CreateAsync(
            CustomerEntity<TUser,TArea> customer,
            TArea area,
            string consignee,
            string consigneePhoneNumber,
            string receivingAddress,
            string customerRemark = null,
            params OrderItemInput[] items)
        {
            #region 各参数基本验证
            //领域服务的方法有必要验证入参来保证业务正确性，它不能祈求调用方或底层的数据库一定会做完整性约束。
            //但是，通常有前端验证、Action模型绑定验证、Application验证、数据库约束等验证。
            //在应用层或UI的Action可能会调用多个api时传入相同参数，那么此时参数验证一次就够了
            //一旦采用多层次都验证，那么整个项目会有大量这种情况存在，会导致性能下降，高并发时可能更严重些。
            //还是做正确的事吧，领域层本就是独立的层，保证业务数据的约束也算是业务逻辑的一部分
            //将来使用充血模型后 很多验证可能移植到实体类上
            customer.RequiredValidate(nameof(customer));//领域层不应访问session，所以不要在customer为空时自动获取当前登录用户所关联的顾客
            items.RequiredValidate(nameof(items));
            consignee.RequiredValidate(nameof(consignee));
            consigneePhoneNumber.RequiredValidate(nameof(consigneePhoneNumber));
            receivingAddress.RequiredValidate(nameof(receivingAddress));
            #endregion

            //所有业务判断都成功时才创建订单对象
            var order = new OrderEntity<TUser, TArea>
            {
                Customer = customer,
                CustomerId = customer.Id,
                OrderNo = Guid.NewGuid().ToString("N"),//将来再考虑用个专门的组件生产简单、不重复的订单号
                OrderTime = DateTimeOffset.Now,
                Status = OrderStatus.Created,
                CustomerRemark = customerRemark,
                //暂时忽略开票
                //InvoiceType = invoiceType,
                //InvoiceTitle = invoiceTitle,
                //TaxId = taxId,
                PaymentStatus = PaymentStatus.WaitingForPayment,
                Area = area,
                Consignee = consignee,
                ConsigneePhoneNumber = consigneePhoneNumber,
                ReceivingAddress = receivingAddress,
            };

            #region 订单明细
            foreach (var item in items)
            {
                var product = new OrderItemEntity<TUser, TArea>
                {
                    Amount = item.Item.Price * item.Quantity,
                    Image = item.Item.GetImages()[0],
                    Integral = item.Item.Integral,
                    Item = item.Item,
                    ItemId = item.Item.Id,
                    Order = order,
                    Price = item.Item.Price,
                    Quantity = item.Quantity,
                    Title = item.Item.Title,
                    TotalIntegral = Convert.ToInt32(item.Item.Integral * item.Quantity),
                };
                order.Items.Add(product);
                order.MerchandiseSubtotal += product.Amount;
                order.Integral += product.TotalIntegral;
            }
            #endregion
            order.PaymentAmount = order.MerchandiseSubtotal;//+各种费用-各种优惠
            //插入之前触发一个事件，允许事件处理程序修改或阻止订单创建
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
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="payMethod"></param>
        /// <returns></returns>
        public async Task<OrderEntity<TUser, TArea>> PayAsync(OrderEntity<TUser, TArea> entity, long payMethod)
        {
            entity.Status = OrderStatus.Processing;
            entity.PaymentStatus = PaymentStatus.Paid;
            entity.PaymentMethodId = payMethod;
            entity.LogisticsStatus = LogisticsStatus.WaitShip;
            await EventBus.TriggerAsync(new OrderPaidEventData<TUser, TArea>(entity));
            return entity;
        }
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="shipmentMethod"></param>
        /// <returns></returns>
        public async Task<OrderEntity<TUser, TArea>> ShipmentAsync(OrderEntity<TUser, TArea> entity, BXJGShopDictionaryEntity shipmentMethod)
        {
            entity.DistributionMethod = shipmentMethod;
            entity.LogisticsStatus = LogisticsStatus.Shipped;
            await EventBus.TriggerAsync(new OrderShipedEventData<TUser, TArea>(entity));
            return entity;
        }
        /// <summary>
        /// 签收
        /// </summary>
        /// <param name="entity">订单</param>
        /// <returns></returns>
        public async Task<OrderEntity<TUser, TArea>> SignAsync(OrderEntity<TUser, TArea> entity)
        {
            entity.LogisticsStatus = LogisticsStatus.Signed;
            await EventBus.TriggerAsync(new OrderSignedEventData<TUser, TArea>(entity));
            return entity;
        }
    }
}
