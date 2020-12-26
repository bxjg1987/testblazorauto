using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.UI.Inputs;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Shop.Catalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DefaultProductCagtegoryBuilder<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<ProductCategoryEntity> set;

        public DefaultProductCagtegoryBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            set = context.Set<ProductCategoryEntity>();
        }

        public void Create(bool insertTestData = true)
        {
            if (!insertTestData)
                return;

            if (set.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId))
                return;

            set.Add(new ProductCategoryEntity
            {
                Code = "00001",
                CreationTime = DateTime.Now,
                DisplayName = "分类1",
                TenantId = _tenantId,
                ShowInHome = true
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

            var cls2 = new ProductCategoryEntity
            {
                Code = "00002",
                CreationTime = DateTime.Now,
                DisplayName = "分类2",
                TenantId = _tenantId
            };
            set.Add(cls2);
            _context.SaveChanges();//必须保持下才能保证生产id
            #region 分类2的sku动态属性定义
            #region 颜色
            var dp1 = new Abp.DynamicEntityProperties.DynamicProperty
            {
                PropertyName = "color" + cls2.Id,
                DisplayName = "颜色",
                InputType = InputTypeBase.GetName<ComboboxInputType>(),
                TenantId = _tenantId
            };
            _context.DynamicProperties.Add(dp1);
            _context.SaveChanges();
            #region 颜色可选值
            _context.DynamicPropertyValues.Add(new Abp.DynamicEntityProperties.DynamicPropertyValue
            {
                DynamicPropertyId = dp1.Id,
                TenantId = _tenantId,
                Value = "红色"
            });
            _context.SaveChanges();
            _context.DynamicPropertyValues.Add(new Abp.DynamicEntityProperties.DynamicPropertyValue
            {
                DynamicPropertyId = dp1.Id,
                TenantId = _tenantId,
                Value = "黄色"
            });
            _context.SaveChanges();
            _context.DynamicPropertyValues.Add(new Abp.DynamicEntityProperties.DynamicPropertyValue
            {
                DynamicPropertyId = dp1.Id,
                TenantId = _tenantId,
                Value = "蓝色"
            });
            _context.SaveChanges();
            #endregion
            #endregion
            #region 尺码
            var dp2 = new Abp.DynamicEntityProperties.DynamicProperty
            {
                PropertyName = "size" + cls2.Id,
                DisplayName = "尺码",
                InputType = InputTypeBase.GetName<ComboboxInputType>(),
                TenantId = _tenantId
            };
            _context.DynamicProperties.Add(dp2);
            _context.SaveChanges();
            #region 尺码可选值
            _context.DynamicPropertyValues.Add(new Abp.DynamicEntityProperties.DynamicPropertyValue
            {
                DynamicPropertyId = dp2.Id,
                TenantId = _tenantId,
                Value = "XS"
            });
            _context.SaveChanges();
            _context.DynamicPropertyValues.Add(new Abp.DynamicEntityProperties.DynamicPropertyValue
            {
                DynamicPropertyId = dp2.Id,
                TenantId = _tenantId,
                Value = "S"
            });
            _context.SaveChanges();
            _context.DynamicPropertyValues.Add(new Abp.DynamicEntityProperties.DynamicPropertyValue
            {
                DynamicPropertyId = dp2.Id,
                TenantId = _tenantId,
                Value = "M"
            });
            _context.SaveChanges();
            _context.DynamicPropertyValues.Add(new Abp.DynamicEntityProperties.DynamicPropertyValue
            {
                DynamicPropertyId = dp2.Id,
                TenantId = _tenantId,
                Value = "X"
            });
            _context.SaveChanges();
            #endregion
            #endregion
            #region 选择
            var dp3 = new Abp.DynamicEntityProperties.DynamicProperty
            {
                PropertyName = "check" + cls2.Id,
                DisplayName = "选择",
                InputType = InputTypeBase.GetName<CheckboxInputType>(),
                TenantId = _tenantId
            };
            _context.DynamicProperties.Add(dp3);
            _context.SaveChanges();
            #endregion
            #region 自定义输入
            var dp4 = new Abp.DynamicEntityProperties.DynamicProperty
            {
                PropertyName = "input" + cls2.Id,
                DisplayName = "输入",
                InputType = InputTypeBase.GetName<SingleLineStringInputType>(),
                TenantId = _tenantId
            };
            _context.DynamicProperties.Add(dp4);
            _context.SaveChanges();
            #endregion

            #region 动态属性与分类关联
            _context.DynamicEntityProperties.Add(new Abp.DynamicEntityProperties.DynamicEntityProperty
            {
                DynamicPropertyId = dp1.Id,
                EntityFullName = typeof(ProductCategoryEntity).FullName + cls2.Id,
                TenantId = _tenantId
            }); 
            _context.SaveChanges();
            _context.DynamicEntityProperties.Add(new Abp.DynamicEntityProperties.DynamicEntityProperty
            {
                DynamicPropertyId = dp2.Id,
                EntityFullName = typeof(ProductCategoryEntity).FullName + cls2.Id,
                TenantId = _tenantId
            });
            _context.SaveChanges();
            _context.DynamicEntityProperties.Add(new Abp.DynamicEntityProperties.DynamicEntityProperty
            {
                DynamicPropertyId = dp3.Id,
                EntityFullName = typeof(ProductCategoryEntity).FullName + cls2.Id,
                TenantId = _tenantId
            });
            _context.SaveChanges();
            _context.DynamicEntityProperties.Add(new Abp.DynamicEntityProperties.DynamicEntityProperty
            {
                DynamicPropertyId = dp4.Id,
                EntityFullName = typeof(ProductCategoryEntity).FullName + cls2.Id,
                TenantId = _tenantId
            });
            _context.SaveChanges();
            #endregion
            #endregion
        }
    }
}
