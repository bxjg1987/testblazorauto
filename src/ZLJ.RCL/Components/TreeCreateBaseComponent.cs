

using Abp.Application.Services.Dto;
using AntDesign;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.RCL.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.RCL.Interceptors;

namespace ZLJ.RCL.Components
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
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    public abstract class TreeCreateBaseComponent<TEntityDto,
                                                  TCreateInput> : BXJG.Utils.RCL.Components.TreeCreateBaseComponent<TEntityDto,
                                                                                                                    TCreateInput>
        where TCreateInput : IHaveParentId<long>, new()
    {
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected virtual async Task OnFinish(EditContext editContext)
        {
            await Save();
        }
        protected virtual void BtnSaveClick()
        {
            //没有权限的按钮直接隐藏，况且应用服务还会判断权限兜底的，因此这里无需判断权限
            frm.Submit();
        }
        /// <summary>
        /// 对表单的引用
        /// </summary>
        protected Form<TCreateInput> frm;
        //await消息显示时，好像会等到消息因此时才结束，没严格测试
        //不过测试发现消息异步显示，并等待200毫秒，消息提示更丝滑
        //
        [Inject]
        public IMessageService MessageService { get; set; }
        protected override async Task ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            _ = MessageService.Error(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(300);//碰到这个，开始刷新
        }
        protected override async Task ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            _ = MessageService.Success(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(300);//碰到这个，开始刷新
        }

        #region 生命周期方法增加统一异常处理拦截器
        /*
         * 肉夹馍的aop有基于规则的匹配方式，但有点复杂，
         * 还是决定使用硬编码方式配置，比较稳妥。即 哪里需要就在哪里加 [AbpExceptionInterceptor]
         * 
         * 父类加了，子类再加这个特征的话会重复，会比较浪费。但是父类不加，如果子类没重写并加拦截器，会导致拦截器无法执行。
         * 所以还是决定在抽象中添加，子类可以重写时不调用父类，自己单独加 [AbpExceptionInterceptor]
         * 最坏的情况是子类重写，且必须调用父类方法时，确实比较浪费，层次不深的话也无所谓了。
         */
#if !DEBUG
        [AbpExceptionInterceptor]
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }
        [AbpExceptionInterceptor]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        [AbpExceptionInterceptor]
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
        [AbpExceptionInterceptor]
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        [AbpExceptionInterceptor]
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        [AbpExceptionInterceptor]
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
             await  base.OnAfterRenderAsync(firstRender);
        }
#endif
        #endregion
    }
}