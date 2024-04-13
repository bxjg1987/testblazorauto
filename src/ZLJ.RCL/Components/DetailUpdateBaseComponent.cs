
using Abp.ObjectMapping;
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
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class DetailUpdateBaseComponent<TAppService,
                                                    TEntityDto,
                                                    TPrimaryKey,
                                                    TGetAllInput,
                                                    TCreateInput,
                                                    TUpdateInput> : BXJG.Utils.RCL.Components.DetailUpdateBaseComponent<TAppService,
                                                    TEntityDto,
                                                    TPrimaryKey,
                                                    TGetAllInput,
                                                    TCreateInput,
                                                    TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>, new()
        //where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>, new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
       
        /// <summary>
        /// 表单引用
        /// </summary>
        protected Form<TUpdateInput> frm;
        /// <summary>
        /// 点击保存按钮时回调
        /// </summary>
        /// <returns></returns>
        protected virtual async Task BtnUpdateClick()
        {
            //没有权限的按钮直接隐藏，况且应用服务还会判断权限兜底的，因此这里无需判断权限
            frm.Submit();
        }
        /// <summary>
        /// 界面消息服务
        /// </summary>
        [Inject]
        public IMessageService MessageService { get; set; }

        //await消息显示时，好像会等到消息因此时才结束，没严格测试
        //不过测试发现消息异步显示，并等待200毫秒，消息提示更丝滑
        //

        protected override async ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            _ = MessageService.Error(msg);
            await Task.Delay(200);
        }
        protected override async ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            _ = MessageService.Success(msg);
            await Task.Delay(200);
        }
    }
}