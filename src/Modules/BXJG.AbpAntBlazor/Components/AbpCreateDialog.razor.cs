using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpBlazor.Components
{
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
    /// <typeparam name="TComponentOptions"></typeparam>
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
        where TEntityDto : IEntityDto<TPrimaryKey>, IExtendableDto//, new()
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        protected DynamicComponent? dc;
        [Parameter]
        public IDictionary<string, object> parameters { get; set; }
        [Parameter]
        public bool Visible { get; set; }
        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        #region 弹窗
        protected virtual bool SaveAndContinue
        {
            get => createComponent == default ? false : createComponent.SaveAndContinue;
            set
            {
                if (createComponent != default)
                    createComponent.SaveAndContinue = value;
            }
        }

        protected virtual async Task RestCreateForm()
        {
            if (createComponent != default)
                createComponent.Reset();
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
        public TCreateComponent createComponent => dc?.Instance as TCreateComponent;


        /// <summary>
        /// 正在执行新增的保存
        /// </summary>
        protected virtual bool isSaving => createComponent == default ? false : createComponent.IsSaving;
        /// <summary>
        /// 点击新增弹窗的保存按钮时执行
        /// </summary>
        /// <returns></returns>
        //[AbpExceptionInterceptor]
        protected virtual async Task SaveCreateClick()
        {
            var r = await createComponent.Save();
            if (r.End)
            {
                // HideCreateDialog();
                // await Refresh();
                //  await this.CloseFeedbackAsync();
                Visible = false;
            }
        }
        #endregion
    }
}
