using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using ZLJ.Core.Authorization.Users;
using System.Threading.Tasks;
using System.Linq;
using BXJG.Utils.Extensions;
using System.Collections.Immutable;

namespace ZLJ.Core.Authorization.Roles
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

        //上面的方法内部会调用下面的方法。
        public override Task SetGrantedPermissionsAsync(Role role, IEnumerable<Permission> permissions)
        {
            var p = permissions.SelectMany(c => c.GetDependencePermissions()).Distinct();
            var list = permissions.Union(p).ToList();
            //我们的项目中，还需要递归上级权限也要授权

            var tmp = list.ToImmutableList();

            foreach (var item in tmp)
            {
                item.RecursionUp(x => { 
                    list.Add(x);
                    return true;
                });
            }

            return base.SetGrantedPermissionsAsync(role, list);
        }
    }
}