using Abp;
using Abp.Application.Navigation;
using Abp.Web.Models;
using BXJG.Utils.RCL;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Net.Http.Json;

namespace BXJG.Utils.RCL.Abps
{
    public class ClientNavigationManager : /*BaseAppServiceClient,*/ IUserNavigationManager
    {
        AppContainer _appContainer;

        ILogger<ClientNavigationManager> logger;
        public ClientNavigationManager(AppContainer appContainer, ILogger<ClientNavigationManager> logger)
        {
            _appContainer = appContainer;
            this.logger = logger;
        }

        public async Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier user)
        {
            //Console.WriteLine("正在从api下载菜单");
            //Console.WriteLine( _appContainer.GetHashCode());
            //await Task.Delay(1);
            //Console.WriteLine(_appContainer.AbpUserConfiguration == default);

            //   var sdfsdf = _appContainer.GetHashCode();
            //   logger.LogDebug($"菜单初始化{_appContainer.AbpUserConfiguration == default}");
            //if (_appContainer.AbpUserConfiguration == default)
            //{
            //    return new UserMenu
            //    {
            //        DisplayName = "木有找到菜单",
            //        Name = "x",
            //        Items = new List<UserMenuItem>()
            //    }; 
            //}
  
            return _appContainer.AbpUserConfiguration?.Nav?.Menus?[menuName];
        }

        public async Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier user)
        {
            return _appContainer.AbpUserConfiguration.Nav.Menus.Select(x => x.Value).ToImmutableList();
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