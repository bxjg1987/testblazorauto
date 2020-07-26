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
using BXJG.Utils.File;

namespace ZLJ.Shop
{
    /// <summary>
    /// 后台管理对商品的接口
    /// </summary>
    public class BXJGShopItemAppService : BXJGShopItemAppService<Tenant, User, Role, TenantManager, UserManager>
    {
        public BXJGShopItemAppService(IRepository<ItemEntity, long> repository, 
                                      ItemCategoryManager dictionaryManager, 
                                      IRepository<BXJGShopDictionaryEntity, long> respDic, 
                                      ItemManager itemManager,
                                      TempFileManager tempFileManager) : base(repository, dictionaryManager, respDic, itemManager, tempFileManager)
        {
        }
    }
}
