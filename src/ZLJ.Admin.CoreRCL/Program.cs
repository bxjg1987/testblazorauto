using BXJG.Common.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.Admin.CoreRCL.Auth;
using ZLJ.Admin.CoreRCL.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorClientCore(builder.Configuration);

builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();


await builder.Build().RunAsync();
