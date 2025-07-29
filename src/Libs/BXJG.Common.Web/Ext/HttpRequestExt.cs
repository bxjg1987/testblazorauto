using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExt
    {
        /// <summary>
        /// 从cookie、请求头或querystring中获取appkey
        /// 关键字是：appkey（不区分大小写）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string? GetAppKey(this HttpRequest request)
        {
            // 从请求头中获取（不区分大小写）
            if (request.Headers.TryGetValue("appkey", out var headerValue) && !string.IsNullOrEmpty(headerValue))
            {
                return headerValue;
            }

            // 从querystring中获取（不区分大小写）
            if (request.Query.TryGetValue("appkey", out var queryValue) && !string.IsNullOrEmpty(queryValue))
            {
                return queryValue;
            }

            // 从cookie中获取（Cookie键名通常区分大小写）
            if (request.Cookies.TryGetValue("appkey", out var cookieValue) && !string.IsNullOrEmpty(cookieValue))
            {
                return cookieValue;
            }

            return null;
        }
    }
}
