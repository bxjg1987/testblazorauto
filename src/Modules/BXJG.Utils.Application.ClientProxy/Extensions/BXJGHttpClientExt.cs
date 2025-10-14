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

using Abp.Domain.Repositories;
using Abp.Linq;
using System.Net.Http.Json;
using BXJG.Utils.Application.Share;

namespace System.Net.Http
{
    /// <summary>
    /// 结合abp和本框架对httpclient进行扩展
    /// </summary>
    public static class BXJGHttpClientExt
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
        /// 通常整个进程使用这个名称来配置和创建客户端，而不同应用有却别的地方，比如api基础地址，由对应的工厂方法去配置
        /// </summary>

        public static string HttpClientName = "BXJGUtilsClient";

        /// <summary>
        /// 创建一个httpclient实例，用于单个应用，所有应用的基础地址在启动时配置，这里的url指基础地址后，不同应用不同的部分
        /// </summary>
        /// <param name="_httpClientFactory"></param>
        /// <param name="url">每个应用的api前缀，如：api/services/admin</param>
        /// <returns></returns>
        public static HttpClient CreateBXJGUtils(this IHttpClientFactory _httpClientFactory, string url=default)
        {
            var hc = _httpClientFactory.CreateClient(HttpClientName);
            if (url.IsNotNullOrWhiteSpaceBXJG())
                hc.BaseAddress = new Uri(hc.BaseAddress, url);//请包装hc.BaseAddress要以/结尾
            return hc;
        }

        /// <summary>
        /// 通常为每个应用提供一个IHttpClientFactory的扩展方法，这里指定抽象组件中默认使用的哪一个
        /// 这样允许具体应用将自己的委托指定到这里，以改变抽象组件中默认的创建逻辑
        /// </summary>
        public static Func<IHttpClientFactory, HttpClient> DefaultFctory = f => f.CreateBXJGUtils();

        //也可以考虑实现自定义的IHttpClientFactory，不过扩展方法的方式更轻


        //static JsonSerializerSettings settings => BXJG.Utils.Application.Share.Consts.settings;


        #region 基本的post、get
        /// <summary>
        /// 发送一个get请求，并返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hc"></param>
        /// <param name="url"></param>
        /// <param name="qs">querystring参数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<T> Get<T>(this HttpClient hc, string url, object qs = default, CancellationToken cancellationToken = default)
        {
            //
            url = url.AddQueryString(qs);

            // Console.WriteLine(  System.Text.Json.JsonSerializer.Serialize(input));
            // Console.WriteLine(  url);

            var str = await hc.GetStringAsync(url);
            return System.Text.Json.JsonSerializer.Deserialize<T>(str,Consts.JsonSerializerOptions);
            //var x = CreateHttpClient().GetFromJsonAsync<T>(url, cancellationToken);
            //Console.WriteLine("返回对象");
            //try
            //{
            //    Console.WriteLine(x.GetValue<int>("totalCount"));
            //}
            //catch 
            //{
            //}

            //return x;
        }
        /// <summary>
        /// 发送一个get请求，不返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hc"></param>
        /// <param name="url"></param>
        /// <param name="qs">querystring参数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task Get(this HttpClient hc, string url, object qs = default, CancellationToken cancellationToken = default)
        {
            //
            url = url.AddQueryString(qs);

            // Console.WriteLine(  System.Text.Json.JsonSerializer.Serialize(input));
            // Console.WriteLine(  url);

            await hc.GetAsync(url, cancellationToken);
            //var x = CreateHttpClient().GetFromJsonAsync<T>(url, cancellationToken);
            //Console.WriteLine("返回对象");
            //try
            //{
            //    Console.WriteLine(x.GetValue<int>("totalCount"));
            //}
            //catch 
            //{
            //}

            //return x;
        }
        /// <summary>
        /// 发送一个post请求并返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hc"></param>
        /// <param name="url">相对url</param>
        /// <param name="ps">post的参数</param>
        /// <param name="qs">querystring参数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<T> Post<T>(this HttpClient hc, string url, object ps = default, object qs = default, CancellationToken cancellationToken = default)
        {
            url = url.AddQueryString(qs);
            //await Console.Out.WriteLineAsync("url");
            //await Console.Out.WriteLineAsync(url);
            var r = await hc.PostAsJsonAsync(url, ps, Consts.JsonSerializerOptions, cancellationToken);
            var str = await r.Content.ReadAsStringAsync();
            T xxx =  System.Text.Json.JsonSerializer.Deserialize<T>(str, Consts.JsonSerializerOptions);
            return xxx;
        }
        /// <summary>
        /// 发送一个post请求，不返回数据
        /// </summary>
        /// <param name="hc"></param>
        /// <param name="url"></param>
        /// <param name="ps"></param>
        /// <param name="qs"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task Post(this HttpClient hc, string url, object ps = default, object qs = default, CancellationToken cancellationToken = default)
        {
            url = url.AddQueryString(qs);
            await hc.PostAsJsonAsync(url, ps, cancellationToken);
        }
        #endregion


