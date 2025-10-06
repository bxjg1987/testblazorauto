using System.Net.Http;

namespace ZLJ.Admin.CoreRCL.AssociatedCompany
{
    public partial class DetailUpdate
    {
      
        //protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientAdmin();

        protected override string FuncName => "客户档案";
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;

        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await base.InitPermission(PermissionNames.BXJGBaseInfoAssociatedCompanyUpdate,
                                      PermissionNames.BXJGBaseInfoAssociatedCompanyDelete);
        }
    }
}
