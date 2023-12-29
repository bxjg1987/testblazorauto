using Abp.Web.Models.AbpUserConfiguration;
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
    public class AbpUserConfigurationService : BaseAppServiceClient
    {
        public AbpUserConfigurationService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }

        public  Task<AbpUserConfigurationDto> GetAll()
        {
            return  Post<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
         //   return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        }
    }
}
