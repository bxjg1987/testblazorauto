using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    public class ItemCategoryManager : GeneralTreeManager<ItemCategoryEntity>
    {
        public ItemCategoryManager(IRepository<ItemCategoryEntity, long> repository) : base(repository)
        {
        }
    }
}
