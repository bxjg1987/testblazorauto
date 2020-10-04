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
    /// <summary>
    /// 为商城模块插入字典数据
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TSelf"></typeparam>
    public class DefaultDataDictionary<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<GeneralTreeEntity> set;
        long? parentId;

        public DefaultDataDictionary(TSelf context, int tenantId, long? parentId = default)
        {
            _context = context;
            _tenantId = tenantId;
            this.parentId = parentId;
            set = context.Set<GeneralTreeEntity>();
        }

        public void Create(bool insertTestData = true)
        {
            var pp = set.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId && c.ParentId == parentId && c.DisplayName == "商品品牌");
            if (!pp)
            {
                var last = set.Where(c => c.ParentId == parentId).OrderBy(c => c.Code).LastOrDefault();
                var lastIndex = 0;
                if (last != null)
                {
                    lastIndex = Convert.ToInt32(last.Code.Split('.').Last());
                }
                var p = new GeneralTreeEntity
                {
                    Code = last == null ? "00001" : (lastIndex + 1).ToString().PadLeft(5, '0'),
                    CreationTime = DateTime.Now,
                    DisplayName = "商品品牌",
                    TenantId = _tenantId
                };
                if (insertTestData)
                {
                    p.Children = new List<GeneralTreeEntity>{
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00001":  (lastIndex+1).ToString().PadLeft(5,'0')+".00001",
                            CreationTime = DateTime.Now,
                            DisplayName = "阿迪达斯1",
                            TenantId = _tenantId,
                         },
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00002":  (lastIndex+1).ToString().PadLeft(5,'0')+".00002",
                            CreationTime = DateTime.Now,
                            DisplayName = "耐克1",
                            TenantId = _tenantId,
                         }
                     };
                }
                set.Add(p);
                _context.SaveChanges();
                _context.Settings.Add(new Setting(this._tenantId, null, CoreConsts.DataDictionayMigrationValuepinpai, p.Id.ToString()));
                _context.SaveChanges();
            }

            var dw = set.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId && c.ParentId == parentId && c.DisplayName == "商品单位");
            if (!dw)
            {
                var last = set.Where(c => c.ParentId == parentId).OrderBy(c => c.Code).LastOrDefault();
                var lastIndex = 0;
                if (last != null)
                {
                    lastIndex = Convert.ToInt32(last.Code.Split('.').Last());
                }
                var p = new GeneralTreeEntity
                {
                    Code = last == null ? "00001" : (lastIndex + 1).ToString().PadLeft(5, '0'),
                    CreationTime = DateTime.Now,
                    DisplayName = "商品单位",
                    TenantId = _tenantId
                };
                if (insertTestData)
                {
                    p.Children = new List<GeneralTreeEntity> {
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00001":  (lastIndex+1).ToString().PadLeft(5,'0')+".00001",
                            CreationTime = DateTime.Now,
                            DisplayName = "个",
                            TenantId = _tenantId,
                         },
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00002":  (lastIndex+1).ToString().PadLeft(5,'0')+".00002",
                            CreationTime = DateTime.Now,
                            DisplayName = "把",
                            TenantId = _tenantId,
                         }
                     };
                }
                set.Add(p);
                _context.SaveChanges();
                _context.Settings.Add(new Setting(this._tenantId, null, CoreConsts.DataDictionayMigrationValuedanwei, p.Id.ToString()));
                _context.SaveChanges();
            }

            var zf = set.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId && c.ParentId == parentId && c.DisplayName == "支付方式");
            if (!zf)
            {
                var last = set.Where(c => c.ParentId == parentId).OrderBy(c => c.Code).LastOrDefault();
                var lastIndex = 0;
                if (last != null)
                {
                    lastIndex = Convert.ToInt32(last.Code.Split('.').Last());
                }
                var p = new GeneralTreeEntity
                {
                    Code = last == null ? "00001" : (lastIndex + 1).ToString().PadLeft(5, '0'),
                    CreationTime = DateTime.Now,
                    DisplayName = "支付方式",
                    TenantId = _tenantId
                };
                if (insertTestData)
                {
                    p.Children = new List<GeneralTreeEntity> {
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00001":  (lastIndex+1).ToString().PadLeft(5,'0')+".00001",
                            CreationTime = DateTime.Now,
                            DisplayName = "微信",
                            TenantId = _tenantId,
                         },
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00002":  (lastIndex+1).ToString().PadLeft(5,'0')+".00002",
                            CreationTime = DateTime.Now,
                            DisplayName = "支付宝",
                            TenantId = _tenantId,
                         },
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00003":  (lastIndex+1).ToString().PadLeft(5,'0')+".00003",
                            CreationTime = DateTime.Now,
                            DisplayName = "货到付款",
                            TenantId = _tenantId,
                         }
                     };
                }
                set.Add(p);
                _context.SaveChanges();

                _context.Settings.Add(new Setting(this._tenantId, null, CoreConsts.DataDictionayMigrationValuezhifufangshi, p.Id.ToString()));
                _context.SaveChanges();
            }

            var psfs = set.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId && c.ParentId == parentId && c.DisplayName == "配送方式");
            if (!psfs)
            {
                var last = set.Where(c => c.ParentId == parentId).OrderBy(c => c.Code).LastOrDefault();
                var lastIndex = 0;
                if (last != null)
                {
                    lastIndex = Convert.ToInt32(last.Code.Split('.').Last());
                }
                var p = new GeneralTreeEntity
                {
                    Code = last == null ? "00001" : (lastIndex + 1).ToString().PadLeft(5, '0'),
                    CreationTime = DateTime.Now,
                    DisplayName = "配送方式",
                    TenantId = _tenantId
                };
                if (insertTestData)
                {
                    p.Children = new List<GeneralTreeEntity> {
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00001":  (lastIndex+1).ToString().PadLeft(5,'0')+".00001",
                            CreationTime = DateTime.Now,
                            DisplayName = "顺丰",
                            TenantId = _tenantId,
                         },
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00002":  (lastIndex+1).ToString().PadLeft(5,'0')+".00002",
                            CreationTime = DateTime.Now,
                            DisplayName = "中通",
                            TenantId = _tenantId,
                         },
                         new GeneralTreeEntity{
                            Code = last==null?"00001.00003":  (lastIndex+1).ToString().PadLeft(5,'0')+".00003",
                            CreationTime = DateTime.Now,
                            DisplayName = "圆通",
                            TenantId = _tenantId,
                         }
                     };
                }
                set.Add(p);
                _context.SaveChanges();
                _context.Settings.Add(new Setting(this._tenantId, null, CoreConsts.DataDictionayMigrationValuepeisongfangshi, p.Id.ToString()));
                _context.SaveChanges();
            }
        }
    }
}
