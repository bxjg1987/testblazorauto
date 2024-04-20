using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Application.Common.ClientProxy
{
    public class DataDictionaryProviderAppService : IDataDictionaryProviderAppService
    {
        HttpClient httpClient;
        public DataDictionaryProviderAppService(IHttpClientFactory httpClientFactory) 
        {
            httpClient = httpClientFactory.CreateHttpClientUtils();
        }

        public Task<IList<GeneralTreeComboboxDto>> GetNodesForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            throw new NotImplementedException();
        }

        public Task<IList<DataDictionaryForSelectDto>> GetTreeForSelectAsync(GeneralTreeGetForSelectInput input)
        {
            return httpClient.Post<IList<DataDictionaryForSelectDto>>("DataDictionaryProvider/GetTreeForSelect", input);
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
