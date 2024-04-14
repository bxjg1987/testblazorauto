using Abp.Web.Models.AbpUserConfiguration;
using BXJG.Utils.Application.Share.Session;

namespace BXJG.Utils.RCL
{
    /// <summary>
    /// 应用全局状态容器
    /// </summary>
    public class AppContainer
    {
        public static readonly AppContainer App = new AppContainer();

        public Task T1, T2;

        public AbpUserConfigurationDto AbpUserConfiguration { get; set; }

        public GetCurrentLoginInformationsOutput CurrentLoginInformations { get; set; }

        public IServiceProvider Services { get; set; }
        // public UserInfo UserInfo { get; set; }

    }
}