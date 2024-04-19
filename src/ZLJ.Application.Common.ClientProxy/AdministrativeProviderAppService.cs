using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Administrative;

namespace ZLJ.Application.Common.ClientProxy
{
    public class AdministrativeProviderAppService : BaseAppServiceClient, IAdministrativeProviderAppService
    {
        public AdministrativeProviderAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public Task<IList<GeneralTreeComboboxDto>> GetNodesForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            throw new NotImplementedException();
        }

        public Task<IList<AdministrativeDto>> GetTreeForSelectAsync(GetListInput input)
        {
            ///api/services/common/BaseInfoAdministrativeProvider/GetTreeForSelect
            return Post<IList<AdministrativeDto>>("api/services/common/BaseInfoAdministrativeProvider/GetTreeForSelect", input);
        }
    }
}
