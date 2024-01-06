using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZLJ.RCL.Components
{
    /*
     * 通常我们在列表页做弹窗新增，但有时候我们希望在选择数据时，若数据不存在则弹出新增，而不是回到列表页去新增。
     * 这就要求我们为数据做一个独立的弹出，在管理列表页面使用此弹出，在选择数据时直接弹出新增。
     * 
     * 标准的做法是为每种数据单独建立新增弹出组件，但那样系统中会建立大量的，相似的弹窗新增组件
     * 因此这里定义一个通用的泛型新增弹窗，
     * 你可以直接使用它承载所有新增组件，且它提供了一定的扩展性，比如：在底部Footer控制默认按钮，以及添加自定义按钮等
     * 也可以为每种数据单独定义弹窗新增组件，然后让它们继承此组件，目前不考虑这种方式
     * 
     * 当前组件只关注扁平化数据的新增弹窗，树形数据有单独的，类似的通用新增弹窗组件
     * 同理，修改和详情弹窗组件也是类似的设计
     * 
     * antblazor中弹窗有Visible和ModalService两种弹窗方式，目前使用前者，它更符合直觉，我们更容易定制界面，没细了解后者。
     * 
     * 列表抽象组件是感觉的，它不引用弹窗相关代码，最终项目自己去处理，因为最终项目都不一定使用弹窗。
     * 
     * 同样的设计思路，我们可以定义通用的 下拉框选择组件，请参考它的源码中的注释。
     * 因为无论是弹窗，还是下拉框，都不是核心组件，它们的逻辑相对于列表、新增和修改详情组件来说，更简单。
     * 这种情况我们尽量使用通用组件，生代码，但丢失灵活性。
     * 而列表、新增和修改详情组件是核心组件，逻辑相对复杂，需要更高的灵活性，所以它们只抽象逻辑部分，节目部分留给子类去实现。
     */

    /// <summary>
    /// 抽象的新增弹窗组件
    /// </summary>
    /// <typeparam name="TAppService"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TCreateComponent"></typeparam>
    public partial class AbpCreateDialog<TAppService,
                                         TEntityDto,
                                         TPrimaryKey,
                                         TGetAllInput,
                                         TCreateInput,
                                         TUpdateInput,
                                         TCreateComponent>
       where TCreateComponent : AbpCreateBaseComponent<TAppService,
                                                       TEntityDto,
                                                       TPrimaryKey,
                                                       TGetAllInput,
                                                       TCreateInput,
                                                       TUpdateInput>
        where TCreateInput : new()
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 内部的模态窗口组件的引用
        /// </summary>
        public Modal ModalRef { get; protected set; }
        /// <summary>
        /// 承载新增组件的动态组件的引用
        /// </summary>
        protected DynamicComponent? dcRef;
        /// <summary>
        /// 对新增组件的引用
        /// </summary>
        public virtual TCreateComponent CreateComponent => dcRef?.Instance as TCreateComponent;
        /// <summary>
        /// 传入到内部新增组件的参数
        /// </summary>
        [Parameter]
        public IDictionary<string, object> Parameters { get; set; }
        ///// <summary>
        ///// 是否显示弹窗
        ///// </summary>
        //[Parameter]
        protected bool isVisible;
        //[Parameter]
        //public EventCallback<bool> VisibleChanged { get; set; }

        #region 弹窗
        /// <summary>
        /// 是否选择了保存并继续
        /// </summary>
        protected virtual bool IsSaveAndContinue
        {
            get => CreateComponent == default ? false : CreateComponent.SaveAndContinue;
            set
            {
                if (CreateComponent != default)
                    CreateComponent.SaveAndContinue = value;
            }
        }
        /// <summary>
        /// 重置表单
        /// </summary>
        /// <returns></returns>
        public virtual async Task Reset()
        {
            if (CreateComponent != default)
                CreateComponent.Reset();
            // (   this.FeedbackRef as ModalOptions).Footer= xx
        }


        /// <summary>
        /// 正在执行新增的保存
        /// </summary>
        protected virtual bool IsSaving => CreateComponent == default ? false : CreateComponent.IsSaving;
        /// <summary>
        /// 新增成功过？
        /// </summary>
        protected bool isAdded;
        /// <summary>
        /// 点击新增弹窗的保存按钮时执行
        /// </summary>
        /// <returns></returns>
        //[AbpExceptionInterceptor]
        protected virtual async Task SaveCreateClick()
        {
            var r = await CreateComponent.Save();
            isAdded = true;
            if (r != default && r.End)
            {
                CloseCore();
            }
        }
        public virtual async Task Close()
        {
            CloseCore();
        }
        /// <summary>
        /// cts.Cancel()，告诉调用方弹窗新增正常结束
        /// </summary>
        CancellationTokenSource cts;
        /// <summary>
        /// 显示新增模特弹窗
        /// 既然是模态的，就没必要异步触发事件，而是等待弹窗处理完成，这样更直接
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> Show()
        {
            cts = new CancellationTokenSource();
            isVisible = true;
            isAdded = false;
            StateHasChanged();
            // return Task.FromCanceled(cts.Token);//Task.FromCanceled 要求传递给它的 CancellationToken 已请求取消，即其 IsCancellationRequested 返回 true。
            return await Task.Delay(-1, cts.Token).ContinueWith(t => isAdded);
        }
        //  [Inject]
        //   public ModalService ModalService { get; set; }
        protected virtual async void CloseCore()
        {
            // this.ModalRef.des
            isVisible = false;
         //   CreateComponent = default;
         //  ModalRef?.Dispose();
          //  ModalRef = default;
        
            dcRef = null;
            cts.Cancel();
            // ModalService.DestroyAllConfirmAsync();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            //确保弹窗title能正常显示
            if (ModalRef.Title == "新增")
                StateHasChanged();
        }
        #endregion
    }
}
