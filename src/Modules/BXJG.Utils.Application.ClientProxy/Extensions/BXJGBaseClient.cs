using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Abp.Extensions;
using BXJG.Common.Contracts;
using BXJG.Common.Sundries;
using Abp.Application.Services.Dto;
using NUglify.JavaScript.Syntax;

namespace System.Net.Http
{
    /*
     * 客户端代理方式有两种
     * 1、使用单个代理类，定义所有接口
     * 2、按后端接口一一对应
     * 
     * 为了配合抽象组件，我们还是决定分开定义
     * 
     * 由于代理服务是分开定义，所以使用命名的httpclient更合理
     * 这样每个单独的代理服务都使用同一个httpclient
     * 另外也可以对httpclient进行扩展方法定义
     * 
     * 在utils app client proxy中定义最抽象的服务代理
     * Common中的不同服务都继承于它
     * 由于comon和admin用一样的httpclient名称，所以在utils定义，在admin应用启动阶段赋值
     * 因为不考虑同一进程承载多个应用的场景
     * 
     * 用扩展的形式对admin进行crud扩展，对common进行provider扩展，这样可以避免深层次的继承
     */

    public static class BXJGBaseClient
    {
        //    /// <summary>
        //    /// 配置需要跟后端api匹配
        //    /// </summary>
        //    public static readonly  JsonSerializerSettings settings = new JsonSerializerSettings
        //    {
        //        //// 设置时间格式
        //        //DateFormatString = "yyyy-MM-dd HH:mm:ss",

        //        // 忽略循环引用
        //       // ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

        //        // 数据格式首字母小写
        //         ContractResolver = new CamelCasePropertyNamesContractResolver(),
        //        //ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
        //        //{
        //        //    NamingStrategy = new CamelCaseNamingStrategy()
        //        //}
        //    // 数据格式按原样输出
        //    // ContractResolver = new DefaultContractResolver(),

        //    // 忽略空值
        //    //  NullValueHandling = NullValueHandling.Ignore
        //};
        ///// <summary>
        ///// 配置需要跟后端api匹配
        ///// </summary>
        //public static readonly JsonSerializerSettings settings = BXJG.Utils.Application.Share.Consts.settings;

        /// <summary>
        /// 反正不考虑一个进程挂多个应用，就一个客户端好了，写死就ok了
        /// </summary>

        public static string HttpClientName = "BXJGUtilsClient";

        //protected  IHttpClientFactory _httpClientFactory;

        //public BXJGBaseClient(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClientFactory = httpClientFactory;
        //}

        public static HttpClient CreateHttpClient(this IHttpClientFactory _httpClientFactory, string url = default)
        {
            var hc = _httpClientFactory.CreateClient(HttpClientName);
            if (url.IsNotNullOrWhiteSpaceBXJG())
                hc.BaseAddress = new Uri(hc.BaseAddress, url);
            return hc;
        }

