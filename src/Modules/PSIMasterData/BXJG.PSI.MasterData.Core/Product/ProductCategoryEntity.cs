using Abp.Domain.Entities;using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.PSI.MasterData.Product
{
    /// <summary>
    /// 产品分类实体
    /// </summary>
    public class ProductCategoryEntity : GeneralTreeEntity<ProductCategoryEntity>
    {
        /// <summary>
        /// 该分类下的产品列表
        /// </summary>
        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}