using Abp;
using Abp.Application.Navigation;
using Abp.Web.Models;
using System.Collections.Immutable;
using System.Net.Http.Json;
using ZLJ.Application.Common.ClientProxy;

namespace ZLJ.Web.Blazor.Abps
{
    public class ClientNavigationManager : /*BaseAppServiceClient,*/ IUserNavigationManager
    {
        AppContainer _appContainer;

        public ClientNavigationManager(AppContainer appContainer)
        {
            _appContainer = appContainer;
        }

        public Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier user)
        {
          return Task.FromResult(  _appContainer.AbpUserConfiguration.Nav.Menus[menuName]);
        }

        public async Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier user)
        {
            return  _appContainer.AbpUserConfiguration.Nav.Menus.Select(x=>x.Value).ToImmutableList();
        }

        //public UserNavigationManager(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        //{

        //}

        //public Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier user)
        //{
        //    return Post<UserMenu>($"api/services/common/UserNavigation/GetMenu?menuName={menuName}");//简单参数直接传递，免得浪费性能
        //}

        //public Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier user)
        //{
        //    return Post<IReadOnlyList<UserMenu>>("api/services/common/UserNavigation/GetMenus");
        //}


    }
}