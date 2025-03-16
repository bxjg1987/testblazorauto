using Abp.Authorization;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using AsyncKeyedLock;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Microsoft.AspNetCore.Authentication.JwtBearer;

public static class TokenValidatedContextExt
{
    private static readonly AsyncKeyedLocker<string> _asyncKeyedLocker = new();
    // private const int _timeout = 30000;

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

        //以下执行其实有并行问题，只不过问题不严重，同一个用户的一次操作可能有多个请求，并发执行时导致多次取数据库做判断
        //尽管abp中使用SemaphoreSlim并提供相应的异步方法的扩展，但是这 需要按用户对应到内存中的SemaphoreSlim实例
        //在这里的场景不太实用。
        //使用 https://github.com/MarkCiliaVincenti/AsyncKeyedLock 缓解这个问题
        //另外貌似AsynEx很久不更新了，且好像存在很多问题，所以Abp已经弃用它，我们也不再使用它了

        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger>();
        //AbpClaimTypes.TenantId
        string? tenantId = context.Principal?.Claims.FirstOrDefault(x => x.Type == AbpClaimTypes.TenantId)?.Value;
        string? userId = context.Principal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var cm = context.HttpContext.RequestServices.GetRequiredService<ICacheManager>();
        if ((await cm.GetSecureStampCache().TryGetValueAsync(tenantId + "_" + userId)).HasValue)
        {
            logger.Debug($"[验证安全戳] 缓存已存在，放弃验证。租户id：{tenantId} 用户id：{userId}");
            return;
        }
        var abpSession = context.HttpContext.RequestServices.GetRequiredService<IAbpSession>();
        using var szzh = abpSession.Use(tenantId.IsNotNullOrWhiteSpaceBXJG() ? int.Parse(tenantId!) : default, userId.IsNotNullOrWhiteSpaceBXJG() ? long.Parse(userId!) : default);
        //var usgnInManager = context.HttpContext.RequestServices.GetRequiredService<UserManager>();
        //var u = await usgnInManager.GetUserAsync(context.Principal);// return null

        var signInManager = context.HttpContext.RequestServices.GetRequiredService<TSigninMna>();

        //目前_asyncKeyedLocker是全局静态，专用于此场景，
        //多个锁实例会不会有问题，参考：https://github.com/MarkCiliaVincenti/AsyncKeyedLock/issues/60
        //这里的锁是尽力而为的，就是在单个用户的一次操作的并发请求可能有多个，它们可能都会执行到这里查询数据库
        //所以这里用个锁，尽量避免这种情况
        //超时时间长短无所谓，即使很短大部分的拿不到锁就不执行判断即可。
        using (var releaser = await _asyncKeyedLocker.LockOrNullAsync($"{tenantId}_{userId}", 20000))
        {
            if (releaser is not null)
            {
                //这里还是有必要再次判断下的
                if ((await cm.GetSecureStampCache().TryGetValueAsync(tenantId + "_" + userId)).HasValue)
                {
                    logger.Debug($"[验证安全戳] 缓存已存在，放弃验证。租户id：{tenantId} 用户id：{userId}");
                    return;
                }

                var user = await signInManager.ValidateSecurityStampAsync(context.Principal);

                if (user == null)
                {
                    logger.Debug($"[验证安全戳] 验证失败，拒绝token。租户id：{tenantId} 用户id：{userId}");
                    context.Fail("Security stamp validation failed, reject token.");
                }
                else
                {
                    //var ssvor = context.HttpContext.RequestServices.GetRequiredService<ZLJ.Core.Identity.SecurityStampValidator>();

                    var opt = context.HttpContext.RequestServices.GetRequiredService<IOptionsSnapshot<SecurityStampValidatorOptions>>();
                    var timeProvider = context.HttpContext.RequestServices.GetRequiredService<TimeProvider>();

                    var gqsj = timeProvider.GetLocalNow() + opt.Value.ValidationInterval;
                    await cm.GetSecureStampCache().SetAsync($"{tenantId}_{userId}", 1, absoluteExpireTime: gqsj);
                    logger.Debug($"[验证安全戳] 验证成功，缓存用户标记。租户id：{tenantId} 用户id：{userId} 过期时间：{gqsj.ToString("yyyy-MM-dd HH:mm:ss")}");
                }
            }
            else
            { 
                //没拿到锁就正常执行，不做数据库操作。
                logger.Debug($"[验证安全戳] 获取锁失败，放弃验证。租户id：{tenantId} 用户id：{userId}");
            }
        }
    }
}
