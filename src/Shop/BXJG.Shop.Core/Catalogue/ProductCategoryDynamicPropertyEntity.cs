using Abp.Domain.Entities;
using Abp.DynamicEntityProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 允许当前商品分类下的商品 有哪些sku的动态属性，如：手机类别下允许有 入网类型、版本(4g*64g)、颜色等属性
    /// 这样做虽然是最直观的方式，但是不够通用，当有别的地方有类似商品类别限制下属商品的动态属性时，都要实现一遍，
    /// 所以目前决定使用abp自带的DynamicEntityPropertity改造实现，就是将类型字段存储为类型+商品分类id的方式
    /// </summary>
    public class ProductCategoryDynamicPropertyEntity : Entity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        /// <summary>
        /// 商品类别Id
        /// </summary>
        public long ProductCategoryId { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        public virtual ProductCategoryEntity ProductCategory { get; set; }
        /// <summary>
        /// 关联的abp动态属性
        /// </summary>
        public int DynamicPropertyId { get; set; }
        public virtual DynamicProperty DynamicProperty { get; set; }
    }
}
