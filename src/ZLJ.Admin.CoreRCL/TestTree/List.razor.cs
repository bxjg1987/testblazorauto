using AntDesign.TableModels;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.RCL.Components;
using Microsoft.AspNetCore.Components.Web;
using ZLJ.Application.Share.TestTree;

namespace ZLJ.Admin.CoreRCL.TestTree
{
    /// <summary>
    /// 后台管理应用 测试树列表 逻辑
    /// </summary>
    public partial class List
    {
        /// <summary>
        /// 当前功能显示名称
        /// </summary>
        protected override string FuncName => "测试树";
        /// <summary>
        /// 当前父节点
        /// </summary>
        long? parentId;
        /// <summary>
        /// 生命周期 初始化
        /// </summary>
        /// <returns></returns>
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            //通过另外的重装可以实现更多权限状态初始化
            await base.InitPermission(TestTreeApplicationShareConsts.PermissionNameCreate,
                                      TestTreeApplicationShareConsts.PermissionNameUpdate,
                                      TestTreeApplicationShareConsts.PermissionNameDelete);
        }

        /// <summary>
        /// 出生日期范围条件，适配antblazor的时间控件的范围
        /// </summary>
        public DateTime?[] BirthdayRange
        {
            get{
                return [ GetAllInput.BirthdayMin, GetAllInput.BirthdayMax ];
            }
            set{
                if(value.Length>=2)
                    GetAllInput.BirthdayMax = value[1];
                if(value.Length>=1)
                    GetAllInput.BirthdayMin = value[0];
            }
        }
        
        //让条件实现IReset接口去实现
        ///// <summary>
        ///// 点击清空条件时执行
        ///// </summary>
        //protected override void BtnClearFilterClick()
        //{
        //    //若条件对象有更多属性，应该在这里清空
        //    GetAllInput.IsSysDefine = default;
        //    base.BtnClearFilterClick();
        //}

        /// <summary>
        /// 绑定到明细行上的属性
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        Dictionary<string, object> Row(RowData<TestTreeDto> row)
        {
            Action<MouseEventArgs> OnDblClick = args =>
            {
                //if (updateIsGranted)
                //    BtnItemEditClick(row.DataItem.Data);
                //else
                    BtnItemDetailClick(row.DataItem.Data);

                StateHasChanged();//经过测试，必须加这个
            };
            return new Dictionary<string, object>
            {
                { "ondblclick", OnDblClick },
            };
        }

        /// <summary>
        /// 是否需要重新加载列表
        /// 各自弹窗改变实体后都会将这个属性设置为true，弹窗关闭时根据此状态决定是否重新加载列表数据
        /// </summary>
        bool isNeedReload;

        /// <summary>
        /// 新增或修改完成时需要执行，并且直接点击x关闭时也需要执行，所以通常将它绑定到弹窗的同名事件上
        /// </summary>
        /// <returns></returns>
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
        #region 新增
        /*
         * 结合blazor8的section时，新增弹窗太简单，不用单独封装弹窗组件，也便于传参到新增表单中
         * 也不要放抽象组件中，因为抽象组件是多个应用共享的，它们可能不用弹窗
         */
        /// <summary>
        /// 新增弹窗是否显示
        /// </summary>
        bool isShowCreateDialog;

        /// <summary>
        /// 显示弹窗
        /// </summary>
        public void BtnCreateClick()
        {
            parentId = default;
            isShowCreateDialog = true;
            //  var r = await dalRef.Show();
            //   if (r)
            //       await Reset();
        }

        /// <summary>
        /// 点击明细中的，添加子节点时执行
        /// </summary>
        /// <param name="pid"></param>
        void BtnAddSubClick(long pid)
        {
            BtnCreateClick();
            parentId = pid;
        }
        void BtnCloseCreateClick()
        {
            isShowCreateDialog = false;
        }

        /// <summary>
        /// 新增后回调
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        void AddEnd(SaveResult<TestTreeDto> sr)
        {
            isNeedReload = true;
            isShowCreateDialog = !sr.End;
        }
        #endregion

        #region 详情和修改
        /// <summary>
        /// 是否显示详情和修改弹窗
        /// </summary>
        bool isShowDetailUpdateDialog;
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
        /// 点击明细中的修改按钮
        /// </summary>
        /// <param name="sr"></param>
        void BtnItemEditClick(TestTreeDto sr)
        {
            isEdit = true;
            detailUpdateId = sr.Id;
            isShowDetailUpdateDialog = true;
        }
        /// <summary>
        /// 点击明细中的详情事件
        /// </summary>
        /// <param name="sr"></param>
        void BtnItemDetailClick(TestTreeDto sr)
        {
            isEdit = false;
            detailUpdateId = sr.Id;
            isShowDetailUpdateDialog = true;
        }
        /// <summary>
        /// 当详情页执行删除后回调
        /// </summary>
        /// <param name="dto"></param>
        void DetailDeleted(TestTreeDto dto)
        {
            isNeedReload = true;
            isShowDetailUpdateDialog = false;
        }
        /// <summary>
        /// 详情中修改保存后执行
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        void DetailUpdated(TestTreeDto sr)
        {
            isNeedReload = true;
            isShowDetailUpdateDialog = false;
        }
        /// <summary>
        /// 点击关闭修改详情弹窗事件
        /// </summary>
        void BtnDetailUpdateCloseClick()
        {
            isShowDetailUpdateDialog = false;
        }
        #endregion
    }
}
