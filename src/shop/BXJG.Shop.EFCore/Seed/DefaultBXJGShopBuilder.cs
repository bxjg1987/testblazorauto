using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Seed
{
    public class DefaultBXJGShopBuilder< TTenant, TRole, TUser, TSelf, TArea>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
        where TArea : GeneralTreeEntity<TArea>, IShopAdministrative
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        public DefaultBXJGShopBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create(bool insertTestData = true)
        {
            new DefaultBXJGShopDataDictionaryBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            new DefaultBXJGShopItemBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            new DefaultBXJGShopCustomerBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            _context.SaveChanges();
        }
    }
}
