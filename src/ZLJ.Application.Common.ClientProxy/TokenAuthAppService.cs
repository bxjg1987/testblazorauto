using BXJG.Utils.Application.ClientProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Auth;
using ZLJ.Application.Common.Share.Models.TokenAuth;

namespace ZLJ.Application.Common.ClientProxy
{
    public class TokenAuthAppService : ITokenAuthAppService
    {
        HttpClient httpClient;
        public TokenAuthAppService(IHttpClientFactory httpClientFactory) 
        {
            httpClient = httpClientFactory.CreateBXJGUtils();
        }

        public  Task<AuthenticateResultModel> Authenticate(AuthenticateModel input)
        {
            return httpClient.Post<AuthenticateResultModel>("api/TokenAuth/Authenticate", input);
        }


        //public Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        //{
        //    return Post<GetCurrentLoginInformationsOutput>("api/services/common/Session/GetCurrentLoginInformations");
        //    //   return await CreateHttpClient().GetFromJsonAsync<AbpUserConfigurationDto>("AbpUserConfiguration/getall");
        //}
    }
}
