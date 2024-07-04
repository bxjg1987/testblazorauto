using AntDesign.TableModels;
using Microsoft.AspNetCore.Components.Web;
using ZLJ.Application.Share.TestSimple;

namespace ZLJ.Admin.CoreRCL.TestSimple
{
    /// <summary>
    /// 普通数据测试列表页逻辑
    /// </summary>
    public partial class List
    {
        /// <summary>
        /// 当前功能显示名称
        /// </summary>
        protected override string FuncName => "普通数据测试";
        /// <summary>
        /// 组件初始化
        /// </summary>
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            //通过另外的重装可以实现更多权限状态初始化
            await base.InitPermission(TestSimpleApplicationShareConsts.PermissionNameCreate,
                                      TestSimpleApplicationShareConsts.PermissionNameUpdate,
                                      TestSimpleApplicationShareConsts.PermissionNameDelete);
        }

        #region 查询列表

        /// <summary>
        /// 出生日期范围条件，适配antblazor的时间控件的范围
        /// </summary>
        public DateTime?[] BirthdayRange
        {
            get{
                return [ GetAllInput.Filter.BirthdayMin, GetAllInput.Filter.BirthdayMax ];
            }
            set{
                if(value.Length>=2)
                    GetAllInput.Filter.BirthdayMax = value[1];
                if(value.Length>=1)
                    GetAllInput.Filter.BirthdayMin = value[0];
            }
        }
                /// <summary>
        /// 表格中每行的额外属性
        /// </summary>
        Dictionary<string, object> OnRow(RowData<TestSimpleDto> row)
        {
            Action<MouseEventArgs> OnDblClick = args =>
            {
                if (updateIsGranted)
                    BtnItemDetailClick(row.DataItem.Data);
                else
                    BtnItemDetailClick(row.DataItem.Data);

                StateHasChanged();//经过测试，必须加
            };
            return new Dictionary<string, object>
            {
                { "ondblclick", OnDblClick },
            };
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
        /// <summary>
        /// 显示弹窗，通常绑定到顶部新增按钮的点击事件上
        /// </summary>
        public void BtnCreateClick()
        {
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
        void OnAddEnd(BXJG.Utils.RCL.Components.SaveResult<TestSimpleDto> sr)
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
        void DetailUpdated(TestSimpleDto sr)
        {
            isShowDetailUpdate = false;
            isNeedReload = true;
            //await LoadListData();
        }        
        /// <summary>
        /// 当详情页执行删除后回调
        /// </summary>
        /// <param name="dto"></param>
        void DetailDeleted(TestSimpleDto dto)
        {
            isNeedReload = true;
            isShowDetailUpdate = false;
        }
        /// <summary>
        /// 点击修改时
        /// </summary>
        /// <param name="sr"></param>
        void BtnItemEditClick(TestSimpleDto sr)
        {
            isEdit = true;
            ShowUpdateOrDetail(sr.Id);
        }
        /// <summary>
        /// 点击详情时
        /// </summary>
        /// <param name="sr"></param>
        void BtnItemDetailClick(TestSimpleDto sr)
        {
            isEdit = false;
            ShowUpdateOrDetail(sr.Id);
        }
        void ShowUpdateOrDetail(long id){
            detailUpdateId = id;
            isShowDetailUpdate = true;
        }
        void CloseUpdateOrDetail() {
            isShowDetailUpdate = false;
        }
        #endregion

        /// <summary>
        /// 新增或修改完成时需要执行，并且直接点击x关闭时也需要执行，所以通常将它绑定到弹窗的同名事件上
        /// </summary>
        Task AfterCloseDialog()
        {
            if (isNeedReload)
            {
                isNeedReload = false;
                LoadListData();
                //StateHasChanged();
            }
            return Task.CompletedTask;
        }
    }
}