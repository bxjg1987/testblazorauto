using Castle.Facilities.Logging;
using ZLJ.Application.Share.Authorization.Permissions;
using BXJG.Common.Http;
using ZLJ.Web.BlazorAuto.Auth;
using Microsoft.AspNetCore.Authentication;
using ZLJ.Web.BlazorAuto.Components;
using ZLJ.RCL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//var _appConfiguration = builder.Configuration;
//var webHostEnvironment = builder.Environment;

//builder.Services.ConfigureApplicationCookie(opt =>
//{
//    //滑动过期1小时，api token过期时间在配置对象TokenAuthConfiguration中
//    opt.SlidingExpiration = true;
//    opt.ExpireTimeSpan = TimeSpan.FromHours(5);
//});
builder.Services.AddAuthentication(/*CookieAuthenticationDefaults.AuthenticationScheme*/ opt => {
    //不加这个，在调用HttpContext.SiginAsync会报错，参考：https://learn.microsoft.com/zh-cn/dotnet/core/compatibility/aspnetcore#identity-signinasync-throws-exception-for-unauthenticated-identity
    opt.RequireAuthenticatedSignIn = false;
    //opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(x => {
    //登录时，根据accessToken的过期时间去设置
    //x.SlidingExpiration = true;
    //x.Cookie.Expiration = TimeSpan.FromDays(1);
    x.Cookie.HttpOnly = true;
});
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<IAccessTokenProvider>(x => x.GetRequiredService<AuthenticationStateProvider>() as PersistingRevalidatingAuthenticationStateProvider);


builder.Services.AddAdminBlazor().AddBXJGCommonWeb();
builder.Services.AddAutoMapper(typeof(Program), typeof(ZLJ.RCL.AppContainer));

builder.Services.AddAdminApiClientProxy(hc =>
{
    hc.BaseAddress = new Uri(builder.Configuration["App:ServerRootAddress"].TrimEnd('/') + "/");
});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

app.Services.GetRequiredService<AppContainer>().Services = app.Services;

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
