using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.Seed.BaseInfo
{
    public class DefaultBXJGBaseInfoDataDictionaryBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;
        private readonly DbSet<DataDictionaryEntity> _set;
        private readonly long? _parentId;

        public DefaultBXJGBaseInfoDataDictionaryBuilder(ZLJDbContext context, int tenantId, long? parentId = default)
        {
            _context = context;
            _tenantId = tenantId;
            _parentId = parentId;
            _set = context.Set<DataDictionaryEntity>();
        }

        public void Create()
        {
            var items = new List<Tuple<string, List<string>, string,string>>
            {
                new("设备品牌", new List<string> {"夏普", "施乐", "爱普生", "理光",},
                    ZLJ.Core.Share.ZLJConsts.DataDictionaryMigrationValuePrinterBrand,"pinpai"),
                new("客户类别", new List<string> { "供应商及客户","供应商", "客户",  },
                    ZLJ.Core.Share.ZLJConsts.DataDictionaryMigrationValueCustomerCategory,"kehuLeibie"),
                new("客户级别", new List<string> {"A级", "B级", "C级",},
                    ZLJ.Core.Share.ZLJConsts.DataDictionaryMigrationValueCustomerLevel,"kehuJibie"),
                new("岗位", new List<string> {"维修人员", "售后", "库管",},
                    ZLJ.Core.Share.ZLJConsts.DataDictionaryMigrationValuePost,"gangwei"),
            };

            foreach (var itemConfig in items)
            {
                var isExists = _set.IgnoreQueryFilters()
                    .Any(c => c.TenantId == _tenantId && c.ParentId == _parentId && c.DisplayName == itemConfig.Item1);
                if (isExists) continue;

                var last = _set.IgnoreQueryFilters().Where(c => c.TenantId == _tenantId && c.ParentId == _parentId)
                    .OrderBy(c => c.Code).LastOrDefault();
                var lastIndex = 0;
                if (last != null)
                {
                    lastIndex = Convert.ToInt32(last.Code.Split('.').Last());
                }

                var currentCode = last == null ? "00001" : (lastIndex + 1).ToString().PadLeft(5, '0');
                var item = new DataDictionaryEntity
                {
                    Code = currentCode,
                    DisplayName = itemConfig.Item1,
                    TenantId = _tenantId,
                    Children = new List<DataDictionaryEntity>(),
                    IsSysDefine = true, Name= itemConfig.Item4,
                };
                if (!itemConfig.Item2.IsNullOrEmpty())
                {
                    for (var i = 0; i < itemConfig.Item2.Count; i++)
                    {
                        item.Children.Add(new DataDictionaryEntity
                        {
                            Code = $"{currentCode}.{(i + 1).ToString().PadLeft(5, '0')}",
                            DisplayName = itemConfig.Item2[i],
                            TenantId = _tenantId,
                            IsSysDefine = true,
                        });
                    }
                }

                _set.Add(item);
                _context.SaveChanges();

                //if (!itemConfig.Item3.IsNullOrWhiteSpace())
                //{
                //    _context.Settings.Add(new Setting(_tenantId, null, itemConfig.Item3, item.Id.ToString()));
                //    _context.SaveChanges();
                //}
            }

        }
    }
}