using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Application.Common.Share.Session;

namespace ZLJ.Application.Common.ClientProxy
{
    public class OuProviderAppService : BaseAppServiceClient,IOuProviderAppService
    {
        public OuProviderAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public Task<IList<GeneralTreeComboboxDto>> GetNodesForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            throw new NotImplementedException();
        }

        public Task<IList<OuDto>> GetTreeForSelectAsync(GetListInput input)
        {
            return Post<IList<OuDto>>("api/services/common/OuProvider/GetTreeForSelect",input);
        }

        //public Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        //{
        //    return Post<GetCurrentLoginInformationsOutput>("api/services/common/Session/GetCurrentLoginInformations");
        //    //   return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        //}
    }
}
