using BXJG.Utils.Application.ClientProxy;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Application.Common.ClientProxy;

namespace System.Net.Http
{
    public static class AdminClient 
    {
        public static HttpClient CreateHttpClientAdmin(this IHttpClientFactory _httpClientFactory)
        {
            return _httpClientFactory.CreateBXJGUtils(  ZLJ.Application.Share.AdminConsts.baseUrl);
        }
    }
}
