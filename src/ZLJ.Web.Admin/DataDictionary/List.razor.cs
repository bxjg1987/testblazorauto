using BXJG.AbpMudBlazor.Interceptor;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.DataDictionary
{
    public partial class List
    {
        protected override string FuncName => "数据字典";

        [ExceptionInterceptor]
        protected override async Task OnInitialized2Async()
        {
           // base.Logger.Debug($"业务逻辑 线程id：{Thread.CurrentThread.ManagedThreadId}");

            await InitPermission(BXJGUtilsConsts.GeneralTreeCreatePermissionName, BXJGUtilsConsts.GeneralTreeUpdatePermissionName, BXJGUtilsConsts.GeneralTreeDeletePermissionName);
            await base.OnInitialized2Async();
           // base.Logger.Debug($"业务逻辑2222222222 线程id：{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
