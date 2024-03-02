using AntDesign.TableModels;
using Microsoft.AspNetCore.Components.Web;
using ZLJ.Application.Share.MultiTenancy;

namespace ZLJ.Admin.CoreRCL.Tenant
{
    public partial class List
    {
        protected override string FuncName => "租户";
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            //if ((await base.AppService.GetAllAsync(new PagedAndSortedResultRequest<PagedPostResultRequestDto> { SkipCount = 0, MaxResultCount = 1, Sorting = "role.Id", Filter = new PagedPostResultRequestDto() })).TotalCount >= 500)
            //{
            //    sj = true;
            //}
            await base.OnInitializedAsync();
            await base.InitPermission(PermissionNames.AdministratorBaseInfoPostCreate, PermissionNames.AdministratorBaseInfoPostUpdate, PermissionNames.AdministratorBaseInfoPostDelete);

        }

   
        protected override Task LoadListData()
        {
          //  GetAllInput.Sorting = $"role.{GetAllInput.Sorting.Replace("role.", "")}";//目前值考虑单列排序
            return base.LoadListData();
        }

        //[AbpExceptionInterceptor]
        protected override async Task BtnReLoadClick()
        {
            //GetAllInput.Filter.IsStatic = default;
            //GetAllInput.Filter.Permission = default;
            //GetAllInput.Filter.OuCode = default;
            if (ou != default)
                ou.Value = default;
            await base.BtnReLoadClick();
        }
        TreeSelectOu ou;
        //protected override Task OnQuery(QueryModel condition)
        //{
        //    if (currOu.IsNotNullOrWhiteSpaceBXJG())
        //        base.GetAllInput.Filter.OuCode = currOu.Split(',')[1];
        //    return base.OnQuery(condition);
        //}

        async Task OnOuChanged(string ou)
        {
            //OnSelectedItemChanged 会触发两次
            //ValueChanged 选择时只会触发一次，但清空时又会触发两次

            // Console.WriteLine(   System.Text.Json.JsonSerializer.Serialize(ou));
           // GetAllInput.Filter.OuCode = ou;// ou.IsNullOrWhiteSpaceBXJG() ? string.Empty : ou.Split(',')[1];
            await base.BtnSearchClick();
        }
        // AbpCreateDialog<IPostAppService, TenantDto, int, PagedAndSortedResultRequest<PagedPostResultRequestDto>, CreateTenantDto, PostEditDto, Create> dalRef;
        Dictionary<string, object> OnRow(RowData<TenantDto> row)
        {
            Action<MouseEventArgs> OnDblClick = args =>
            {
                OnDetail(row.DataItem.Data);
                StateHasChanged();
            };
            return new Dictionary<string, object>
            {
                { "ondblclick", OnDblClick },
            };
        }

        #region 新增
        /// <summary>
        /// 是否需要刷新列表页面
        /// </summary>
        bool isCreated;
        /// <summary>
        /// 关闭新增弹窗的核心逻辑
        /// </summary>
        /// <returns></returns>
        async Task CloseCreateDialogCore()
        {
            isCreateDialogVisible = false;
            if (isCreated)
                await BtnSearchClick();
        }
        /*
         * 结合blazor8的section时，新增弹窗太简单，不用单独封装弹窗组件，也便于传参到新增表单中
         * 也不要放抽象组件中，因为抽象组件是多个应用共享的，它们可能不用弹窗
         */

        /// <summary>
        /// 新增弹窗是否显示
        /// </summary>
        bool isCreateDialogVisible;
        // Guid addToolId = Guid.NewGuid(),addBodyId=Guid.NewGuid();
        //[AbpExceptionInterceptor]
        /// <summary>
        /// 显示弹窗
        /// </summary>
        public void ShowCreateDialog()
        {
            isCreateDialogVisible = true;
            //  var r = await dalRef.Show();
            //   if (r)
            //       await Reset();
        }
        /// <summary>
        /// 点击关闭新增弹窗时执行
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        private async Task CloseDialog()
        {
            await CloseCreateDialogCore();
        }
        /// <summary>
        /// 新增后回调
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        async Task OnAddEnd(SaveResult<TenantDto> sr)
        {
            isCreated = true;
            if (sr.End)
            {
                await CloseCreateDialogCore();
            }
        }

        #endregion

        #region 详情和修改
        /// <summary>
        /// 是否显示详情和修改弹窗
        /// </summary>
        bool isShowDetailUpdate;
        /// <summary>
        /// 当前详情弹窗是否是修改模式
        /// false查看模式 true修改模式
        /// </summary>
        bool isEdit;

        /// <summary>
        /// 当前详情或修改的实体的id
        /// </summary>
        int detailUpdateId = 0;

        /// <summary>
        /// 新增后回调
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        async Task OnDetailUpdate(TenantDto sr)
        {
            isShowDetailUpdate = false;

            await BtnSearchClick();

        }

        void OnEdit(TenantDto sr)
        {
            isEdit = true;
            detailUpdateId = sr.Id;
            isShowDetailUpdate = true;
        }

        void OnDetail(TenantDto sr)
        {
            isEdit = false;
            detailUpdateId = sr.Id;
            isShowDetailUpdate = true;
        }


        #endregion
    }
}
