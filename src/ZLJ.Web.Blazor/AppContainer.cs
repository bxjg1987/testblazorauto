using Abp.Web.Models.AbpUserConfiguration;

namespace ZLJ.Web.Blazor
{
    /// <summary>
    /// 应用全局状态容器
    /// </summary>
    public class AppContainer
    {
        public static readonly AppContainer App = new AppContainer();

        public AbpUserConfigurationDto AbpUserConfiguration { get; set; }

        public  IServiceProvider Services { get; set; }
       // public UserInfo UserInfo { get; set; }

    }
}