using Castle.Facilities.Logging;
using ZLJ.Application.Share.Authorization.Permissions;
using BXJG.Common.Http;
using ZLJ.Web.BlazorAuto.Auth;
using Microsoft.AspNetCore.Authentication;
using ZLJ.Web.BlazorAuto.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using BXJG.Utils.RCL;
using NUglify.Html;
using AntDesign;
using ZLJ.Application.Common.ClientProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using ZLJ.RCL.Interceptors;
//JsonSerializerOptions.Default.PropertyNameCaseInsensitive = true;


var builder = WebApplication.CreateBuilder(args);

//var _appConfiguration = builder.Configuration;
//var webHostEnvironment = builder.Environment;

//builder.Services.ConfigureApplicationCookie(opt =>
//{
//    //滑动过期1小时，api token过期时间在配置对象TokenAuthConfiguration中
//    opt.SlidingExpiration = true;
//    opt.ExpireTimeSpan = TimeSpan.FromHours(5);
//});
builder.Services.AddAuthentication(/*CookieAuthenticationDefaults.AuthenticationScheme*/ opt =>
{
    //不加这个，在调用HttpContext.SiginAsync会报错，参考：https://learn.microsoft.com/zh-cn/dotnet/core/compatibility/aspnetcore#identity-signinasync-throws-exception-for-unauthenticated-identity
    opt.RequireAuthenticatedSignIn = false;
    //opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(x =>
{
    //登录时，根据accessToken的过期时间去设置
    //x.SlidingExpiration = true;
    //x.Cookie.Expiration = TimeSpan.FromDays(1);
    x.Cookie.HttpOnly = true;
});
builder.Services.AddAuthorization();
//builder.Services.AddAuthorization(opt => {
//    opt.AddPolicy("Administrator", ab => {
//        ab.RequireAssertion(c => 
//        {
//           // var sdfsdf = c.Resource.GetType();
//            return Task.FromResult(true);
//        });

//    });
//});
//builder.Services.AddSingleton<IAuthorizationHandler, sdfd>();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<IAccessTokenProvider>(x => x.GetRequiredService<AuthenticationStateProvider>() as PersistingRevalidatingAuthenticationStateProvider);


builder.Services.AddAdminBlazor().AddBXJGCommonWeb();
//builder.Services.AddAutoMapper(typeof(Program), typeof(ZLJ.RCL.AppContainer));

builder.Services.AddAdminApiClientProxy(hc =>
{
    hc.BaseAddress = new Uri(builder.Configuration["App:ServerRootAddress"].TrimEnd('/') + "/");
});

builder.Services.AddHttpContextAccessor();

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(2);//session 滑动过期时间
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;//对于应用来说是必须的，绕过用户同意
//});

var app = builder.Build();

//app.Services.GetRequiredService<AppContainer>().Services = app.Services;

app.UseStatusCodePages(async statusCodeContext =>
{
    if (statusCodeContext.HttpContext.Response.StatusCode == 404)
        statusCodeContext.HttpContext.Response.Redirect("/404");
    //if(statusCodeContext.respo)
    //// using static System.Net.Mime.MediaTypeNames;
    //statusCodeContext.HttpContext.Response.ContentType = Text.Plain;

    //await statusCodeContext.HttpContext.Response.WriteAsync(
    //    $"Status Code Page: {statusCodeContext.HttpContext.Response.StatusCode}");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();



app.UseRouting();
app.UseAuthentication();
app.Use(async (context, next) =>
{
   // AbpExceptionInterceptor1.Services.Value = context.RequestServices;
    Console.WriteLine($"http请求中间件中的ioc实例：{context.RequestServices.GetHashCode()}");
    await next.Invoke();
});
//app.UseSession();
//全局状态会初始化三次，blazor server一次，wasm一次，http请求一次。
//实际代码中在http请求一次，也就是这里，blazor由于路由共享，在路由中初始化一次即可。
//放容器中后，授权需要时才会去调用
//app.Use(async (context, next) =>
//{
//    /*
//     * 目前的项目结构，这里的逻辑不太好封装，顶多搞到当前项目的单独类文件中
//     * http中这次的初始化，目前好像只有授权时是必须的，所以这里仅初始化授权
//     * 使用session延长全局状态的有效期，也不用太长，因为进入blazor后，基本不会再发啥http请求。
//     */

//    var appContainer = context.RequestServices.GetRequiredService<AppContainer>();
//    if (context.User?.Identity != default && context.User.Identity.IsAuthenticated)
//    {
//        const string k = "grantedPermissions";

//        var r = context.Session.GetString(k);
//        //Dictionary<string, string> ps;
//        if (r.IsNotNullOrWhiteSpaceBXJG())
//        {
//            var ps = JsonSerializer.Deserialize<string[]>(r);
//            appContainer.AbpUserConfiguration = Task.FromResult(new Abp.Web.Models.AbpUserConfiguration.AbpUserConfigurationDto
//            {
//                Auth = new Abp.Web.Models.AbpUserConfiguration.AbpUserAuthConfigDto
//                {
//                    GrantedPermissions = ps.ToDictionary(x => x, x => true.ToString())
//                }
//            });
//        }
//        else
//        {
//            app.Logger.LogDebug("http请求时，从服务端拿全局状态中的权限字符串。appcontainer对象：" + appContainer.GetHashCode());
//            var abpUserCfgService = context.RequestServices.GetRequiredService<AbpUserConfigurationService>();
//            appContainer.AbpUserConfiguration = abpUserCfgService.GetPermissions().ContinueWith(t =>
//            {
//                var zfc = JsonSerializer.Serialize(t.Result.GrantedPermissions.Select(x => x.Key));
//                // app.Logger.LogDebug("已授权：" + zfc);
//                context.Session.SetString(k, zfc);
//                return new Abp.Web.Models.AbpUserConfiguration.AbpUserConfigurationDto
//                {
//                    Auth = new Abp.Web.Models.AbpUserConfiguration.AbpUserAuthConfigDto { GrantedPermissions = t.Result.GrantedPermissions }
//                };
//            });
//        }
//    }
//    else
//    {
//        appContainer.AbpUserConfiguration = Task.FromResult(new Abp.Web.Models.AbpUserConfiguration.AbpUserConfigurationDto
//        {
//            Auth = new Abp.Web.Models.AbpUserConfiguration.AbpUserAuthConfigDto
//            {
//                GrantedPermissions = new Dictionary<string, string>()
//            }
//        });
//    }
//    //appContainer.AbpUserConfiguration = new Abp.Web.Models.AbpUserConfiguration.AbpUserConfigurationDto { 
//    //    Auth = await abpUserCfgService.GetPermissions()
//    //};
//    //已server模式运行时，app将appcontainer传递给route时会报错，wasm模式时没有问题
//    // appContainer.AbpUserConfiguration.Localization.Values = default;
//    // Do work that can write to the Response.
//    await next.Invoke();
//    // Do logging or other work that doesn't write to the Response.
//});

app.UseAuthorization();
app.UseAntiforgery();


//注销
app.Map("/account/logout", async (HttpContext x) =>
{
    await x.SignOutAsync();
    //await x.RequestServices.GetRequiredService<IAuthenticationService>().SignOutAsync(x, default, default);
    x.Response.Redirect("/");
});

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(ZLJ.Admin.CoreRCL.Share.Routes).Assembly);

app.Run();
