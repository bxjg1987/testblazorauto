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
using ZLJ.Administrative;
using BXJG.CMS.EFCore.EFMaps;
using BXJG.CMS.Ad;
using BXJG.CMS.Article;
using BXJG.CMS.Column;

namespace ZLJ.EntityFrameworkCore
{
    public class ZLJDbContext : AbpZeroDbContext<Tenant, Role, User, ZLJDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public virtual DbSet<OrganizationUnitEntity> OrganizationUnitEntities { get; set; }
        public virtual DbSet<GeneralTreeEntity> GeneralTreeEntities { get; set; }
        public virtual DbSet<EquipmentInfoEntity> EquipmentInfos { get; set; }
        public virtual DbSet<AdministrativeEntity> Administratives { get; set; }

        //后期考虑实现动态DbSet简化实体注册

        #region 注册商城模块中的实体
        public virtual DbSet<BXJGShopDictionaryEntity> BXJGShopDictionaries { get; set; }
        public virtual DbSet<ItemEntity> BXJGShopItems { get; set; }
        public virtual DbSet<CustomerEntity<User, AdministrativeEntity>> BXJGShopCustomers { get; set; }
        public virtual DbSet<OrderEntity<User, AdministrativeEntity>> BXJGShopOrders { get; set; }
        #endregion

        #region CMS
        public virtual DbSet<AdEntity> BXJGCMSAds { get; set; }
        public virtual DbSet<AdControlEntity> BXJGCMSAdControls { get; set; }
        public virtual DbSet<AdPositionEntity> BXJGCMSAdPositions { get; set; }
        public virtual DbSet<AdRecordEntity> BXJGCMSAdRecords { get; set; }
        public virtual DbSet<ArticleEntity<GeneralTreeEntity>> BXJGCMSArticles { get; set; }
        public virtual DbSet<ColumnEntity<GeneralTreeEntity>> BXJGCMSColumns { get; set; }
        #endregion

        public ZLJDbContext(DbContextOptions<ZLJDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //扫描并应用商城模块中的ef映射
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(ZLJEntityFrameworkModule).Assembly)
                .ApplyConfigurationBXJGShop()
                .ApplyConfiguration(new CustomerMap<User, AdministrativeEntity, CustomerEntity<User, AdministrativeEntity>> ())
                .ApplyConfiguration(new OrderMap<User, AdministrativeEntity, OrderEntity<User, AdministrativeEntity>>())
                .ApplyConfiguration(new OrderItemMap<User, AdministrativeEntity, OrderItemEntity<User, AdministrativeEntity>>())
                .ApplyConfiguration(new ItemMap< ItemEntity>())
                .ApplyConfigurationBXJGCMS()
                .ApplyConfiguration(new ColumnMap<GeneralTreeEntity>())
                .ApplyConfiguration(new ArticleMap<GeneralTreeEntity>());
        }
    }
}
