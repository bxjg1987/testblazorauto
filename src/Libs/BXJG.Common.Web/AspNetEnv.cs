using BXJG.Common.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using System;

namespace BXJG.Common.Web
{
    public class AspNetEnv : IEnv
    {
        private readonly IWebHostEnvironment webEnvironment;
        private readonly IHttpContextAccessor configuration;

        public AspNetEnv(IWebHostEnvironment webEnvironment, IHttpContextAccessor configuration)
        {
            this.webEnvironment = webEnvironment;
            this.configuration = configuration;
        }

        public string WebRoot => webEnvironment.WebRootPath;

        public string RootUrl
        {
            get
            {
                var httpContext = configuration.HttpContext;
                if (httpContext == null)
                    throw new InvalidOperationException("RootUrl 依赖 HttpContext，不可在非 HTTP 请求上下文中调用（如后台服务、定时任务等）");
                return httpContext.Request.Scheme + "://" + httpContext.Request.Host.Value + "/";
            }
        } //configuration["urls"].Split(';')[0];

        public string SecureDirectory => webEnvironment.ContentRootPath;
    }
}
