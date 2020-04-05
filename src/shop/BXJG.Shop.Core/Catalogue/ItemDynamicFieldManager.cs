using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
   public  class ItemDynamicFieldManager : GeneralTreeManager<ItemDynamicFieldEntity>
    {
        public ItemDynamicFieldManager(IRepository<ItemDynamicFieldEntity, long> repository) : base(repository)
        {
        }
    }
}
