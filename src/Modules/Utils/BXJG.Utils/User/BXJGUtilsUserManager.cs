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

/*
 * 参考utils抽象.md中的文档，这里本应该用组合而非继承
 * 但由于abp官方模板中，模板项目中的用户管理器是继承abp的usermanager，所以这里保持一直的思路
 * 让具体项目的usermanager继承这里的
 * 
 * 而utils中的其它领域服务、应用服务遵循组合的思路
 */

public class BXJGUtilsUserManager<TRole, TUser> : AbpUserManager<TRole, TUser>//, IBXJGUtilsUserManager<TRole,TUser>
   where TRole : AbpRole<TUser>, new()
   where TUser : AbpUser<TUser>
{
    /// <summary>
    /// 超级管理员登录名，默认是admin，允许子类重写
    /// </summary>
    protected virtual string SuperAdminUserName => "admin";

    /// <summary>
    /// 超级管理员角色名，默认是Admin，允许子类重写
    /// </summary>
    protected virtual string SuperAdminRoleName => "Admin";

    protected BXJGUtilsUserManager(AbpRoleManager<TRole, TUser> roleManager,
                          AbpUserStore<TRole, TUser> userStore,
                          IOptions<IdentityOptions> optionsAccessor,
                          IPasswordHasher<TUser> passwordHasher,
                          IEnumerable<IUserValidator<TUser>> userValidators,
                          IEnumerable<IPasswordValidator<TUser>> passwordValidators, 
                          ILookupNormalizer keyNormalizer,
                          IdentityErrorDescriber errors, 
                          IServiceProvider services,
                          ILogger<UserManager<TUser>> logger, 
                          IPermissionManager permissionManager,
                          IUnitOfWorkManager unitOfWorkManager,
                          ICacheManager cacheManager,
                          IRepository<Abp.Organizations. OrganizationUnit, long> organizationUnitRepository,
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
    /// <remarks>
    /// TODO: 需核查逻辑 - 存在两个问题：
    /// 1. if (flag == false) 判断应在 foreach (var item2 in ps2) 循环外部，当前在循环内部导致只要有一个依赖权限未被授权就禁止，而非遍历完所有依赖权限后再判断
    /// 2. ps2 为空时内层 foreach 不执行，item 不会被禁止，需确认是否符合业务预期
    /// </remarks>
    public override async Task ProhibitPermissionAsync(TUser user, Permission permission)
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
    public override async Task GrantPermissionAsync(TUser user, Permission permission)
    {
        var list = new List<Permission> { permission };
        list.AddRange(permission.GetDependencePermissions());
        await SetGrantedPermissionsAsync(user, list);
    }
    public override Task SetGrantedPermissionsAsync(TUser user, IEnumerable<Permission> permissions)
    {
        var p = permissions.SelectMany(c => c.GetDependencePermissions()).Distinct();
        return base.SetGrantedPermissionsAsync(user, permissions.Union(p));
    }


    public override async Task<IdentityResult> UpdateAsync(TUser user)
    {
        var r = await base.UpdateAsync(user);
        if (r.Succeeded && (user.IsActive == false || user.IsDeleted))
            await UpdateSecurityStampAsync(user);
        return r;
    }
    public ICacheManager CacheManager { get; set; }
    public override async Task<IdentityResult> UpdateSecurityStampAsync(TUser user)
    {
        var r = await base.UpdateSecurityStampAsync(user);

        //清空下缓存
        await CacheManager.GetSecureStampCache().RemoveAsync($"{user.TenantId}_{user.Id}");
        //base.Logger.LogWarning($"{user.UserName}更新了安全戳");
        //  cm.GetUserPermissionCache
        return r;
    }

    /// <summary>
    /// 删除用户前检查是否为超级管理员，超级管理员不允许删除
    /// </summary>
    /// <param name="user">要删除的用户</param>
    /// <returns>删除结果</returns>
    public override async Task<IdentityResult> DeleteAsync(TUser user)
    {
        if (user.UserName == SuperAdminUserName)
        {
            return IdentityResult.Failed(new IdentityError { Description = "不允许删除超级管理员" });
        }
        return await base.DeleteAsync(user);
    }
    
    /// <summary>
    /// 设置用户角色前检查，超级管理员必须至少拥有Admin角色
    /// </summary>
    /// <param name="user">要设置角色的用户</param>
    /// <param name="roleNames">角色名称列表</param>
    /// <returns>设置结果</returns>
    public override async Task<IdentityResult> SetRolesAsync(TUser user, string[] roleNames)
    {
        if (user.UserName == SuperAdminUserName)
        {
            var roleNameList = roleNames.ToList();
            if (!roleNameList.Contains(SuperAdminRoleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = "超级管理员必须至少拥有Admin角色" });
            }
        }
        return await base.SetRolesAsync(user, roleNames);
    }

   
}
