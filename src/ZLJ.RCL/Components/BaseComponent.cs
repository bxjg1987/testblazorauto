
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
        /// <summary>
        /// 界面消息服务
        /// </summary>
        [Inject]
        public IMessageService MessageService { get; set; }
      
        //await消息显示时，好像会等到消息因此时才结束，没严格测试
        //不过测试发现消息异步显示，并等待200毫秒，消息提示更丝滑
        //

        protected override async ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            _= MessageService.Error(msg);
            await Task.Delay(200);
        }
        protected override async ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            _ = MessageService.Success(msg);
            await Task.Delay(200);
        }
     
        
        #region 生命周期方法增加统一异常处理拦截器
        //肉夹馍不会拦截子类，但往往子类才是主要逻辑，才是出错的地方，所以父类加这个意义不大
        //[AbpExceptionInterceptor]
        //public override async Task SetParametersAsync(ParameterView parameters)
        //{
        //   await base.SetParametersAsync(parameters);
        //}
        //[AbpExceptionInterceptor]
        //protected override void OnParametersSet()
        //{
        //    base.OnParametersSet();
        //}
        //[AbpExceptionInterceptor]
        //protected override async Task OnParametersSetAsync()
        //{
        //    await base.OnParametersSetAsync();
        //}
        //[AbpExceptionInterceptor]
        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();
        //}
        //[AbpExceptionInterceptor]
        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();
        //}
        //[AbpExceptionInterceptor]
        //protected override void OnAfterRender(bool firstRender)
        //{
        //    base.OnAfterRender(firstRender);
        //}
        //[AbpExceptionInterceptor]
        //protected override Task OnAfterRenderAsync(bool firstRender)
        //{
        //    return base.OnAfterRenderAsync(firstRender);
        //}
        #endregion
    }
}
