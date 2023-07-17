using Abp.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ZLJ.App.Common;

namespace ZLJ.Web.Host.Startup
{
    public class AppDistinguishiMddleware
    {
        private readonly RequestDelegate _next;
        private readonly CommonApplicationConfiguration commonApplicationConfiguration;
        public AppDistinguishiMddleware(RequestDelegate next, CommonApplicationConfiguration commonApplicationConfiguration)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
            this.commonApplicationConfiguration = commonApplicationConfiguration;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User != default && context.User!.Identity!.IsAuthenticated)
            {
                var sdfsdf = context.User.Claims.SingleOrDefault(c => c.Type.Equals("appKey", StringComparison.OrdinalIgnoreCase));
                if (sdfsdf != null)
                {
                    context.Items["appKey"] = sdfsdf.Value;
                }
            }

            // /api/services/customer
            if (!context.Items.ContainsKey("appKey")&&context.Request.Path.Value.StartsWith("/api/services/"))
            {
                var appKey1 = context.Request.Path.Value.Split('/')[3];
                if (commonApplicationConfiguration.Apps.ContainsKey(appKey1))
                {
                    context.Items["appKey"] = appKey1;
                }
                else if (appKey1.Equals("app", StringComparison.OrdinalIgnoreCase))
                {
                    context.Items["appKey"] = "admin";
                }
            }

            if (!context.Items.ContainsKey("appKey"))
            {
                if (context.GetRouteData().Values.TryGetValue("appKey", out var appKey))
                {
                    context.Items["appKey"] = appKey;
                }
                else if (context.Request.Query.TryGetValue("appKey", out var appKey1))
                {
                    context.Items["appKey"] = appKey1;
                }
                else if (context.Request.Path.Value.TrimEnd('/').IsNullOrWhiteSpace() || context.Request.Path.Value.StartsWith("/hangfire"))
                {
                    context.Items["appKey"] = "main";
                }
                else if (context.Request.Cookies.TryGetValue("appKey", out var appKey0))
                {
                    context.Items["appKey"] = appKey0;
                }
                //根据二级域名获取appKey
                else
                    context.Items["appKey"] = "admin";

            }
            //context.Items["appInfo"] = apps[appKey.ToString()];
           // await _next(context);
            //try
            //{
                await _next(context);
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }
    }
}
