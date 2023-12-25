using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Utils.EFCore.Seed
{
    /// <summary>
    /// 为商城模块插入顾客演示数据
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TSelf"></typeparam>
    public class DefaultCustomerAndRoleBuilder1<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        //DbSet<CustomerEntity> items;
        //DbSet<TRole> roles;
        public DefaultCustomerAndRoleBuilder1(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            //items = context.Set<CustomerEntity>();
            //roles = context.Set<TRole>();
        }

        public void Create(bool insertTestData = true)
        {
        }
    }
}
