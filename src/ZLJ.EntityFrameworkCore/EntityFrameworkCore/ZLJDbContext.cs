using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using ZLJ.BaseInfo;
using BXJG.GeneralTree;
using ZLJ.Asset;
using BXJG.Shop.Common;

namespace ZLJ.EntityFrameworkCore
{
    public class ZLJDbContext : AbpZeroDbContext<Tenant, Role, User, ZLJDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public virtual DbSet<OrganizationUnitEntity> OrganizationUnitEntities { get; set; }

        public virtual DbSet<GeneralTreeEntity> GeneralTreeEntities { get; set; }
        public virtual DbSet<EquipmentInfoEntity> EquipmentInfos { get; set; }

        public virtual DbSet<BXJGShopDictionaryEntity> BXJGShopDictionaries { get; set; }
        public ZLJDbContext(DbContextOptions<ZLJDbContext> options)
            : base(options)
        {
        }
    }
}
