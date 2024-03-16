using Abp.Application.Navigation;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Json;

namespace ZLJ.Application.Common.ClientProxy
{
    public class BaseAppServiceClient
    {
        /// <summary>
        /// 配置需要跟后端api匹配
        /// </summary>
        public static readonly  JsonSerializerSettings settings = new JsonSerializerSettings
        {
            //// 设置时间格式
            //DateFormatString = "yyyy-MM-dd HH:mm:ss",

            // 忽略循环引用
           // ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            // 数据格式首字母小写
             ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
            //{
            //    NamingStrategy = new CamelCaseNamingStrategy()
            //}
        // 数据格式按原样输出
        // ContractResolver = new DefaultContractResolver(),

        // 忽略空值
        //  NullValueHandling = NullValueHandling.Ignore
    };


        IHttpClientFactory _httpClientFactory;

        public BaseAppServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient CreateHttpClient()
        {
            return _httpClientFactory.CreateClient(Consts.ZLJ_ADMIN_HTTP_CLIENT_NAME);
        }

        protected async Task<T> Get<T>(string url, object ps=default, CancellationToken cancellationToken=default) {
            //
            url = url.AddQueryString(ps);

            // Console.WriteLine(  System.Text.Json.JsonSerializer.Serialize(input));
            // Console.WriteLine(  url);

            var str = await CreateHttpClient().GetStringAsync(url, cancellationToken);
            return JsonConvert.DeserializeObject<T>(str,settings);
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

        protected async Task<T> Post<T>(string url, object ps = default, object qs=default, CancellationToken cancellationToken = default)
        {
            url = url.AddQueryString(qs);

            var r = await CreateHttpClient().PostAsJsonAsync(url,ps, cancellationToken);
            var str = await r.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<T>(str, settings);
        }
        protected async Task Post(string url, object ps = default, object qs = default, CancellationToken cancellationToken = default)
        {
            url = url.AddQueryString(qs);

           await CreateHttpClient().PostAsJsonAsync(url, ps, cancellationToken);
           
        }
    }
}
