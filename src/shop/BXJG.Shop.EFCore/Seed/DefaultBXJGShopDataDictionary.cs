using Abp.EntityFrameworkCore;
using BXJG.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Linq;
using Abp.Zero.EntityFrameworkCore;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Configuration;

namespace BXJG.Shop.Seed
{
    public class DefaultBXJGShopDataDictionary<TTenant, TRole, TUser, TSelf, TDataDictionary>
         where TTenant : AbpTenant<TUser>
        where TDataDictionary : GeneralTreeEntity<TDataDictionary>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<TDataDictionary> set;
        long? parentId;
        bool insertTestData;

        public DefaultBXJGShopDataDictionary(TSelf context, int tenantId, long? parentId = default, bool insertTestData = true)
        {
            _context = context;
            _tenantId = tenantId;
            this.parentId = parentId;
            set = context.Set<TDataDictionary>();
            this.insertTestData = insertTestData;
        }

        public void Create()
        {
            var pp = set.Any(c => c.ParentId == parentId && c.DisplayName == "商品品牌");
            if (!pp)
            {
                var last = set.Where(c => c.ParentId == parentId).OrderBy(c => c.Code).LastOrDefault();
                var lastIndex = 0;
                if (last != null)
                {
                    lastIndex = Convert.ToInt32(last.Code.Split('.').Last());
                }
                var p = new TDataDictionary
                {
                    Code = last == null ? "00001" : (lastIndex + 1).ToString().PadLeft(5, '0'),
                    CreationTime = DateTime.Now,
                    DisplayName = "商品品牌",
                    TenantId = _tenantId
                };
                if (insertTestData)
                {
                    p.Children = new List<TDataDictionary> {
                         new TDataDictionary{
                            Code = last==null?"00001.00001":  (lastIndex+1).ToString().PadLeft(5,'0')+".00001",
                            CreationTime = DateTime.Now,
                            DisplayName = "阿迪达斯1",
                            TenantId = _tenantId,
                         },
                         new TDataDictionary{
                            Code = last==null?"00001.00002":  (lastIndex+1).ToString().PadLeft(5,'0')+".00002",
                            CreationTime = DateTime.Now,
                            DisplayName = "耐克1",
                            TenantId = _tenantId,
                         }
                     };
                }
                set.Add(p);
                _context.SaveChanges();
                _context.Settings.Add(new Setting(this._tenantId, null, BXJGShopConsts.DataDictionayMigrationValuepinpai, p.Id.ToString()));
                _context.SaveChanges();
            }

            var zf = set.Any(c => c.ParentId == parentId && c.DisplayName == "支付方式");
            if (!zf)
            {
                var last = set.Where(c => c.ParentId == parentId).OrderBy(c => c.Code).LastOrDefault();
                var lastIndex = 0;
                if (last != null)
                {
                    lastIndex = Convert.ToInt32(last.Code.Split('.').Last());
                }
                var p = new TDataDictionary
                {
                    Code = last == null ? "00001" : (lastIndex + 1).ToString().PadLeft(5, '0'),
                    CreationTime = DateTime.Now,
                    DisplayName = "支付方式",
                    TenantId = _tenantId
                };
                if (insertTestData)
                {
                    p.Children = new List<TDataDictionary> {
                         new TDataDictionary{
                            Code = last==null?"00001.00001":  (lastIndex+1).ToString().PadLeft(5,'0')+".00001",
                            CreationTime = DateTime.Now,
                            DisplayName = "微信",
                            TenantId = _tenantId,
                         },
                         new TDataDictionary{
                            Code = last==null?"00001.00002":  (lastIndex+1).ToString().PadLeft(5,'0')+".00002",
                            CreationTime = DateTime.Now,
                            DisplayName = "支付宝",
                            TenantId = _tenantId,
                         },
                         new TDataDictionary{
                            Code = last==null?"00001.00003":  (lastIndex+1).ToString().PadLeft(5,'0')+".00003",
                            CreationTime = DateTime.Now,
                            DisplayName = "货到付款",
                            TenantId = _tenantId,
                         }
                     };
                }
                set.Add(p);
                _context.SaveChanges();

                _context.Settings.Add(new Setting(this._tenantId, null, BXJGShopConsts.DataDictionayMigrationValuezhifufangshi, p.Id.ToString()));
                _context.SaveChanges();
            }

            var psfs = set.Any(c => c.ParentId == parentId && c.DisplayName == "配送方式");
            if (!psfs)
            {
                var last = set.Where(c => c.ParentId == parentId).OrderBy(c => c.Code).LastOrDefault();
                var lastIndex = 0;
                if (last != null)
                {
                    lastIndex = Convert.ToInt32(last.Code.Split('.').Last());
                }
                var p = new TDataDictionary
                {
                    Code = last == null ? "00001" : (lastIndex + 1).ToString().PadLeft(5, '0'),
                    CreationTime = DateTime.Now,
                    DisplayName = "配送方式",
                    TenantId = _tenantId
                };
                if (insertTestData)
                {
                    p.Children = new List<TDataDictionary> {
                         new TDataDictionary{
                            Code = last==null?"00001.00001":  (lastIndex+1).ToString().PadLeft(5,'0')+".00001",
                            CreationTime = DateTime.Now,
                            DisplayName = "顺丰",
                            TenantId = _tenantId,
                         },
                         new TDataDictionary{
                            Code = last==null?"00001.00002":  (lastIndex+1).ToString().PadLeft(5,'0')+".00002",
                            CreationTime = DateTime.Now,
                            DisplayName = "中通",
                            TenantId = _tenantId,
                         },
                         new TDataDictionary{
                            Code = last==null?"00001.00003":  (lastIndex+1).ToString().PadLeft(5,'0')+".00003",
                            CreationTime = DateTime.Now,
                            DisplayName = "圆通",
                            TenantId = _tenantId,
                         }
                     };
                }
                set.Add(p);
                _context.SaveChanges();
                _context.Settings.Add(new Setting(this._tenantId, null, BXJGShopConsts.DataDictionayMigrationValuepeisongfangshi, p.Id.ToString()));
                _context.SaveChanges();
            }

           
        }
    }
}
