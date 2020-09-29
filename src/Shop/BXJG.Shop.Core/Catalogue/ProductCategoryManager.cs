using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 商品分类领域服务
    /// </summary>
    public class ProductCategoryManager : GeneralTreeManager<ProductCategoryEntity>
    {
        public ProductCategoryManager(IRepository<ProductCategoryEntity, long> repository) : base(repository)
        {
        }
    }
}
