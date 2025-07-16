using ZLJ.Application.Share.TestTree;

namespace ZLJ.Admin.CoreRCL.TestTree
{
    public partial class DetailUpdate
    {
        //[Parameter]
        //public object Master {  get; set; }

        protected override string FuncName => "测试树";
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;

        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await base.InitPermission(TestTreeApplicationShareConsts.PermissionNameUpdate,
                                      TestTreeApplicationShareConsts.PermissionNameDelete);
        }
    }
}