
using Abp.ObjectMapping;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.RCL.Interceptors;
namespace ZLJ.RCL.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TGetAllInput和TCreateInput
     * 不要考虑同一个组件反复编辑不同的数据，那样稍微复杂了点，是特殊情况，就具体项目中特殊处理，暂时不考虑放抽象类中
     * 编辑时可能要加载些下拉框数据，这通常比较耗时，若在初始化组件时就加载，在用户仅仅查看时就比较浪费，所以在首次进入编辑模式时再做这些操作。
     * 
     * blazor文档中推荐不要在组件内部修改 [Parameter]属性，这种属性仅仅用于外部组件向其传递参数用，可能由于外部组件的刷新，导致此组件状态异常
     * 
     * 虽然有些简单的数据列表页可能直接传递dto过来进行处理，但有时候列表数据过于复杂，需要重新查询下，简单起见统一为根据id重新查询。
     * 若方法是虚的，则返回类型通通使用ValueTask，因为子类重写时可能不是异步的
     */

    /// <summary>
    /// 基于antblazor和abp的通用详情页组件，它包含查看详情页和修改，以及二者之间的切换
    /// 新增抽象组件是单独定义的，因为它是对数据从无到有的创建，而详情组件是对以后的数据进行查看和处理
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入类型</typeparam>
    public abstract class TreeDetailUpdateBaseComponent<TAppService,
                                                        TEntityDto,
                                                        TCreateInput,
                                                        TEditDto,
                                                        TGetAllInput> : BXJG.Utils.RCL.Components.TreeDetailUpdateBaseComponent<TAppService,
                                                        TEntityDto,
                                                        TCreateInput,
                                                        TEditDto,
                                                        TGetAllInput>
        where TEntityDto : IGeneralTree<TEntityDto>, IExtendableObj,new()
        //where TGetAllInput : new()
        where TEditDto : IHaveParentId<long>,new()
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
    {
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected virtual async Task OnFinish(EditContext editContext)
        {
            await Save();
        }
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task BtnDeleteClick()
        {
            await base.BtnDeleteClick();
        }
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task BtnRefreshClick()
        {
            await base.BtnRefreshClick();
        }
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task BtnResetClick()
        {
            await base.BtnResetClick();
        }
        /// <summary>
        /// 表单引用
        /// </summary>
        protected Form<TEditDto> frm;

        /// <summary>
        /// 点击保存按钮时回调
        /// </summary>
        /// <returns></returns>
        protected virtual void BtnUpdateClick()
        {
            //没有权限的按钮直接隐藏，况且应用服务还会判断权限兜底的，因此这里无需判断权限
            frm.Submit();
        }
       
        //await消息显示时，好像会等到消息因此时才结束，没严格测试
        //不过测试发现消息异步显示，并等待200毫秒，消息提示更丝滑
        //
        [Inject]
        public IMessageService MessageService { get; set; }

        protected override async Task ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            _ = MessageService.Error(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(800);//碰到这个，开始刷新
        }
        protected override async Task ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            _ = MessageService.Success(msg);//它是阻塞到显示完成因此元素后，所以不能等待它
            await Task.Delay(800);//碰到这个，开始刷新
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
            await base.OnAfterRenderAsync(firstRender);
        }
#endif
        #endregion
    }
}