using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Shop.Seed
{
    /// <summary>
    /// 为商城模块录入商品信息演示数据
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TSelf"></typeparam>
    public class DefaultProductBuilder<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<ProductEntity> products;
        DbSet<GeneralTreeEntity> dics;
        DbSet<ProductCategoryEntity> cls;

        public DefaultProductBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            products = context.Set<ProductEntity>();
            this.cls = context.Set<ProductCategoryEntity>();
            dics = context.Set<GeneralTreeEntity>();
        }

        public void Create(bool insertTestData = true)
        {
            if (!insertTestData)
                return;

            if (products.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId))
                return;

            #region 插入产品演示数据
            var pp = dics.IgnoreQueryFilters().Include(c => c.Children).Where(c => c.DisplayName == "商品品牌" && c.TenantId == _tenantId).Single();
            var dw = dics.IgnoreQueryFilters().Include(c => c.Children).Where(c => c.DisplayName == "商品单位" && c.TenantId == _tenantId).Single();
            products.Add(new ProductEntity
            {
                UnitId = dw.Children[new Random().Next(0, dw.Children.Count)].Id,
                //AvailableEnd = DateTime.Now.AddHours(325),
                //AvailableStart = DateTime.Now.AddHours(-17),
                CategoryId = 1,
                BrandId = pp.Children[new Random().Next(0, pp.Children.Count)].Id,
                DescriptionShort = "简短描述",
                Title = "吉普JEEP短袖t恤男2020夏季商务休闲男装T恤男士条纹翻领体恤POLO衫打底衫上衣 蓝色条纹 L",
                TenantId = this._tenantId,
                //Published = true,
                Focus = false,
                Home = true,
                New = true,
                Hot = false,
                Images = "/upload/20201004/442ee74ddb0186fb.jpg",
                Skus = new List<SkuEntity> {
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="1", Integral=3, OldPrice=8, Price=3},
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="2", Integral=68, OldPrice=398, Price=2.54m },
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="3", Integral=4, OldPrice=398, Price=8 }
                }
            });
            this._context.SaveChanges();
            products.Add(new ProductEntity
            {
                UnitId = dw.Children[new Random().Next(0, dw.Children.Count)].Id,
                //AvailableEnd = DateTime.Now.AddHours(145),
                //AvailableStart = DateTime.Now.AddHours(-237),
                CategoryId = 2,
                BrandId = pp.Children[new Random().Next(0, pp.Children.Count)].Id,
                DescriptionShort = "简短描述",
                Title = "云妆蝶梦 t恤女短袖2020夏季新品韩版大码圆领印花纯棉体恤女装打底衫休闲百搭棉上衣 M806 小花蓝色",
                TenantId = this._tenantId,
                //Published = false,
                Focus = true,
                Home = true,
                New = false,
                Hot = false,
                Images = "/upload/20201004/d4047d7267bcf1fc.jpg",
                Skus = new List<SkuEntity> {
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="1", Integral=35, OldPrice=8, Price=3},
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="2", Integral=35, OldPrice=398, Price=2.54m },
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="3", Integral=66, OldPrice=398, Price=8 }
                }
            });
            this._context.SaveChanges();
            products.Add(new ProductEntity
            {
                UnitId = dw.Children[new Random().Next(0, dw.Children.Count)].Id,
                //AvailableEnd = DateTime.Now.AddHours(145),
                //AvailableStart = DateTime.Now.AddHours(-237),
                CategoryId = 3,
                BrandId = pp.Children[new Random().Next(0, pp.Children.Count)].Id,
                DescriptionShort = "简短描述",
                Title = "力开力朗 双肩包 442 户外大容量登山包休闲旅行背包50L 带防雨罩 桔色",
                TenantId = this._tenantId,
                //Sku = "XTW02",
                //Published = true,
                Focus = true,
                Home = true,
                New = false,
                Hot = true,
                Images = "/upload/20201004/57feec7cN5f2eac85.jpg",
                Skus = new List<SkuEntity> {
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="1", Integral=53, OldPrice=64, Price=72},
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="2", Integral=43, OldPrice=63, Price=636m },
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="3", Integral=67, OldPrice=53, Price=42 }
                }
            });
            this._context.SaveChanges();
            products.Add(new ProductEntity
            {
                UnitId = dw.Children[new Random().Next(0, dw.Children.Count)].Id,
                //AvailableEnd = DateTime.Now.AddHours(145),
                //AvailableStart = DateTime.Now.AddHours(-237),
                CategoryId = 4,
                BrandId = pp.Children[new Random().Next(0, pp.Children.Count)].Id,
                DescriptionShort = "简短描述",
                Title = "【2020春夏新款】外交官Diplomat行李箱拉杆箱登机箱万向轮男女旅行箱密码箱TC-623系列 镜面蓝色 19英寸 / 登机箱 / 无侧边手提&脚垫",
                TenantId = this._tenantId,
                //Published = true,
                Focus = false,
                Home = true,
                New = false,
                Hot = false,
                Images = "/upload/20201004/8f71080a3e183310.jpg",
                Skus = new List<SkuEntity> {
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="1", Integral=8, OldPrice=8, Price=3},
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="2", Integral=69, OldPrice=31, Price=3.4m },
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="3", Integral=95, OldPrice=7, Price=38.4m }
                }
            });
            this._context.SaveChanges();
            products.Add(new ProductEntity
            {
                UnitId = dw.Children[new Random().Next(0, dw.Children.Count)].Id,
                //AvailableEnd = DateTime.Now.AddHours(421),
                //AvailableStart = DateTime.Now.AddHours(-57),
                CategoryId = 4,
                BrandId = pp.Children[new Random().Next(0, pp.Children.Count)].Id,
                DescriptionShort = "简短描述",
                Title = "苹果联想戴尔小米电脑包双肩包15.6寸14寸17.3寸男女笔记本背包 红色(带USB接口) 14寸",
                TenantId = this._tenantId,
                //Sku = "KY0x-192",
                //Published = false,
                Focus = true,
                Home = false,
                New = false,
                Hot = true,
                Images = "/upload/20201004/b.jpg",
                Skus = new List<SkuEntity> {
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="1", Integral=74, OldPrice=8, Price=353},
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="2", Integral=68, OldPrice=398, Price=62.54m },
                    new SkuEntity{  DynamicEntityProperty1Id=1, DynamicEntityProperty1Value="3", Integral=4, OldPrice=2, Price=84 }
                }
            });
            this._context.SaveChanges();
            products.Add(new ProductEntity
            {
                UnitId = dw.Children[new Random().Next(0, dw.Children.Count)].Id,
                //AvailableEnd = DateTime.Now.AddHours(11),
                //AvailableStart = DateTime.Now.AddHours(-257),
                CategoryId = 3,
                BrandId = pp.Children[new Random().Next(0, pp.Children.Count)].Id,
                DescriptionShort = "简短描述",
                Title = "李宁短袖T恤男子半袖运动服篮球系列男装ATSN159",
                TenantId = this._tenantId,
                //Published = true,
                Focus = false,
                Home = true,
                New = false,
                Hot = false,
                Images = "/upload/20201004/a.jpg"
            });
            this._context.SaveChanges();
            #endregion

            #region 插入abp实体动态属性值 以实现sku
            //动态属性初始化参考 ProductDynamicPropertyBuilder
            //var dep = _context.DynamicEntityProperties.IgnoreQueryFilters().Where(c => c.TenantId == _tenantId).ToList();
            //var p = 0;
            //for (int i = 0; i < 5; i++)
            //{
            //    for (int k = 0; k < 3; k++)
            //    {
            //        for (int j = 0; j < 3; j++)
            //        {
            //            p++;
            //            _context.DynamicEntityPropertyValues.Add(new DynamicEntityPropertyValue(dep[0], p.ToString(), (k + 1).ToString(), _tenantId));
            //            _context.DynamicEntityPropertyValues.Add(new DynamicEntityPropertyValue(dep[1], p.ToString(), (j + 1).ToString(), _tenantId));
            //            _context.SaveChanges();
            //        }
            //    }
            //}
            #endregion
        }
    }
}
