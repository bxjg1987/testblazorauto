using Abp.Application.Services.Dto;
using BXJG.Common;
using BXJG.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpMudBlazor.Components
{
    /*
     * 新增是从无到有的创建，跟修改、查询详情、删除不同，后者是数据已经存在后的操作，因此分开定义
     * 
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TGetAllInput、TUpdateInput
     */

    /// <summary>
    /// 基于mudblazor和abp的通用新增页组件
    /// 修改抽象组件是单独定义的，详情组件是对以后的数据进行查看和处理，而新增组件它是对数据从无到有的创建
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public class AbpMudCreateBaseComponent<TAppService,
                                           TEntityDto,
                                           TPrimaryKey,
                                           TGetAllInput,
                                           TCreateInput,
                                           TUpdateInput> : AbpMudBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 缓存当前主服务对象
        /// </summary>
        private TAppService? appService;
        /// <summary>
        /// 获取主服务
        /// </summary>
        protected virtual TAppService AppService => appService ??= ScopedServices.GetRequiredService<TAppService>();
        /// <summary>
        /// 此功能的名称
        /// </summary>
        protected virtual string FuncName => $"请重写{nameof(FuncName)}属性";
        /// <summary>
        /// 新增时的模型
        /// </summary>
        protected virtual TCreateInput CreateDto { get; set; }

        //protected override async Task OnInitialized2Async()
        //{
        //    //列表传递过来的dto信息没有详情中的dto多
        //    Model = await AppService.GetAsync(new EntityDto<TPrimaryKey>(Model.Id));
        //}

        #region 权限
        /// <summary>
        /// 是否有新增权限
        /// </summary>
        protected bool createIsGranted = true;
        /// <summary>
        /// 初始化权限状态
        /// </summary>
        /// <param name="createPermissionName"></param>
        /// <returns></returns>
        protected virtual async Task InitPermission(string? createPermissionName = default)
        {
            if (createPermissionName.IsNotNullOrWhiteSpaceBXJG())
                createIsGranted = await PermissionChecker.IsGrantedAsync(createPermissionName);
        }
        #endregion
        /// <summary>
        /// 正在保存...
        /// </summary>
        protected bool Saving = false;
        /// <summary>
        /// 核心的保存逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Save()
        {
            Saving = true;
            await base.SafelyExecuteAsync(async () =>
            {
                var r = await AppService.CreateAsync(CreateDto);
                Snackbar.Add("新增成功！", Severity.Success);
                AfterSave(r);
            });
            Saving = false;
        }
        /// <summary>
        /// 保存后回调
        /// </summary>
        protected virtual void AfterSave(TEntityDto dto) { }
    }

    /// <summary>
    /// 基于mudblazor和abp的通用新增弹窗页组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public class AbpMudCreateDialogBaseComponent<TAppService,
                                                 TEntityDto,
                                                 TPrimaryKey,
                                                 TGetAllInput,
                                                 TCreateInput,
                                                 TUpdateInput> : AbpMudCreateBaseComponent<TAppService,
                                                                                           TEntityDto,
                                                                                           TPrimaryKey,
                                                                                           TGetAllInput,
                                                                                           TCreateInput,
                                                                                           TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 当前弹窗对象
        /// </summary>
        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; private set; }
        /// <summary>
        /// 点击关闭按钮时执行
        /// </summary>
        protected virtual void Cancel() => MudDialog.Cancel();
        /// <summary>
        /// 新增成功后回调
        /// </summary>
        /// <param name="dto"></param>
        protected override void AfterSave(TEntityDto dto)
        {
            MudDialog.Close(dto);
        }
    }
}