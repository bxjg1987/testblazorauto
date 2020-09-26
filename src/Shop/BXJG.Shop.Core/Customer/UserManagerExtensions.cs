using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    public static class UserManagerExtensions
    {
        /// <summary>
        /// 判断指定用户是否是顾客（是否属于Customer角色）
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="userManager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async ValueTask<bool> IsCustomerAsync<TUser>(this UserManager<TUser> userManager, TUser user) where TUser : class
        {
            return await userManager.IsInRoleAsync(user, BXJGShopConsts.CustomerRoleName);
        }
    }
}
