using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using ZLJ.BaseInfo;
using BXJG.GeneralTree;
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
using System;
using BXJG.Equipment;

namespace ZLJ.EntityFrameworkCore
{
    public class ZLJDbContext : AbpZeroDbContext<Tenant, Role, User, ZLJDbContext>
    {
        /* Define a DbSet for each entity of the application */

        #region 主模块
        public virtual DbSet<OrganizationUnitEntity> OrganizationUnitEntities { get; set; }
        public virtual DbSet<GeneralTreeEntity> GeneralTreeEntities { get; set; }
        public virtual DbSet<AdministrativeEntity> Administratives { get; set; }
        #endregion

        //后期考虑实现动态DbSet简化实体注册

        #region 注册商城模块中的实体
        public virtual DbSet<BXJGShopDictionaryEntity> BXJGShopDictionaries { get; set; }
        public virtual DbSet<ItemCategoryEntity> BXJGShopItemCategories { get; set; }
        public virtual DbSet<ItemEntity<GeneralTreeEntity>> BXJGShopItems { get; set; }
        public virtual DbSet<CustomerEntity<User, AdministrativeEntity>> BXJGShopCustomers { get; set; }
        public virtual DbSet<OrderEntity<User, AdministrativeEntity, GeneralTreeEntity>> BXJGShopOrders { get; set; }
        #endregion

        #region CMS
        public virtual DbSet<AdEntity> BXJGCMSAds { get; set; }
        public virtual DbSet<AdControlEntity> BXJGCMSAdControls { get; set; }
        public virtual DbSet<AdPositionEntity> BXJGCMSAdPositions { get; set; }
        public virtual DbSet<AdRecordEntity> BXJGCMSAdRecords { get; set; }
        public virtual DbSet<ArticleEntity<GeneralTreeEntity>> BXJGCMSArticles { get; set; }
        public virtual DbSet<ColumnEntity<GeneralTreeEntity>> BXJGCMSColumns { get; set; }
        #endregion

        #region 设备管理
        public virtual DbSet<BXJG.Equipment.EquipmentInfo.EquipmentInfoEntity> BXJGEquipmentInfo { get; set; }
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
                .ApplyConfigurationBXJGShop<User, AdministrativeEntity, GeneralTreeEntity>()
                .ApplyConfigurationBXJGCMS<GeneralTreeEntity>()
                .ApplyConfigurationBXJGEquipment();//若这里使用泛型版本，DbSet中又使用EquipmentInfoEntity类型，则报错
        }

    }
}
