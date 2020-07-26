using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Common;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.BaseInfo.EFCore.Seed
{
    public class DefaultBXJGBaseInfoBuilder< TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        public DefaultBXJGBaseInfoBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            new DefaultBXJGBaseInfoAdministrativeBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create();
            _context.SaveChanges();
        }
    }
}
