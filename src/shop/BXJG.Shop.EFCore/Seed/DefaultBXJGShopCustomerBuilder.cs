using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using BXJG.Shop.Customer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Shop.Seed
{
    /// <summary>
    /// 为商城模块插入顾客演示数据
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TSelf"></typeparam>
    public class DefaultBXJGShopCustomerBuilder<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<CustomerEntity> items;
        DbSet<TRole> roles;
        public DefaultBXJGShopCustomerBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            items = context.Set<CustomerEntity>();
            roles = context.Set<TRole>();
        }

        public void Create(bool insertTestData = true)
        {
            //初始化顾客的默认角色
            var role = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == BXJGShopConsts.CustomerRoleName);
            if (role == null)
            {
                var sdf = new TRole();
                sdf.TenantId = _tenantId;
                sdf.Name = BXJGShopConsts.CustomerRoleName;
                sdf.DisplayName = BXJGShopConsts.CustomerRoleName;
                sdf.SetNormalizedName();
                sdf.IsStatic = true;
                role = _context.Roles.Add(sdf).Entity;
                _context.SaveChanges();
            }

            if (!insertTestData)
                return;

            if (items.IgnoreQueryFilters().Where(c=>c.TenantId == _tenantId).Any())
                return;

           // var role = roles.Single(c => c.Name == BXJGShopConsts.CustomerRoleName);
            var adminUser = new TUser
            {
                TenantId = _tenantId,
                UserName = "cust1",
                Name = "测试顾客",
                Surname = "测试顾客",
                EmailAddress = "ssssss@sdfs.com",
                Roles = new List<UserRole>()
            };
            adminUser.SetNormalizedNames();


            adminUser.Password = new PasswordHasher<TUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
            adminUser.IsEmailConfirmed = true;
            adminUser.IsActive = true;

            _context.Users.Add(adminUser);
            _context.SaveChanges();
            _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, role.Id));
            _context.SaveChanges();

            var cust1 = new CustomerEntity
            {
                Birthday = DateTime.Now.AddYears(-30),
                Gender = Gender.Man,
                TenantId = this._tenantId,
                UserId = adminUser.Id
            };
            items.Add(cust1);


            var adminUser1 = new TUser
            {
                TenantId = _tenantId,
                UserName = "cust2",
                Name = "测试顾客2",
                Surname = "测试顾客2",
                EmailAddress = "ssssss@sdfs2.com",
                Roles = new List<UserRole>()
            };
            adminUser1.SetNormalizedNames();


            adminUser1.Password = new PasswordHasher<TUser>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
            adminUser1.IsEmailConfirmed = true;
            adminUser1.IsActive = true;

            _context.Users.Add(adminUser1);
            _context.SaveChanges();

            var cust2 = new CustomerEntity
            {
                Birthday = DateTime.Now.AddYears(-22).AddDays(116),
                Gender = Gender.Woman,
                TenantId = this._tenantId,
                UserId = adminUser1.Id
            };
            items.Add(cust2);

            // Assign Admin role to admin user


            this._context.SaveChanges();
            _context.UserRoles.Add(new UserRole(_tenantId, adminUser1.Id, role.Id));
            _context.SaveChanges();
        }
    }
}
