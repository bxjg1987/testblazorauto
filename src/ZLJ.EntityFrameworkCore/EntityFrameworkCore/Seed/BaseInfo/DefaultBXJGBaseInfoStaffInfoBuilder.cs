using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.File;
using ZLJ.BaseInfo.StaffInfo;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo.Post;
using ZLJ.Authorization.Roles;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.Seed.BaseInfo
{
    public class DefaultBXJGBaseInfoStaffInfoBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;
        private readonly DbSet<StaffInfoEntity> _items;

        public DefaultBXJGBaseInfoStaffInfoBuilder(ZLJDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
            _items = context.Set<StaffInfoEntity>();
        }

        public void Create()
        {
            if (_items.IgnoreQueryFilters().Where(c=>c.TenantId==_tenantId).Any())
                return;

            //var posts = _context.Set<PostEntity>().ToList();

            var ent1 = new StaffInfoEntity
            {
                TenantId = _tenantId,
                Name = "李白",
                UserName = "libai",
                Password = "12334444",
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true,
                EmailAddress = "xxx@xxx.com",
                IsActive = true,
                NormalizedEmailAddress = "XXX@XXX.COM",
                 Surname= "X",
                  NormalizedUserName="LIBAI",
                 
                Gender = BXJG.Common.Gender.Man,
                Birthday = System.DateTimeOffset.Parse("1995-12-15"),
                No = "KG0001",
                PhoneNumber = "13800138001",
                CurrentAddress = "重庆市渝中区",
                //Post = Posts.FirstOrDefault(x => x.DisplayName == "库管"),
            };

            ent1.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(ent1, "123qwe");


            _items.Add(ent1);


            var ent2 = new StaffInfoEntity
            {
                TenantId = _tenantId,
                Name = "李清照",
                UserName = "liqingzhao",
                Password = "12334444",
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true,
                EmailAddress = "xxx2@xxx.com",
                IsActive = true,
                NormalizedEmailAddress = "XXX2@XXX.COM",
                Surname = "X",
                NormalizedUserName = "LIQINGZHAO",
                Gender = BXJG.Common.Gender.Woman,
                Birthday = System.DateTimeOffset.Parse("1996-2-15"),
                No = "KG0002",
                PhoneNumber = "13800138002",
                CurrentAddress = "重庆市渝中区",
                //Post = Posts.FirstOrDefault(x => x.DisplayName == "库管"),
            };

            ent2.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(ent2, "123qwe");

            _items.Add(ent2);

            var ent3 = new StaffInfoEntity
            {
                TenantId = _tenantId,
                Name = "杜甫",
                UserName= "dufu",
                //Password = "12334444",
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true,
                EmailAddress = "xxx3@xxx.com",
                IsActive = true,
                NormalizedEmailAddress = "XXX3@XXX.COM",
                Surname = "X",
                NormalizedUserName = "DUFU",
                Gender = BXJG.Common.Gender.Man,
                Birthday = System.DateTimeOffset.Parse("1994-2-15"),
                No = "KG0003",
                PhoneNumber = "13800138003",
                CurrentAddress = "重庆市渝中区",
                //Post = Posts.FirstOrDefault(x => x.DisplayName == "维修人员"),
            };

            ent3.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(ent3, "123qwe");


            _items.Add(ent3);

            var ent4 = new StaffInfoEntity
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



            _context.SaveChanges();

            var sdfsdf = _context.Roles.IgnoreQueryFilters().Where(c => c.TenantId == _tenantId && c.Name == StaticRoleNames.Tenants.Admin).Single();
            _context.UserRoles.Add(new UserRole(_tenantId, ent1.Id, sdfsdf.Id));
        }
    }
}