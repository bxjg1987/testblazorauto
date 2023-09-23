using Abp.Application.Services.Dto;
using BXJG.Common;
using BXJG.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpMudBlazor.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TGetAllInput和TCreateInput
     */

    /// <summary>
    /// 基于mudblazor和abp的通用详情页组件，它包含查看详情页和修改，以及二者之间的切换
    /// 新增抽象组件是单独定义的，因为它是对数据从无到有的创建，而详情组件是对以后的数据进行查看和处理
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class AbpMudDetailBaseComponent<TAppService,
                                                    TEntityDto,
                                                    TPrimaryKey,
                                                    TGetAllInput,
                                                    TCreateInput,
                                                    TUpdateInput> : AbpMudBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 缓存当前主服务对象
        /// </summary>
        private TAppService? appService;
        /// <summary>
        /// 获取主服务
        /// </summary>
        protected virtual TAppService AppService => appService ??= ScopedServices.GetRequiredService<TAppService>();
        /// <summary>
        /// 此功能的名称
        /// </summary>
        protected virtual string FuncName => $"请重写{nameof(FuncName)}属性";
        /// <summary>
        /// 与Dto二选一
        /// </summary>
        [Parameter]
        public TPrimaryKey Id { get; set; }
        /// <summary>
        /// 列表页传递过来的视图模型
        /// 与Id二选一
        /// </summary>
        [Parameter]
        public TEntityDto Dto { get; set; }
        /// <summary>
        /// 当前编辑模型
        /// </summary>
        protected TUpdateInput editDto;
        ///// <summary>
        ///// 有时候数据比较复杂时，列表页中加载的数据只包含部分属性
        ///// 而详情页中需要更详细的信息，此时可能需要重新查询一次
        ///// </summary>
        ///// <returns></returns>
        //protected virtual async Task Reload()
        //{
        //    //列表传递过来的dto信息没有详情中的dto多
        //    Dto = await AppService.GetAsync(new EntityDto<TPrimaryKey>(Dto.Id));
        //}
        protected override async Task OnInitialized2Async()
        {
            if (!default(TPrimaryKey)!.Equals(Id))
            {
                Dto = await AppService.GetAsync(new EntityDto<TPrimaryKey>(Id));
            }
            else
            {
                Id = Dto.Id;
            }
            DtoMapToEditDto();
        }
        /// <summary>
        /// 显示模型转换为编辑模型，默认使用automapper
        /// </summary>
        protected virtual void DtoMapToEditDto() => editDto = base.ObjectMapper.Map<TUpdateInput>(Dto);

        #region 权限
        /// <summary>
        /// 是否有修改权限
        /// </summary>
        protected bool updateIsGranted = true;
        /// <summary>
        /// 是否有删除权限
        /// </summary>
        protected bool deleteIsGranted = true;
        /// <summary>
        /// 初始化权限状态
        /// </summary>
        /// <param name="createPermissionName"></param>
        /// <param name="updatePermissionName"></param>
        /// <param name="deletePermissionName"></param>
        /// <returns></returns>
        protected virtual async Task InitPermission( string updatePermissionName = default, string deletePermissionName = default)
        {
            if (updatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                updateIsGranted = await PermissionChecker.IsGrantedAsync(updatePermissionName);
            if (deletePermissionName.IsNotNullOrWhiteSpaceBXJG())
                deleteIsGranted = await PermissionChecker.IsGrantedAsync(deletePermissionName);
        }
        #endregion

        /// <summary>
        /// true修改模式，false查看模式
        /// </summary>
        [Parameter]
        public bool IsEdit { get; set; }
        /// <summary>
        /// true修改模式，false查看模式
        /// 参考：https://learn.microsoft.com/zh-cn/aspnet/core/blazor/components/overwriting-parameters?view=aspnetcore-6.0
        /// </summary>
        protected bool isEdit;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            isEdit = IsEdit;
        }
        #region 保存
        /// <summary>
        /// 是否显示保存按钮和进入只读模式的按钮
        /// </summary>
        protected virtual bool IsShowSave => isEdit && updateIsGranted;
        /// <summary>
        /// 是否显示进入编辑模式的按钮
        /// </summary>
        protected virtual bool IsShowGoEdit => !isEdit && updateIsGranted;
        /// <summary>
        /// 是否正在保存
        /// </summary>
        protected bool saving = false;
        /// <summary>
        /// 点击保存按钮时执行
        /// </summary>
        /// <returns></returns>
        protected virtual async Task BtnSaveClick()
        {
            saving = true;
            await SafelyExecuteAsync(async () =>
            {
                Dto = await AppService.UpdateAsync(editDto);
                Snackbar.Add("修改成功！", Severity.Success);
                AfterSave();
            });
            saving = false;
        }
        /// <summary>
        /// 保存后回调
        /// </summary>
        protected virtual void AfterSave() { }
        #endregion
        #region 删除
        /// <summary>
        /// 是否显示删除确认
        /// </summary>
        protected bool isShowDeleteConfirm = false;
        /// <summary>
        /// 是否正在删除
        /// </summary>
        protected bool deleting = false;
        /// <summary>
        /// 点击删除按钮时执行
        /// </summary>
        /// <returns></returns>
        protected virtual async Task BtnDeleteClick()
        {
            isShowDeleteConfirm = false;
            deleting = true;
            await SafelyExecuteAsync(async () =>
            {
                await AppService.DeleteAsync(new EntityDto<TPrimaryKey>(Id));
                ShowError($"删除成功！");
                AfterDelete();
            });
            deleting = false;
        }
        /// <summary>
        /// 删除之后执行的逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual void AfterDelete() { }
        #endregion
    }
    public abstract class AbpMudDetailDialogBaseComponent<TAppService,
                                                          TEntityDto,
                                                          TPrimaryKey,
                                                          TGetAllInput,
                                                          TCreateInput,
                                                          TUpdateInput> : AbpMudDetailBaseComponent<TAppService,
                                                                                                    TEntityDto,
                                                                                                    TPrimaryKey,
                                                                                                    TGetAllInput,
                                                                                                    TCreateInput,
                                                                                                    TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 当前弹窗对象
        /// </summary>
        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; private set; }
        /// <summary>
        /// 删除后回调，默认关闭弹窗
        /// </summary>
        protected override void AfterDelete()
        {
            MudDialog.Close(Dto);
        }
        /// <summary>
        /// 保存后回调，默认关闭弹窗
        /// </summary>
        protected override void AfterSave()
        {
            MudDialog.Close(Dto);
        }
        /// <summary>
        /// 点击关闭按钮时回调
        /// </summary>
        protected virtual void BtnCancelClick()
        {
            MudDialog.Cancel();
        }
        /// <summary>
        /// 开始编辑
        /// </summary>
        protected virtual void BeginEdit()
        {
            isEdit = true;
            MudDialog.SetTitle($"修改{FuncName}");
        }
        /// <summary>
        /// 放弃编辑
        /// </summary>
        protected virtual void CancelEdit()
        {
            isEdit = false;
            MudDialog.SetTitle($"查看{FuncName}详情");
        }
    }
}