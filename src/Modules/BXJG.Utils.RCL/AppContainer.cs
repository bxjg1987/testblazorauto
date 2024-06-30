using Abp.Web.Models.AbpUserConfiguration;
using BXJG.Utils.Application.Share.Session;
using Microsoft.AspNetCore.SignalR.Client;
using System.Dynamic;

namespace BXJG.Utils.RCL
{
    /*
     * 我们虽然使用的是blazor auto，但可以任意切换
     * 即便如此，本质上我们还是前后端分离的，blazor这边仅作为后端api的前端
     * 
     * 用户首次访问时需要获取一些应用全局状态+当前用户的全局状态
     * 这些状态变动几率不大，可以认为首次加载页面到下次刷新期间这些状态不会变动。
     * 
     * 在abp中，对应的是AbpUserConfigurationDto、GetCurrentLoginInformationsOutput
     * 前者是当前应用+当前用户的全局状态，后者是当前登录用户的基本信息
     * 这俩有些数据重合了，懒得优化了。
     * 
     * 注：http请求一个scope，blazorserver电路要给scope，组件inject时用的是电路scope，但owmbasecomponent是独立的scope
     * 
     * 可以直接使用ioc作为全局状态容器，为了同时适配wasm和server模式，应注册为scope
     * 当有多个这样的对象或后期增加时，每个对象都得去注册以便，还要考虑他们的初始化
     * 可能需要与后端多次交互。
     * 
     * 与其如此，结合上门说的这种状态的特征，我们定义一个小范围的对象，它仅存储这种很小变化的状态
     * 将来需要加新的状态时，这里多个属性，后端统一接口增加多一个数据的获取，
     * 一次性加载所有这种属性。
     */

    ///// <summary>
    ///// 应用全局状态容器
    ///// </summary>
    //public class AppContainer : DynamicObject//,IDictionary<string, object>
    //{
    //    //private readonly Dictionary<string, object> _;
    //    // public Task Task { get; set; }
    //    //public static readonly AppContainer App = new AppContainer();

    //    //// public Task T1, T2;
    //    ///// <summary>
    //    ///// 没登陆时，仅加载一部分数据
    //    ///// 登录后，加载完整数据
    //    ///// </summary>
    //    //public Task<AbpUserConfigurationDto> AbpUserConfiguration { get; set; }
    //    ///// <summary>
    //    ///// 仅登录后才有数据
    //    ///// </summary>
    //    //public Task<GetCurrentLoginInformationsOutput> CurrentLoginInformations { get; set; }


    //    //public Lazy< Task<IEnumerable<string>> >
    //    //public IServiceProvider Services { get; set; }
    //    //// public UserInfo UserInfo { get; set; }
    //    ///// <summary>
    //    ///// 当前公共的全局的signalR连接
    //    ///// </summary>
    //    //public HubConnection CommonHubConnection { get; internal set; }
    //}


}