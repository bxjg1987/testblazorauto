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
using Abp.Collections.Extensions;
using TinyPinyin;
//using ZLJ.Core.Share.Authorization.Users;

namespace ZLJ.Core.Authorization.Users
{
    //public interface IUserManager : IUserManager<Role, User>,IAbpUserManager<Role, User>
    //{
    //}
    public class UserManager : BXJG.Utils.User.BXJGUtilsUserManager<Role, User>//, IUserManager
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

        //public override Task<IdentityResult> CreateAsync(User user, string password)
        //{
        //    return base.CreateAsync(user, password);
        //}
        public override Task<IdentityResult> CreateAsync(User user)
        {
            user.Pinyin = PinyinHelper.GetPinyinInitials(user.Name);


            if (!user.IsEnableAccount)
            {
                //if (user.UserName.IsNullOrWhiteSpaceBXJG())
                    user.UserName = PinyinHelper.GetPinyin(user.Name) + Abp.RandomHelper.GetRandom(0,10000);// ; Guid.NewGuid().ToString("n");

                //if (user.Password.IsNullOrWhiteSpaceBXJG())
                    user.Password = "A2_c"+BXJG.Common.RandomHelper.GetRandomString();

                user.IsLockoutEnabled = true;
                user.LockoutEndDateUtc = DateTime.MaxValue;
            }
            if (user.EmailAddress.IsNullOrWhiteSpaceBXJG())
                user.EmailAddress = user.UserName + "@a.cn";

            return base.CreateAsync(user);
        }

        //public override Task<IdentityResult> UpdateAsync(User user)
        //{
        //    if (user.IsEnableAccount)
        //    { 
        //        if(user.chan)
        //    }

        //    return base.UpdateAsync(user);
        //}

        public override async Task<IdentityResult> UpdateAsync(User user)
        {
            //return base.UpdateAsync(user);
            //}
            //protected override async Task<IdentityResult> UpdateUserAsync(User user)
            //{
            if (!user.IsEnableAccount)
            {
                //if (user.UserName.IsNullOrWhiteSpaceBXJG())
                //    user.UserName = PinyinHelper.GetPinyin(user.Name) + Abp.RandomHelper.GetRandom(0, 10000);// ; Guid.NewGuid().ToString("n");

                //if (user.Password.IsNullOrWhiteSpaceBXJG())
                //    user.Password = "A2_c" + BXJG.Common.RandomHelper.GetRandomString();

                //下面的方法会调用UpdateUserAsync(User user)，所以不要重写UpdateUserAsync(User user) 以免死循环
                await SetLockoutEnabledAsync(user, true);
                await SetLockoutEndDateAsync(user, new DateTimeOffset(Clock.Now).UtcDateTime.AddYears(300));
                // user.IsLockoutEnabled = true;
                // user.LockoutEndDateUtc = DateTime.MaxValue;
            }
            else if (await IsLockedOutAsync(user))
            {
                await ResetAccessFailedCountAsync(user);
                //await SetLockoutEnabledAsync(user, false);
                await SetLockoutEndDateAsync(user, null);
            }
            return await base.UpdateAsync(user);
            //   return await base.UpdateUserAsync(user);
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
        ///// <summary>
        ///// 拒绝权限
        ///// 若未授权，不处理，否则将授权状态设置为false，表示禁止使用此权限
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="permission"></param>
        ///// <returns></returns>
        //public override async Task ProhibitPermissionAsync(User user, Permission permission)
        //{
        //    var ps = permission.GetDependencePermissions();//我依赖的权限
        //    foreach (var item in ps)
        //    {
        //        var ps2 = item.GetDependentedPermissions();//依赖我的权限
        //        bool flag = false;
        //        foreach (var item2 in ps2)
        //        {
        //            if (await IsGrantedAsync(user.Id, item2))
        //            {
        //                flag = true;
        //                break;
        //            }
        //            if (flag == false)
        //            {
        //                await base.ProhibitPermissionAsync(user, item);
        //            }
        //        }
        //    }

        //    await base.ProhibitPermissionAsync(user, permission);
        //}
        //public override async Task GrantPermissionAsync(User user, Permission permission)
        //{
        //    var list = new List<Permission> { permission };
        //    list.AddRange(permission.GetDependencePermissions());
        //    await SetGrantedPermissionsAsync(user, list);
        //}
        //public override Task SetGrantedPermissionsAsync(User user, IEnumerable<Permission> permissions)
        //{
        //    var p = permissions.SelectMany(c => c.GetDependencePermissions()).Distinct();
        //    return base.SetGrantedPermissionsAsync(user, permissions.Union(p));
        //}
    }
}
