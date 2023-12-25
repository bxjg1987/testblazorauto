using Abp.Web.Models.AbpUserConfiguration;

namespace ZLJ.Admin.CoreRCL.Startup
{
    /// <summary>
    /// 应用全局状态容器
    /// </summary>
    public class AppContainer
    {
        public AbpUserConfigurationDto AbpUserConfiguration { get; set; }

       // public UserInfo UserInfo { get; set; }

    }
}