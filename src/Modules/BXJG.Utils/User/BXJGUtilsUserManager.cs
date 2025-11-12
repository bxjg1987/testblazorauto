using Abp.Authorization.Users;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Microsoft.AspNetCore.Components.Routing;

namespace BXJG.Utils.User;

public class BXJGUtilsUserManager<Role, User> : AbpUserManager<Role, User>
   where Role : AbpRole<User>, new()
   where User : AbpUser<User>
{
    protected BXJGUtilsUserManager(AbpRoleManager<Role, User> roleManager,
                          AbpUserStore<Role, User> userStore,
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
                          ISettingManager settingManager,
                          IRepository<UserLogin, long> userLoginRepository) : base(roleManager,
                                                                                   userStore,
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
                                                                                   settingManager,
                                                                                   userLoginRepository)
    {
    }
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


    public override async Task<IdentityResult> UpdateAsync(User user)
    {
        var r = await base.UpdateAsync(user);
        if (r.Succeeded && (user.IsActive == false || user.IsDeleted))
            await UpdateSecurityStampAsync(user);
        return r;
    }
    public ICacheManager CacheManager { get; set; }
    public override async Task<IdentityResult> UpdateSecurityStampAsync(User user)
    {
        var r = await base.UpdateSecurityStampAsync(user);

        //清空下缓存
        await CacheManager.GetSecureStampCache().RemoveAsync($"{user.TenantId}_{user.Id}");
        //base.Logger.LogWarning($"{user.UserName}更新了安全戳");
        //  cm.GetUserPermissionCache
        return r;
    }
}
