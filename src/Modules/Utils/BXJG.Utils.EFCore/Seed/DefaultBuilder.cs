using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;

namespace BXJG.Utils.EFCore.Seed
{
    public class DefaultBuilder< TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser> ,new()
        where TUser : AbpUser<TUser>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        long? parentId;
        public DefaultBuilder(TSelf context, int tenantId, long? parentId = default)
        {
            _context = context;
            this.parentId = parentId;
            _tenantId = tenantId;
        }

        public void Create(bool insertTestData = true)
        {
            //new DefaultDataDictionary<TTenant, TRole, TUser, TSelf>(_context, _tenantId, parentId).Create(insertTestData);
            //new DefaultProductCagtegoryBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            //new DefaultProductDynamicPropertyBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            //new DefaultProductBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            //new DefaultCustomerAndRoleBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
            //new DefaultOrderBuilder<TTenant, TRole, TUser, TSelf>(_context, _tenantId).Create(insertTestData);
        }
    }
}
