using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.Share.OU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;
using GetListInput = ZLJ.Application.Common.Share.OU.GetListInput;

namespace ZLJ.Application.Common.ClientProxy
{
    public class OuProviderAppService : IOUSelectProviderAppService
    {
        HttpClient httpClient;
        public OuProviderAppService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateHttpClientCommon();
        }
        public Task<IList<GeneralTreeComboboxDto>> GetNodesForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            throw new NotImplementedException();
        }

        public Task<IList<OUSelectDto>> GetTreeForSelectAsync(GetListInput input)
        {
            return httpClient.Post<IList<OUSelectDto>>("OuProvider/GetTreeForSelect", input);
        }

        //public Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        //{
        //    return Post<GetCurrentLoginInformationsOutput>("api/services/common/Session/GetCurrentLoginInformations");
        //    //   return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        //}
    }
}
