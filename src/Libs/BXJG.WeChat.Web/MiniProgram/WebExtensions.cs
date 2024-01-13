using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.Web.MiniProgram
{
    public static class WebExtensions
    {
        public static IApplicationBuilder UseWeChatMiniProgram(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
