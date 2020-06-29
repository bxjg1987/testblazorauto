using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Shop.Catalogue;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Shop.Seed
{
    public class DefaultBXJGShopItemBuilder<TTenant, TRole, TUser, TSelf, TDataDictionary>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<ItemEntity<TDataDictionary>> items;

        DbSet<ItemCategoryEntity> cls;

        public DefaultBXJGShopItemBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            items = context.Set<ItemEntity<TDataDictionary>>();
            this.cls = context.Set<ItemCategoryEntity>();
        }

        public void Create(bool insertTestData = true)
        {
            if (!insertTestData)
                return;

            if (items.Any())
                return;

         
           
            cls.Add(new ItemCategoryEntity
            {
                Code = "00001",
                Icon = "a.jpg",
                DisplayName = "服装",
                Image1 = "a1.jpg",
                Image2 = "a2.jpg",
                TenantId = this._tenantId,
                ShowInHome = true,
                CreationTime= DateTime.Now,
                Children = new List<ItemCategoryEntity>
                {
                    new ItemCategoryEntity
                    {
                        Code = "00001.00001",
                        Icon = "a1.jpg",
                        DisplayName = "休闲男装",
                        Image1 = "a11.jpg",
                        Image2 = "a12.jpg",
                        CreationTime= DateTime.Now,
                        TenantId = this._tenantId,
                        ShowInHome = false,
                        Children = new List<ItemCategoryEntity>
                        {
                            new ItemCategoryEntity
                            {
                                Code = "00001.00001.00001",
                                Icon = "a11.jpg",
                                DisplayName = "宽松舒适",
                                Image1 = "a111.jpg",
                                CreationTime= DateTime.Now,
                                Image2 = "a112.jpg",
                                TenantId = this._tenantId,
                                ShowInHome = false
                            },
                            new ItemCategoryEntity
                            {
                                Code = "00001.00001.00002",
                                Icon = "a12.jpg",
                                DisplayName = "紧身塑性",
                                Image1 = "a112234.jpg",
                                CreationTime= DateTime.Now,
                                Image2 = "a11222.jpg",
                                TenantId = this._tenantId,
                                ShowInHome = false
                            }
                        }
                    },
                    new ItemCategoryEntity{
                        Code = "00001.00002",
                        Icon = "asdfsd.jpg",
                        DisplayName = "测试分类",
                        Image1 = "asdf1.jpg",
                        Image2 = "asdfsdf2.jpg",
                        CreationTime= DateTime.Now,
                        TenantId = this._tenantId,
                        ShowInHome = false,
                    }
                }
            });
            this._context.SaveChanges();

            cls.Add(new ItemCategoryEntity {
                Code = "00002",
                Icon = "sdfsa.jpg",
                DisplayName = "家电",
                Image1 = "asdf1.jpg",
                Image2 = "asssdf2.jpg",
                CreationTime = DateTime.Now,
                TenantId = this._tenantId,
                ShowInHome = true,
            });
            this._context.SaveChanges();
           
            items.Add(new ItemEntity<TDataDictionary>
            {
                AvailableEnd = DateTime.Now.AddHours(325),
                AvailableStart = DateTime.Now.AddHours(-17),
                CategoryId = 1,
                BrandId = 20,
                DescriptionShort = "简短描述",
                Title = "吉普JEEP短袖t恤男2020夏季商务休闲男装T恤男士条纹翻领体恤POLO衫打底衫上衣 蓝色条纹 L",
                TenantId = this._tenantId,
                Sku = "PT001",
                Published = true,
                Focus = false,
                Home = true,
                New = true,
                Hot = false,
                OldPrice = 218,
                Price = 86,
                Integral = 86,
                Images = "upload/442ee74ddb0186fb.jpg",
            });
            this._context.SaveChanges();
            items.Add(new ItemEntity<TDataDictionary>
            {
                AvailableEnd = DateTime.Now.AddHours(145),
                AvailableStart = DateTime.Now.AddHours(-237),
                CategoryId = 2,
                BrandId = 21,
                DescriptionShort = "简短描述",
                Title = "云妆蝶梦 t恤女短袖2020夏季新品韩版大码圆领印花纯棉体恤女装打底衫休闲百搭棉上衣 M806 小花蓝色",
                TenantId = this._tenantId,
                Sku = "XTW02",
                Published = false,
                Focus = true,
                Home = true,
                New = false,
                Hot = false,
                OldPrice = 68,
                Price = 38,
                Integral = 38,
                Images = "upload/d4047d7267bcf1fc.jpg",

            });
            this._context.SaveChanges();
            items.Add(new ItemEntity<TDataDictionary>
            {
                AvailableEnd = DateTime.Now.AddHours(145),
                AvailableStart = DateTime.Now.AddHours(-237),
                CategoryId = 3,
                //BrandId = 103,
                DescriptionShort = "简短描述",
                Title = "力开力朗 双肩包 442 户外大容量登山包休闲旅行背包50L 带防雨罩 桔色",
                TenantId = this._tenantId,
                //Sku = "XTW02",
                Published = true,
                Focus = true,
                Home = true,
                New = false,
                Hot = true,
                OldPrice = 1689,
                Price = 988,
                Integral = 988,
                Images = "upload/57feec7cN5f2eac85.jpg",

            });
            this._context.SaveChanges();
            items.Add(new ItemEntity<TDataDictionary>
            {
                //AvailableEnd = DateTime.Now.AddHours(145),
                //AvailableStart = DateTime.Now.AddHours(-237),
                CategoryId = 4,
                BrandId = 22,
                DescriptionShort = "简短描述",
                Title = "【2020春夏新款】外交官Diplomat行李箱拉杆箱登机箱万向轮男女旅行箱密码箱TC-623系列 镜面蓝色 19英寸 / 登机箱 / 无侧边手提&脚垫",
                TenantId = this._tenantId,
                Sku = "KY0x-192",
                Published = true,
                Focus = false,
                Home = true,
                New = false,
                Hot = false,
                //OldPrice = 386,
                Price = 698,
                Integral = 698,
                Images = "upload/8f71080a3e183310.jpg",
            });
            this._context.SaveChanges();
            items.Add(new ItemEntity<TDataDictionary>
            {
                AvailableEnd = DateTime.Now.AddHours(421),
                AvailableStart = DateTime.Now.AddHours(-57),
                CategoryId = 4,
                BrandId = 23,
                DescriptionShort = "简短描述",
                Title = "苹果联想戴尔小米电脑包双肩包15.6寸14寸17.3寸男女笔记本背包 红色(带USB接口) 14寸",
                TenantId = this._tenantId,
                //Sku = "KY0x-192",
                Published = false,
                Focus = true,
                Home = false,
                New = false,
                Hot = true,
                OldPrice = 198,
                Price = 108,
                Integral = 108,
                Images = "upload/b.jpg",
            });
            this._context.SaveChanges();
            items.Add(new ItemEntity<TDataDictionary>
            {
                AvailableEnd = DateTime.Now.AddHours(11),
                AvailableStart = DateTime.Now.AddHours(-257),
                CategoryId = 3,
                BrandId = 22,
                DescriptionShort = "简短描述",
                Title = "李宁短袖T恤男子半袖运动服篮球系列男装ATSN159",
                TenantId = this._tenantId,
                Sku = "Wk9913",
                Published = true,
                Focus = false,
                Home = true,
                New = false,
                Hot = false,
                OldPrice = 298,
                Price = 99,
                Integral = 99,
                Images = "upload/a.jpg",
            });
            this._context.SaveChanges();
        }
    }
}
