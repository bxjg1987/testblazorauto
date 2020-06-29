using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Events.Bus;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace BXJG.Shop.Sale
{
    /*
     * 商品上架信息ItemEntity中的信息可能发生变化，比如销售价，OrderItem会在顾客购买时记录下来购买时的售价 类似的字段有很多
     * 总额 总积分等 本可以查询时计算，但为将来查询更方便直接在保存信息时计算
     * 由于涉及到钱，最后做下并发控制，虽然订单明细不属于聚合根，订单才是聚合根，所以并发控制应该放订单上
     * 但是将来可能不遵循ddd，而直接操作订单明细，因此在订单明细上也加个并发控制字段
     * 原本应该只给关键字段，如数量、金额之类的的字段加并发控制，但是比较麻烦，现在就直接使用rowVersion来做吧
     * 
     * 订单中的商品 数量 价格信息一旦购买后就说明成交了，不允许任意调整
     * 非要调整 应该定义单独的方法，调整需要有操作日志之类的，且这种日志必须使用数据库日志，与业务记录保持在同一个事务
     */

    /// <summary>
    /// 订单中的产品和数量信息，将来可能包含更多信息
    /// 目前所有属性私有化，将来根据业务需要 提供相应的业务方法
    /// </summary>
    public class OrderItemEntity<TUser,TArea, TDataDictionary> : Entity
        //where TUser : AbpUserBase
        //where TArea : GeneralTreeEntity<TArea>, IAdministrative
    {
        /// <summary>
        /// 关联的订单Id
        /// </summary>
        public long OrderId { get;  set; }
        /// <summary>
        /// 关联的订单实体
        /// </summary>
        public virtual OrderEntity<TUser, TArea, TDataDictionary> Order { get;  set; }
        /// <summary>
        /// 关联的商品上架信息Id
        /// </summary>
        public long ItemId { get;  set; }
        /// <summary>
        /// 关联的商品上架信息
        /// </summary>
        public virtual ItemEntity<TDataDictionary> Item { get;  set; }
        /// <summary>
        /// 产品标题
        /// </summary>
        public string Title { get;  set; }
        /// <summary>
        /// 产品图片
        /// 与商品上架信息不同，这里只需要单张图片
        /// </summary>
        public string Image { get;  set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get;  set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get;  set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get;  set; }

        //以下冗余字段 而非每次计算，方便统计

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get;  set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public int TotalIntegral { get;  set; }
        /// <summary>
        /// 乐观并发控制标记
        /// https://docs.microsoft.com/zh-cn/ef/core/modeling/concurrency?tabs=data-annotations
        /// </summary>
        public byte[] RowVersion { get; private set; }
    }
}
