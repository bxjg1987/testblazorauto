using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.txdl
{
    public class TXDLHttpClientFactory:Abp.Dependency.ISingletonDependency
    {
        private readonly IHttpClientFactory httpClientFactory;

        public TXDLHttpClientFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public HttpClient CreateHttpClient()
        {
            return httpClientFactory.CreateClient(TXDLCoreConsts.HttpClientName);
        }
    }
}
