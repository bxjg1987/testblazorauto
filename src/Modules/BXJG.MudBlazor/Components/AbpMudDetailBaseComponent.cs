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
    /// 基于mudblazor和abp的通用详情页组件，它包含查看详情页修改，以及此模式的切换
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
        public virtual TPrimaryKey Id { get; set; }
        /// <summary>
        /// 列表页传递过来的视图模型
        /// 与Id二选一
        /// </summary>
        [Parameter]
        public virtual TEntityDto Dto { get; set; }
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

        #region 权限
        /// <summary>
        /// 是否有新增权限
        /// </summary>
        protected bool createIsGranted = true;
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
        protected virtual async Task InitPermission(string createPermissionName = default, string updatePermissionName = default, string deletePermissionName = default)
        {
            if (createPermissionName.IsNotNullOrWhiteSpaceBXJG())
                createIsGranted = await PermissionChecker.IsGrantedAsync(createPermissionName);
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
        /// 是否显示保存按钮和进入只读模式的按钮
        /// </summary>
        protected virtual bool IsShowSave => IsEdit && updateIsGranted;
        /// <summary>
        /// 是否显示进入编辑模式的按钮
        /// </summary>
        protected virtual bool IsShowGoEdit => !IsEdit && updateIsGranted;
        /// <summary>
        /// 点击修改按钮时执行，点击后进入修改模式
        /// </summary>
        protected virtual void BtnGoEditClick() => IsEdit = true;
        /// <summary>
        /// 点击取消修改按钮时执行，点击后进入查看模式
        /// </summary>
        protected virtual void BtnBackEditClick() => IsEdit = false;
        /// <summary>
        /// 点击保存按钮时执行
        /// </summary>
        /// <returns></returns>
        protected virtual Task BtnSaveClick()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 点击删除按钮时执行
        /// </summary>
        /// <returns></returns>
        public virtual async Task BtnDeleteClick()
        {
            await SafelyExecuteAsync(async () =>
            {
                await AppService.DeleteAsync(new EntityDto<TPrimaryKey>(Id));
                ShowError($"删除成功！");
            });
        }
        /// <summary>
        /// 显示模型转换为编辑模型
        /// </summary>
        protected virtual void DtoMapToEditDto() => editDto = base.ObjectMapper.Map<TUpdateInput>(Dto);
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
        [Inject]
        public IDialogService DialogService { get; set; }
    }
}