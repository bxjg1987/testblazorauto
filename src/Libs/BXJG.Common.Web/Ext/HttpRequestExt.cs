using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExt
    {
     
        /// <summary>
        /// 从cookie、请求头或querystring中获取值
        /// 关键字是：key（不区分大小写）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string? GetFromQueryStringOrHeaderOrCookie(this HttpRequest request, string key)
        {
            // 从请求头中获取（不区分大小写）
            if (request.Headers.TryGetValue(key, out var headerValue) && !string.IsNullOrEmpty(headerValue))
            {
                return headerValue;
            }

            // 从querystring中获取（不区分大小写）
            if (request.Query.TryGetValue(key, out var queryValue) && !string.IsNullOrEmpty(queryValue))
            {
                return queryValue;
            }

            // 从cookie中获取（Cookie键名通常区分大小写）
            if (request.Cookies.TryGetValue(key, out var cookieValue) && !string.IsNullOrEmpty(cookieValue))
            {
                return cookieValue;
            }


            return null;
        }
        /// <summary>
        /// abp那个判断对新浏览器没用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsAjaxRequestBXJG(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                if (request.Headers.TryGetValue("Authorization", out var auth))
                {

                    if (auth.First().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        return true;
                }

                if (request.Headers.TryGetValue("Sec-Fetch-Mode", out var cors))
                {
                    if (cors == "cors")
                        return true;
                }

                return request.Headers["X-Requested-With"] == "XMLHttpRequest";//|| request.Headers.ContainsKey("Sec-Fetch-Dest");
            }

            // request.HttpContext.RequestServices.GetService < Castle.Core.Logging .<>>

            return false;
        }
    }
}
