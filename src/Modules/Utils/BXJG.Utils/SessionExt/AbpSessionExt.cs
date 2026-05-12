using Abp.Runtime.Session;
using BXJG.Common.Session;
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
        /// 根据key获取字符串值
        /// 目前的实现是从请求头、querystring或cookie中获取key对应的值
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static string? GetAppKey(this IAbpSession abpSession)
        {
            return abpSession.GetString("appKey");
        }
        /// <summary>
        /// 获取当前应用appKey
        /// 目前的实现是从请求头、querystring或cookie中获取appKey对应的值
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static string? GetString(this IAbpSession abpSession, string key)
        {
            return (abpSession as AbpSessionWithHttpContext)?.httpContextAccessor.GetString(key);
        }

        public static T? Get<T>(this IAbpSession abpSession, string key)  where T : struct
        {
            return (abpSession as AbpSessionWithHttpContext)?.httpContextAccessor.Get<T>(key);
        }
    }
}
