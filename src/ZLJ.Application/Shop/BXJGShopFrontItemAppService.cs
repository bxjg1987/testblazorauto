using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Shop
{
    public class BXJGShopFrontItemAppService : BXJGShopFrontItemAppService<GeneralTreeEntity>
    {
        public BXJGShopFrontItemAppService(IRepository<ItemEntity<GeneralTreeEntity>, long> repository, ItemCategoryManager dictionaryManager) : base(repository, dictionaryManager)
        {
        }
    }
}
