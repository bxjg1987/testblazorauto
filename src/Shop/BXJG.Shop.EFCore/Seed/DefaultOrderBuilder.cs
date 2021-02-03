using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Shop.Customer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BXJG.Shop.Sale;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using BXJG.Common;

namespace BXJG.Shop.Seed
{
    public class DefaultOrderBuilder<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>

    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<OrderEntity> items;
        DbSet<ProductEntity> orderItems;
        public DefaultOrderBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            items = context.Set<OrderEntity>();
            orderItems = context.Set<ProductEntity>();
        }

        public void Create(bool insertTestData = true)
        {
            if (!insertTestData)
                return;

            if (items.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId))
                return;

            var ois = orderItems.IgnoreQueryFilters().Where(c => c.TenantId == _tenantId).ToList();

            var order = new OrderEntity(1, Guid.NewGuid().ToString("N"), DateTimeOffset.Now, 4, "张三", "17723896676", "收货地址", "顾客备注信息");
            order.TenantId = 1;
            order.AddItem(new OrderItemEntity(order, 1, 1, "商品标题", "sdf图片地址", 3, 3, 3));
            order.AddItem(new OrderItemEntity(order, 2, default, "商品标题1", "sdf图片地址1", 4, 4, 4));
            items.Add(order);
            _context.SaveChanges();


            var order1 = new OrderEntity(1, Guid.NewGuid().ToString("N"), DateTimeOffset.Now, 4, "张三", "17723896676", "收货地址", "顾客备注信息");
            order1.TenantId = 1;
            order1.AddItem(new OrderItemEntity(order1, 1, default, "商品标题sdf", "sdf图片地sdf址", 34,52, 3));
            order1.AddItem(new OrderItemEntity(order1, 2, 1, "商品标题1sdfddf", "sdf图片ffff地址1", 63, 34, 6));
            items.Add(order1);
            _context.SaveChanges();

        }
    }
}
