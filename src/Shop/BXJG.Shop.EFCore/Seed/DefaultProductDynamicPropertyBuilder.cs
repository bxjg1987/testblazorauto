using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.DynamicEntityProperties;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.UI.Inputs;
using BXJG.Shop.Catalogue;

namespace BXJG.Shop.Seed
{
    /// <summary>
    /// sku需要的动态属性功能相关数据的初始化
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TSelf"></typeparam>
    public class DefaultProductDynamicPropertyBuilder<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;

        public DefaultProductDynamicPropertyBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create(bool insertTestData = true)
        {
            if (!insertTestData)
                return;

            if (_context.DynamicProperties.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId && c.PropertyName == "规格"))
                return;

            _context.DynamicProperties.Add(new DynamicProperty
            {
                InputType = InputTypeBase.GetName<ComboboxInputType>(),
                PropertyName = "规格",
                TenantId = _tenantId
            });
            _context.SaveChanges();

            _context.DynamicPropertyValues.Add(new DynamicPropertyValue
            {
                DynamicPropertyId = 1,
                TenantId = _tenantId,
                Value = "大"
            });
            _context.SaveChanges();

            _context.DynamicPropertyValues.Add(new DynamicPropertyValue
            {
                DynamicPropertyId = 1,
                TenantId = _tenantId,
                Value = "中"
            });
            _context.SaveChanges();

            _context.DynamicPropertyValues.Add(new DynamicPropertyValue
            {
                DynamicPropertyId = 1,
                TenantId = _tenantId,
                Value = "小"
            });
            _context.SaveChanges();

            _context.DynamicEntityProperties.Add(new DynamicEntityProperty
            {
                DynamicPropertyId = 1,
                EntityFullName = typeof(SkuEntity).FullName,
                TenantId = _tenantId
            });
            _context.SaveChanges();
        }
    }
}
