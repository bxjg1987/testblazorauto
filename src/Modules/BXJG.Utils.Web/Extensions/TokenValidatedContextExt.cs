using Abp.Authorization;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.JwtBearer;
public static class TokenValidatedContextExt
{
    /// <summary>
    /// 验证安全戳
    /// </summary>
    /// <typeparam name="TSigninMna"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task ValidateSecurityStampAsync<TSigninMna, TUser>(this TokenValidatedContext context) where TSigninMna : SignInManager<TUser> where TUser : class
    {
        //abp has bug，这里不改变下租户，获取用户始终为空，参考：https://github.com/aspnetboilerplate/aspnetboilerplate/issues/7105
        //参考SecurityStampValidator的父类，貌似是手动开启了uow，所以可以正常

        //AbpClaimTypes.TenantId
        string? tenantId = context.Principal?.Claims.FirstOrDefault(x => x.Type == AbpClaimTypes.TenantId)?.Value;
        string? userId = context.Principal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var cm = context.HttpContext.RequestServices.GetRequiredService<ICacheManager>();
        if ((await cm.GetSecureStampCache().TryGetValueAsync(tenantId + "_" + userId)).HasValue)
            return;

        var abpSession = context.HttpContext.RequestServices.GetRequiredService<IAbpSession>();
        using var szzh = abpSession.Use(tenantId.IsNotNullOrWhiteSpaceBXJG() ? int.Parse(tenantId!) : default, userId.IsNotNullOrWhiteSpaceBXJG() ? long.Parse(userId!) : default);
        //var usgnInManager = context.HttpContext.RequestServices.GetRequiredService<UserManager>();
        //var u = await usgnInManager.GetUserAsync(context.Principal);// return null
       
        var signInManager = context.HttpContext.RequestServices.GetRequiredService<TSigninMna>();
        var user = await signInManager.ValidateSecurityStampAsync(context.Principal);

        //ssvor.Options.ValidationInterval
        if (user == null)
        {
            context.Fail("Security stamp validation failed, reject token.");
        }
        else
        {

            //var ssvor = context.HttpContext.RequestServices.GetRequiredService<ZLJ.Core.Identity.SecurityStampValidator>();
           
            var opt = context.HttpContext.RequestServices.GetRequiredService<IOptionsSnapshot<SecurityStampValidatorOptions>>();
            var timeProvider = context.HttpContext.RequestServices.GetRequiredService<TimeProvider>();


            await cm.GetSecureStampCache().SetAsync($"{tenantId}_{userId}", 1, absoluteExpireTime: timeProvider.GetLocalNow() + opt.Value.ValidationInterval);
        }
    }
}
