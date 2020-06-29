using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZLJ.Shop
{
    class BXJGShopFrontItemAppService : BXJG.Shop.Catalogue.BXJGShopFrontItemAppService<GeneralTreeEntity>
    {
        public BXJGShopFrontItemAppService(IRepository<ItemEntity<GeneralTreeEntity>, long> repository, ItemCategoryManager dictionaryManager) : base(repository, dictionaryManager)
        {
        }
    }
}
