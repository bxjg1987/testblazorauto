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
    public class SessionAppService : BaseAppServiceClient//, IApplicationService
    {
        public SessionAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }

        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            //await Task.Delay (7000);
            return await Post<GetCurrentLoginInformationsOutput>("api/services/common/Session/GetCurrentLoginInformations");
            //   return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        }
    }
}
