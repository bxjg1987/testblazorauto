using BootstrapBlazor.Components;
using BXJG.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.BootstrapServer.Shared
{
    public partial class App
    {
        ///// <summary>
        ///// 必须使用这种方式，不能从服务中获取
        ///// 它不需要级联，内层组件需要时，自己inject即可
        ///// </summary>
        //[Inject]
        //public ISnackbar Snackbar { get; set; }
        /// <summary>
        /// 级联传递给所有内层组件，以便在当前界面共享一个事件总线，用于界面解耦通信
        /// </summary>
        Zhongjie zhongjie;
        [Inject]
        public ILoggerFactory loggerFactory { get; set; }
        protected override void OnInitialized()
        {
            zhongjie = new Zhongjie(loggerFactory);
            //行不通，blazor server 控件事件并不一定在主线程中
            // Zhongjie.Current.Value = zhongjie;
            // BXJG.AbpMudBlazor.GloableStatic.Snackbar.Value = Snackbar;
        }

        public void Dispose()
        {
            //
            //  Zhongjie.Current.Value= null;
            // BXJG.AbpMudBlazor.GloableStatic.Snackbar.Value = null;
            zhongjie.Zhuxiao();
        }
        [Inject]
        protected MessageService MessageService { get; private set; }
        [Inject]
        protected Castle.Core.Logging.ILoggerFactory LoggerFactory {get; private set;}
        private async Task OnErrorHandleAsync(ILogger logger, Exception ex)
        {
            var l = LoggerFactory.Create("App");
            if (ex is UserFriendlyException uex)
            {
                await MessageService.Show(new MessageOption
                {
                    Color = Color.Danger,
                    Content = uex.Message,
                    ShowBorder = true,
                    ShowShadow = true
                });
            }
            else
            {
                l.Error("未处理异常！", ex);
                await MessageService.Show(new MessageOption
                {
                    Color = Color.Danger,
                    Content = "发生未处理异常！请稍后重试，若多次失败，请联系系统管理员。",
                    ShowBorder = true,
                    ShowShadow = true
                });
            }
        }
    }
}
