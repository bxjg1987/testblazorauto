using Abp.Runtime.Session;
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
        /// 获取当前客户id
        /// </summary>
        /// <param name="abpSession"></param>
        /// <returns></returns>
        public static long? GetCustomerId(this IAbpSession abpSession)
        {
            var sdfdf = abpSession as ClaimsAbpSession;
            return sdfdf.GetFieldOrPropertyValue<IPrincipalAccessor>("PrincipalAccessor").GetValueByClaimType<long>("customerId");
        }
    }
}
