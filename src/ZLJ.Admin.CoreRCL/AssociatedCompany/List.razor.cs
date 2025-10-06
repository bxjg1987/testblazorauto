using AntDesign.Core.Helpers;
using AntDesign.Core.Helpers.MemberPath;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components.Web;
using NUglify.Html;
using ZLJ.Application.Share.AssociatedCompany;

namespace ZLJ.Admin.CoreRCL.AssociatedCompany
{
    /// <summary>
    /// 
    /// </summary>
    public partial class List
    {
        protected override string FuncName => "客户档案";


        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await base.InitPermission(PermissionNames.BXJGBaseInfoAssociatedCompanyCreate,
                                      PermissionNames.BXJGBaseInfoAssociatedCompanyUpdate,
                                      PermissionNames.BXJGBaseInfoAssociatedCompanyDelete);
        }

        #region 查询列表
        Dictionary<string, object> OnRow(RowData<AssociatedCompanyDto> row)
        {
            Action<MouseEventArgs> OnDblClick = args =>
            {
                if (updateIsGranted)
                    BtnEditClick(row.DataItem.Data);
                else
                    BtnDetailClick(row.DataItem.Data);

                StateHasChanged();//经过测试，必须加
            };
            return new Dictionary<string, object>
            {
                { "ondblclick", OnDblClick },
            };
        }

        //protected override Task LoadCore()
        //{
        //    base.Sorting = base.Sorting.Replace("DisplayName asc","EquipmentInfo.");


        //    return base.LoadCore();
        //}

        protected override void BtnClearFilterClick()
        {
            GetAllInput.Filter.LevelId = default;
            GetAllInput.Filter.AreaCode = default;
            GetAllInput.Filter.IsActive = default;
            GetAllInput.Filter.Keywords = default;
            base.BtnClearFilterClick();
        }
        #endregion

        /// <summary>
        /// 是否需要刷新列表页面
        /// </summary>
        bool isNeedReload;

        #region 新增

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
        /// 显示弹窗，通常绑定到顶部新增按钮的点击事件上
        /// </summary>
        public void BtnAddClick()
        {
            //table.SetFieldOrPropertyValue("_shouldRender", false);
            
            var r = PathHelper.SetLambda<Table<AssociatedCompanyDto>, bool>("_shouldRender");
            r.Compile().Invoke(table, false);
            //r.Compile().Invoke(table, true);



            isCreateDialogVisible = true;
            //  var r = await dalRef.Show();
            //   if (r)
            //       await Reset();
        }
        /// <summary>
        /// 点击关闭新增弹窗时执行，通常绑定到底部关闭按钮的点击事件上
        /// </summary>
        /// <returns></returns>
        private void CloseCreateDialog()
        {
            isCreateDialogVisible = false;
        }
        /// <summary>
        /// 新增后回调
        /// 存在继续新增和结束新增的情况
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        void OnAddEnd(BXJG.Utils.RCL.Components.SaveResult<AssociatedCompanyDto> sr)
        {
            isNeedReload = true;
            if (sr.End)
            {
                CloseCreateDialog();
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
        long detailUpdateId;

        /// <summary>
        /// 修改完成的事件
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        void OnDetailUpdate(AssociatedCompanyDto sr)
        {
            isShowDetailUpdate = false;
            isNeedReload = true;
            //await LoadListData();
        }
        /// <summary>
        /// 点击修改时
        /// </summary>
        /// <param name="sr"></param>
        void BtnEditClick(AssociatedCompanyDto sr)
        {
            isEdit = true;
            ShowUpdateOrDetail(sr.Id);
        }
        /// <summary>
        /// 点击详情时
        /// </summary>
        /// <param name="sr"></param>
        void BtnDetailClick(AssociatedCompanyDto sr)
        {
            isEdit = false;
            ShowUpdateOrDetail(sr.Id);
        }
        void ShowUpdateOrDetail(long id)
        {
            detailUpdateId = id;
            isShowDetailUpdate = true;
        }
        void CloseUpdateOrDetail()
        {
            isShowDetailUpdate = false;
        }
        #endregion

        /// <summary>
        /// 关闭新增或修改详情弹窗时执行
        /// 新增或修改完成时需要执行，并且直接点击x关闭时也需要执行，所以通常将它绑定到弹窗的同名事件上
        /// </summary>
        Task OnAfterCloseDialog()
        {
            //base.MicrosoftLogger.LogDebug("关闭了......");
            //isShowDetailUpdate = false;
            if (isNeedReload)
            {
                isNeedReload = false;
                LoadListData();
                // StateHasChanged();
            }
            return Task.CompletedTask;
        }
    }
}
