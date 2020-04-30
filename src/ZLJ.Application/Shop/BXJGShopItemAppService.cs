using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using BXJG.Shop.Catalogue;
using Abp.Domain.Repositories;

namespace ZLJ.Shop
{
    public class BXJGShopItemAppService : BXJGShopItemAppService<Tenant, User, Role, TenantManager, UserManager>
    {
        public BXJGShopItemAppService(IRepository<ItemEntity, long> repository) : base(repository)
        {
        }
    }
}
