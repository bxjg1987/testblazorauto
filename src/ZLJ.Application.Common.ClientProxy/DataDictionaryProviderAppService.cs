using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Application.Common.ClientProxy
{
    public class DataDictionaryProviderAppService : BaseAppServiceClient,IDataDictionaryProviderAppService
    {
        public DataDictionaryProviderAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public Task<IList<GeneralTreeComboboxDto>> GetNodesForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            throw new NotImplementedException();
        }

        public Task<IList<DataDictionaryForSelectDto>> GetTreeForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            return Post<IList<DataDictionaryForSelectDto>>("api/services/bxjgutils/DataDictionaryProvider/GetTreeForSelect", input);
        }

        //public Task<IList<GeneralTreeNodeDto>> GetTreeForSelectAsync(GeneralTreeGetForSelectInput input)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        //{
        //    return Post<GetCurrentLoginInformationsOutput>("api/services/common/Session/GetCurrentLoginInformations");
        //    //   return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        //}
    }
}
