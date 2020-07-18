using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Shop.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Seed
{
    public class DefaultBXJGShopItemCagtegoryBuilder<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        DbSet<BXJGShopDictionaryEntity> set;

        public DefaultBXJGShopItemCagtegoryBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            set = context.Set<BXJGShopDictionaryEntity>();
        }

        public void Create(bool insertTestData = true)
        {
            var spfl = set.Find(1L);
            if (spfl == null)
            {
                set.Add(new BXJGShopDictionaryEntity
                {
                    Code = "00001",
                    CreationTime = DateTime.Now,
                    DisplayName = "商品分类",
                    TenantId = _tenantId,
                    IsTree=true,
                    IsSysDefine=true
                });
                _context.SaveChanges();//必须保持下才能保证生产id=1
            }

            spfl = set.Find(2L);
            if (spfl == null)
            {
                set.Add(new BXJGShopDictionaryEntity
                {
                    Code = "00002",
                    CreationTime = DateTime.Now,
                    DisplayName = "品牌",
                    TenantId = _tenantId,
                    IsTree = true,
                    IsSysDefine = true
                });
                _context.SaveChanges();//必须保持下才能保证生产id
            }

            spfl = set.Find(3L);
            if (spfl == null)
            {
                set.Add(new BXJGShopDictionaryEntity
                {
                    Code = "00003",
                    CreationTime = DateTime.Now,
                    DisplayName = "支付方式",
                    TenantId = _tenantId,
                    IsTree = true,
                    IsSysDefine = true
                });
                _context.SaveChanges();//必须保持下才能保证生产id
            }

            spfl = set.Find(4L);
            if (spfl == null)
            {
                set.Add(new BXJGShopDictionaryEntity
                {
                    Code = "00004",
                    CreationTime = DateTime.Now,
                    DisplayName = "配送方式",
                    TenantId = _tenantId,
                    IsTree = true,
                    IsSysDefine = true
                });
                _context.SaveChanges();//必须保持下才能保证生产id
            }

            //后续可能添加更多项，而此时项目可能存在某些数据，而那些数据可能与我们希望的字典id冲突
            //因此先预设100个，这样后续用户添加的字典的id会从101开始
            spfl = set.Find(5L);
            if (spfl == null)
            {
                for (int i = 5; i < 101; i++)
                {
                    set.Add(new BXJGShopDictionaryEntity
                    {
                        Code = i.ToString().PadLeft(5, '0'),
                        CreationTime = DateTime.Now,
                        DisplayName = "预设",
                        TenantId = _tenantId,
                        IsTree = true,
                        IsSysDefine = true
                    });
                }
            }

            //演示数据
            if (insertTestData)
            {
                spfl = set.Find(101L);
                if (spfl == null)
                {
                    set.Add(new BXJGShopDictionaryEntity
                    {
                        Code = "00001.00001",
                        CreationTime = DateTime.Now,
                        DisplayName = "T恤",
                        TenantId = _tenantId,
                        ParentId=1,
                        IsTree=true
                    });
                    _context.SaveChanges();//必须保持下才能保证生产id
                }
                spfl = set.Find(102L);
                if (spfl == null)
                {
                    set.Add(new BXJGShopDictionaryEntity
                    {
                        Code = "00001.00002",
                        CreationTime = DateTime.Now,
                        DisplayName = "箱包",
                        TenantId = _tenantId,
                        ParentId = 1,
                        IsTree=true
                    });
                    _context.SaveChanges();//必须保持下才能保证生产id
                }
                spfl = set.Find(103L);
                if (spfl == null)
                {
                    set.Add(new BXJGShopDictionaryEntity
                    {
                        Code = "00002.00001",
                        CreationTime = DateTime.Now,
                        DisplayName = "阿迪",
                        TenantId = _tenantId,
                        ParentId = 2
                    });
                    _context.SaveChanges();//必须保持下才能保证生产id
                }
                spfl = set.Find(104L);
                if (spfl == null)
                {
                    set.Add(new BXJGShopDictionaryEntity
                    {
                        Code = "00002.00002",
                        CreationTime = DateTime.Now,
                        DisplayName = "香奈儿",
                        TenantId = _tenantId,
                        ParentId=2
                    });
                    _context.SaveChanges();//必须保持下才能保证生产id
                }

                spfl = set.Find(105L);
                if (spfl == null)
                {
                    set.Add(new BXJGShopDictionaryEntity
                    {
                        Code = "00003.00001",
                        CreationTime = DateTime.Now,
                        DisplayName = "微信",
                        TenantId = _tenantId,
                        ParentId = 3
                    });
                    _context.SaveChanges();//必须保持下才能保证生产id
                }
                spfl = set.Find(106L);
                if (spfl == null)
                {
                    set.Add(new BXJGShopDictionaryEntity
                    {
                        Code = "00003.00002",
                        CreationTime = DateTime.Now,
                        DisplayName = "支付宝",
                        TenantId = _tenantId,
                        ParentId = 3
                    });
                    _context.SaveChanges();//必须保持下才能保证生产id
                }

                spfl = set.Find(107L);
                if (spfl == null)
                {
                    set.Add(new BXJGShopDictionaryEntity
                    {
                        Code = "00004.00001",
                        CreationTime = DateTime.Now,
                        DisplayName = "顺丰",
                        TenantId = _tenantId,
                        ParentId = 4
                    });
                    _context.SaveChanges();//必须保持下才能保证生产id
                }
                spfl = set.Find(108L);
                if (spfl == null)
                {
                    set.Add(new BXJGShopDictionaryEntity
                    {
                        Code = "00004.00002",
                        CreationTime = DateTime.Now,
                        DisplayName = "中通",
                        TenantId = _tenantId,
                        ParentId = 4
                    });
                    _context.SaveChanges();//必须保持下才能保证生产id
                }
            }
        }
    }
}
