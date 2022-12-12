using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class AspNetCoreHttpExtensions
    {
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
                if (request.Headers.TryGetValue("Sec-Fetch-Mode", out var cors))
                {
                    if (cors == "cors")
                        return true;
                }

              return request.Headers["X-Requested-With"] == "XMLHttpRequest";//|| request.Headers.ContainsKey("Sec-Fetch-Dest");
            }

            return false;
        }
    }
}
