using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Common;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLJ.BaseInfo.Administrative;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.Seed.BaseInfo
{
    public class DefaultBXJGBaseInfoAdministrativeBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;
        DbSet<AdministrativeEntity> items;

        public DefaultBXJGBaseInfoAdministrativeBuilder(ZLJDbContext context, int tenantId)
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
                DisplayName = "重庆市",
                Children = new List<AdministrativeEntity> {
                    new AdministrativeEntity
                    {
                        Code = "00001.00001",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "江北区",
                        //Children = new List<AdministrativeEntity> {
                        //    new AdministrativeEntity
                        //    {
                        //        Code = "00001.00001.00001",
                        //        Level = AdministrativeLevel.County,
                        //        TenantId = _tenantId,
                        //        DisplayName = "江北区"
                        //    },
                        //    new AdministrativeEntity
                        //    {
                        //        Code = "00001.00001.00002",
                        //        Level = AdministrativeLevel.County,
                        //        TenantId = _tenantId,
                        //        DisplayName = "萧山区"
                        //    }
                        //}
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00002",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "渝北区",
                        //Children = new List<AdministrativeEntity> {
                        //    new AdministrativeEntity
                        //    {
                        //        Code = "00001.00002.00001",
                        //        Level = AdministrativeLevel.County,
                        //        TenantId = _tenantId,
                        //        DisplayName = "北仑区"
                        //    },
                        //    new AdministrativeEntity
                        //    {
                        //        Code = "00001.00002.00002",
                        //        Level =AdministrativeLevel.County,
                        //        TenantId = _tenantId,
                        //        DisplayName = "江北区"
                        //    },
                        //    new AdministrativeEntity
                        //    {
                        //        Code = "00001.00002.00003",
                        //        Level = AdministrativeLevel.County,
                        //        TenantId = _tenantId,
                        //        DisplayName = "鄞州区"
                        //    }
                        //}
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00003",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "沙坪坝区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00004",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "九龙坡区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00005",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "南岸区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00006",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "巴南区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00007",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "渝中区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00008",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "大渡口区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00009",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "永川区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00010",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "万州区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00011",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "咸阳县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00012",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "北碚区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00013",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "涪陵区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00014",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "秀山区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00015",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "石柱县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00016",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "忠县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00017",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "合川区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00018",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "开州区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00019",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "长寿区"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00020",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "荣昌县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00021",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "梁平县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00022",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "云阳县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00023",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "潼南"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00024",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "江津"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00025",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "彭水"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00026",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "綦江"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00027",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "璧山"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00028",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "黔江"
                    },

                    new AdministrativeEntity
                    {
                        Code = "00001.00029",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "大足县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00030",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "巫山县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00031",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "巫溪县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00032",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "垫江县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00033",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "丰都县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00034",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "武隆县"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00035",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "万盛"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00036",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "铜梁"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00037",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "南川"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00038",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "奉节"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00039",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "双桥"
                    },
                    new AdministrativeEntity
                    {
                        Code = "00001.00040",
                        Level = AdministrativeLevel.County,
                        TenantId = _tenantId,
                        DisplayName = "城口"
                    }
                }
            };
            items.Add(zj);

            _context.SaveChanges();//先保存一次，上面的自增id才固定
        }
    }
}
