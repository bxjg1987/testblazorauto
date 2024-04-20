using Abp.Application.Services;
using Abp.Web.Models.AbpUserConfiguration;
using BXJG.Utils.Application.Share.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.ClientProxy
{
    public class SessionAppService 
    {
        HttpClient httpClient;
        public SessionAppService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateHttpClientCommon();
            Console.WriteLine( "qqqqqqqqq:"+ httpClient.BaseAddress);
        }

        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            //await Task.Delay (7000);
            return await httpClient.Post<GetCurrentLoginInformationsOutput>("Session/GetCurrentLoginInformations");
            //   return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        }
    }
}
