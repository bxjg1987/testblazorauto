using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Events.Bus;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Common;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{
    /*
     * 同商品上架信息一样，这里会详细记录订单的设计思考过程
     * --------------------------------------------------------------------
     * 
     * >>>>>>>>>>>>>>>统一说明：凡涉及到需要记录的，都放到订单跟踪里（下面有说明）比如各种状态的变化
     * 
     * 订单基本信息
     *      下单用户：也就是哪个人下的单
     *      下单时间：系统当前时间，abp里好像有个Clock类，因为服务器时间未必准确 不过简单起见就先用服务器时间吧
     *      订单号：简单的话就guid，nopcommerce就是用的guid，未来可能考虑使用订单生产规则
     *      状态：参考dtcms和nopcommonerce     
     *              已生成：用户购买了商品时创建的订单 对应nopcommerce中dpending 待定的意思，用户可能随时放弃这个单子
     *              进行中：已付款、退款申请中、各种处理中的状态...
     *              已取消：无论因为什么原因导致交易未完成都使用此状态。某些系统会细分 未付款之前取消叫取消。付款后申请退款 退货 使用关闭
     *              已完成：正常交易完成
     *      注意 还有物流状态、付款状态，下面会说。有一种思路是用数字表示所有的状态，比如5表示 订单开始 已付款 但是在申请退款 且货品已被拦截 
     *      等多个状态组合成一个订单状态，这太复杂了，还是分开的好
     *      
     *      订单备注：后台管理人员为订单设置的备注
     *      用户备注：用户下单是写的备注
     *      
     * 支付
     *      费用计算的主要思路是 
     *          各种费用相加 = 订单金额  
     *          订单金额减去各种优惠 = 付款金额
     *          
     *      商品小计
     *      运费
     *      是否开具发票
     *      发票税金
     *      订单金额
     *      积分
     *      支付方式：微信 支付宝 现金 网银
     *      付款金额
     *      支付状态：
     *          参考nopcommerce https://admin-demo.nopcommerce.com/Admin/Order/Edit/4
     *          Authorized状态对应：用户申请退款
     *          Void我方拒绝退款
     *         
     *          待支付：用户刚下的单，未付款，此时后台
     *          已支付：
     *          申请退款：用户申请退款
     *          已退款：
     *          部分退款：
     *              部分退款的金额记录放在订单跟踪里，因为可能多次部分退款(nopm的方式)，部分退款时也可以再次变为已付款，多次部分退款总额不得操作订单总额
     *      
     *          
     * 配送
     *      收货人
     *      电话
     *      省市区县
     *      详细地址
     *      邮编
     *      配送方式
     *      物流单号:
     *      物流状态：
     *          无需运送
     *          未发货
     *          已发货
     *          已拒收
     *          已签收
     *      
     *      将来可能做分多个物流单发送
     *      
     *      
     * 产品信息
     *      产品图片
     *      标题title
     *      销售价
     *      优惠价
     *      数量
     *      金额
     *      积分
     * 
     * 如只用CustomerId关联顾客，则可以省去TUser这个泛型参数，外键配置可以在Shop.EFCoe中通过API方式配置映射， 将来查询时使用join
     * 因为商城系统中的订单是一个非常核心的概念，一旦引入泛型 则与此关联的很多概念都必须也是泛型，所以会很麻烦
     * 而订单中关联的顾客又特别重要，所以使用泛型的方式确实会提供很多便利
     * 综合来考虑 使用泛型虽然会让设计变得麻烦，但是大量的业务逻辑处理中会变得简单
     * 
     * 从订单的设计开始，后续所以模型的ef映射都移动到BXJG.EFCore中
     * 
     * ---------------------------------------------------------------------------------------
     * 2020-4-16 变形精怪
     * 不要设置默认值，因为从数据库查询时会自动赋值，所以默认值因为会造成多一次赋值
     * 谨慎属性赋值时做更多处理，比如添加明细 自动更新 金额。因为从数据库查询时 可能并不加载明细，而金额是直接从数据库取值
     */
    /// <summary>
    /// 订单实体类
    /// </summary>
    /// <typeparam name="TUser">系统用户类型</typeparam>
    /// <typeparam name="TArea">送货地址区域类型</typeparam>
    public class OrderEntity<TUser, TArea> : FullAuditedEntity<long>, IMustHaveTenant
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IShopAdministrative
    {
        public const int OrderNoMaxLength = 36;//guid长度 32+4个分隔符，将来可能使用其它格式的订单号
        public const int CustomerRemarkMaxLength = 500;
        public const int ConsigneeMaxLength = 20;
        public const int ConsigneePhoneNumberMaxLength = 50;
        public const int ReceivingAddressMaxLength = 200;
        public const int ZipCodeMaxLength = 50;
        public const int LogisticsNumberMaxLength = 50;

        public int TenantId { get; set; }//应该私有化，但受IMustHaveTenant限制，只能public

        #region 订单信息
        /// <summary>
        /// 关联的顾客的Id
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 关联的顾客的实体
        /// 注意顾客与User是一对一关联的
        /// </summary>
        public virtual CustomerEntity<TUser> Customer { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 下单时间
        /// 虽然父类已经有了CreateDate，但是类型为DateTime。况且CreateDate是表示这条信息的创建时间，OrderTime是下单业务发生的时间，这是两个不一样的概念
        /// </summary>
        public DateTimeOffset OrderTime { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }
        /// <summary>
        /// 顾客下单时填写的备注
        /// </summary>
        public string CustomerRemark { get; set; }
        #endregion

        #region 支付信息
        /// <summary>
        /// 商品小计
        /// 一个订单的中的多个商品价格相加的价格，但是商品列表可能随时在变动，所以这个属性只代表数据库中的商品小计字段的值
        /// 可以通过对应的方法来根据商品列表计算得到商品小计
        /// </summary>
        public decimal MerchandiseSubtotal { get; set; }
        ///// <summary>
        ///// 配送费
        ///// </summary>
        //public decimal DistributionFee { get; set; }

        ///// <summary>
        ///// 是否需要开票
        ///// </summary>
        //public InvoiceType InvoiceType { get; set; }
        ///// <summary>
        ///// 发票抬头
        ///// </summary>
        //public string InvoiceTitle { get; set; }
        ///// <summary>
        ///// 税号
        ///// </summary>
        //public string TaxId { get; set; }
        ///// <summary>
        ///// 发票税金
        ///// </summary>
        //public decimal InvoiceTax { get; set; }

        /// <summary>
        /// 可得积分
        /// </summary>
        public long Integral { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public BXJGShopDictionaryEntity PaymentMethod { get; set; }
        /// <summary>
        /// 支付方式Id
        /// 未支付时 就不存在支付方式，因此可空
        /// </summary>
        public long? PaymentMethodId { get; set; }
        /// <summary>
        /// 付款金额
        /// 顾客最终支付金额
        /// </summary>
        public decimal PaymentAmount { get; set; }
        /// <summary>
        /// 支付状态
        /// 某些场景下，并不是顾客下单就可以付款，而是需要后台审核后才能付款
        /// 因此使用? 表示此时订单处于不可付款状态，也就是没有付款状态
        /// </summary>
        public PaymentStatus? PaymentStatus { get; set; }
        #endregion

        #region 物流配送
        /// <summary>
        /// 送货地址所属区域
        /// </summary>
        public virtual TArea Area { get; set; }
        /// <summary>
        /// 送货地址所属区域Id
        /// </summary>
        public long AreaId { get; set; }
        /// <summary>
        /// 收货人
        /// 不一定就是下单人
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ConsigneePhoneNumber { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceivingAddress { get; set; }
        ///// <summary>
        ///// 已弃用，京东没有这个字段
        ///// </summary>
        //public string ZipCode { get; set; }
        /// <summary>
        /// 配送方式
        /// </summary>
        public BXJGShopDictionaryEntity DistributionMethod { get; set; }
        /// <summary>
        /// 配送方式
        /// 刚创建订单时配送方式尚未确定
        /// </summary>
        public long? DistributionMethodId { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNumber { get; set; }
        /// <summary>
        /// 物流状态
        /// 刚创建订单时没有物流状态，因此加个?
        /// </summary>
        public LogisticsStatus? LogisticsStatus { get; set; }
        #endregion

        #region 商品列表
        /// <summary>
        /// 订单商品明细
        /// </summary>
        public virtual IList<OrderItemEntity<TUser, TArea>> Items { get; set; }
        #endregion

        //订单跟踪

        /// <summary>
        /// 乐观并发
        /// </summary>
        public byte[] RowVersion { get; private set; }
    }
}
