using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Administrative;

namespace ZLJ.Application.Common.ClientProxy
{
    public class AdministrativeProviderAppService :  IAdministrativeProviderAppService
    {
        HttpClient httpClient;
        public AdministrativeProviderAppService(IHttpClientFactory httpClientFactory) 
        {
            httpClient = httpClientFactory.CreateHttpClientCommon();
        }

        public Task<IList<GeneralTreeComboboxDto>> GetNodesForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            throw new NotImplementedException();
        }

        public Task<IList<AdministrativeDto>> GetTreeForSelectAsync(GetListInput input)
        {
            ///api/services/common/BaseInfoAdministrativeProvider/GetTreeForSelect
            return httpClient.Post<IList<AdministrativeDto>>("BaseInfoAdministrativeProvider/GetTreeForSelect", input);
        }
    }
}
