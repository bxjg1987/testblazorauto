using Abp.Web.Models.AbpUserConfiguration;
using BXJG.Utils.Application.ClientProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.ClientProxy;

namespace ZLJ.Application.Common.ClientProxy
{
    //貌似客户端才需要此接口
    public class AbpUserConfigurationService //: BXJGBaseClient
    {
        HttpClient _httpClient;
        public AbpUserConfigurationService(IHttpClientFactory httpClientFactory) //: base(httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public  async Task<AbpUserConfigurationDto> GetAll()
        {
            //await Task.Delay(5000);
            return await _httpClient.Post<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
         //   return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        }
    }
}
