


namespace ZLJ.Web.Admin.DataDictionary
{
    //[ExceptionInterceptor]
    public partial class List
    {
        protected override string FuncName => "数据字典";

    // [ExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
          //  base.CancellationTokenProvider
           // base.CancellationTokenSource
           // base.Logger.Debug($"业务逻辑 线程id：{Thread.CurrentThread.ManagedThreadId}");

           // throw new NotImplementedException();
          throw new UserFriendlyException("xxxxxxxxxxxxx");
            await InitPermission(BXJGUtilsConsts.GeneralTreeCreatePermissionName, BXJGUtilsConsts.GeneralTreeUpdatePermissionName, BXJGUtilsConsts.GeneralTreeDeletePermissionName);
            await base.OnInitializedAsync();
           // base.Logger.Debug($"业务逻辑2222222222 线程id：{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
