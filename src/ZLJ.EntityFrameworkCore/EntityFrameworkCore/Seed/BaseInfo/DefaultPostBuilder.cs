using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using ZLJ.Core.Authorization;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using Abp.Zero.EntityFrameworkCore;
using ZLJ.Core.BaseInfo.Post;

namespace ZLJ.EntityFrameworkCore.EntityFrameworkCore.Seed.BaseInfo
{
    public class DefaultPostBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;

        public DefaultPostBuilder(ZLJDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var adminRole = _context.Set<PostEntity>().IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == "销售部");
            if (adminRole == null)
            {
                _context.Set<PostEntity>().Add(new PostEntity(_tenantId, "xiaoshou", "销售员") { IsStatic = false });
                _context.SaveChanges();
                _context.Set<PostEntity>().Add(new PostEntity(_tenantId, "caigou", "采购员") { IsStatic = false });
                _context.SaveChanges();
                _context.Set<PostEntity>().Add(new PostEntity(_tenantId, "kuguan", "库管") { IsStatic = false });
                _context.SaveChanges();
                _context.Set<PostEntity>().Add(new PostEntity(_tenantId, "caiwu", "财务") { IsStatic = false });
                _context.SaveChanges();
                _context.Set<PostEntity>().Add(new PostEntity(_tenantId, "jishu", "技术员") { IsStatic = false });
                _context.SaveChanges();
                _context.Set<PostEntity>().Add(new PostEntity(_tenantId, "wenyuan", "文员") { IsStatic = false });
                _context.SaveChanges();
                _context.Set<PostEntity>().Add(new PostEntity(_tenantId, "zongjingli", "总经理") { IsStatic = false });
                _context.SaveChanges();
            }
        }
    }
}
