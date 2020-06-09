using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.CMS.Ad;
using BXJG.CMS.Article;
using BXJG.CMS.Column;
using BXJG.Common;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.CMS.EFCore.Seed
{
    public class DefaultBXJGCMSBuilder<TTenant, TRole, TUser, TSelf, TDataDictionary>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>, new()
        where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        private readonly TSelf _context;
        private readonly int _tenantId;
        public DefaultBXJGCMSBuilder(TSelf context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create(bool insertTestData = true)
        {
            #region 栏目
            var columnSet = _context.Set<ColumnEntity<TDataDictionary>>();
            columnSet.Add(new ColumnEntity<TDataDictionary>
            {
                Code = "00001",
                CreationTime = new DateTime(2017, 3, 1),
                IsSysDefine = true,
                TenantId = this._tenantId,
                ColumnType = ColumnType.SinglePage,
                ContentTypeId = 30,//这里应该由调用方指定，而不是设定死
                Description = "关于公司情况的介绍",
                SeoTitle = "SeoTitle",
                SeoKeyword = "SeoKeyword",
                SeoDescription = "SeoDescription",
                DisplayName = "关于我们",
                ExtensionData = "{\"sss\":33}",
                Icon = "ssssicon",
                ListTemplate = "newslist",
                DetailTemplate = "newsdetail"
            });
            _context.SaveChanges();
            #endregion

            #region 系统预定义文章
            var articleSet = _context.Set<ArticleEntity<TDataDictionary>>();
            articleSet.Add(new ArticleEntity<TDataDictionary>
            {
                Content = "硬核科技是一家........",
                CreationTime = new DateTime(2017, 3, 1),
                IsSysDefine = true,
                TenantId = this._tenantId,
                Title = "公司简介",
                ColumnId = 1,
                SeoDescription = "SeoDescription",
                SeoKeyword = "SeoKeyword",
                SeoTitle = "SeoTitle",
                Summary = "Summary",
                Published = true
            });
            _context.SaveChanges();
            #endregion



            if (!insertTestData)
                return;

            var adPositionSet = _context.Set<AdPositionEntity>();
            if (!adPositionSet.Any())
            {
                adPositionSet.Add(new AdPositionEntity
                {
                    DisplayName = "首页轮播",
                    CreationTime = new DateTime(2020, 5, 24),
                    Height = 200,
                    Width = 0,
                    TenantId = _tenantId
                });
                _context.SaveChanges();
                adPositionSet.Add(new AdPositionEntity
                {
                    DisplayName = "测试广告位2",
                    CreationTime = new DateTime(2020, 3, 17),
                    Height = 300,
                    Width = 300,
                    TenantId = _tenantId
                });
                _context.SaveChanges();
                adPositionSet.Add(new AdPositionEntity
                {
                    DisplayName = "测试广告位3",
                    CreationTime = new DateTime(2020, 3, 17),
                    Height = 300,
                    Width = 300,
                    TenantId = _tenantId
                });
                _context.SaveChanges();
            }

            var adControlSet = _context.Set<AdControlEntity>();
            if (!adControlSet.Any())
            {
                adControlSet.Add(new AdControlEntity
                {
                    AdControlType = AdControlType.Rotation,
                    CreationTime = new DateTime(2018, 4, 21),
                    ExtensionData = @"{ ""sudu"":47  }",
                    TenantId = _tenantId
                });
                _context.SaveChanges();
                adControlSet.Add(new AdControlEntity
                {
                    AdControlType = AdControlType.Image,
                    CreationTime = new DateTime(2018, 6, 3),
                    ExtensionData = null,// @"{ ""sudu"":47  }",
                    TenantId = _tenantId
                });
                _context.SaveChanges();
                adControlSet.Add(new AdControlEntity
                {
                    AdControlType = AdControlType.Html,
                    CreationTime = new DateTime(2019, 11, 12),
                    ExtensionData = null,// @"{ ""sudu"":47  }",
                    TenantId = _tenantId
                });

                _context.SaveChanges();
            }

            var adSet = _context.Set<AdEntity>();
            if (!adSet.Any())
            {
                adSet.Add(new AdEntity
                {
                    AdType = AdType.Image,
                    Content = "upload/ad1.jpg",
                    CreationTime = new DateTime(2019, 6, 19),
                    TenantId = _tenantId,
                    Url = "http://www.baidu.com",
                    Title = "测试广告1"
                });
                _context.SaveChanges();
                adSet.Add(new AdEntity
                {
                    AdType = AdType.Image,
                    Content = "upload/ad2.jpg",
                    CreationTime = new DateTime(2019, 6, 19),
                    TenantId = _tenantId,
                    Url = "http://www.baidu.com",
                    Title = "测试广告2"
                });
                _context.SaveChanges();
                adSet.Add(new AdEntity
                {
                    AdType = AdType.Image,
                    Content = "upload/ad3.jpg",
                    CreationTime = new DateTime(2019, 6, 19),
                    TenantId = _tenantId,
                    Url = "http://www.baidu.com",
                    Title = "测试广告3"
                });
                _context.SaveChanges();
                adSet.Add(new AdEntity
                {
                    AdType = AdType.Html,
                    Content = "<h3>这是一段html广告</h3>",
                    CreationTime = new DateTime(2019, 2, 3),
                    TenantId = _tenantId,
                    Url = "http://www.qq.com",
                    Title = "测试广告4"
                });
                _context.SaveChanges();
            }

            var adRecordSet = _context.Set<AdRecordEntity>();
            if (!adRecordSet.Any())
            {
                _context.Add(new AdRecordEntity
                {
                    AdControlId = 1,
                    AdId = 1,
                    AdPositionId = 1,
                    CreationTime = new DateTime(2020, 5, 22),
                    Published = true,
                    TenantId = this._tenantId,
                    SortIndex = 1
                });
                _context.SaveChanges();
                _context.Add(new AdRecordEntity
                {
                    AdControlId = 1,
                    AdId = 2,
                    AdPositionId = 1,
                    CreationTime = new DateTime(2020, 5, 20),
                    Published = true,
                    TenantId = this._tenantId,
                    SortIndex = 0
                });
                _context.SaveChanges();
                _context.Add(new AdRecordEntity
                {
                    AdControlId = 2,
                    AdId = 3,
                    AdPositionId = 2,
                    CreationTime = new DateTime(2020, 5, 20),
                    Published = true,
                    PublishStartTime = new DateTime(2020, 3, 11),
                    TenantId = this._tenantId,
                    SortIndex = 0
                });
                _context.SaveChanges();
                _context.Add(new AdRecordEntity
                {
                    AdControlId = 3,
                    AdId = 4,
                    AdPositionId = 3,
                    CreationTime = new DateTime(2020, 5, 20),
                    Published = true,
                    PublishEndTime = new DateTime(2030, 3, 11),
                    TenantId = this._tenantId,
                    SortIndex = 0
                });
                _context.SaveChanges();
            }
        }
    }
}
