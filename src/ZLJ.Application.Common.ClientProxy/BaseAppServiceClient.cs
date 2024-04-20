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
using BXJG.Utils.Application.ClientProxy;

namespace System.Net.Http
{

    public static class CommonClient
    {
        public static HttpClient CreateHttpClientUtils(this IHttpClientFactory _httpClientFactory)
        {
            return _httpClientFactory.CreateHttpClient(ZLJ.Application.Common.Share.Consts.baseUrl_bxjg);
        }
        public static HttpClient CreateHttpClientCommon(this IHttpClientFactory _httpClientFactory)
        {
            return _httpClientFactory.CreateHttpClient( ZLJ.Application.Common.Share.Consts.baseUrl);
        }
    }
}
