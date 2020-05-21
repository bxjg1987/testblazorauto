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
using BXJG.Shop.Common;
using BXJG.Shop.Catalogue;
using BXJG.Common;

namespace BXJG.Shop.Seed
{
    public class DefaultBXJGShopOrderBuilder<TTenant, TRole, TUser, TSelf, TArea>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<OrderEntity<TUser, TArea>> items;
        DbSet<ItemEntity> orderItems;
        public DefaultBXJGShopOrderBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            items = context.Set<OrderEntity<TUser, TArea>>();
            orderItems = context.Set<ItemEntity>();
        }

        public void Create(bool insertTestData = true)
        {
            if (!insertTestData)
                return;

            if (items.Any())
                return;

            var ois = orderItems.ToList();

            var order = new OrderEntity<TUser, TArea>
            {
                AreaId = 4,
                Consignee = "张三",
                ConsigneePhoneNumber = "17723896676",
                CustomerId = 1,
                CustomerRemark = "顾客备注信息",
                DistributionMethodId = 107,
                TenantId = this._tenantId,
                Status = OrderStatus.Created,
                ReceivingAddress = "收货地址",
                //PaymentStatus = PaymentStatus.Paid,
                //PaymentMethodId = 
                PaymentAmount = ois.Take(2).Sum(c=>c.Price),
                OrderTime = new DateTimeOffset(2020, 5, 15, 21, 2, 3, TimeSpan.Zero),
                OrderNo = Guid.NewGuid().ToString("N"),
                Integral = 324,
                MerchandiseSubtotal = 318,
                Items = ois.Take(2).Select(c => new OrderItemEntity<TUser, TArea>
                {
                    Amount = c.Price * 3,
                    Quantity = 3,
                    Image = c.Images.Split(',').First(),
                    Integral = c.Integral,
                    ItemId = c.Id,
                    OrderId = 1,
                    Price = c.Price,
                    Title = c.Title,
                    TotalIntegral = c.Integral * 3
                }).ToList()
            };


            items.Add(order);





            _context.SaveChanges();


            var order1 = new OrderEntity<TUser, TArea>
            {
                AreaId = 6,
                Consignee = "李四",
                ConsigneePhoneNumber = "18323335646",
                CustomerId = 2,
                CustomerRemark = "顾客备注信息，test",
                DistributionMethodId = 108L,
                TenantId = this._tenantId,
                Status = OrderStatus.Processing,
                ReceivingAddress = "收货地址,test",
                PaymentStatus = PaymentStatus.Paid,
                PaymentMethodId =  105L,
                PaymentAmount = ois.Skip(2).Sum(c => c.Price),
                OrderTime = new DateTimeOffset(2020, 3, 11, 15, 7, 25, TimeSpan.Zero),
                OrderNo = Guid.NewGuid().ToString("N"),
                Integral = ois.Skip(2).Sum(c => c.Integral),
                MerchandiseSubtotal = ois.Skip(2).Sum(c => c.Price)-5,
                Items = ois.Skip(2).Select(c => new OrderItemEntity<TUser, TArea>
                {
                    Amount = c.Price * 1,
                    Quantity = 1,
                    Image = c.Images.Split(',').First(),
                    Integral = c.Integral,
                    ItemId = c.Id,
                    OrderId = 1,
                    Price = c.Price,
                    Title = c.Title,
                    TotalIntegral = c.Integral * 1
                }).ToList()
            };

            items.Add(order1);

            _context.SaveChanges();

        }
    }
}
