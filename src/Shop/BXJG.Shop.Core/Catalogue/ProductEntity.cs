using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 产品实体(spu)
    /// </summary>
    public class ProductEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant
    {
        #region 基础信息
        /// <summary>
        /// 所属租户id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 简短描述
        /// </summary>
        public string DescriptionShort { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string DescriptionFull { get; set; }

        /* 
         * 将来考虑设计一个图片模块
         * 文件模块（文件的上传下载、与实体的关联） -> 图片模块在文件模块基础上增加缩略图等功能
         * 
         * 每个sku也应该有自己的图片列表，暂未实现
         */
        /// <summary>
        /// 轮播图片集合，多个用英文逗号,分割
        /// </summary>
        public string Images { get; set; }
        //使用方法而不是属性，让调用方明确知道此调用会经过计算而不是直接获取变量的值
        /// <summary>
        /// 获取图片集合
        /// </summary>
        /// <returns></returns>
        public string[] GetImages()
        {
            return Images.Split(',');
        }
        /// <summary>
        /// 所属类别
        /// </summary>
        public virtual ProductCategoryEntity Category { get; set; }
        /// <summary>
        /// 所属类别id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public virtual GeneralTreeEntity Brand { get; set; }
        /// <summary>
        /// 品牌id
        /// </summary>
        public long? BrandId { get; set; }

        //目前来说单位可以直接存具体的值，所以可以使用string类型
        //但是将来单位可能拆包 组包，为了将来考虑 这里就用数据字典吧
        /// <summary>
        /// 单位外键实体
        /// </summary>
        public virtual GeneralTreeEntity Unit { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public long? UnitId { get; set; }
        /// <summary>
        /// sku列表
        /// </summary>
        public virtual List<SkuEntity> Skus { get; set; }

        /* 
         * 可以但不建议在这里定义可选的动态属性DynamicProperty，因为动态属性属于通用功能
         * 可选的动态属性等待abp5.14来实现，参考 https://github.com/aspnetboilerplate/aspnetboilerplate/issues/5813
         * 在管理sku时可显示的动态属性由这里决定
         * 
         * 此限定目前考虑是放在产品实体而不是产品分类实体上
         */
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
        public bool Published { get; protected set; }
        /// <summary>
        /// 上架时间
        /// 已发布且当前时间处于上/下架范围内时才会显示在前端
        /// 若不设置，则不限制上架开始时间
        /// </summary>
        public DateTimeOffset? AvailableStart { get; protected set; }
        /// <summary>
        /// 下架时间
        /// 已发布且当前时间处于上/下架范围内时才会显示在前端
        /// 若不设置则不限制上架结束时间
        /// </summary>
        public DateTimeOffset? AvailableEnd { get; protected set; }
        #endregion

        #region 方法
        /// <summary>
        /// 发布此商品
        /// </summary>
        /// <param name="yxq">开始发布时间，默认当前时间</param>
        /// <param name="js">结束时间</param>
        public void Publish(DateTimeOffset? yxq = default, DateTimeOffset? js = default)
        {
            Published = true;
            AvailableStart = yxq ?? DateTimeOffset.Now;
            AvailableEnd = js ?? AvailableStart.Value.AddYears(10);
            DomainEvents.Add(new ProductPublishEventData(this));
        }
        /// <summary>
        /// 发布此商品
        /// </summary>
        /// <param name="yxq">开始发布时间，默认当前时间</param>
        /// <param name="js">有效期，单位秒</param>
        public void PublishDuration(DateTimeOffset? yxq = default, long js = 60 * 60 * 24 * 365 * 10)
        {
            yxq = yxq ?? DateTimeOffset.Now;
            Publish(yxq, yxq.Value.AddSeconds(js));
        }
        /// <summary>
        /// 取消发布指定商品
        /// </summary>
        /// <returns></returns>
        public void UnPublish()
        {
            Published = false;
            DomainEvents.Add(new ProductPublishEventData(this));
        }
        #endregion
    }
}
