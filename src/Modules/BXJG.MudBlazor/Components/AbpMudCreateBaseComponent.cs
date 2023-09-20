using Abp.Application.Services.Dto;
using BXJG.Common;
using BXJG.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpMudBlazor.Components
{
    /*
     * 新增是从无到有的创建，跟修改、查询详情、删除不同，后者是数据已经存在后的操作，因此分开定义
     * 由于应用服务接口是开放泛型，所以这里可以省略些泛型
     */

    /// <summary>
    /// 基于mudblazor和abp的通用新增页组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    public class AbpMudCreateBaseComponent<TAppService,
                                                 TEntityDto,
                                                 TPrimaryKey,
                                                 TCreateInput> : AbpMudBaseComponent
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto, TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
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
        /// 编辑时的模型
        /// </summary>
        public virtual TCreateInput Model { get; set; }

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
        /// 是否有修改权限
        /// </summary>
        protected bool updateIsGranted = true;
        /// <summary>
        /// 是否有删除权限
        /// </summary>
        protected bool deleteIsGranted = true;
        /// <summary>
        /// 初始化权限状态
        /// </summary>
        /// <param name="createPermissionName"></param>
        /// <param name="updatePermissionName"></param>
        /// <param name="deletePermissionName"></param>
        /// <returns></returns>
        protected virtual async Task InitPermission(string createPermissionName = default, string updatePermissionName = default, string deletePermissionName = default)
        {
            if (createPermissionName.IsNotNullOrWhiteSpaceBXJG())
                createIsGranted = await PermissionChecker.IsGrantedAsync(createPermissionName);
            if (updatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                updateIsGranted = await PermissionChecker.IsGrantedAsync(updatePermissionName);
            if (deletePermissionName.IsNotNullOrWhiteSpaceBXJG())
                deleteIsGranted = await PermissionChecker.IsGrantedAsync(deletePermissionName);
        }
        #endregion
    }
}