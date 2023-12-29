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

        public Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier user)
        {
            return Post<UserMenu>($"api/services/common/UserNavigation/GetMenu?menuName={menuName}");//简单参数直接传递，免得浪费性能
        }

        public Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier user)
        {
            return Post<IReadOnlyList<UserMenu>>("api/services/common/UserNavigation/GetMenus");
        }
    }
}