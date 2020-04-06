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
    public class ItemAppService : ItemAppService<Tenant, User, Role, TenantManager, UserManager>
    {
        public ItemAppService(IRepository<ItemEntity, long> repository) : base(repository)
        {
        }
    }
}
