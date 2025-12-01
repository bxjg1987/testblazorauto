using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Runtime.Session
{
    public static class SessionExt
    {
        /// <summary>
        /// 获取指定claimType的值
        /// 通常用来提供不同应用用户的额外属性。
        /// 记得在UserClaimsPrincipalFactory 设置值 
        /// 不同应用的Application通常可以提供进一步简化的扩展方法。
        /// 通常全局数据过滤中提供针对特定范围的数据过滤。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="principalAccessor"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public static TKey GetValueByClaimType<TKey>(this IPrincipalAccessor principalAccessor, string claimType) {
            var userEmailClaim = principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
           // userEmailClaim.get
            if (string.IsNullOrEmpty(userEmailClaim?.Value))
                return default;

            //return userEmailClaim.Value.ToNullable<TKey>();
            // return (TKey)Convert.ChangeType(userEmailClaim.Value, typeof(TKey));
            var t = typeof(TKey);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (userEmailClaim.Value == null)
                {
                    return default;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (TKey)Convert.ChangeType(userEmailClaim.Value, t);
        }
    }
}
