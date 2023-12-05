using Abp.AspNetCore;
using Abp.AspNetCore.Dependency;
using Abp.Dependency;
using Castle.Facilities.Logging;
using Abp.Castle.Logging.Log4Net;
using ZLJ;
using ZLJ.Identity;
using ZLJ.Web.HostBlazor.Components;
using ZLJ.Web.HostBlazor.Startup;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCastleWindsor(IocManager.Instance.IocContainer);

var _defaultCorsPolicyName = "localhost";
var _apiVersion = "v1";

var _appConfiguration = builder.Configuration;
var webHostEnvironment = builder.Environment;
//AuthorizeAttribute
string defaultConnectionString = _appConfiguration.GetConnectionString(ZLJConsts.ConnectionStringName)!;

builder.Services.AddMvcCore();//经过测试，这个必须加

IdentityRegistrar.Register(builder.Services);

#region hangfire
string hangfireConnectionString = _appConfiguration.GetConnectionString("HangfireSqlServer")!;

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


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.Adddd(builder.Configuration);

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ZLJ.Admin.CoreRCL.Routes).Assembly);

app.Run();
