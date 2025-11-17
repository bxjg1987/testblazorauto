

using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Admin.CoreRCL.OU
{
    public partial class DetailUpdate
    {
        //[Parameter]
        //public object Master { get; set; }
        //protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientUtils();

        protected override string FuncName => "部门";
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;
        //IEnumerable<OUSelectDto> ps= new List<OUSelectDto>();
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await base.InitPermission(BXJG.Utils.Application.Share.Auth.PermissionNames.GeneralTreeUpdatePermissionName,
                                      BXJG.Utils.Application.Share.Auth.PermissionNames.GeneralTreeDeletePermissionName);
            //ps = await HttpClientFactory.CreateHttpClientCommon().GetTreeForSelect<OUSelectDto>(new { ParentName = string.Empty });
        }
    }
}
