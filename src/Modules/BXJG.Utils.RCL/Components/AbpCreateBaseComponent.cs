using Abp.Application.Services.Dto;
using BXJG.Common;
using BXJG.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Rougamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TGetAllInput、TUpdateInput
     * 很多ui都提供了自己的表单验证方式，有些还不支持微软默认的表单验证逻辑，但通常都支持基于Attribute验证方式，所以抽象类中不定义验证相关的东东
     */

    /// <summary>
    /// 通用新增页组件
    /// 查看详情和修改数据的抽象组件是单独定义的（因为要切换查看和编辑模式，所以定义在同一个组件中的），
    /// 查看详情和修改组件是对以后的数据进行查看和处理，而新增组件它是对数据从无到有的创建，因此分开定义的。
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class AbpCreateBaseComponent<TAppService,
                                                 TEntityDto,
                                                 TPrimaryKey,
                                                 TGetAllInput,
                                                 TCreateInput,
                                                 TUpdateInput> : AbpBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : new()
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
        protected TCreateInput? createDto;
        /// <summary>
        /// 正在执行重置
        /// </summary>
        protected bool isReseting = false;
        /// <summary>
        /// 重置按钮点击时回调，由于事件无法使用ValueTask，所以这里用了Task
        /// </summary>
        /// <returns></returns>
        public virtual async Task BtnResetClick()
        {
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
            createDto = new TCreateInput();
            return ValueTask.CompletedTask;
        }
        /// <summary>
        /// 初始化时，初始化新增模型
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await BtnResetClick();
        }
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
        /// <summary>
        /// 保存后是否继续新增
        /// </summary>
        protected bool saveAndContinue = false;
        /// <summary>
        /// 正在保存...
        /// </summary>
        protected bool isSaving = false;
        /// <summary>
        /// 核心的保存逻辑
        /// </summary>
        /// <returns>新增任务是否结束</returns>
        public virtual async Task<bool> BtnSaveClick()
        {
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            isSaving = true;
            try
            {
                return await SaveCore();
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
        protected virtual async Task<bool> SaveCore()
        {
            var yz = await Validate();
            if (!yz)
                return false;
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            var r = await AppService.CreateAsync(createDto);
            await ShowSuccessMessage(msg: "新增成功！");
            if (saveAndContinue)
            {
                await BtnResetClick();
                return false;
            }
            return true;
        }
        /// <summary>
        /// 表单验证的核心逻辑
        /// </summary>
        /// <returns>true验证成功；false验证失败</returns>
        protected abstract ValueTask<bool> Validate();
    }
}