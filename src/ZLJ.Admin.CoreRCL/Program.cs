using Abp.Runtime.Session;
//using Blazor.Extensions.Logging;
using BXJG.Common.Http;
using BXJG.Common.RCL.Loggers;
using BXJG.Utils.RCL;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using ZLJ.Admin.ClientProxy;
using ZLJ.Admin.CoreRCL.Auth;



var builder = WebAssemblyHostBuilder.CreateDefault(args);

//builder.Services.AddLogging(lb =>
//{
//    //lb.AddBrowserConsole();
//    lb.AddProvider(new BrowserConsoleLoggerProvider()).SetMinimumLevel(LogLevel.Debug);
//});
//builder.Logging.AddProvider(new BrowserConsoleLoggerProvider());

builder.Services.AddAdminBlazor().AddCommonRCLClient().AddAuthorizationCore();
////硬编码所有权限
////获取从接口获取，所有权限的字符串并不需要登录
////然后与登录后的 已授权的比较，若未登录，则直接判断授权失败，否则比较即可。
//var allPermissions = new string[] { "Administrator" };
//builder.Services.AddAdminBlazor().AddCommonRCLClient().AddAuthorizationCore(opt => {
//    opt.AddPolicy("Administrator", ab => {
//        ab.RequireAssertion(c =>
//        {
//            // var sdfsdf = c.Resource.GetType();
//            return Task.FromResult(true);
//        });

//    });
//});


builder.Services.AddAdminApiClientProxy(hc =>
{
    hc.BaseAddress = new Uri(builder.Configuration["App:ServerRootAddress"].TrimEnd('/') + "/");
});

var host = builder.Build();
var l= host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("ccccccc");
l.LogDebug("ccccccccccccccccccc");
l.LogWarning("bbbbbbbbbbbbbbbb");
//host.Services.GetRequiredService<AppContainer>().Services = host.Services;
//var a = host.Services.GetRequiredService<AbpUserConfigurationService>();
//var b = host.Services.GetRequiredService<AppContainer>();
////这里调用接口会导致PersistentAuthenticationStateProvider构造函数理解执行，由于是单例，后续永远无法获取accesstoken
//_ = a.GetAll().ContinueWith(d =>
//{
//    b.AbpUserConfiguration = d.Result;
//Console.WriteLine(JsonSerializer.Serialize(b.AbpUserConfiguration));
//});

Console.WriteLine("客户端运行时已启动....");
await host.RunAsync();

//await builder.Build().RunAsync();
