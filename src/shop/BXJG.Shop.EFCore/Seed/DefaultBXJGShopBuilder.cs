using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Seed
{
    public class DefaultBXJGShopBuilder< TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser> ,new()
        where TUser : AbpUser<TUser>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        long? parentId;
        public DefaultBXJGShopBuilder(TSelf context, int tenantId, long? parentId = default)
        {
            _context = context;
            this.parentId = parentId;
            _tenantId = tenantId;
        }

        public void Create(bool insertTestData = true)
        {
            new DefaultBXJGShopDataDictionary<TTenant, TRole, TUser, TSelf>(_context, _tenantId, parentId).Create(insertTestData);
            //new DefaultBXJGShopItemCagtegoryBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            new DefaultBXJGShopItemBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            new DefaultBXJGShopCustomerBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            new DefaultBXJGShopOrderBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            //_context.SaveChanges();
        }
        //public void CreateDataDictionary(bool insertTestData = true, long? parentId=default)
        //{
        //    new DefaultBXJGShopDataDictionary<TTenant, TRole, TUser, TSelf>(_context, _tenantId, parentId, insertTestData).Create();
        //    _context.SaveChanges();
        //}
    }
}
