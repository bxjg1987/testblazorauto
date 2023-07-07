using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using ZLJ.Authorization;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.Customer;

namespace ZLJ.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly ZLJDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(ZLJDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            // Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }
            var wxry = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == "wxry");
            if (wxry == null)
            {
                wxry = _context.Roles.Add(new Role(_tenantId, "wxry", "维修人员") { IsStatic = true }).Entity;
                _context.SaveChanges();

                //_context.Permissions.Add(new RolePermissionSetting
                //{
                //    TenantId = _tenantId,
                //    Name = PermissionNames.emp,
                //    IsGranted = true,
                //    RoleId = adminRole.Id
                //});
            }

            var custAdmin = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == CustomerRole.CustomerAdminRole);
            if (custAdmin == null)
            {
                custAdmin = _context.Roles.Add(new Role(_tenantId, CustomerRole.CustomerAdminRole, "客户管理员") { IsStatic = true }).Entity;
                _context.SaveChanges();
            }
            // Grant all permissions to admin role
            #region 后台管理员
            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new ZLJAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name)
                            //&& !p.Name.StartsWith(PermissionNames.EmployeeApp)
                            && !p.Name.StartsWith(PermissionNames.Customer))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.SaveChanges();
            }
            #endregion

            #region 员工端
            grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == wxry.Id)
                .Select(p => p.Name)
                .ToList();

            permissions = PermissionFinder.GetAllPermissions(new ZLJAuthorizationProvider())
                                          .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                                                      !grantedPermissions.Contains(p.Name)
                                                      /* &&
                                                       p.Name.StartsWith(PermissionNames.EmployeeApp)*/
                                                      )
                                          .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                   permissions.Where(c => c.Name.StartsWith("EmployeeApp")).Select(permission => new RolePermissionSetting
                   {
                       TenantId = _tenantId,
                       Name = permission.Name,
                       IsGranted = true,
                       RoleId = wxry.Id
                   })
               );
                _context.SaveChanges();
            }
            #endregion
            #region 客户
            grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == custAdmin.Id)
                .Select(p => p.Name)
                .ToList();

            permissions = PermissionFinder.GetAllPermissions(new ZLJAuthorizationProvider())
                                          .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                                                      !grantedPermissions.Contains(p.Name) &&
                                                      p.Name.StartsWith(PermissionNames.Customer))
                                          .ToList();
            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                   permissions.Select(permission => new RolePermissionSetting
                   {
                       TenantId = _tenantId,
                       Name = permission.Name,
                       IsGranted = true,
                       RoleId = custAdmin.Id
                   })
               );
                _context.SaveChanges();
            }
            #endregion
            // Admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;
                adminUser.PhoneNumber = "17723896676";
                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }

            //维修人员
            var sdff = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == "sdff");
            if (sdff == null)
            {
                sdff = User.CreateTenantAdminUser(_tenantId, "admin@defaul98ttenant.com");
                sdff.UserName = "sdff";
                sdff.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                sdff.IsEmailConfirmed = true;
                sdff.IsActive = true;
                sdff.PhoneNumber = "13333333321";

                _context.Users.Add(sdff);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, sdff.Id, wxry.Id));
                _context.SaveChanges();
            }
        }
    }
}
