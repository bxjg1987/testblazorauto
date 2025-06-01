using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ZLJ.Web.BlazorAuto
{
    //https://learn.microsoft.com/zh-cn/aspnet/core/blazor/fundamentals/environments?view=aspnetcore-9.0#read-the-environment-client-side-in-a-blazor-web-app
    public class ServerHostEnvironment(IWebHostEnvironment env, NavigationManager nav) :
     IWebAssemblyHostEnvironment
    {
        public string Environment => env.EnvironmentName;
        public string BaseAddress => nav.BaseUri;
    }
}
