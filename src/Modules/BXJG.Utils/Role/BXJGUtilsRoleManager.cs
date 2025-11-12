using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Role
{
    public class BXJGUtilsRoleManager<TRole, TUser> : AbpRoleManager<TRole, TUser>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
    {
        public BXJGUtilsRoleManager(AbpRoleStore<TRole, TUser> store, IEnumerable<IRoleValidator<TRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<AbpRoleManager<TRole, TUser>> logger, IPermissionManager permissionManager, ICacheManager cacheManager, IUnitOfWorkManager unitOfWorkManager, IRoleManagementConfig roleManagementConfig, IRepository<OrganizationUnit, long> organizationUnitRepository, IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository) : base(store, roleValidators, keyNormalizer, errors, logger, permissionManager, cacheManager, unitOfWorkManager, roleManagementConfig, organizationUnitRepository, organizationUnitRoleRepository)
        {
        }
        public override Task SetGrantedPermissionsAsync(TRole role, IEnumerable<Permission> permissions)
        {
            var p = permissions.SelectMany(c => c.GetDependencePermissions()).Distinct();
            var list = permissions.Union(p).ToList();
            //我们的项目中，还需要递归上级权限也要授权

            //var tmp = list.ToImmutableList();

            //foreach (var item in tmp)
            //{
            //    item.RecursionUp(x => { 
            //        list.Add(x);
            //        return true;
            //    });
            //}

            return base.SetGrantedPermissionsAsync(role, list);
        }
    }
}
