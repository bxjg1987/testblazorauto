

using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.RCL.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace BXJG.Utils.RCL.Components
{
    /*
     * 某些操作都定义了三个方法
     * 顶层是为了应用全局异常拦截器
     * 二层方法是为了当前类和子类方便调用，里面包含loading的处理，不能直接调用顶层方法，免得全局异常拦截器被多次应用
     * 三层方法是方便子类重写
     * 
     */

    /// <summary>
    /// 基于antblazor和abp的通用新增页组件
    /// 查看详情和修改数据的抽象组件是单独定义的（因为要切换查看和编辑模式，所以定义在同一个组件中的），
    /// 查看详情和修改组件是对以后的数据进行查看和处理，而新增组件它是对数据从无到有的创建，因此分开定义的。
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入类型</typeparam>
    public abstract class TreeCreateBaseComponent<TAppService,
                                                  TEntityDto,
                                                  TCreateInput,
                                                  TEditDto,
                                                  TGetAllInput> : BaseComponent
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
        where TCreateInput : IHaveParentId<long>, new()
    {
        /// <summary>
        /// 请使用AppService
        /// </summary>
        TAppService appService;
        /// <summary>
        /// 获取主服务
        /// </summary>
        protected virtual TAppService AppService => appService ??= ScopedServices.GetRequiredService<TAppService>();
        /// <summary>
        /// 此功能的名称
        /// </summary>
        public abstract string FuncName { get; }// => $"请重写{nameof(FuncName)}属性";
        /// <summary>
        /// 父级id
        /// </summary>
        [Parameter]
        public long? ParentId { get; set; }
        /// <summary>
        /// 新增时的模型
        /// </summary>
        protected TCreateInput createDto = new TCreateInput();
        /// <summary>
        /// 正在执行重置
        /// </summary>
        protected bool isReseting;
        /// <summary>
        /// 重置按钮点击时回调，由于事件无法使用ValueTask，所以这里用了Task
        /// </summary>
        /// <returns></returns>

        

        public virtual async Task BtnResetClick()
        {
            await Reset();
        }
        /// <summary>
        /// 带loading处理的reset
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Reset()
        {
            if (isReseting)
                return;

            isReseting = true;
            try
            {
                await ResetCore();
            }
            finally
            {
                isReseting = false;
            }
            //StateHasChanged();
        }
        /// <summary>
        /// 重置的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual ValueTask ResetCore()
        {
            createDto = new TCreateInput();
            createDto.ParentId = ParentId;
            return ValueTask.CompletedTask;
        }
        /// <summary>
        /// 初始化时，初始化新增模型
        /// </summary>
        /// <returns></returns>

        

        protected override async Task OnInitializedAsync()
        {
            await Reset();
        }
        /// <summary>
        /// 保存后是否继续新增
        /// </summary>
        protected bool saveAndContinue;
        /// <summary>
        /// 正在保存...
        /// </summary>
        protected bool isSaving;
        

        

       
        /// <summary>
        /// 核心的保存逻辑
        /// </summary>
        /// <returns>新增任务是否结束</returns>
        protected virtual async Task Save()
        {
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            if(isSaving) return;
            isSaving = true;
            try
            {
                await SaveCore();
            }
            finally
            {
                isSaving = false;
            }
        }
        /// <summary>
        /// 保存的核心逻辑
        /// </summary>
        /// <returns>新增任务是否结束</returns>
        protected virtual async Task SaveCore()
        {
            //var yz = await Validate();
            //if (!yz)
            //    return;
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            var r = await AppService.CreateAsync(createDto);
         ShowSuccessMessage(msg: "新增成功！");//没必要等待
            if (saveAndContinue)
            {
                await Reset();
                _ = OnAddEnd.InvokeAsync(new SaveResult<TEntityDto> { Dto = r });
            }
            else
                _= OnAddEnd.InvokeAsync(new SaveResult<TEntityDto> { Dto = r, End = true });
        }
        /// <summary>
        /// 新增成功，且不再继续新增时触发
        /// </summary>
        [Parameter]
        public EventCallback<SaveResult<TEntityDto>> OnAddEnd { get; set; }
      
    }
}