using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZLJ.BaseInfo.AssociatedCompany;
using BXJG.Utils.GeneralTree;
using ZLJ.Customer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ZLJ.Authorization.Users;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.Seed.BaseInfo
{
    public class DefaultBXJGBaseInfoAssociatedCompanyBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;
        readonly DbSet<AssociatedCompanyEntity> _items;

        public DefaultBXJGBaseInfoAssociatedCompanyBuilder(ZLJDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            _items = context.Set<AssociatedCompanyEntity>();
        }

        public void Create()
        {
            if (_items.Any())
                return;

            var levels = _context.Set<GeneralTreeEntity>()
                .Include(x => x.Children)
                .Where(x => x.IsSysDefine && x.DisplayName == "客户级别")
                .SelectMany(x => x.Children)
                .ToList();
            var categorys = _context.Set<GeneralTreeEntity>()
                .Include(x => x.Children)
                .Where(x => x.IsSysDefine && x.DisplayName == "客户类别")
                .SelectMany(x => x.Children)
                .ToList();
            _items.Add(new AssociatedCompanyEntity
            {
                TenantId = _tenantId,
                Name = "重庆市万州区新东方教育培训学校有限公司",
                LinkMan = "赵婷",
                LinkPhone = "02389867386",
                TaxNo = "SH202202389867386",
                Address = "重庆市万州区太白路94号禾森商厦第五层（商场1）、（商场2）",
                Level = levels.FirstOrDefault(x => x.DisplayName == "A级"),
                Category = categorys.FirstOrDefault(x => x.DisplayName == "供应商及客户"),
            });
            _items.Add(new AssociatedCompanyEntity
            {
                TenantId = _tenantId,
                Name = "重庆澳腾汽车部件有限责任公司",
                LinkMan = "陈静",
                LinkPhone = "15281046652",
                TaxNo = "SH202215281046652",
                Address = "重庆市渝北区玉峰山镇石桐三路15号",
                Level = levels.FirstOrDefault(x => x.DisplayName == "A级"),
                Category = categorys.FirstOrDefault(x => x.DisplayName == "供应商及客户"),
            });
            _items.Add(new AssociatedCompanyEntity
            {
                TenantId = _tenantId,
                Name = "重庆寓客寓家公寓管理有限公司",
                LinkMan = "张",
                LinkPhone = "17709096671",
                TaxNo = "SH202217709096671",
                Address = "重庆市渝中区邹容路131号 7-11#",
                Level = levels.FirstOrDefault(x => x.DisplayName == "B级"),
                Category = categorys.FirstOrDefault(x => x.DisplayName == "供应商"),
            });

            var zhkh = new AssociatedCompanyEntity
            {
                TenantId = _tenantId,
                Name = "重庆冠群装饰设计工程有限公司",
                LinkMan = "胡",
                LinkPhone = "15803695026",
                TaxNo = "SH202215803695026",
                Address = "渝北区中渝都会首站3栋19-8",
                Level = levels.FirstOrDefault(x => x.DisplayName == "C级"),
                Category = categorys.FirstOrDefault(x => x.DisplayName == "客户"),
                
            };
            _items.Add(zhkh);
            _context.SaveChanges();
            var gl15803695026 = new CustomerStaffInfoEntity
            {
                TenantId = _tenantId,
                Name = "胡",
                UserName = "15803695026",
                //Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe"),
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true,
                EmailAddress = "xxx4@15803695026.com",
                IsActive = true,
                NormalizedEmailAddress = "XXX4@15803695026.COM",
                Surname = "X",
                NormalizedUserName = "15803695026",
                Gender = BXJG.Common.Gender.Man,
                Birthday = System.DateTimeOffset.Parse("1993-3-24"),
                //No = "KG0004",
                PhoneNumber = "15803695026",
                EquipmentPwd = "123qwe",
                Pinyin = "H"
                //CurrentAddress = "重庆市渝中区",
                //Post = Posts.FirstOrDefault(x => x.DisplayName == "售后"),
            };
            gl15803695026.CustomerId = zhkh.Id;
            gl15803695026.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(gl15803695026, "123qwe");
            zhkh.Staffs = new System.Collections.Generic.List<Customer.CustomerStaffInfoEntity> { gl15803695026 };

            /*
             *  var ent4 = new StaffInfoEntity
            {
                TenantId = _tenantId,
                Name = "孟浩然",
                UserName = "menghaoran",
                //Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe"),
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true,
                EmailAddress = "xxx4@xxx.com",
                IsActive = true,
                NormalizedEmailAddress = "XXX4@XXX.COM",
                Surname = "X",
                NormalizedUserName = "MENGHAORAN",
                Gender = BXJG.Common.Gender.Man,
                Birthday = System.DateTimeOffset.Parse("1993-3-24"),
                No = "KG0004",
                PhoneNumber = "13800138004",
                CurrentAddress = "重庆市渝中区",
                //Post = Posts.FirstOrDefault(x => x.DisplayName == "售后"),
            };
            ent4.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(ent4, "123qwe");
            _items.Add(ent4);
             */
            _context.SaveChanges();
        }
    }
}