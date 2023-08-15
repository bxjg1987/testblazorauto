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
using ZLJ.App.Admin.Authorization.Permissions;
using ZLJ.App.Common.Authorization;
using ZLJ.BaseInfo.StaffInfo;
using System.Collections.Generic;
using System.Net.Mail;

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
            #region 角色
            // Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }
            //var wxry = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == "wxry");
            //if (wxry == null)
            //{
            //    wxry = _context.Roles.Add(new Role(_tenantId, "wxry", "维修人员") { IsStatic = true }).Entity;
            //    _context.SaveChanges();
            //}

            var custAdmin = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == CustomerRole.CustomerAdminRole);
            if (custAdmin == null)
            {
                custAdmin = _context.Roles.Add(new Role(_tenantId, CustomerRole.CustomerAdminRole, "客户管理员") { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            #endregion

            #region 授权
            // Grant all permissions to admin role
            #region 后台管理员
            //获取已给管理员分配的权限
            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();
            //获取所有权限定义中，没有授权给管理员角色的项
            var permissions = PermissionFinder
                //abp默认是不引用application层的，由于我们的权限定义从core移动到application层了，所以这里直接引用，注意efcore层的模块不会依赖application层的模块，仅仅是为了这里能访问权限定义
                .GetAllPermissions(new ZLJAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
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
            #region 客户
            grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == custAdmin.Id)
                .Select(p => p.Name)
                .ToList();

            permissions = PermissionFinder.GetAllPermissions(new CustAppAuthorizationProvider())
                                          .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                                                      !grantedPermissions.Contains(p.Name))
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

            #endregion

            #region 用户
            // Admin user

            //超级管理员可以不属于任何部门
            ////员工和部门是多对多？
            //var dept = _context.OrganizationUnitEntities.IgnoreQueryFilters().First(u => u.TenantId == _tenantId);

            var adminUser = _context.BXJGBaseInfoStaffInfo.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = new StaffInfoEntity {
                    TenantId = _tenantId,
                    UserName = AbpUserBase.AdminUserName,
                    Name = "理员",
                    Surname = "管",
                    EmailAddress = "admin@defaul98ttenant.com",
                    Roles = new List<UserRole>()
                };
                adminUser.SetNormalizedNames();
                adminUser.Password = new PasswordHasher<StaffInfoEntity>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;
                adminUser.PhoneNumber = "17723896676";

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                //_context.SaveChanges();

                ////部门设置，超级管理员可以不属于任何部门
                //_context.UserOrganizationUnits.Add(new UserOrganizationUnit(_tenantId, adminUser.Id, dept.Id));
                _context.SaveChanges();
            }

            var cust = _context.BXJGBaseInfoAssociatedCompany.IgnoreQueryFilters().First(u => u.TenantId == _tenantId);
            //管理员可以不设置部门
            //var custDept = _context.CustomerOUEntities.IgnoreQueryFilters().First(u => u.TenantId == _tenantId&& cust.Id==u.CustomerId);
            //客户
            var sdff = _context.CustomerStaffInfos.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == "libai");
            if (sdff == null)
            {
                sdff = new CustomerStaffInfoEntity
                {
                    TenantId = _tenantId,
                    UserName = "libai",
                    Name = "白",
                    Surname = "李",
                    EmailAddress = "admin@defaul98ttenant.com",
                    Roles = new List<UserRole>()
                };

                sdff.SetNormalizedNames();

                sdff.Password = new PasswordHasher<CustomerStaffInfoEntity>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(sdff, "123qwe");
                sdff.IsActive = true;
                sdff.PhoneNumber = "13333333321";
                sdff.CustomerId = cust.Id;

                _context.Users.Add(sdff);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, sdff.Id, custAdmin.Id));
                ////部门设置 管理员可以不设置部门
                //_context.UserOrganizationUnits.Add(new UserOrganizationUnit(_tenantId, custAdmin.Id, custDept.Id));
                _context.SaveChanges();
            }
            #endregion
        }
    }
}