        #region 普通数据的crud
        public static Task<TDto> Create<TDto>(this HttpClient client, object createDto, string url = default, CancellationToken cancellationToken = default)
        {
            url = NewMethod<TDto>(url);
            return client.Post<TDto>($"{url}/Create", createDto, default, cancellationToken);
        }
        public static Task<TDto> Update<TDto>(this HttpClient client, object updateDto, string url = default, CancellationToken cancellationToken = default)
        {
            url = NewMethod<TDto>(url);
            return client.Post<TDto>($"{url}/Update", updateDto, default, cancellationToken);
        }
        public static Task Delete(this HttpClient client, object input, string url, CancellationToken cancellationToken = default)
        {
            return client.Post($"{url}/Delete", input, default, cancellationToken);
        }
        public static Task<BatchOperationOutput<TKey>> DeleteBatch<TKey>(this HttpClient client, object input, string url, CancellationToken cancellationToken = default)
        {
            return client.Post<BatchOperationOutput<TKey>>($"{url}/DeleteBatch", input, default, cancellationToken);
        }
        /// <summary>
        /// 获取但个数据
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="getInput">通常是entitydto<key></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<TDto> Get<TDto>(this HttpClient client, object getInput, string url = default, CancellationToken cancellationToken = default)
        {
            url = NewMethod<TDto>(url);
            return client.Post<TDto>($"{url}/Get", getInput, default, cancellationToken);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TDto">明细项目类型</typeparam>
        /// <param name="getInput">分页和条件参数</param>
        /// <param name="url">具体功能的字符串</param>
        /// <param name="cancellationToken">异步取消对象</param>
        /// <returns>分页数据</returns>
        public static Task<PagedResultDto<TDto>> GetAll<TDto>(this HttpClient client, object getInput, string url = default, CancellationToken cancellationToken = default)
        {
            url = NewMethod<TDto>(url);
            return client.Post<PagedResultDto<TDto>>($"{url}/GetAll", getInput, default, cancellationToken);
        }
        #endregion
        #region 普通数据的provider
        /// <summary>
        /// 获取但个数据
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="getInput">通常是entitydto<key></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<TDto> GetProvider<TDto>(this HttpClient client, object getInput, string url = default, CancellationToken cancellationToken = default)
        {
            url = NewMethod<TDto>(url);
            return client.Post<TDto>($"{url}/Get", getInput, default, cancellationToken);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TDto">明细项目类型</typeparam>
        /// <param name="getInput">分页和条件参数</param>
        /// <param name="url">具体功能的字符串</param>
        /// <param name="cancellationToken">异步取消对象</param>
        /// <returns>分页数据</returns>
        public static Task<PagedResultDto<TDto>> GetAllProvider<TDto>(this HttpClient client, object getInput, string url = default, CancellationToken cancellationToken = default)
        {
            url = NewMethod<TDto>(url);
            return client.Post<PagedResultDto<TDto>>($"{url}/GetAll", getInput, default, cancellationToken);
        }
        #endregion

        //public static Task<T> Get<T>(string url, object ps = default, CancellationToken cancellationToken = default)
        //{
        //    return CreateHttpClient().Get<T>(url, ps, cancellationToken);
        //    //
        //    //url = url.AddQueryString(ps);

        //    //// Console.WriteLine(  System.Text.Json.JsonSerializer.Serialize(input));
        //    //// Console.WriteLine(  url);

        //    //var str = await CreateHttpClient().GetStringAsync(url);
        //    //return JsonConvert.DeserializeObject<T>(str,settings);
        //    //var x = CreateHttpClient().GetFromJsonAsync<T>(url, cancellationToken);
        //    //Console.WriteLine("返回对象");
        //    //try
        //    //{
        //    //    Console.WriteLine(x.GetValue<int>("totalCount"));
        //    //}
        //    //catch 
        //    //{
        //    //}

        //    //return x;
        //}

        //public static Task<T> Post<T>(string url, object ps = default, object qs = default, CancellationToken cancellationToken = default)
        //{
        //    return CreateHttpClient().Post<T>(url, ps, qs, cancellationToken);
        //    //url = url.AddQueryString(qs);

        //    //var r = await CreateHttpClient().PostAsJsonAsync(url,ps, cancellationToken);
        //    //var str = await r.Content.ReadAsStringAsync();
        //    //return JsonConvert.DeserializeObject<T>(str, settings);
        //}
        //public static Task Post(string url, object ps = default, object qs = default, CancellationToken cancellationToken = default)
        //{
        //    return CreateHttpClient().Post(url, ps, qs, cancellationToken);
        //    // url = url.AddQueryString(qs);

        //    //await CreateHttpClient().PostAsJsonAsync(url, ps, cancellationToken);

        //}

        public static string NewMethod<TDto>(string url)
        {
            if (url.IsNullOrWhiteSpace())
                url = typeof(TDto).Name.TrimEnd("Dto".ToCharArray());
            return url;
        }
    }
}