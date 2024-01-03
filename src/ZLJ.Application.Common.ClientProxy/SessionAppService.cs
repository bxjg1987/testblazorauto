using Abp.Application.Services;
using Abp.Web.Models.AbpUserConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Session;

namespace ZLJ.Application.Common.ClientProxy
{
    public class SessionAppService : BaseAppServiceClient//, IApplicationService
    {
        public SessionAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }

        public Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            return Post<GetCurrentLoginInformationsOutput>("api/services/common/Session/GetCurrentLoginInformations");
            //   return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        }
    }
}
