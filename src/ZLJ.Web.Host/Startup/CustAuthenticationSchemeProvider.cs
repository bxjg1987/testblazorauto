using Abp.AspNetCore.Mvc.Extensions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ZLJ.Web.Host.Startup
{
    //https://github.com/dotnet/aspnetcore/blob/c85baf8db0c72ae8e68643029d514b2e737c9fae/src/Http/Authentication.Core/src/AuthenticationCoreServiceCollectionExtensions.cs

    public class CustAuthenticationSchemeProvider : AuthenticationSchemeProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public CustAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
          //  _options = options.Value;
            this.httpContextAccessor = httpContextAccessor;
        }
        protected CustAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options, IDictionary<string, AuthenticationScheme> schemes) : base(options, schemes)
        {
          //  _options = options.Value;
        }
        private Task<AuthenticationScheme?> GetAppDefaultScheme(Func<Task<AuthenticationScheme?>> func)
        {
            if (httpContextAccessor.HttpContext.Request.IsAjaxRequestBXJG())
            {
                //httpContextAccessor.HttpContext.RequestServices.GetService<Castle.Core.Logging.ILogger>().Debug($"获取默认身份验证方案：是ajax请求");
                return func();
            }
            else
            {
                //httpContextAccessor.HttpContext.RequestServices.GetService<Castle.Core.Logging.ILogger>().Debug($"获取默认身份验证方案：不是ajax请求");
                return GetSchemeAsync(IdentityConstants.ApplicationScheme);
            }
            //经常命中adminApp，用上面的方式更容易
            // return GetSchemeAsync(IdentityConstants.ApplicationScheme);
            //if (httpContextAccessor != default && httpContextAccessor.HttpContext!.Items.ContainsKey("appKey"))
            //{
            //    var appKey = httpContextAccessor.HttpContext.GetAppKey();
            //    switch (appKey)
            //    {
            //        case "custApp":
            //        case "staffApp":
            //        case "mainApp":
            //            return GetSchemeAsync(IdentityConstants.ApplicationScheme);
            //    }
            //}
            //return func();
        }
        public override Task<AuthenticationScheme?> GetDefaultAuthenticateSchemeAsync() => GetAppDefaultScheme(base.GetDefaultAuthenticateSchemeAsync);
        public override Task<AuthenticationScheme?> GetDefaultChallengeSchemeAsync() => GetAppDefaultScheme(base.GetDefaultChallengeSchemeAsync);
        public override Task<AuthenticationScheme?> GetDefaultSignInSchemeAsync() => GetAppDefaultScheme(base.GetDefaultSignInSchemeAsync);
    }
}
