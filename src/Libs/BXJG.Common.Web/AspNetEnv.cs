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
                return configuration.HttpContext.Request.Scheme + "://" + configuration.HttpContext.Request.Host.Value + "/";
            }
        } //configuration["urls"].Split(';')[0];

        public string SecureDirectory => webEnvironment.ContentRootPath;
    }
}
