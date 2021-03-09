using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using ZLJ.BaseInfo;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using BXJG.Shop;
using BXJG.Shop.Sale;
using BXJG.Shop.EFMaps;
using BXJG.CMS.EFCore.EFMaps;
using BXJG.CMS.Ad;
using BXJG.CMS.Article;
using BXJG.CMS.Column;
using System;
using BXJG.BaseInfo.EFCore.EFMaps;
using ZLJ.BaseInfo.Administrative;
using BXJG.Equipment.EquipmentInfo;
using BXJG.Equipment.EFCore.EFMaps;
using BXJG.Shop.ShoppingCart;
using BXJG.WorkOrder.EFMaps;

namespace ZLJ.EntityFrameworkCore
{
    public class ZLJDbContext : AbpZeroDbContext<Tenant, Role, User, ZLJDbContext>
    {
        /* Define a DbSet for each entity of the application */

        #region 主模块
        public virtual DbSet<OrganizationUnitEntity> OrganizationUnitEntities { get; set; }
        public virtual DbSet<GeneralTreeEntity> BXJGGeneralTreeEntities { get; set; }
        public virtual DbSet<AdministrativeEntity> BXJGBaseInfoAdministratives { get; set; }
        #endregion

        //后期考虑实现动态DbSet简化实体注册

        #region 注册商城模块中的实体
        public virtual DbSet<ProductCategoryEntity> BXJGShopProductCategory { get; set; }
        public virtual DbSet<ProductEntity> BXJGShopProduct { get; set; }
        public virtual DbSet<CustomerEntity> BXJGShopCustomer { get; set; }
        //public virtual DbSet<ShippingAddressEntity>
        public virtual DbSet<OrderEntity> BXJGShopOrder { get; set; }
        public virtual DbSet<ShoppingCartEntity> BXJGShopShoppingCart { get; set; }
        #endregion

        #region CMS
        public virtual DbSet<AdEntity> BXJGCMSAds { get; set; }
        public virtual DbSet<AdControlEntity> BXJGCMSAdControls { get; set; }
        public virtual DbSet<AdPositionEntity> BXJGCMSAdPositions { get; set; }
        public virtual DbSet<AdRecordEntity> BXJGCMSAdRecords { get; set; }
        public virtual DbSet<ArticleEntity> BXJGCMSArticles { get; set; }
        public virtual DbSet<ColumnEntity> BXJGCMSColumns { get; set; }
        #endregion

        #region 设备管理
        public virtual DbSet<EquipmentInfoEntity> BXJGEquipmentInfo { get; set; }
        #endregion

        #region 工单
        public virtual DbSet<BXJG.WorkOrder.WorkOrderCategory.CategoryEntity> BXJGWorkOrderCategory { get; set; }
        public virtual DbSet<BXJG.WorkOrder.WorkOrder.OrderEntity> BXJGWorkOrder { get; set; }
        #endregion

        public ZLJDbContext(DbContextOptions<ZLJDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //注册各模块中的ef映射
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(ZLJEntityFrameworkModule).Assembly)
                .ApplyConfigurationBXJGShop()
                .ApplyConfigurationBXJGCMS()
                .ApplyConfigurationBXJGEquipment()
                .ApplyConfigurationBXJGBaseInfo()
                .ApplyConfigurationBXJGWorkOrder();
        }

    }
}
