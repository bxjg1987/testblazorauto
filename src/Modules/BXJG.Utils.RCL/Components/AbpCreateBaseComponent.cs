using Abp.Application.Services.Dto;
using Abp.UI;
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
        /// 获取主服务
        /// </summary>
        public virtual TAppService AppService => ScopedServices.GetRequiredService<TAppService>();
        /// <summary>
        /// 此功能的名称
        /// </summary>
        public abstract string FuncName { get; }// => $"请重写{nameof(FuncName)}属性";
        /// <summary>
        /// 新增时的模型
        /// </summary>
        public TCreateInput? CreateDto { get; protected set; }
        /// <summary>
        /// 正在执行重置
        /// </summary>
        public bool IsReseting { get; protected set; }
        /// <summary>
        /// 重置按钮点击时回调，由于事件无法使用ValueTask，所以这里用了Task
        /// </summary>
        /// <returns></returns>
        public virtual async Task Reset()
        {
            IsReseting = true;
            try
            {
                await ResetCore();
            }
            finally
            {
                IsReseting = false;
            }
        }
        /// <summary>
        /// 重置的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual ValueTask ResetCore()
        {
            CreateDto = new TCreateInput();
            return ValueTask.CompletedTask;
        }
        /// <summary>
        /// 初始化时，初始化新增模型
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await CheckPermission();
            await Reset();
        }
        /// <summary>
        /// 新增权限判断
        /// </summary>
        /// <returns></returns>
        protected abstract Task CheckPermission();
        /// <summary>
        /// 保存后是否继续新增
        /// </summary>
        public bool SaveAndContinue { get;  set; }
        /// <summary>
        /// 正在保存...
        /// </summary>
        public bool IsSaving { get; protected set; }
        /// <summary>
        /// 新增返回对象
        /// </summary>
        public class SaveResult
        {
            /// <summary>
            /// 新增后返回的dto对象
            /// </summary>
            public TEntityDto Dto { get; set; }
            /// <summary>
            /// 新增是否结束了，
            /// 若没有勾选“保存并继续”，则新增后表示新增结束
            /// 验证不过也会返回false
            /// </summary>
            public bool End { get; set; }
        }
        /// <summary>
        /// 核心的保存逻辑
        /// </summary>
        /// <returns>新增任务是否结束</returns>
        public virtual async Task<SaveResult> Save()
        {
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            IsSaving = true;
            try
            {
                return await SaveCore();
            }
            finally
            {
                IsSaving = false;
            }
        }
        /// <summary>
        /// 保存的核心逻辑
        /// </summary>
        /// <returns>新增任务是否结束</returns>
        protected virtual async Task<SaveResult> SaveCore()
        {
            var yz = await Validate();
            if (!yz)
                return new SaveResult();
            //木有权限时保存按钮不可点击
            //验证不过时此方法不应该被调用
            var r = await AppService.CreateAsync(CreateDto);
            ShowSuccessMessage(msg: "新增成功！");//没必要等待
            if (SaveAndContinue)
            {
                await Reset();
                return new SaveResult { Dto = r };
            }
            return new SaveResult { Dto = r, End = true };
        }
        /// <summary>
        /// 表单验证的核心逻辑
        /// </summary>
        /// <returns>true验证成功；false验证失败</returns>
        protected abstract ValueTask<bool> Validate();
    }
}