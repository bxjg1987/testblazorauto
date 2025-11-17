
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace ZLJ.Admin.CoreRCL.Staffinfo
{
    public partial class DetailUpdate
    {
       
        //[Parameter]
        //public object Master {  get; set; }

        protected override string FuncName => "合同套餐";
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

           
        }

     

      
    }
}