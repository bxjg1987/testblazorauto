using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using BXJG.Shop.Catalogue;
using Abp.Domain.Repositories;
using BXJG.Shop.Common;

namespace ZLJ.Shop
{
    public class BXJGShopItemAppService : BXJGShopItemAppService<Tenant, User, Role, TenantManager, UserManager>
    {
        public BXJGShopItemAppService(IRepository<ItemEntity, long> repository, BXJGShopDictionaryManager dictionaryManager, IRepository<BXJGShopDictionaryEntity, long> respDic) : base(repository, dictionaryManager, respDic)
        {
        }
    }
}
