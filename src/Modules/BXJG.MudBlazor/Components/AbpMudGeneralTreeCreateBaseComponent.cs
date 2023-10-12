using BXJG.AbpMudBlazor.Interceptor;
using BXJG.Common.Dto;
using BXJG.Utils.GeneralTree;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
     * 原本这里不需要定义TEditDto、TGetAllInput泛型参数
     * 但abp原本的crudappservice里面是保护完整的curd的，没有分开定义
     * 我们的通用树服务也是保持这种设计方式的，
     * 这也是合理的，对于前后端分离方式来说很直观。
     * 
     * 所以这里强制引入这俩无用的泛型参数，方便 泛型应用服务注入。
     * 否则若去掉这俩泛型参数，实际的应用服务必须实现这里的约束接口，导致应用服务会出现奇怪的方法
     * 因为泛型类中的参数并不都支持协变
     */

    /// <summary>
    /// 基于mud abp 的通用树 新增组件 抽象类
    /// </summary>
    /// <typeparam name="TAppService">主应用服务</typeparam>
    /// <typeparam name="TEntityDto">查询模型类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入参数类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入参数类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    public class AbpMudGeneralTreeCreateBaseComponent<TAppService,
                                                      TEntityDto,
                                                      TCreateInput,
                                                      TEditDto,
                                                      TGetAllInput> : AbpMudBaseComponent
        where TCreateInput : IHaveParentId<long>, new() // GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        //where TEntityDto : IGeneralTree<TEntityDto>// GeneralTreeGetTreeNodeBaseDto<TEntityDto>, IExtendableDto//, new()
        //where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        //where TGetAllInput : GeneralTreeGetTreeInput, new()
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
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
        /// <summary>
        /// 组件初始时初始化表单
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await BtnResetClick();
        }
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
    }

    /// <summary>
    /// 基于mud abp 的通用树 新增弹窗组件 抽象类
    /// </summary>
    /// <typeparam name="TCreateDialog">新增弹窗组件</typeparam>
    /// <typeparam name="TDetailDialog">详情弹窗组件</typeparam>
    /// <typeparam name="TAppService">主应用服务</typeparam>
    /// <typeparam name="TEntityDto">查询模型类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入参数类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入参数类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    public class AbpMudGeneralTreeCreateDialogBaseComponent<TCreateDialog,
                                                            TDetailDialog,
                                                            TAppService,
                                                            TEntityDto,
                                                            TCreateInput,
                                                            TEditDto,
                                                            TGetAllInput> : AbpMudGeneralTreeCreateBaseComponent<TAppService,
                                                                                                                 TEntityDto,
                                                                                                                 TCreateInput,
                                                                                                                 TEditDto,
                                                                                                                 TGetAllInput>
        where TCreateInput : IHaveParentId<long>, new()
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
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