using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using ZLJ.BaseInfo;
using BXJG.GeneralTree;
using ZLJ.Asset;
using BXJG.Shop.Common;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using BXJG.Shop;
using BXJG.Shop.Sale;
using BXJG.Shop.EFMaps;
namespace ZLJ.EntityFrameworkCore
{
    public class ZLJDbContext : AbpZeroDbContext<Tenant, Role, User, ZLJDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public virtual DbSet<OrganizationUnitEntity> OrganizationUnitEntities { get; set; }
        public virtual DbSet<GeneralTreeEntity> GeneralTreeEntities { get; set; }
        public virtual DbSet<EquipmentInfoEntity> EquipmentInfos { get; set; }

        public virtual DbSet<BXJGShopDictionaryEntity> BXJGShopDictionaries { get; set; }
        public virtual DbSet<ItemEntity> BXJGShopItems { get; set; }
        public virtual DbSet<CustomerEntity<User>> BXJGShopCustomers { get; set; }
        public virtual DbSet<OrderEntity<User>> BXJGShopOrders { get; set; }


        public ZLJDbContext(DbContextOptions<ZLJDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //扫描并应用商城模块中的ef映射
            modelBuilder.ApplyConfigurationBXJGShop<User>();
        }
    }
}
