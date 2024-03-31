using BXJG.Common.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace BXJG.Common.RCL.Auth
{
    /*
     * 貌似blazor客户端模式中无法使用BackgroundService
     * 幸亏.net8中的blazor客户端模式已经支持多线程，所以搞个死循环现成来做
     * 
     * blazor web app 同时支持静态，server和客户端，静态和server是基于cookie身份验证的，而cookie身份验证是可以滑动过期的
     * 而以客户端模式运行时，是前后端分离模式，需要去拿accessToken，而它默认没有滑动过期，是通过刷新token来实现的
     * 
     * auto模式比较特殊，既然cookie身份验证过了，还有滑动过期，那我们就不要直接由客户是向authserver拿token，而是让服务端帮客户端拿
     * 服务端与authserver可以做成是信任的。这样让客户端的token变成滑动过期
     * 
     * 这里有个麻烦事情，这里的代码是客户端的webassembly中执行的，木有在浏览器中执行，请求是不会自动携带 cookie
     * 
     * 变通下，通过js开个定时任务去拿accessToken，然后这里通过js互操作去拿accessToken
     * 
     * 或者在首次登录时，通过blazor状态切换传递accessToken 和刷新token到客户端中，
     * 后期使用刷新token搞，但这样就没与cookie的滑动过期关联了
     * 仔细一想，这样的方式还不如借助cookie的滑动过期
     * 
     * 或者cookie保持原来的，blazor客户端保持刷新token的方式，在首次登陆拿到的token和刷新token通过状态切换的方式传递到blazor客户端
     */

    //public class AccessTokenProvider : IAccessTokenProvider//, IDisposable
    //{
    //    ILogger logger;
    //    AuthenticationStateProvider authenticationStateProvider;

    //    public AccessTokenProvider(ILogger<AccessTokenProvider> logger, AuthenticationStateProvider authenticationStateProvider)
    //    {
    //        this.logger = logger;
    //        this.authenticationStateProvider = authenticationStateProvider;
    //    }

    //    public string GetAccessToken()
    //    {
    //        var str = authenticationStateProvider.GetAuthenticationStateAsync().ConfigureAwait(false).GetAwaiter().GetResult().User.FindFirst(c => c.Type == "AccessToken")?.Value;
    //        //Console.WriteLine("请求前获取accessToken:" + str);
    //        return str;
    //    }
    //    //public void Update(string a, string b, int c)
    //    //{
    //    //    logger.LogDebug($"accessToken被设置了:{a}");
    //    //    accessToken = a;
    //    //    refreshToken = b;
    //    //    expiration = c;
    //    //}
    //    //CancellationTokenSource cts = new CancellationTokenSource();

    //    //public AccessTokenProvider()
    //    //{
    //    //    Task.Run(async () =>
    //    //    {
    //    //        while (!cts.IsCancellationRequested)
    //    //        {
    //    //            await Task.Delay(1);
    //    //        }
    //    //    });
    //    //}

    //    //public void Dispose()
    //    //{
    //    //    cts?.Cancel();
    //    //}

    //}
}