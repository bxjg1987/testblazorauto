using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using ZLJ.Core.Authorization.Roles;
using System.Linq;

namespace ZLJ.Core.Authorization.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        public UserManager(
            RoleManager roleManager,
            UserStore store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ISettingManager settingManager, IRepository<UserLogin, long> userLoginRepository)
            : base(
                roleManager,
                store,
                optionsAccessor,
                passwordHasher,
                userValidators,
                passwordValidators,
                keyNormalizer,
                errors,
                services,
                logger,
                permissionManager,
                unitOfWorkManager,
                cacheManager,
                organizationUnitRepository,
                userOrganizationUnitRepository,
                organizationUnitSettings,
                settingManager, userLoginRepository)
        {
        }
        //public IUnitOfWorkManager unitOfWorkManager { get; set; }
        // public override Task<User> FindByIdAsync(string userId)
        // {
        //     try
        //     {
        //         return base.FindByIdAsync(userId);
        //     }
        //     catch (Exception ex)
        //     {
        //         var curr = unitOfWorkManager.Current;
        //         throw;
        //     }

        // }
        /// <summary>
        /// 拒绝权限
        /// 若未授权，不处理，否则将授权状态设置为false，表示禁止使用此权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public override async Task ProhibitPermissionAsync(User user, Permission permission)
        {
            var ps = permission.GetDependencePermissions();//我依赖的权限
            foreach (var item in ps)
            {
                var ps2 = item.GetDependentedPermissions();//依赖我的权限
                bool flag = false;
                foreach (var item2 in ps2)
                {
                    if (await IsGrantedAsync(user.Id, item2))
                    {
                        flag = true;
                        break;
                    }
                    if (flag == false)
                    {
                        await base.ProhibitPermissionAsync(user, item);
                    }
                }
            }

            await base.ProhibitPermissionAsync(user, permission);
        }
        public override async Task GrantPermissionAsync(User user, Permission permission)
        {
            var list = new List<Permission> { permission };
            list.AddRange(permission.GetDependencePermissions());
            await SetGrantedPermissionsAsync(user, list);
        }
        public override Task SetGrantedPermissionsAsync(User user, IEnumerable<Permission> permissions)
        {
            var p = permissions.SelectMany(c => c.GetDependencePermissions()).Distinct();
            return base.SetGrantedPermissionsAsync(user, permissions.Union(p));
        }
    }
}
