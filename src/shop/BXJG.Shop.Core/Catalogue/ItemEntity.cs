using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BXJG.Shop.Catalogue
{
    /*
     * 【【【以下注释记录思考过程，请从上往下看到底。】】】】
     * -----------------------------------------------------------------
     * 
     * 基本信息：
     * 标题、品牌、单位、规格、描述、图片、所属分类等 只跟商品相关的信息      
     * 这个概念是有必要的，将来可能对接上游的供应链 下游连接销售、上架。将来来可能统计某个商品的销售情况
     * 可以修改 因为只包含商品属性
     * 
     * 成本价格：
     * 商品成本+其它费用的成本合计     
     * 成本价格是可能会变动的，每次采购 每次销售的成本价都不同，所以成本价的变动需要有记录，且记录是不允许修改，因为可能被别的地方引用了
     * 目前只考虑销售(上架)，因此成本价只关联到上架信息
     * 
     * 销售价格：
     * 原价、现价（销售价）、折扣价......      
     * 也是需要记录的，不允许修改，与一次上架关联
     * 
     * 物流信息：
     * 默认快递公司、运费、等
     * 
     * 上架信息：
     * 是否显示在首页、是否是新品、是否是热卖...上架开始/结束时间等等，各种优惠   包含上面的 基本信息、成本价、销售价、物流等   
     * 每次上架都有记录，但允许修改
     * 可以考虑上架结束的 不允许修改。
     * 
     * 可以发现上面的除基本信息外，其它各项都会有记录，且都不允许修改（除了在销售期间的上架信息）
     * 但它们在业务中确实是不同的概念，因此分开是有好处的，因为将来某些功能可能只关心其中的某些概念
     * 所以将后续四个概念合并不太好
     * 
     * 最最最简单的情况 可以先不考虑成本和物流
     * 
     * 销售订单又是另外一个概念，它引用上面的不变的概念，比如销售价格信息等。也可能包含一些冗余字段，比如上架信息可能有自己的标题，而不是取商品的标题，那么最终订单
     * 也可能保存这个冗余字段
     * -----------------------------------------------------------------------
     * 简单起见 只考虑销售价和上架 且合并。再没有产生订单时允许修改，否则只能修改部分字段（比如是否热卖、开始/结束时间等）。【注意考虑并发问题】
     * -----------------------------------------------------------------------
     * 如果考虑上架信息包含足够多的商品冗余字段，那么商品信息中最终只剩一个 商品唯一Id，此时就等同于与上架信息合并了。只是上架信息包含一个商品Id 也许可以直接用sku
     * 将来与上游的采购（供应链）通过sku来对接
     * 将来统计也根据此商品唯一id来统计
     * 
     * 不要只用系统自增id，因为将来别的系统可能有自己的id，对接不方便。
     * 
     * 所以现在我们只有一个“上架信息”的概念，里面包含所有信息，部分信息允许随时修改，价格等关键信息一旦产生订单则不允许修改（可以提供复制功能创建新的上架信息）
     * -------------------------------------------------------------------------------------------------
     * 2020-4-7更新
     * 仔细考虑订单直接关联上架信息很麻烦，比如有个用户购买了商品，后台又修改了商品品牌，所以哪些可以修改 哪些不能修改很难定下来
     * 订单关联上架信息还有个问题，订单确定后不关心 是否热卖、是否首页显示这样的，但是因为外键关联了 也关联了
     * 另外上架信息中的产品信息 跟订单中的商品信息虽然很多字段重复，但这确实是两个不同的概念，关心的字段也不同
     * 总的来说 上架信息 跟 订单中的商品信息 分开是比较保险的方式
     * 
     * 另外需要注意上架信息的并发问题，一个多个后台操作人员修改同一个上架信息 最好做下并发处理。比如：经理将某个商品下架 另一个用户在修改商品价格，最终可能导致商品没有下架
     * 不过这个问题可以以后来处理
     * 
     * 为将来方便统计 商品上架信息 和 订单中的产品  都会引用一个id 这个作为产品id，注意此时 商品上架信息的id并非商品id
     */

    /// <summary>
    /// 上架信息实体类
    /// 里面可能包含部分业务逻辑方法
    /// 设计时考虑不提供继承的方式扩展，因为那样太复杂
    /// 你可以使用关联和事件的方式参与到这个模块中来
    /// </summary>
    public class ItemEntity : FullAuditedEntity<long>, IMustHaveTenant
    {
        #region 基本信息
        public const int TitleMaxLength = 100;
        public const int SkuMaxLength = 50;
        public const int DescriptionShortMaxLength = 10000;
        public const int ImagesMaxLength = 5000;

        public int TenantId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(TitleMaxLength)]
        public string Title { get; set; }
        /// <summary>
        /// sku
        /// </summary>
        [StringLength(SkuMaxLength)]
        public string Sku { get; set; }
        /// <summary>
        /// 简短描述
        /// </summary>
        [StringLength(DescriptionShortMaxLength)]
        public string DescriptionShort { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string DescriptionFull { get; set; }
        /// <summary>
        /// 轮播图片集合，多个用英文逗号,分割
        /// </summary>
        [StringLength(ImagesMaxLength)]
        public string Images { get; set; }
        /// <summary>
        /// 所属类别
        /// </summary>
        public virtual BXJGShopDictionaryEntity Category { get; set; }
        /// <summary>
        /// 所属类别id
        /// </summary>
        public long CategoryId { get; set; }
        #endregion

        #region 价格信息
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OldPrice { get; set; }
        /// <summary>
        /// 现价(销售价)
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        #endregion

        #region 上架信息
        /// <summary>
        /// 是否热卖
        /// </summary>
        public bool Hot { get; set; }
        /// <summary>
        /// 是否新品
        /// </summary>
        public bool New { get; set; }
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        public bool Home { get; set; }
        /// <summary>
        /// 是否显示在轮播图片中
        /// </summary>
        public bool Focus { get; set; }
        /// <summary>
        /// 是否已发布
        /// 已发布且当前时间处于上/下架范围内时才会显示在前端
        /// </summary>
        public bool Published { get; set; }
        /// <summary>
        /// 上架时间
        /// 已发布且当前时间处于上/下架范围内时才会显示在前端
        /// </summary>
        public DateTimeOffset? AvailableStart { get; set; }
        /// <summary>
        /// 下架时间
        /// 已发布且当前时间处于上/下架范围内时才会显示在前端
        /// </summary>
        public DateTimeOffset? AvailableEnd { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 发布此商品
        /// </summary>
        /// <param name="yxq">开始发布时间，默认当前时间</param>
        /// <param name="js">结束时间</param>
        public void Publish(DateTimeOffset yxq = default, DateTimeOffset? js = null)
        {
            Published = true;
            AvailableStart = yxq;
            AvailableEnd = js ?? yxq.AddYears(10);
        }
        /// <summary>
        /// 发布此商品
        /// </summary>
        /// <param name="yxq">开始发布时间，默认当前时间</param>
        /// <param name="js">有效期，单位秒</param>
        public void Publish(DateTimeOffset yxq = default, long js = 60 * 60 * 24 * 365 * 10)
        {
            Publish(yxq, yxq.AddSeconds(js));
        }
        #endregion
    }
}