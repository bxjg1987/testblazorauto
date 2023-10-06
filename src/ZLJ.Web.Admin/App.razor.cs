using BXJG.Common;
using Microsoft.Extensions.Logging;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin
{
    public partial class App
    {
        /// <summary>
        /// 必须使用这种方式，不能从服务中获取
        /// 它不需要级联，内层组件需要时，自己inject即可
        /// </summary>
        [Inject]
        public ISnackbar Snackbar { get; set; }
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
    }
}
