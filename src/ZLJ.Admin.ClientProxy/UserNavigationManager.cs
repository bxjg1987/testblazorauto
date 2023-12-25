using Abp;
using Abp.Application.Navigation;
using System.Net.Http.Json;
using ZLJ.Application.Common.ClientProxy;

namespace ZLJ.Admin.ClientProxy
{
    public class UserNavigationManager : BaseAppServiceClient,IUserNavigationManager
    {
        public UserNavigationManager(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }


        public async Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier user)
        {
            return await CreateHttpClient().GetFromJsonAsync<UserMenu>("AbpUserConfiguration/getall");
        }

        public async Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier user)
        {
            return await CreateHttpClient().GetFromJsonAsync<IReadOnlyList<UserMenu>>("AbpUserConfiguration/getall");
        }
    }
}
