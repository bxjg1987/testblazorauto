using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /*
     * 有销售记录关联的通过外键约束保证无法删除
     * 可以用IsActive来禁用它
     */

    public class SkuEntity : Entity<long>//, IPassivable
    {
        //sku属性由abp动态属性提供

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
        /// <summary>
        /// 所属产品(spu)的Id
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 所属产品(spu)
        /// </summary>
        public virtual ProductEntity Product { get; set; }
        ///// <summary>
        ///// 是否启用
        ///// </summary>
        //public bool IsActive { get; set; }
    }
}
