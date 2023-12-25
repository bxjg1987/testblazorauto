using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.WorkOrder.WorkOrderCategory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ZLJ.EntityFrameworkCore.Seed
{
    public class WorkOrderBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;
        private readonly bool insertTestData;

        public WorkOrderBuilder(ZLJDbContext context, int tenantId, bool insertTestData = true)
        {
            _context = context;
            _tenantId = tenantId;
            this.insertTestData = insertTestData;
        }

        public void Create()
        {
            var cls = _context.Set<CategoryEntity>();
            if (insertTestData && !cls.IgnoreQueryFilters().Any())
            {
                cls.Add(new CategoryEntity
                {
                    Code = "00001",
                    CreationTime = DateTime.Now,
                    DisplayName = "普通",
                    IsDefault = true,
                    TenantId = _tenantId
                });
                cls.Add(new CategoryEntity
                {
                    Code = "00002",
                    CreationTime = DateTime.Now,
                    DisplayName = "测试",
                    TenantId = _tenantId,
                    Children = new CategoryEntity[] {
                        new CategoryEntity
                        {
                            Code = "00002.00001",
                            CreationTime = DateTime.Now,
                            DisplayName = "测试子类",
                            TenantId = _tenantId
                        }
                    }
                });
                _context.SaveChanges();
            }
        }
    }
}