using BXJG.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.MudBlazor.Components
{
    /// <summary>
    /// 基于mudblazor和abp的组件
    /// </summary>
    public class AbpMudBlazorBaseComponent : AbpBaseComponent
    {
        //private ISnackbar snackbar;

        /// <summary>
        /// 经过测试，它是Scope的生命周期，且使用ScopedServices.GetRequiredService方式不能用
        /// </summary>
        [Inject]
        protected virtual ISnackbar Snackbar { get; private set; }// => snackbar ??= ScopedServices.GetRequiredService<ISnackbar>();

        public override ValueTask ShowErrorAsync(string msg)
        {
            Snackbar.Add(msg, Severity.Error);
            return ValueTask.CompletedTask;
        }
        public override void ShowError(string msg)
        {
            Snackbar.Add(msg, Severity.Error);
        }

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
