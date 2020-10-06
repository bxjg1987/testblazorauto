using Abp.Events.Bus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 商品发布成功或失败时触发的事件
    /// 事件处理程序中可以通过商品的 Published属性来判断是发布还是取消发布
    /// </summary>
    public class ProductPublishEventData : EntityEventData<ProductEntity>
    {
        public ProductPublishEventData(ProductEntity entity) : base(entity)
        {
        }
    }
}
