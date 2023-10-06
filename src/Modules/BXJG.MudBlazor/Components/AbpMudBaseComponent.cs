using BXJG.AbpMudBlazor.Interceptor;
using BXJG.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpMudBlazor.Components
{
    /// <summary>
    /// 基于mudblazor和abp的组件
    /// </summary>
    public class AbpMudBaseComponent : AbpBaseComponent
    {
        //private ISnackbar snackbar;

        /// <summary>
        /// 经过测试，它是Scope的生命周期，且使用ScopedServices.GetRequiredService方式不能用
        /// </summary>
        [Inject]
        protected virtual ISnackbar Snackbar { get; private set; }// => snackbar ??= ScopedServices.GetRequiredService<ISnackbar>();
        [ExceptionInterceptor]
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        [ExceptionInterceptor]
        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
        }
        [ExceptionInterceptor]
        protected override void OnParametersSet()
        {
            // base.OnParametersSet();
        }
        [ExceptionInterceptor]
        protected override Task OnParametersSetAsync()
        {
            return Task.CompletedTask;
        }
        [ExceptionInterceptor]
        protected override Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }
        [ExceptionInterceptor]
        protected override void OnAfterRender(bool firstRender)
        {
            //  base.OnAfterRender(firstRender);
        }
        [ExceptionInterceptor]
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }

        //public override ValueTask ShowErrorAsync(string msg)
        //{
        //    Snackbar.Add(msg, Severity.Error);
        //    return ValueTask.CompletedTask;
        //}
        //public override void ShowError(string msg)
        //{
        //    Snackbar.Add(msg, Severity.Error);
        //}

        ///// <summary>
        ///// 显示成功的消息，木有考虑本地化
        ///// </summary>
        ///// <param name="opt"></param>
        ///// <param name="msg"></param>
        //public virtual void ShowSuccess(string opt = "操作", string msg = "{0}成功！")
        //{
        //    Snackbar.Add(string.Format(msg, opt), Severity.Success);
        //}
    }
}