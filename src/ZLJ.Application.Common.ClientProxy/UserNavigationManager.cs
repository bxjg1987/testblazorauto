using Abp;
using Abp.Application.Navigation;
using Abp.Web.Models;
using System.Net.Http.Json;
using ZLJ.Application.Common.ClientProxy;

namespace ZLJ.Admin.ClientProxy
{
    public class UserNavigationManager : BaseAppServiceClient, IUserNavigationManager
    {
        public UserNavigationManager(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }


        public async Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier user)
        {
            var r = await CreateHttpClient().GetFromJsonAsync<AjaxResponse<UserMenu>>($"api/services/common/UserNavigation/GetMenu?menuName={menuName}");
            return r.Result;
        }

        public async Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier user)
        {
            var r = await CreateHttpClient().GetFromJsonAsync<AjaxResponse<IReadOnlyList<UserMenu>>>("api/services/common/UserNavigation/GetMenus");

            return r.Result;
        }
    }
}