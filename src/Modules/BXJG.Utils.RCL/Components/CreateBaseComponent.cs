using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace BXJG.Utils.RCL.Components
{
    /// <summary>
    /// 通用新增页组件
    /// 查看详情和修改数据的抽象组件是单独定义的（因为要切换查看和编辑模式，所以定义在同一个组件中的），
    /// 查看详情和修改组件是对以后的数据进行查看和处理，而新增组件它是对数据从无到有的创建，因此分开定义的。
    /// </summary>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    public abstract class CreateBaseComponent<TEntityDto, TCreateInput> : BaseComponent
        where TCreateInput : new()
    {
        /// <summary>
        /// 此功能的名称
        /// </summary>
        public abstract string FuncName { get; }// => $"请重写{nameof(FuncName)}属性";
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
        /// 是否正在提交或重置
        /// </summary>
        protected virtual bool IsBusy => isReseting || isSaving;

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
        }
        /// <summary>
        /// 重置的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual ValueTask ResetCore()
        {
            if (createDto!=null&&createDto is IReset t)
                t.Reset();
            else
                createDto = new TCreateInput();
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
        public bool isSaveAndContinue;
        /// <summary>
        /// 正在保存...
        /// </summary>
        protected bool isSaving;
        //protected virtual async Task BtnSaveClick()
        //{
        //    //没有权限的按钮直接隐藏，况且应用服务还会判断权限兜底的，因此这里无需判断权限
        //    frm.Submit();
        //}





        //protected bool isAdded;
        /// <summary>
        /// 核心的保存逻辑
        /// </summary>
        /// <returns>新增任务是否结束</returns>
        protected virtual async Task Save()
        {
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            if (isSaving) return;
            isSaving = true;
            try
            {
                var r = await SaveCore();
                isSaving = false;
                //后续逻辑都是辅助性的，因此放到异步中，加快主操作速度
                _ = InvokeAsync(async () =>
                {
                    await ShowSuccessMessage(msg: "新增成功！");//没必要等待
                    if (isSaveAndContinue)
                    {
                        await Reset();
                        await OnAddEnd.InvokeAsync(new SaveResult<TEntityDto> { Dto = r });
                    }
                    else
                        await OnAddEnd.InvokeAsync(new SaveResult<TEntityDto> { Dto = r, End = true });
                }).ConfigureAwait(false);
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
        protected virtual async Task<TEntityDto> SaveCore()
        {
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            return await HttpClient.Create<TEntityDto>(createDto); //AppService.CreateAsync(createDto);
        }
        /// <summary>
        /// 新增成功，且不再继续新增时触发
        /// </summary>
        [Parameter]
        public EventCallback<SaveResult<TEntityDto>> OnAddEnd { get; set; }
    }
}