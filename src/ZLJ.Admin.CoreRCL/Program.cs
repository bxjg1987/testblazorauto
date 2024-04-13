using Abp.Runtime.Session;
//using Blazor.Extensions.Logging;
using BXJG.Common.Http;
using BXJG.Common.RCL.Loggers;
using BXJG.Utils.RCL;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text.Json;
using ZLJ.Admin.ClientProxy;
using ZLJ.Admin.CoreRCL.Auth;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLogging(lb =>
{
    //lb.AddBrowserConsole();
    lb.AddProvider(new BrowserConsoleLoggerProvider()).SetMinimumLevel(LogLevel.Debug);
});
//builder.Logging.AddProvider(new BrowserConsoleLoggerProvider());

builder.Services.AddAdminBlazor().AddCommonRCLClient().AddAuthorizationCore();



builder.Services.AddAdminApiClientProxy(hc =>
{
    hc.BaseAddress = new Uri(builder.Configuration["App:ServerRootAddress"].TrimEnd('/') + "/");
});

var host = builder.Build();
host.Services.GetRequiredService<AppContainer>().Services = host.Services;
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
