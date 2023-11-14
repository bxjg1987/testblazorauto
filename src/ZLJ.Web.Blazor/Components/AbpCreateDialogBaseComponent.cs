using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Blazor.Components
{
    /*
     * 已经定义了干净的列表和新增表单抽象组件，列表和新增抽象组件均是独立的，它们互相不知道对方
    如果整个系统使用弹窗风格，那么大部分时候，新增弹窗看起来都类似
    所以定义一个通用的扁平化新增弹窗组件，所有扁平化数据新增弹窗都可以使用此弹窗组件
    不过也要考虑扩展性，两个方面：继承或定制通用扁平化数据的弹窗组件

    在最终项目的列表页中去实现弹窗的呈现，这样具体项目灵活性更大，也许它根本不使用弹窗的方式

    弹窗除了可以用在列表页，还可能用于下拉数据不够时，立即新增，而不是回到管理列表有去新增。

    ant提供两种弹窗方式，一种是普通的开发组件razor的方式，另一种是ModalService方式，前者给予用户更灵活的布局方式，后者适合用弹出确认框之类的场景
    所以我们目前使用前者的方式
    抽象弹窗中会丢失一些ant弹窗的灵活性，这是正常的，应为这里的弹窗只关注新增组件的呈现这件事，若具体项目需要更大的灵活性就不应该使用此通用弹窗组件

    列表、弹窗、新增表单 在抽象模块中是独立的，在具体项目中去做呈现，因为具体项目可能根本不用弹窗的方式
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
    public abstract class AbpCreateDialogBaseComponent<TAppService,
                                         TEntityDto,
                                         TPrimaryKey,
                                         TGetAllInput,
                                         TCreateInput,
                                         TUpdateInput,
                                         TCreateComponent> : AbpBaseComponent
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
        protected DynamicComponent? dc;
        [Parameter]
        public IDictionary<string, object> Parameters { get; set; }
        [Parameter]
        public bool Visible { get; set; }
        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        #region 弹窗
        protected virtual bool IsSaveAndContinue
        {
            get => CreateComponent == default ? false : CreateComponent.SaveAndContinue;
            set
            {
                if (CreateComponent != default)
                    CreateComponent.SaveAndContinue = value;
            }
        }

        protected virtual async Task Reset()
        {
            if (CreateComponent != default)
                CreateComponent.Reset();
            // (   this.FeedbackRef as ModalOptions).Footer= xx
        }

        /*
         * 在抽象中最好的方式是使用modalservice来做弹窗，这样能更简化子类弹窗相关代码
         * 但弹窗内部组件必须继承FeedbackComponent<TComponentOptions>
         * 我们的新增和详情组件有自己的父类，所以外面还需要包一层，简单点是包一层通用组件
         * 包一层的组件使用动态组件来渲染真正的内部组件，也许还可以拿到内部组件的引用
         * 这样开发和使用都比较复杂
         * 
         * 简单一点，使用Visible的方式吧，抽象类中之定义弹窗相关方法，子类去做具体布局
         */
        /// <summary>
        /// 对新增组件的引用
        /// </summary>
        public TCreateComponent CreateComponent => dc?.Instance as TCreateComponent;


        /// <summary>
        /// 正在执行新增的保存
        /// </summary>
        protected virtual bool IsSaving => CreateComponent == default ? false : CreateComponent.IsSaving;
        /// <summary>
        /// 点击新增弹窗的保存按钮时执行
        /// </summary>
        /// <returns></returns>
        //[AbpExceptionInterceptor]
        protected virtual async Task SaveCreateClick()
        {
            var r = await CreateComponent.Save();
            if (r.End)
            {
                await CloseCore();
            }
        }

        protected virtual async Task Close()
        {
            await CloseCore();
        }

        protected virtual async Task CloseCore()
        {
            Visible = false;
            await VisibleChanged.InvokeAsync();
        }
        #endregion
    }
}
