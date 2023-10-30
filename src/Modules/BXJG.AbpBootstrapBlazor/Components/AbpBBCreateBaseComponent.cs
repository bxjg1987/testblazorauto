using Abp.Application.Services.Dto;
using BXJG.AbpBootstrapBlazor.Interceptors;
using BXJG.Common;
using BXJG.Utils;
using BXJG.Utils.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Rougamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpBootstrapBlazor.Components
{
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
    public abstract class AbpBBCreateBaseComponent<TAppService,
                                                   TEntityDto,
                                                   TPrimaryKey,
                                                   TGetAllInput,
                                                   TCreateInput,
                                                   TUpdateInput> : AbpCreateBaseComponent<TAppService,
                                                                                          TEntityDto,
                                                                                          TPrimaryKey,
                                                                                          TGetAllInput,
                                                                                          TCreateInput,
                                                                                          TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 对表单的引用
        /// </summary>
        protected ValidateForm validateForm;
        [AbpBBException]
        protected override async Task BtnResetClick()
        {
            await base.BtnResetClick();
        }
        [AbpBBException]
        protected override async Task BtnSaveClick()
        {
            await base.BtnSaveClick();
        }
        [Inject]
        public MessageService MessageService { get; set; }

        protected override async ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            await MessageService.Show(new MessageOption()
            {
                Content = msg,
                Color = Color.Danger,
                ShowShadow = true,
                ShowBorder = true,
            });
        }
        protected override async ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            await MessageService.Show(new MessageOption()
            {
                Content = msg,
                Color = Color.Success,
                ShowShadow = true,
                ShowBorder = true
            });
        }

        #region 生命周期方法增加统一异常处理拦截器
        [AbpBBException]
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }
        [AbpBBException]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        [AbpBBException]
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
        [AbpBBException]
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        [AbpBBException]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        [AbpBBException]
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        [AbpBBException]
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }
        #endregion

    }

    ///// <summary>
    ///// 基于mudblazor和abp的通用新增弹窗页组件
    ///// 查看详情和修改数据的抽象组件是单独定义的（因为要切换查看和编辑模式，所以定义在同一个组件中的），
    ///// 查看详情和修改组件是对以后的数据进行查看和处理，而新增组件它是对数据从无到有的创建，因此分开定义的。
    ///// </summary>
    ///// <typeparam name="TAppService">应用服务类型</typeparam>
    ///// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    ///// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    ///// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    ///// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    ///// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    //public class AbpMudCreateDialogBaseComponent<TAppService,
    //                                             TEntityDto,
    //                                             TPrimaryKey,
    //                                             TGetAllInput,
    //                                             TCreateInput,
    //                                             TUpdateInput> : AbpMudCreateBaseComponent<TAppService,
    //                                                                                       TEntityDto,
    //                                                                                       TPrimaryKey,
    //                                                                                       TGetAllInput,
    //                                                                                       TCreateInput,
    //                                                                                       TUpdateInput>
    //    where TEntityDto : IEntityDto<TPrimaryKey>
    //    where TCreateInput : new()
    //    where TUpdateInput : IEntityDto<TPrimaryKey>
    //    where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    //{
    //    /// <summary>
    //    /// 当前弹窗对象
    //    /// </summary>
    //    [CascadingParameter]
    //    protected MudDialogInstance MudDialog { get; private set; }
    //    /// <summary>
    //    /// 点击关闭按钮时执行
    //    /// </summary>
    //    protected virtual void Cancel() => MudDialog.Cancel();
    //    /// <summary>
    //    /// 新增成功后回调
    //    /// </summary>
    //    /// <param name="dto"></param>
    //    protected override ValueTask AfterSave(TEntityDto dto)
    //    {
    //        if (!saveAndContinue)
    //            MudDialog.Close(dto);
    //        return ValueTask.CompletedTask;
    //    }
    //}
}