        #region 针对控制器的扩展
        /// <summary>
        /// post一个json格式的数据到controller/action，并获取返回值
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="client"></param>
        /// <param name="data">要提交的对象</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">action</param>
        /// <param name="qs">querystring参数</param>
        /// <param name="cancellationToken">异步取消对象</param>
        /// <returns>TResult</returns>
        public static Task<TResult> Post<TResult>(this HttpClient client, string controller, string action, object data, object qs = default, CancellationToken cancellationToken = default)
        {
            return client.Post<TResult>($"{controller}/{action}", data, qs, cancellationToken);
        }
        /// <summary>
        /// post一个json格式的数据到controller/action，不需要返回值
        /// </summary>
        /// <typeparam name="TDto">返回类型</typeparam>
        /// <param name="client"></param>
        /// <param name="data">要提交的对象</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">action</param>
        /// <param name="qs">querystring参数</param>
        /// <param name="cancellationToken">异步取消对象</param>
        /// <returns></returns>
        public static Task Post(this HttpClient client, string controller, string action, object data, object qs = default, CancellationToken cancellationToken = default)
        {
            return client.Post($"{controller}/{action}", data, qs, cancellationToken);
        }
        //我们的框架中，很少用get，如果需要请调用上面的Get基础扩展方法
        #endregion




