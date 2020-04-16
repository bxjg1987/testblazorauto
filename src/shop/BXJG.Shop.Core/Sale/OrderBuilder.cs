using Abp.Authorization.Users;
using Abp.Dependency;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{
    /* 2020-4-16 变形精怪
     * 什么是业务正确的订单OrderEntity？
     *      一个订单必须指明是哪个客户购买的，必须包含买的哪些商品，订单价格必须根据业务规则计算得来（商品总额+额外费用-优惠等等）
     *      还有其它很多业务规则限制，最终才能成为一个业务正确的订单
     * 
     * 如何保证创建一个业务正常的订单OrderEntity？
     * 
     *      最简单的办法是为OrderEntity定义一个构造函数，然后要求调用方必须传入必要的参数，然后在构造函数内部根据各种业务规则来初始化这个订单，
     *      构造函数执行完将得到一个业务正常的订单
     *      这种方式有太多问题，各种业务规则会涉及到很多其它组件，按DDD的思路 实体类不应该包含那些组件，如果一个实体与其他实体关联则应该定义领域服务
     *      另外实体也作为EF的实体，有太多限制，比如你不能为了限制业务规则 将某些属性设置为私有的
     *      再者内部业务处理可能涉及到数据库访问、网络请求等，这些操作需要异步化，在构造函数中不方便处理
     *      可能还存在其它问题....
     *      
     *      另一种方式是在OrderManager领域服务中定义CreateAsync方法来创建订单，在方法的参数中定义订单需要的必要参数，用参数默认值的方式定义可选参数
     *      在方法内部做各种业务处理，最后创建出业务正常的订单
     *      这种方式虽然感觉是可行的，但是通常这个方法是 先创建订单，然后持久化，然后再做其它后续处理，相当于把创建订单的任务和其它任务定义在同一个方法中
     *      
     *      另一种方式是将订单实体的构造函数定义为internal，然后专门创建一个Builder对象来负责订单创建
     *      Builder对象提供各种方法、属性和其它成员对这个订单进行配置，最后调用Build方法生成一个订单，这个方法中做各种业务处理，Build时若发现未满足业务正常订单的要求
     *      时，抛出异常
     *      这种方式也不负责持久化，它只是在内存中创建一个业务正常订单。
     *      
     *      最后一种方式是在OrerManager领域服务中定义一个BuildOrderAsync的方法，通过必填和可选参数来约束调用方。内部根据业务规则最终创建一个业务正确的订单
     *      此方式是方式2、3的综合体。相对于方式3唯一步骤的是
     *      偶尔我们创建订单可能是分很多步骤创建的，而BuildOrderAsync方式要求在调用此方法之前必须一次性准备好所有的参数
     *      比较而言OrderBuilder方式更灵活，在BuildAsync方法执行前，随时可以继续配置这张订单
     * 
     * 是否需要接口IOrderBuilder？
     *      也许需要，也许不需要，在不明确的前提下 不好定义这个接口，不知道需要哪些成员
     *      所以先不用，将来需要了再重构
     *      如果将来考虑使用接口，那多半还需要为每一个IOrderBuilder接口配备一个OrderBuilder的工厂类
     *      其实要实现扩展也不一定用接口才是首选，可以在Builder内部注入各种组件来回调，实现扩展
     *      
     * 为Builder提供默认构造函数是有必要的
     *      如果OrderBuilder只保留默认无参构造函数，那么调用方var orderBuider = new OrderBuilder();之后，都不知道该干什么，不做任何配置的情况下
     *      直接orderBuilder.Build();又会报错。
     *      所以如果我们为OrderBuilder定义带参数的构造函数，这些参数是创建订单必填参数，那么调用方最简单的创建业务正确的订单代码会非常简洁
     *      new OrderBuilder(p1,p2,p3).BuildAsync();
     *      ---------------------------------------------------------
     *      注意：构造函数中不能有异步代码
     *      ---------------------------------------------------------
     *      所以目前考虑：构造函数中的必填参数 同步业务逻辑在构造函数中处理，需要异步处理的逻辑放BuildAsync方法中处理
     *      
     * 如果调用方依然new OrderEntity()咋整？
     *      即使我们提供了OrderBuilder对象，调用方依然可能不使用它，而是直接自己去new一个订单出来，此时订单可能还是一个不满足业务规则的订单
     *      按编程的角度我们是可以做到强约束，让调用方无法直接new订单，比如为订单实体定义private的构造函数，OrderBuilder定义为OrderEntity的内部类
     *      OrderEntity的只读属性也可以直接约束。
     *      但是因为我们使用的EF，所以无法做到这些业务约束。所以只能口头约束调用方不要直接new OrderEntity
     * 
     * 
     * 与ABP的耦合
     *      商城系统本身是独立的业务，没必要与ABP耦合，比如OrderEntity没必要继承ABP的FullAuditedEntity，而是根据需要自己定义ABP提供的属性
     *      OrderBuilder也没必要实现ITransientDependency来实现自动的IOC注册，而是使用asp.net core的默认方式提供扩展方法来注册到IOC
     *      但是如果真要做这样的分离工作量巨大，所以这里就不考虑分开设计
     * 
     * ----------------------------------------------------------------------------------
     * 简单起见先在OrderManager.CreateAsync来创建订单，后续再考虑设计OrderBuilder
     * 
     */

    /// <summary>
    /// 负责在内存中创建一个满足业务规则的订单实体
    /// </summary>
    public class OrderBuilder<TUser> : BXJGShopDomainServiceBase
        where TUser : AbpUserBase
    {
        /// <summary>
        /// 订单关联的顾客
        /// </summary>
        public CustomerEntity<TUser> Customer { get; set; }
        /// <summary>
        /// 顾客下单时写的备注
        /// </summary>
        public string CustomerRemark { get; set; }
        /// <summary>
        /// 是否开具发票
        /// </summary>
        public bool InvoiceRequired { get; set; }
        /// <summary>
        /// 收货人
        /// 不一定就是下单人
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ConsigneePhoneNumber { get; set; }
        
        //省市区县 暂时忽略，后期补充

        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceivingAddress { get; set; }

        public OrderBuilder(CustomerEntity<TUser> customer)
        {
            this.Customer = customer;
        }

        //显然这里行不通，必须为OrderBuilder<TUser>定义一个工厂类来实现这种需求
        //public OrderBuilder(long customerId)
        //{

        //}

        protected virtual Task ValiateCustomerAsync()
        {
            return Task.CompletedTask;
        }

        public async Task<OrderEntity<TUser>> BuildAsync()
        {
            //构造函数中的参数 需要同步验证的则在构造函数内部处理，需要异步处理的放这里

            await ValiateCustomerAsync();

            var order = new OrderEntity<TUser>
            {
                Customer = this.Customer,
                CustomerId = this.Customer.Id,
                OrderNo = Guid.NewGuid().ToString("N"),  //这里将来需要用一个单独的组件来生成容易记忆又不重复的订单号，暂时用guid实现用能先
                Status = OrderStatus.Created,
                CustomerRemark = this.CustomerRemark,
                InvoiceRequired= this.InvoiceRequired,
                PaymentStatus= PaymentStatus.WaitingForPayment
            };
            //MerchandiseSubtotal 小计
            //DistributionFee 配送费
            return order;
        }
    }
}
