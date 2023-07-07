using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using ZLJ.Authorization.Users;
using System.Threading.Tasks;
using System.Linq;

namespace ZLJ.Authorization.Roles
{
    public class RoleManager : AbpRoleManager<Role, User>
    {
        public RoleManager(
            RoleStore store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<AbpRoleManager<Role, User>> logger,
            IPermissionManager permissionManager,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRoleManagementConfig roleManagementConfig,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
            : base(
                  store,
                  roleValidators,
                  keyNormalizer,
                  errors, logger,
                  permissionManager,
                  cacheManager,
                  unitOfWorkManager,
                  roleManagementConfig,
                organizationUnitRepository,
                organizationUnitRoleRepository)
        {
        }

        //public override Task SetGrantedPermissionsAsync(int roleId, IEnumerable<Permission> permissions)
        //{
        //   var p= permissions.SelectMany(c => c.GetDependencePermissions());
        //    return base.SetGrantedPermissionsAsync(roleId, permissions.Union(p).GroupBy(c => c.Name).SelectMany(c => c));
        //}
        public override Task SetGrantedPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            var p = permissions.SelectMany(c => c.GetDependencePermissions()).Distinct();
            return base.SetGrantedPermissionsAsync(role, permissions.Union(p));
        }
    }
}
