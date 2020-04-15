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
     * 原本考虑定义一个OrderBuilder对象单独负责订单的创建，其实是有必要的。但是目前的场景 先直接定义到订单领域服务中
     * 将来有需求时再重构，避免过度设计
     */

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
