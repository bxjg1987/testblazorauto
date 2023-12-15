using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.ClientProxy
{
    public class BaseAppServiceClient
    {
        IHttpClientFactory _httpClientFactory;

        public BaseAppServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient CreateHttpClient()
        {
            return _httpClientFactory.CreateClient(Consts.ZLJ_ADMIN_HTTP_CLIENT_NAME);
        }


    }
}
