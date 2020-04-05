using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Common
{
    public class BXJGShopDictionaryManager : GeneralTreeManager<BXJGShopDictionaryEntity>
    {
        public BXJGShopDictionaryManager(IRepository<BXJGShopDictionaryEntity, long> repository) : base(repository)
        {
        }
    }
}
