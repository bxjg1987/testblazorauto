
using BXJG.Utils.Application.Share.Auth;

namespace ZLJ.Admin.CoreRCL.DataDictionary
{
    public partial class DetailUpdate
    {
        [Parameter]
        public object Master { get; set; }
        protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientUtils();

        protected override string FuncName => "数据字典";
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;

#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await base.InitPermission(PermissionNames.GeneralTreeUpdatePermissionName,
                                      PermissionNames.GeneralTreeDeletePermissionName);
        }
    }
}
