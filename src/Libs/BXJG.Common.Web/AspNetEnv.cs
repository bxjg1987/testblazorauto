using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace BXJG.Common
{
    public class AspNetEnv: IEnv
    {
        private readonly IWebHostEnvironment webEnvironment;
        private readonly IHttpContextAccessor configuration;

        public AspNetEnv(IWebHostEnvironment webEnvironment, IHttpContextAccessor configuration) {
            this.webEnvironment = webEnvironment;
            this.configuration = configuration;
        }

        public string WebRoot => webEnvironment.WebRootPath;

        public string RootUrl => configuration.HttpContext.Request.PathBase; //configuration["urls"].Split(';')[0];

        public string SecureDirectory => webEnvironment.ContentRootPath;
    }
}
