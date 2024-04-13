using AntDesign.TableModels;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.RCL.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ZLJ.Admin.CoreRCL.DataDictionary
{
    public partial class List
    {
        long? parentId;
        void AddSub(long pid) {
            
            ShowCreateDialog();
            parentId = pid;
        }
        // string currOu;

        protected override string FuncName => "数据字典";
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await base.InitPermission(PermissionNames.AdministratorBaseInfoPostCreate, PermissionNames.AdministratorBaseInfoPostUpdate, PermissionNames.AdministratorBaseInfoPostDelete);

        }
        
        //protected override Task LoadListData()
        //{
        //    return base.LoadListData();
        //}

        //[AbpExceptionInterceptor]
        protected override async Task BtnClearFilterClick()
        {
            //GetAllInput.Filter.IsStatic = default;
            //GetAllInput.Filter.Permission = default;
            //GetAllInput.Filter.OuCode = default;
            //GetAllInput.IsOnlyLoadChild = false;
            GetAllInput.IsSysDefine = default;
         
            await base.BtnClearFilterClick();
        }
        
      

        // AbpCreateDialog<IPostAppService, PostDto, int, PagedAndSortedResultRequest<PagedPostResultRequestDto>, CreatePostDto, PostEditDto, Create> dalRef;
        Dictionary<string, object> OnRow(RowData<DataDictionaryDto> row)
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
            {
                //ant搞出了bug，必须重置下
               
                isCreated=false;
                await LoadListData(); 
            }
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
            parentId = default;
            //  var r = await dalRef.Show();
            //   if (r)
            //       await Reset();
        }
        /// <summary>
        /// 点击关闭新增弹窗时执行
        /// </summary>
        /// <returns></returns>
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        private async Task CloseDialog()
        {
            await CloseCreateDialogCore();
        }
        /// <summary>
        /// 新增后回调
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        async Task OnAddEnd(SaveResult<DataDictionaryDto> sr)
        {
            base.MicrosoftLogger.LogDebug($"新增事件触发了！！！");
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
        long detailUpdateId = 0;

        /// <summary>
        /// 新增后回调
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        async Task OnDetailUpdate(DataDictionaryDto sr)
        {
            isShowDetailUpdate = false;

            await LoadListData();

        }

        void OnEdit(DataDictionaryDto sr)
        {
            isEdit = true;
            detailUpdateId = sr.Id;
            isShowDetailUpdate = true;
        }

        void OnDetail(DataDictionaryDto sr)
        {
            isEdit = false;
            detailUpdateId = sr.Id;
            isShowDetailUpdate = true;
        }


        #endregion
    }
}
