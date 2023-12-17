using Abp.Application.Navigation;
using Abp.UI;
using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.ClientProxy.Extensions
{
    /// <summary>
    /// 对System.Net.Http中的类进行扩展
    /// </summary>
    public static class NetHttpExt
    {
        /// <summary>
        /// 从后端api获取数据，若成功则返回被包装的原始数据
        /// 若失败，则抛出用户友好异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public static async Task<T> GetFromJsonUnWrapAsync<T>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
        {
            var r = await httpClient.GetFromJsonAsync<AjaxResponse<T>>(url, cancellationToken);
            if (!r.Success)
                throw new UserFriendlyException(r.Error.Code, r.Error.Message, r.Error.Details);
            return r.Result;
        }

        /// <summary>
        /// 从后端api获取数据，若成功则返回被包装的原始数据
        /// 若失败，则抛出用户友好异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public static async Task<T> GetFromJsonUnWrapAsync<T>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
        {
            var r = await httpClient.PostAsJsonAsync<AjaxResponse<T>>(url, cancellationToken);
            if (!r.Success)
                throw new UserFriendlyException(r.Error.Code, r.Error.Message, r.Error.Details);
            return r.Result;
        }
    }
}
