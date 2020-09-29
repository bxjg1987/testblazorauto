using Abp.Events.Bus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 产品发布状态改变时触发
    /// </summary>
    public class ProductPublishEventData : EntityEventData<ProductEntity>
    {
        public ProductPublishEventData(ProductEntity entity) : base(entity)
        {
        }
    }
}
