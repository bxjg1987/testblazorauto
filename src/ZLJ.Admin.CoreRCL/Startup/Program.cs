using Abp.Runtime.Session;
using BXJG.Common.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using ZLJ.Admin.ClientProxy;
using ZLJ.Admin.CoreRCL.Auth;
using ZLJ.Admin.CoreRCL.Extensions;
using ZLJ.Admin.CoreRCL.Startup;


var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Services.AddBlazorClientCore()
                .AddAuthorizationCore()
                .AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>()
                .AddSingleton<AccessTokenProvider>()
                .AddSingleton<IAccessTokenProvider>(s => s.GetRequiredService<AccessTokenProvider>())
                .AddSingleton<AppContainer>()
                .AddTransient<IAbpSession,MyAbpSession>();

builder.Services.AddAdminApiClientProxy(hc =>
{
    hc.BaseAddress = new Uri(builder.Configuration["ApiUrlRoot"].TrimEnd('/') + "/");
});

var host = builder.Build();
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
