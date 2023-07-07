using BXJG.Utils.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLJ.EntityFrameworkCore.Seed
{
    public class DefaultDataDictionaryBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;
        public DefaultDataDictionaryBuilder(ZLJDbContext context, int tenantId)
        {
            _context = context; _tenantId = tenantId;
        }

        public void Create()
        {
            //未考虑多租户情况

            GeneralTreeEntity shebeiquyu = null;
            if (!_context.BXJGGeneralTreeEntities.IgnoreQueryFilters().Any(c => c.TenantId==_tenantId&& c.Code=="00001"))
            {
                shebeiquyu = _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
                {
                    Code = "00001",
                    DisplayName = "设备分布区域",
                    TenantId = _tenantId
                }).Entity;
            }

            GeneralTreeEntity xueli = null;
            if (!_context.BXJGGeneralTreeEntities.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId && c.Code == "00002"))
            {
                xueli = _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
                {
                    Code = "00002",
                    DisplayName = "学历",
                    IsSysDefine = true,
                    TenantId = _tenantId
                }).Entity;
            }

            GeneralTreeEntity minzu = null;
            if (!_context.BXJGGeneralTreeEntities.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId && c.Code == "00003"))
            {
                minzu = _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
                {
                    Code = "00003",
                    DisplayName = "民族",
                    IsSysDefine = true,
                    TenantId = _tenantId
                }).Entity;
            }

            // //预留固定id给固定的节点
            // for (int i = 4; i < 50; i++)
            // {
            //     var ccc = "000" + i.ToString().PadLeft(2, '0'); ;
            //
            //     if (!_context.BXJGGeneralTreeEntities.IgnoreQueryFilters().Any(c => c.TenantId == _tenantId && c.Code == ccc))
            //     {
            //         _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
            //         {
            //             Code = ccc,
            //             DisplayName = "预留" + i,
            //             IsSysDefine = true,
            //             TenantId = _tenantId
            //         });
            //     }
            // }


            _context.SaveChanges();//先保存一次，上面的自增id才固定

            if (shebeiquyu != null)
            {
                _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
                {
                    Code = "00001.00001",
                    DisplayName = "渝北",
                    IsSysDefine = true,
                    TenantId = _tenantId,
                    ParentId = shebeiquyu.Id
                });
                _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
                {
                    Code = "00001.00002",
                    DisplayName = "九龙坡",
                    IsSysDefine = true,
                    TenantId = _tenantId,
                    ParentId = shebeiquyu.Id
                });
            }

            if (xueli != null)
            {
                _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
                {
                    Code = "00002.00001",
                    DisplayName = "博士及以上",
                    IsSysDefine = true,
                    TenantId = _tenantId,
                    ParentId = xueli.Id
                });
                _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
                {
                    Code = "00002.00002",
                    DisplayName = "硕士",
                    IsSysDefine = true,
                    TenantId = _tenantId,
                    ParentId = xueli.Id
                });
            }

            if (minzu != null)
            {
                _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
                {
                    Code = "00003.00001",
                    DisplayName = "汉族",
                    IsSysDefine = true,
                    TenantId = _tenantId,
                    ParentId = minzu.Id
                });
                _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
                {
                    Code = "00003.00002",
                    DisplayName = "维吾尔族",
                    IsSysDefine = true,
                    TenantId = _tenantId,
                    ParentId = minzu.Id
                });
            }
            _context.SaveChanges();




            ////岗位
            //GeneralTreeEntity gangwei = null;
            //if (!_context.BXJGGeneralTreeEntities.Any(c => c.Id == 4))
            //{
            //    gangwei= _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
            //    {
            //        Code = "00004",
            //        DisplayName = "岗位",
            //        IsSysDefine = true,
            //        TenantId =_tenantId
            //    }).Entity;
            //}
            ////设备分类
            //if (!_context.BXJGGeneralTreeEntities.Any(c => c.Id == 5))
            //{
            //    _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
            //    {
            //        Code = "00005",
            //        DisplayName = "设备分类",
            //        IsSysDefine = true,
            //        TenantId =_tenantId,
            //        IsTree = true
            //    });
            //}
            ////单位
            //if (!_context.BXJGGeneralTreeEntities.Any(c => c.Id == 6))
            //{
            //    _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
            //    {
            //        Code = "00006",
            //        DisplayName = "单位",
            //        IsSysDefine = true,
            //        TenantId =_tenantId
            //    });
            //}
            ////品牌
            //if (!_context.BXJGGeneralTreeEntities.Any(c => c.Id == 6))
            //{
            //    _context.BXJGGeneralTreeEntities.Add(new GeneralTreeEntity
            //    {
            //        Code = "00007",
            //        DisplayName = "品牌",
            //        IsSysDefine = true,
            //        TenantId =_tenantId
            //    });
            //}
        }
    }
}
