using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common.Session
{
    /*
     * 应用层不应依赖web层，但又需要web提供的session，如：当前租户、当前用户、当前xxx等信息。
     * 所以加个接口来隔离应用层和web层。
     * 
     * 有些信息是用户登录后，获取的，但是某些用户没登陆可以别的方式确定，获取用户中的值只是众多方式的一种
     * 比如当前租户，用户登录后已用户所在租户为准，否则从当前请求获取
     */

    public interface ISession
    {
        object Get(string key);
    }

    public class Session : ISession
    {
        IServiceProvider serviceProvider;
        public Session(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public object Get(string key)
        {
            var r = serviceProvider.GetServices<ISession>();//.Where(d => d.GetType() != this.GetType());
            //遍历r，找到第一个不为空的
            foreach (var d in r)
            {
                if (d.GetType() == GetType())
                    continue;

                var r2 = d.Get(key);
                if (r2 != null)
                    return r2;
            }
            return null;
        }
    }

    public static class SessionExtensions
    {
        /// <summary>
        /// 从cookie、请求头或querystring中获取appkey
        /// 关键字是：appkey（不区分大小写）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string? GetAppKey(this ISession request)
        {
            return request.Get("appKey")?.ToString();
        }
        public static T? Get<T>(this ISession request, string key)
            where T : struct
        {
            var r = request.Get(key);
            //if (r.IsNullOrWhiteSpaceBXJG())
            //    return null;

            try
            {
                return (T)Convert.ChangeType(r, typeof(T));
            }
            catch
            {
                return null;
            }
        }
        public static string? GetString(this ISession request, string key)
        {
            var r = request.Get(key);
            return r?.ToString();
        }
    }
}
