
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using ZLJ.Application.Common.Share.Administrative;
using ZLJ.Application.Common.Share.OU;



namespace ZLJ.Admin.CoreRCL.Staffinfo
{
    public partial class DetailUpdate
    {

        //[Parameter]
        //public object Master {  get; set; }

        //IEnumerable<AdministrativeDto> ouds;

        //IEnumerable<OUSelectDto> ouds = new List<OUSelectDto>() { 
        //    new OUSelectDto{  Id=2, DisplayName="xxxxx" },
        //                new OUSelectDto{  Id=4, DisplayName="xxxxx2" },
        //                new OUSelectDto{  Id=5, DisplayName="xxxxx3" }
        //};
        protected override string FuncName => "员工";
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;
    
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await base.InitPermission(PermissionNames.BXJGBaseInfoStaffInfoUpdate,
                                      PermissionNames.BXJGBaseInfoStaffInfoDelete);
            //algorithm = (IAlgorithm)base.ScopedServices.GetService(AlgorithmContainer[editDto.RentalPlanType].AlgorithmType)!;

            // ouds = await HttpClientFactory.CreateHttpClientCommon().GetTreeForSelect<AdministrativeDto>(new { ParentName=string.Empty });
            //await Task.Delay(2000);
            //StateHasChanged();
        }

        //protected override void OnAfterRender(bool firstRender)
        //{
        //    Console.WriteLine("xxxxxxxxxxx"+IsShowUpdate);
        //    base.OnAfterRender(firstRender);
        //}
    }
}