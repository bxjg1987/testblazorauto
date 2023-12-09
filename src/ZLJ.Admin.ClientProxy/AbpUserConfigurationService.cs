using Abp.Web.Models.AbpUserConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.ClientProxy;

namespace ZLJ.Admin.ClientProxy
{
    public class AbpUserConfigurationService : BaseAppServiceClient
    {
        public AbpUserConfigurationService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }

        public async Task<AbpUserConfigurationDto> GetAll()
        {
            return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        }
    }
}
