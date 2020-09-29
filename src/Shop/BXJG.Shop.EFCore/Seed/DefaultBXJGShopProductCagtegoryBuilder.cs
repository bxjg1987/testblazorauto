using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Seed
{
    /// <summary>
    /// 录入商品分类演示数据
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TSelf"></typeparam>
    public class DefaultBXJGShopProductCagtegoryBuilder<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<ProductCategoryEntity> set;

        public DefaultBXJGShopProductCagtegoryBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            set = context.Set<ProductCategoryEntity>();
        }

        public void Create(bool insertTestData = true)
        {
            if (!insertTestData)
                return;

            if (set.Any())
                return;

            set.Add(new ProductCategoryEntity
            {
                Code = "00001",
                CreationTime = DateTime.Now,
                DisplayName = "分类1",
                TenantId = _tenantId, 
                ShowInHome=true
            });
            _context.SaveChanges();//必须保存下才能保证生产id

            set.Add(new ProductCategoryEntity
            {
                Code = "00001.00001",
                CreationTime = DateTime.Now,
                DisplayName = "子类A",
                TenantId = _tenantId,
                ParentId = 1
            });
            _context.SaveChanges();//必须保持下才能保证生产id
            set.Add(new ProductCategoryEntity
            {
                Code = "00001.00002",
                CreationTime = DateTime.Now,
                DisplayName = "子类B",
                TenantId = _tenantId,
                ParentId = 1
            });
            _context.SaveChanges();//必须保持下才能保证生产id
            set.Add(new ProductCategoryEntity
            {
                Code = "00001.00002.00001",
                CreationTime = DateTime.Now,
                DisplayName = "子类B-1",
                TenantId = _tenantId,
                ParentId = 3
            });
            _context.SaveChanges();//必须保持下才能保证生产id

            set.Add(new ProductCategoryEntity
            {
                Code = "00002",
                CreationTime = DateTime.Now,
                DisplayName = "分类2",
                TenantId = _tenantId
            });
            _context.SaveChanges();//必须保持下才能保证生产id
        }
    }
}
