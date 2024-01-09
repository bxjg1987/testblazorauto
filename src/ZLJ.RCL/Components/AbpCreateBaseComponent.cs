

using Abp.Application.Services.Dto;
using BXJG.Utils.Application.Share;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.RCL.Interceptors;

namespace ZLJ.RCL.Components
{

    /// <summary>
    /// 新增返回对象
    /// </summary>
    public class SaveResult<TEntityDto>
    {
        /// <summary>
        /// 新增后返回的dto对象
        /// </summary>
        public TEntityDto Dto { get; set; }
        /// <summary>
        /// 新增是否结束了，
        /// 若没有勾选“保存并继续”，则新增后表示新增结束
        /// 验证不过也会返回false
        /// </summary>
        public bool End { get; set; }
    }


    /// <summary>
    /// 基于BootstrapBlazor和abp的通用新增页组件
    /// 查看详情和修改数据的抽象组件是单独定义的（因为要切换查看和编辑模式，所以定义在同一个组件中的），
    /// 查看详情和修改组件是对以后的数据进行查看和处理，而新增组件它是对数据从无到有的创建，因此分开定义的。
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class AbpCreateBaseComponent<TAppService,
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
        /// 新增时的模型
        /// </summary>
        public TCreateInput? CreateDto { get; protected set; }
        /// <summary>
        /// 正在执行重置
        /// </summary>
        public bool IsReseting { get; protected set; }
        /// <summary>
        /// 重置按钮点击时回调，由于事件无法使用ValueTask，所以这里用了Task
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        public virtual async Task Reset()
        {
            IsReseting = true;
            try
            {
                await ResetCore();
            }
            finally
            {
                IsReseting = false;
            }
            //StateHasChanged();
        }
        /// <summary>
        /// 重置的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual ValueTask ResetCore()
        {
            CreateDto = new TCreateInput();
            return ValueTask.CompletedTask;
        }
        /// <summary>
        /// 初始化时，初始化新增模型
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await Reset();
        }
        /// <summary>
        /// 保存后是否继续新增
        /// </summary>
        public bool SaveAndContinue { get; set; }
        /// <summary>
        /// 正在保存...
        /// </summary>
        public bool IsSaving { get; protected set; }


        //protected bool isAdded;
        /// <summary>
        /// 核心的保存逻辑
        /// </summary>
        /// <returns>新增任务是否结束</returns>
        [AbpExceptionInterceptor]
        public virtual async Task Save()
        {
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            IsSaving = true;
            try
            {
                await SaveCore();
            }
            finally
            {
                IsSaving = false;
            }
        }
        /// <summary>
        /// 新增成功，且不再继续新增时触发
        /// </summary>
        [Parameter]
        public EventCallback<SaveResult<TEntityDto>> OnAddEnd { get; set; }
        /// <summary>
        /// 保存的核心逻辑
        /// </summary>
        /// <returns>新增任务是否结束</returns>
        protected virtual async Task SaveCore()
        {
            var yz = await Validate();
            if (!yz)
                return;
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            var r = await AppService.CreateAsync(CreateDto);
            ShowSuccessMessage(msg: "新增成功！");//没必要等待
            if (SaveAndContinue)
            {
                await Reset();
                await OnAddEnd.InvokeAsync(new SaveResult<TEntityDto> { Dto = r });
            }
            else
                await OnAddEnd.InvokeAsync(new SaveResult<TEntityDto> { Dto = r, End = true });
        }
        /// <summary>
        /// 表单验证的核心逻辑
        /// </summary>
        /// <returns>true验证成功；false验证失败</returns>
        protected virtual ValueTask<bool> Validate()
        {
            return ValueTask.FromResult(validateForm.Validate());
        }
        /// <summary>
        /// 对表单的引用
        /// </summary>
        protected Form<TCreateInput> validateForm;


    }
}