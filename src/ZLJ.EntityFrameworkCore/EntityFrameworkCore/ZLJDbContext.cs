using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ.EntityFrameworkCore
{
    public class ZLJDbContext : AbpZeroDbContext<Tenant, Role, User, ZLJDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public ZLJDbContext(DbContextOptions<ZLJDbContext> options)
            : base(options)
        {
        }
    }
}
