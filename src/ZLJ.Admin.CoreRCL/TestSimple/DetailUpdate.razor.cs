
using ZLJ.Application.Share.TestSimple;

namespace ZLJ.Admin.CoreRCL.TestSimple
{
    public partial class DetailUpdate
    {
        [Parameter]
        public object Master {  get; set; }

        protected override string FuncName => "测试1";
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
            await base.InitPermission(TestSimpleApplicationShareConsts.PermissionNameUpdate,
                                      TestSimpleApplicationShareConsts.PermissionNameDelete);
        }
    }
}