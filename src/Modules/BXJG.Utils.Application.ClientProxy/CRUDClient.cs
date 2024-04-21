using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.ClientProxy
{
    /// <summary>
    /// 提供curd功能的代理服务
    /// </summary>
    public abstract class BaseClient
    {
        protected HttpClient client;
        protected BaseClient(IHttpClientFactory httpClientFactory)
        {
            client = CreateHttpClient(httpClientFactory);
        }
        /// <summary>
        /// 不同应用的代理服务应提供自己的实现
        /// </summary>
        /// <param name="hcf"></param>
        /// <returns></returns>
        protected virtual HttpClient CreateHttpClient(IHttpClientFactory hcf)
        {
            return hcf.CreateClient();
        }
    }

    /// <summary>
    /// 提供curd功能的代理服务
    /// </summary>
    public abstract class CRUDClient<TKey, TDto, TCreateDto, TEditDto> : BaseClient
    {
        protected CRUDClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }
        public Task<TDto> Create() { 
        
        }
    }
}
