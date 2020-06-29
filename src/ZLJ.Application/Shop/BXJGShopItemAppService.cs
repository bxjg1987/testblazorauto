using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using BXJG.Shop.Catalogue;
using Abp.Domain.Repositories;
using BXJG.Shop.Common;
using BXJG.GeneralTree;

namespace ZLJ.Shop
{
    /// <summary>
    /// 后台管理对商品的接口
    /// </summary>
    public class BXJGShopItemAppService : BXJGShopItemAppService<Tenant, User, Role, TenantManager, UserManager, GeneralTreeEntity>
    {
        public BXJGShopItemAppService(IRepository<ItemEntity<GeneralTreeEntity>, long> repository, 
                                      ItemCategoryManager dictionaryManager, 
                                      IRepository<BXJGShopDictionaryEntity, long> respDic, 
                                      ItemManager<GeneralTreeEntity> itemManager) : base(repository, dictionaryManager, respDic, itemManager)
        {
        }
    }
}
