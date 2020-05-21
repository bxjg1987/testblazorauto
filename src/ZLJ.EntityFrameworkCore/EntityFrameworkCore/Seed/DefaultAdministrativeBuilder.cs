using BXJG.Common;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLJ.Administrative;

namespace ZLJ.EntityFrameworkCore.Seed
{
    public class DefaultAdministrativeBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;
        DbSet<AdministrativeEntity> items;

        public DefaultAdministrativeBuilder(ZLJDbContext context, int tenantId)
        {
            _context = context; _tenantId = tenantId;
            items = context.Set<AdministrativeEntity>();
        }

        public void Create()
        {
            if (items.Any())
                return;

            var zj = new AdministrativeEntity
            {
                Code = "00001",
                Level =  AdministrativeLevel.Province,
                TenantId = _tenantId,
                DisplayName = "浙江省",
                Children = new List<AdministrativeEntity> {
                    new AdministrativeEntity
                    {
                        Code = "00001.00001",
                        Level = AdministrativeLevel.City,
                        TenantId = _tenantId,
                        DisplayName = "杭州",
                        Children = new List<AdministrativeEntity> {
                            new AdministrativeEntity
                            {
                                Code = "00001.00001.00001",
                                Level = AdministrativeLevel.County,
                                TenantId = _tenantId,
                                DisplayName = "江北区"
                            },
                            new AdministrativeEntity
                            {
                                Code = "00001.00001.00002",
                                Level = AdministrativeLevel.County,
                                TenantId = _tenantId,
                                DisplayName = "萧山区"
                            }
                        }
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00002",
                        Level = AdministrativeLevel.City,
                        TenantId = _tenantId,
                        DisplayName = "宁波",
                        Children = new List<AdministrativeEntity> {
                            new AdministrativeEntity
                            {
                                Code = "00001.00002.00001",
                                Level = AdministrativeLevel.County,
                                TenantId = _tenantId,
                                DisplayName = "北仑区"
                            },
                            new AdministrativeEntity
                            {
                                Code = "00001.00002.00002",
                                Level =AdministrativeLevel.County,
                                TenantId = _tenantId,
                                DisplayName = "江北区"
                            },
                            new AdministrativeEntity
                            {
                                Code = "00001.00002.00003",
                                Level = AdministrativeLevel.County,
                                TenantId = _tenantId,
                                DisplayName = "鄞州区"
                            }
                        }
                    }
                }
            };
            items.Add(zj);

            _context.SaveChanges();//先保存一次，上面的自增id才固定
        }
    }
}
