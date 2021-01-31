using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
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
using Abp.Dependency;
using BXJG.Common;
using ZLJ.BaseInfo.Administrative;
using System.Linq;

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
    /// 没提供的功能可能在订单实体上
    /// </summary>
    public class OrderManager : DomainServiceBase
    {
        protected readonly IRepository<OrderEntity, long> repository;
        protected readonly IRepository<CustomerEntity, long> customerRepository;
        //protected readonly CustomerManager customerManager;
        protected readonly ISettingManager settingManager;
        //领域层不应该访问session  protected readonly IAbpSession session;

        public OrderManager(
            IRepository<OrderEntity, long> repository,
            IRepository<CustomerEntity, long> customerRepository,
            ISettingManager settingManager
            /*,CustomerManager customerManager*/)
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
        /// <param name="customerRemark">顾客提交订单时填写的备注信息</param>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<OrderEntity> CreateAsync(
            CustomerEntity customer,
            AdministrativeEntity area,
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
            //customer.RequiredValidate(nameof(customer));//领域层不应访问session，所以不要在customer为空时自动获取当前登录用户所关联的顾客
            //items.RequiredValidate(nameof(items));
            //consignee.RequiredValidate(nameof(consignee));
            //consigneePhoneNumber.RequiredValidate(nameof(consigneePhoneNumber));
            //receivingAddress.RequiredValidate(nameof(receivingAddress));
            #endregion

            //业务判断...略...
            //用户是否在黑名单中
            //被购买的商品的状态是否正常
            //等等业务判断

            var order = new OrderEntity(customer, Guid.NewGuid().ToString("N"))
            {
                OrderTime = DateTimeOffset.Now,
                //Status = OrderStatus.Created,
                CustomerRemark = customerRemark,
                //暂时忽略开票
                //InvoiceType = invoiceType,
                //InvoiceTitle = invoiceTitle,
                //TaxId = taxId,
                //PaymentStatus = PaymentStatus.WaitingForPayment,
                Area = area,
                AreaId = area.Id,
                Consignee = consignee,
                ConsigneePhoneNumber = consigneePhoneNumber,
                ReceivingAddress = receivingAddress
            };
            // order.Init();
            #region 订单明细
            foreach (var item in items)
            {
                var product = new OrderItemEntity
                {
                    Order = order,

                    Product = item.Product,
                    ProductId = item.Product.Id,

                    Sku = item.Sku,
                    SkuId = item.Sku.Id,

                    Title = item.Product.Title,
                    Image = item.Product.GetImages()?.First().Key,

                    Integral = item.Sku != null ? item.Sku.Integral : item.Product.Integral,
                    Price = item.Sku != null ? item.Sku.Price : item.Product.Price,
                    Quantity = item.Quantity,
                    Amount = item.CalculationAmount(),// item.Product.Price * item.Quantity,
                    TotalIntegral = item.CalculationIntegral()// Convert.ToInt32(item.Product.Integral * item.Quantity),
                };
                order.Items.Add(product);
                //order.MerchandiseSubtotal += product.Amount;
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
            //await EventBus.TriggerAsync(new EntityCreatedEventData<OrderEntity>(order));//
            return order;
        }
        ///// <summary>
        ///// 立即购买或购物车生成订单时会调用此方法
        ///// 它只是生成订单，并未真正完成下单，这里生成订单返回给前端，由顾客继续补录订单信息后提交完成下单
        ///// </summary>
        ///// <param name="customer">顾客</param>
        ///// <param name="items">所购买的商品列表</param>
        ///// <returns></returns>
        //public virtual async Task<OrderEntity> BuildOrderAsync(CustomerEntity customer, params OrderItemInput[] items)
        //{
        //    //var addr = customer.GetDefaltShippingAddress();
        //    var order = new OrderEntity
        //    {
        //        //Customer = customer,
        //        //CustomerId = customer.Id,
        //        //OrderNo = Guid.NewGuid().ToString("N"),//将来再考虑用个专门的组件生产简单、不重复的订单号
        //        //OrderTime = DateTimeOffset.Now,
        //        //Status = OrderStatus.Created,
        //        //CustomerRemark = customerRemark,
        //        //暂时忽略开票
        //        //InvoiceType = invoiceType,
        //        //InvoiceTitle = invoiceTitle,
        //        //TaxId = taxId,
        //        //PaymentStatus = PaymentStatus.WaitingForPayment,
        //        //Area = addr.Area,
        //        //AreaId = addr.Id,
        //        //Consignee = consignee,
        //        //ConsigneePhoneNumber = consigneePhoneNumber,
        //        //ReceivingAddress = receivingAddress
        //    };
        //    // order.Init();
        //    #region 订单明细
        //    foreach (var item in items)
        //    {
        //        var product = new OrderItemEntity
        //        {
        //            Order = order,

        //            Product = item.Product,
        //            ProductId = item.Product.Id,

        //            Sku = item.Sku,
        //            SkuId = item.Sku.Id,

        //            Title = item.Product.Title,
        //            Image = item.Product.GetImages()?.First().Key,

        //            Integral = item.Sku != null ? item.Sku.Integral : item.Product.Integral,
        //            Price = item.Sku != null ? item.Sku.Price : item.Product.Price,
        //            Quantity = item.Quantity,
        //            Amount = item.CalculationAmount(),// item.Product.Price * item.Quantity,
        //            TotalIntegral = item.CalculationIntegral()// Convert.ToInt32(item.Product.Integral * item.Quantity),
        //        };
        //        order.Items.Add(product);
        //        order.MerchandiseSubtotal += product.Amount;
        //        order.Integral += product.TotalIntegral;
        //    }
        //    #endregion
        //    order.PaymentAmount = order.MerchandiseSubtotal;//+各种费用-各种优惠
        //    return order;
        //}
    }
}
