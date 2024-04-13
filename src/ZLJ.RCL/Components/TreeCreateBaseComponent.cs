

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
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入类型</typeparam>
    public abstract class TreeCreateBaseComponent<TAppService,
                                                  TEntityDto,
                                                  TCreateInput,
                                                  TEditDto,
                                                  TGetAllInput> : BXJG.Utils.RCL.Components.TreeCreateBaseComponent<TAppService,
                                                  TEntityDto,
                                                  TCreateInput,
                                                  TEditDto,
                                                  TGetAllInput>
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
        where TCreateInput : IHaveParentId<long>, new()
    {
       
        protected virtual async Task BtnSaveClick()
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