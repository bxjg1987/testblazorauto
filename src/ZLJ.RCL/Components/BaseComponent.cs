
using Abp.Application.Features;
using Abp.Configuration;
using Abp.Runtime.Session;
using Abp.Threading;
using BXJG.Common.Events;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.RCL.Interceptors;

namespace ZLJ.RCL.Components
{
    /// <summary>
    /// 抽象组件，提供abp相关和antblazor相关功能
    /// 不跨项目、跨应用 共享
    /// </summary>
    public abstract class BaseComponent : BXJG.Utils.RCL.Components.BaseComponent
    {

        ////别担心，抽象的curd provider都不会继承BaseComponent
        //protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientCommon();
        /// <summary>
        /// 界面消息服务
        /// </summary>
        [Inject]
        public IMessageService MessageService { get; set; }

        //await消息显示时，好像会等到消息因此时才结束，没严格测试
        //不过测试发现消息异步显示，并等待200毫秒，消息提示更丝滑
        //


        protected override async Task ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            _ = MessageService.Error(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(300);
        }
        protected override async Task ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            _ = MessageService.Success(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(300);
        }


        #region 生命周期方法增加统一异常处理拦截器
        /*
         * 肉夹馍的aop有基于规则的匹配方式，但有点复杂，
         * 还是决定使用硬编码方式配置，比较稳妥。即 哪里需要就在哪里加 [AbpExceptionInterceptor]
         * 
         * 父类加了，子类再加这个特征的话会重复，会比较浪费。但是父类不加，如果子类没重写并加拦截器，会导致拦截器无法执行。
         * 所以还是决定在抽象中添加，子类可以重写时不调用父类，自己单独加 [AbpExceptionInterceptor]
         * 最坏的情况是子类重写，且必须调用父类方法时，确实比较浪费，层次不深的话也无所谓了。
         */
#if !DEBUG
        [AbpExceptionInterceptor]
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }
        [AbpExceptionInterceptor]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        [AbpExceptionInterceptor]
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
        [AbpExceptionInterceptor]
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        [AbpExceptionInterceptor]
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        [AbpExceptionInterceptor]
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }
#endif
        #endregion
    }
}
