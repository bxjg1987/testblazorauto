using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.Seed.BaseInfo
{
    public class DefaultBXJGBaseInfoBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;
        private readonly bool insertTestData;

        public DefaultBXJGBaseInfoBuilder(ZLJDbContext context, int tenantId, bool insertTestData = false)
        {
            _context = context;
            _tenantId = tenantId;
            this.insertTestData = insertTestData;
        }

        public void Create()
        {
            new DefaultBXJGBaseInfoDataDictionaryBuilder(_context, _tenantId).Create();
            new DefaultBXJGBaseInfoAdministrativeBuilder(_context, _tenantId).Create();
            new DefaultBXJGBaseInfoAssociatedCompanyBuilder(_context, _tenantId).Create();
            new DefaultPostBuilder(_context, _tenantId).Create();
            new DefaultBXJGBaseInfoStaffInfoBuilder(_context, _tenantId).Create();
            _context.SaveChanges();
        }
    }
}