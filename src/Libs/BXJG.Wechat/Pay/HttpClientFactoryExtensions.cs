using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.Pay
{
    public static class HttpClientFactoryExtensions
    {
        public static HttpClient CreateClientForWX(this IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient(WXPayConst.HttpClientKey);
        }
    }
}
