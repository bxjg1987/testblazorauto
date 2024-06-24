using Abp.Web.Models.AbpUserConfiguration;
using BXJG.Utils.Application.Share.Session;
using Microsoft.AspNetCore.SignalR.Client;

namespace BXJG.Utils.RCL
{
    /// <summary>
    /// 应用全局状态容器
    /// </summary>
    public class AppContainer
    {
        public static readonly AppContainer App = new AppContainer();

        public Task T1, T2;
        /// <summary>
        /// 没登陆时，仅加载一部分数据
        /// 登录后，加载完整数据
        /// </summary>
        public AbpUserConfigurationDto AbpUserConfiguration { get; set; }
        /// <summary>
        /// 仅登录后才有数据
        /// </summary>
        public GetCurrentLoginInformationsOutput CurrentLoginInformations { get; set; }

        public IServiceProvider Services { get; set; }
        // public UserInfo UserInfo { get; set; }
        /// <summary>
        /// 当前公共的全局的signalR连接
        /// </summary>
        public HubConnection CommonHubConnection { get; internal set; }
    }
}