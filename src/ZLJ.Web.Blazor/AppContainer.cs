using Abp.Web.Models.AbpUserConfiguration;
using ZLJ.Application.Common.Share.Session;

namespace ZLJ.Web.Blazor
{
    /// <summary>
    /// 应用全局状态容器
    /// </summary>
    public class AppContainer
    {
        public static readonly AppContainer App = new AppContainer();

        public AbpUserConfigurationDto AbpUserConfiguration { get; set; }

        public Task< GetCurrentLoginInformationsOutput> CurrentLoginInformations { get; set; }

        public  IServiceProvider Services { get; set; }
       // public UserInfo UserInfo { get; set; }

    }
}