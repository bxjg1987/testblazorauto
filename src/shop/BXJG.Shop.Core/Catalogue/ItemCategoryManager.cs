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
    public class ItemCategoryManager : GeneralTreeManager<ItemCategoryEntity>
    {
        public ItemCategoryManager(IRepository<ItemCategoryEntity, long> repository) : base(repository)
        {
        }
    }
}