        #region 普通数据的crud
        public static Task<TDto> Create<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod<TDto>();
            return client.Post<TDto>(controller, "create", data, default, cancellationToken);
        }
        public static Task<TDto> Update<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod<TDto>();
            return client.Post<TDto>(controller, "update", data, default, cancellationToken);
        }
        public static Task Delete(this HttpClient client, object data, string controller, CancellationToken cancellationToken = default)
        {
            return client.Post(controller, "delete", data, default, cancellationToken);
        }
        public static Task Delete<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod<TDto>();
            return client.Delete(data, controller, cancellationToken);
        }

        public static Task<BatchOperationOutput<TKey>> DeleteBatch<TKey>(this HttpClient client, object data, string controller, CancellationToken cancellationToken = default)
        {
            return client.Post<BatchOperationOutput<TKey>>(controller, $"DeleteBatch", data, default, cancellationToken);
        }
     
        public static Task<BatchOperationOutput<TKey>> DeleteBatch<TKey, TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod<TDto>();
            return client.DeleteBatch<TKey>(data, controller, cancellationToken);
        }

        //public static Task<BatchOperationOutput<TKey>> DeleteBatchTree<TKey>(this HttpClient client, object data, string controller, CancellationToken cancellationToken = default)
        //{
        //    return client.Post<BatchOperationOutput<TKey>>(controller, $"DeleteBatchTree", data, default, cancellationToken);
        //}

        //public static Task<BatchOperationOutput<TKey>> DeleteBatchTree<TKey, TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        //{
        //    controller = controller.NewMethod<TDto>();
        //    return client.DeleteBatchTree<TKey>(data, controller, cancellationToken);
        //}


        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="getInput">通常是entitydto<key></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<TDto> Get<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod<TDto>();
            return client.Post<TDto>(controller, "get", data, default, cancellationToken);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TDto">明细项目类型</typeparam>
        /// <param name="getInput">分页和条件参数</param>
        /// <param name="url">具体功能的字符串</param>
        /// <param name="cancellationToken">异步取消对象</param>
        /// <returns>分页数据</returns>
        public static Task<PagedResultDto<TDto>> GetAll<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod<TDto>();
            return client.Post<PagedResultDto<TDto>>(controller, "getAll", data, default, cancellationToken);
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
        public static Task<TDto> GetProvider<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod2<TDto>();
            return client.Post<TDto>(controller, "get", data, default, cancellationToken);
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="TDto">明细项目类型</typeparam>
        /// <param name="getInput">分页和条件参数</param>
        /// <param name="url">具体功能的字符串</param>
        /// <param name="cancellationToken">异步取消对象</param>
        /// <returns>分页数据</returns>
        public static Task<PagedResultDto<TDto>> GetAllProvider<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod2<TDto>();
            return client.Post<PagedResultDto<TDto>>(controller, "getall", data, default, cancellationToken);
        }
        #endregion


        #region 树形数据crud
        //这里是泛型抽象，所以树型数据的很多操作可以跟普通数据很多操作重合
        /// <summary>
        /// 移动树形节点
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="client"></param>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<TDto> Move<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod<TDto>();
            return client.Post<TDto>(controller, "move", data, default, cancellationToken);
        }

       
        ///// <summary>
        ///// 树形数据只有批量删除
        ///// </summary>
        ///// <param name="client"></param>
        ///// <param name="controller"></param>
        ///// <param name="data"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //public static Task DeleteTree<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        //{
        //    controller = controller.NewMethod<TDto>();
        //    return client.Delete(data, controller, cancellationToken);
        //}





        ///// <summary>
        ///// 树形数据只有批量删除
        ///// </summary>
        ///// <param name="client"></param>
        ///// <param name="controller"></param>
        ///// <param name="data"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //public static Task<BatchOperationOutputLong> DeleteBatch(this HttpClient client, object data, string controller, CancellationToken cancellationToken = default)
        //{
        //    return client.Post<BatchOperationOutputLong>(controller, $"deletebatch", data, default, cancellationToken);
        //}
        ///// <summary>
        ///// 树形数据只有批量删除
        ///// </summary>
        ///// <param name="client"></param>
        ///// <param name="controller"></param>
        ///// <param name="data"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //public static Task<BatchOperationOutputLong> DeleteBatchTree<TDto>(this HttpClient client, object data, string controller=default, CancellationToken cancellationToken = default)
        //{
        //    controller = controller.NewMethod<TDto>();
        //    return client.DeleteBatch(data, controller, cancellationToken);
        //}

        /// <summary>
        /// 获取不分页的列表
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="client"></param>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<List<TDto>> GetList<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod<TDto>();
            return client.Post<List<TDto>>(controller, "GetAll", data, default, cancellationToken);
        }
        #endregion

        #region 树形数据的provider
        /// <summary>
        /// 获取树形的下拉框数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Task<List<TDto>> GetTreeForSelect<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod2<TDto>();
            return client.Post<List<TDto>>(controller, "GetTreeForSelect", data, default, cancellationToken);
        }
        /// <summary>
        /// 获取扁平化的下拉框数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Task<List<TDto>> GetNodesForSelect<TDto>(this HttpClient client, object data, string controller = default, CancellationToken cancellationToken = default)
        {
            controller = controller.NewMethod2<TDto>();
            return client.Post<List<TDto>>(controller, "GetNodesForSelect", data, default, cancellationToken);
        }
        #endregion

        public static string NewMethod<TDto>(this string url)
        {
            if (url.IsNullOrWhiteSpace())
            {

                url = typeof(TDto).Name;
                url = url.Substring(0,url.Length-3);
            }

            return url;
        }
        public static string NewMethod2<TDto>(this string url)
        {
            if (url.IsNullOrWhiteSpace())
            {
                url = typeof(TDto).Name;
                url = url.Substring(0, url.Length - 3);
                //如果url不是provider结尾，则加上
                if (!url.EndsWith("Provider"))
                {
                    url += "Provider";
                }
                //url += "Provider";
            }
            return url;
        }
    }
}