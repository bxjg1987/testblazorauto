using Abp.Application.Services.Dto;
using BXJG.Common;
using BXJG.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Rougamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TGetAllInput、TUpdateInput
     * 
     * 不使用MudForm，因为它使用了另外一套表单验证方式，懒得学，因为我们可能不用mud，
     * mudblazor中的输入控件本身也支持blazor原生的editform
     */

    /// <summary>
    /// 基于mudblazor和abp的通用新增页组件
    /// 查看详情和修改数据的抽象组件是单独定义的（因为要切换查看和编辑模式，所以定义在同一个组件中的），
    /// 查看详情和修改组件是对以后的数据进行查看和处理，而新增组件它是对数据从无到有的创建，因此分开定义的。
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public  class AbpCreateBaseComponent<TAppService,
                                           TEntityDto,
                                           TPrimaryKey,
                                           TGetAllInput,
                                           TCreateInput,
                                           TUpdateInput> : AbpBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : new()
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
        /// 新增时的模型
        /// </summary>
        protected TCreateInput? createDto;
        /// <summary>
        /// 编辑上下文
        /// </summary>
        protected EditContext? editContext;
        /// <summary>
        /// 自定义的验证消息存储器
        /// 推荐尽可能使用数据注释Attribute的验证，若有特殊验证可以订阅<see cref="editContext"/>的事件
        /// </summary>
        protected ValidationMessageStore validationMessageStore;
        /// <summary>
        /// 表单对象，之类界面中的EditForm应该使用ref关联此字段
        /// </summary>
        protected EditForm editForm;
        /// <summary>
        /// 创建初始的新增模型对象，默认通过无参构造函数反射创建
        /// </summary>
        /// <returns></returns>
        protected virtual ValueTask<TCreateInput> CreateDtoInstance()
        {
            return ValueTask.FromResult(/*Activator.CreateInstance<TCreateInput>()*/new TCreateInput());
        }
        /// <summary>
        /// 保存后是否继续新增
        /// </summary>
        protected bool saveAndContinue = false;
        /// <summary>
        /// 正在执行重置
        /// </summary>
        protected bool isReseting = false;
        ///// <summary>
        ///// 重置
        ///// </summary>
        ///// <returns></returns>
        //protected virtual  Task BtnResetClick()
        //{
        //    return SafelyExecuteAsync(async ()=>await ResetCore());
        //}
        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        [ExceptionInterceptor]
        protected virtual async Task BtnResetClick()
        {
            isReseting = true;
            try
            {
                //editContext.MarkAsUnmodified();
                //validationMessageStore.Clear();
                createDto = await CreateDtoInstance();
                editContext = new EditContext(createDto!);
                validationMessageStore = new ValidationMessageStore(editContext);
            }
            finally
            {
                isReseting = false;
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await BtnResetClick();
        }
        ////在异步中初始化表单相关信息，这样可以给子类一个机会去异步初始化CreateDto
        //protected override async Task OnInitializedAsync()
        //{
        //    await OnInitializedAsync();
        //    await Reset();
        //    // editContext.OnValidationRequested += EditContext_OnValidationRequested;
        //}

        //private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
        //{
        //    if (btnSaveDisabled)
        //        btnSaveDisabled = false;
        //    // throw new NotImplementedException();
        //}

        //protected override async Task OnInitialized2Async()
        //{
        //    //列表传递过来的dto信息没有详情中的dto多
        //    Model = await AppService.GetAsync(new EntityDto<TPrimaryKey>(Model.Id));
        //}

        #region 权限
        /// <summary>
        /// 是否有新增权限
        /// </summary>
        protected bool createIsGranted = true;
        /// <summary>
        /// 初始化权限状态
        /// </summary>
        /// <param name="createPermissionName"></param>
        /// <returns></returns>
        protected virtual async Task InitPermission(string? createPermissionName = default)
        {
            if (createPermissionName.IsNotNullOrWhiteSpaceBXJG())
                createIsGranted = await PermissionChecker.IsGrantedAsync(createPermissionName);
        }
        #endregion
        ///// <summary>
        ///// 保存按钮初始禁用
        ///// </summary>
        //protected bool btnSaveDisabled = true;
        /// <summary>
        /// 是否禁用保存按钮
        /// 没授权的根本不显示
        /// </summary>
        public virtual bool IsBtnSaveDisabled =>/* btnSaveDisabled && Saving ||*/ editContext == null || editContext.GetValidationMessages().Any();
        /// <summary>
        /// 正在保存...
        /// </summary>
        protected bool isSaving = false;

        /// <summary>
        /// 核心的保存逻辑
        /// </summary>
        /// <returns></returns>
        [ExceptionInterceptor]
        protected virtual async Task BtnSaveClick()
        {
            if (!editContext!.Validate())
                return;

            isSaving = true;
            try
            {
                var r = await AppService.CreateAsync(createDto);
                Snackbar.Add("新增成功！", Severity.Success);
                await AfterSave(r);
                if (saveAndContinue)
                    await BtnResetClick();
            }
            finally
            {
                isSaving = false;
            }
        }
        /// <summary>
        /// 保存后回调
        /// </summary>
        protected virtual ValueTask AfterSave(TEntityDto dto) => ValueTask.CompletedTask;
        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    editContext.OnValidationRequested -= EditContext_OnValidationRequested;
        //}
    }

    /// <summary>
    /// 基于mudblazor和abp的通用新增弹窗页组件
    /// 查看详情和修改数据的抽象组件是单独定义的（因为要切换查看和编辑模式，所以定义在同一个组件中的），
    /// 查看详情和修改组件是对以后的数据进行查看和处理，而新增组件它是对数据从无到有的创建，因此分开定义的。
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public class AbpMudCreateDialogBaseComponent<TAppService,
                                                 TEntityDto,
                                                 TPrimaryKey,
                                                 TGetAllInput,
                                                 TCreateInput,
                                                 TUpdateInput> : AbpMudCreateBaseComponent<TAppService,
                                                                                           TEntityDto,
                                                                                           TPrimaryKey,
                                                                                           TGetAllInput,
                                                                                           TCreateInput,
                                                                                           TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 当前弹窗对象
        /// </summary>
        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; private set; }
        /// <summary>
        /// 点击关闭按钮时执行
        /// </summary>
        protected virtual void Cancel() => MudDialog.Cancel();
        /// <summary>
        /// 新增成功后回调
        /// </summary>
        /// <param name="dto"></param>
        protected override ValueTask AfterSave(TEntityDto dto)
        {
            if (!saveAndContinue)
                MudDialog.Close(dto);
            return ValueTask.CompletedTask;
        }
    }
}