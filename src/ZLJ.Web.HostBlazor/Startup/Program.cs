using Abp.AspNetCore;
using Abp.AspNetCore.Dependency;
using Abp.Dependency;
using Castle.Facilities.Logging;
using Abp.Castle.Logging.Log4Net;
using ZLJ;
using ZLJ.Core.Identity;
using ZLJ.Web.HostBlazor.Components;
using ZLJ.Web.HostBlazor.Startup;
using Hangfire;
using Hangfire.SqlServer;
using ZLJ.Web.HostBlazor.Auth;
using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Abp.Hangfire;
using ZLJ.Application.Share.Authorization.Permissions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCastleWindsor(IocManager.Instance.IocContainer);

var _defaultCorsPolicyName = "localhost";
var _apiVersion = "v1";

var _appConfiguration = builder.Configuration;
var webHostEnvironment = builder.Environment;
//AuthorizeAttribute
string defaultConnectionString = _appConfiguration.GetConnectionString(ZLJ.Core.ZLJConsts.ConnectionStringName)!;

builder.Services.AddMvcCore();//经过测试，这个必须加

IdentityRegistrar.Register(builder.Services);
builder.Services.ConfigureApplicationCookie(opt =>
{
    //滑动过期1小时，api token过期时间在配置对象TokenAuthConfiguration中
    opt.SlidingExpiration = true;
    opt.ExpireTimeSpan = TimeSpan.FromHours(1);
});
//builder.Services.AddAuthentication().AddCookie
//builder.Services.Configure<CookieAuthenticationOptions>(opt =>
//{
//    opt.SlidingExpiration = true;
// //   opt.ex
//    opt.ExpireTimeSpan = TimeSpan.Zero;
//});

#region hangfire
string hangfireConnectionString =defaultConnectionString;// _appConfiguration.GetConnectionString("HangfireSqlServer")!;

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        //.UseDashboardMetric( ),
        .UseSqlServerStorage(hangfireConnectionString, new SqlServerStorageOptions
        {
            //默认值已满足需求
        }));
#endregion

#region abp
builder.Services.AddAbpWithoutCreatingServiceProvider<ZLJWebHostModule>(
              // Configure Log4Net logging
              //options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseAbpLog4Net().WithConfig(webHostEnvironment.IsDevelopment() ? "log4net.config" : "log4net.Production.config"))
              options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseAbpLog4Net().WithConfig("log4net.config"))
          , true);
#endregion

//AbpUserConfigurationController
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddZLJBlazorServer().AddAdminBlazor();

var app = builder.Build();

app.Use((ctx, next) =>
{
    ctx.Request.Headers["Accept-Language"] = ctx.Request.Headers["Accept-Language"].ToString()
                                                                                   .Replace("zh-CN,", "zh-Hans,")
                                                                                   .Replace("zh-CN;", "zh-Hans;")
                                                                                   .Replace("zh,", "zh-Hans,")
                                                                                   .Replace("zh;", "zh-Hans;");
    return next();
});
app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.


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

app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();


app.UseAbpRequestLocalization();

app.Map("/account/logout", async (HttpContext x) =>
{
    await x.RequestServices.GetRequiredService<SignInManager>().SignOutAsync();
    x.Response.Redirect("/");
});
app.UseHangfireDashboard(options: new DashboardOptions
{
    Authorization = new[] { new AbpHangfireAuthorizationFilter(PermissionNames.HangFireDashboard) }
});
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ZLJ.Admin.CoreRCL.Share.Routes).Assembly);

app.Run();
