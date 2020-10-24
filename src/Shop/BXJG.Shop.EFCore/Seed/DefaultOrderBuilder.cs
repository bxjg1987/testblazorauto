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

            var order = new OrderEntity
            {
                AreaId = 4,
                Consignee = "张三",
                ConsigneePhoneNumber = "17723896676",
                CustomerId = 1,
                CustomerRemark = "顾客备注信息",
                DistributionMethodId = 30,
                //TenantId = this._tenantId,
                //Status = OrderStatus.Created,
                ReceivingAddress = "收货地址",
                //PaymentStatus = PaymentStatus.Paid,
                //PaymentMethodId = 31,
                PaymentAmount = ois.Take(2).Sum(c => c.Price),
                OrderTime = new DateTimeOffset(2020, 5, 15, 21, 2, 3, TimeSpan.Zero),
                OrderNo = Guid.NewGuid().ToString("N"),
                Integral = 324,
                MerchandiseSubtotal = 318,
                Items = ois.Take(2).Select(c => new OrderItemEntity
                {
                    Amount = c.Price * 3,
                    Quantity = 3,
                    Image = c.Images.Split(',').First(),
                    Integral = c.Integral,
                    ProductId = c.Id,
                    OrderId = 1,
                    Price = c.Price,
                    Title = c.Title,
                    TotalIntegral = c.Integral * 3
                }).ToList()
            };


            items.Add(order);





            _context.SaveChanges();


            var order1 = new OrderEntity
            {
                AreaId = 6,
                Consignee = "李四",
                ConsigneePhoneNumber = "18323335646",
                CustomerId = 2,
                CustomerRemark = "顾客备注信息，test",
                DistributionMethodId = 34,
                TenantId = this._tenantId,
                //Status = OrderStatus.Processing,
                ReceivingAddress = "收货地址,test",
                //PaymentStatus = PaymentStatus.Paid,
                //PaymentMethodId = 33,
                PaymentAmount = ois.Skip(2).Sum(c => c.Price),
                OrderTime = new DateTimeOffset(2020, 3, 11, 15, 7, 25, TimeSpan.Zero),
                OrderNo = Guid.NewGuid().ToString("N"),
                Integral = ois.Skip(2).Sum(c => c.Integral),
                MerchandiseSubtotal = ois.Skip(2).Sum(c => c.Price) - 5,
                Items = ois.Skip(2).Select(c => new OrderItemEntity
                {
                    Amount = c.Price * 1,
                    Quantity = 1,
                    Image = c.Images.Split(',').First(),
                    Integral = c.Integral,
                    ProductId = c.Id,
                    OrderId = 1,
                    Price = c.Price,
                    Title = c.Title,
                    TotalIntegral = c.Integral * 1
                }).ToList()
            };
            order1.Pay(33);
            items.Add(order1);

            _context.SaveChanges();

        }
    }
}
