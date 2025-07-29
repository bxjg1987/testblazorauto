using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Runtime.Session
{
    public static class AbpSessionExt
    {
        /// <summary>
        /// 获取当前应用appKey
        /// 目前的实现是从请求头、querystring或cookie中获取appKey对应的值
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static string? GetAppKey(this IAbpSession abpSession)
        {
            var req = (abpSession as AbpSessionWithHttpContext)?.httpContextAccessor?.HttpContext?.Request;
            if (req != default)
            {
                return req.GetAppKey();
            }
            return default;
        }
    }
}
