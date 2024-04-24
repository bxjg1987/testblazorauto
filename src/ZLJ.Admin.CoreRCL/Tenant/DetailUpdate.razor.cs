using System.Net.Http;

namespace ZLJ.Admin.CoreRCL.Tenant
{
    public partial class DetailUpdate
    {
        [Parameter]
        public object Master { get; set; } = new object();
        protected override string FuncName => "角色岗位";
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await base.InitPermission(PermissionNames.AdminTenantUpdate, PermissionNames.AdminTenantDelete);
        }
    }
}
