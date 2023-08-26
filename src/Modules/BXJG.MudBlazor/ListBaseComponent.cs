using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using BXJG.Common.Dto;
using BXJG.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.MudBlazor
{
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class ListBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TUpdateInput,
                                   TGetInput,
                                   TDeleteInput,
                                   TFormComponent,
                                   TAppService> : AbpBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TGetInput : IEntityDto<TPrimaryKey>
        where TFormComponent: ComponentBase
        where TDeleteInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
    {
        #region 基础

        /// <summary>
        /// 缓存当前主服务对象
        /// </summary>
        private TAppService? appService;
        /// <summary>
        /// 获取主服务
        /// </summary>
        protected TAppService AppService => appService ??= ScopedServices.GetRequiredService<TAppService>();

        private ISnackbar snackbar;

        protected ISnackbar Snackbar => snackbar ??= ScopedServices.GetRequiredService<ISnackbar>();

        private IDialogService dialogService;
        protected IDialogService DialogService => dialogService ??= ScopedServices.GetRequiredService<IDialogService>();
        /// <summary>
        /// 此功能的名称
        /// </summary>
        protected virtual string FuncName => "xxx";
        #endregion

        #region 生命周期
        protected override async Task OnInitialized2Async()
        {
            await base.OnInitialized2Async();
            await InitPermission();
        }
        #endregion

        #region 权限

        //不要在组件中使用AuthorizeAttribute，因为这样会导致组件的渲染速度变慢，因为每次都要去检查权限
        //不要靠应用层定义的权限，因为前后端分离时，应用接口就不应该提供权限名

        /// <summary>
        /// 是否可以新增
        /// </summary>
        protected bool canCreate = true;
        protected bool canUpdate = true;
        protected bool canDelete = true;

        protected virtual string CreatePermissionName => "";
        protected virtual string UpdatePermissionName => "";
        protected virtual string DeletePermissionName => "";

        protected virtual async Task InitPermission()
        {
            if (CreatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                canCreate = await base.PermissionChecker.IsGrantedAsync(CreatePermissionName);
            if (UpdatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                canUpdate = await base.PermissionChecker.IsGrantedAsync(UpdatePermissionName);
            if (DeletePermissionName.IsNotNullOrWhiteSpaceBXJG())
                canDelete = await base.PermissionChecker.IsGrantedAsync(DeletePermissionName);
        }

        #endregion

        #region 列表
        /// <summary>
        /// 当前页码
        /// </summary>
        protected int pageIndex = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        protected int pageCount = 1;
        /// <summary>
        /// 表格数据加载
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual async Task<GridData<TEntityDto>> LoadDataAsync(GridState<TEntityDto> state)
        {
            var cd = new TGetAllInput();
            if (cd is IDynamicCondition cdd)
                cdd.Conditions = state.FilterDefinitions.MapToDynamicCondition().ToList();

            if (cd is IPagedAndSortedResultRequest cd2)
            {
                cd2.MaxResultCount = state.PageSize;
                cd2.SkipCount = state.Page == 0 ? 0 : ((state.Page - 1) * state.PageSize);
            }

            if (cd is ISortedResultRequest cd3)
            {
                cd3.Sorting = state.SortDefinitions.ToLinqDynamicCore();
            }

            var dtos = await AppService.GetAllAsync(cd);
            pageCount = (int)Math.Ceiling(dtos.TotalCount / (state.PageSize * 1d));
            if (pageCount == 0)
                pageCount = 1;

            //不加这个，进入最后一页会无限刷新，mudblazor的bug估计
            if (pageIndex < pageCount)
                base.StateHasChanged();//不加这个，首次的页数显示不对

            return new GridData<TEntityDto>
            {
                TotalItems = dtos.TotalCount,
                Items = dtos.Items
            };
        }

        /// <summary>
        /// 已选中的项
        /// </summary>
        protected HashSet<TEntityDto> selectedItems = new HashSet<TEntityDto>();
        /// <summary>
        /// 批量选择变化时回调
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual Task SelectedItemsChanged(HashSet<TEntityDto> items)
        {
            selectedItems = items;
            return Task.CompletedTask;
            //_events.Insert(0, $"Event = SelectedItemsChanged, Data = {System.Text.Json.JsonSerializer.Serialize(items)}");
        }
        //未选中项时，直接禁用按钮
        ///// <summary>
        ///// 批量操作时，检查是否有选中项
        ///// </summary>
        ///// <returns></returns>
        //protected void CheckSelect()
        //{
        //}


        #endregion

        #region 表单
        /// <summary>
        /// 新增或修改弹窗的配置对象
        /// </summary>
        protected DialogOptions DialogOptions = new DialogOptions { CloseOnEscapeKey=true };
        private void OpenDialog()
        {
            DialogService.Show<TFormComponent>($"Simple Dialog", DialogOptions);
        }
        #endregion
    }

    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TUpdateInput,
                                   TGetInput,
                                   TAppService> : ListBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TUpdateInput,
                                                                    TGetInput,
                                                                    EntityDto<TPrimaryKey>,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TUpdateInput,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TUpdateInput,
                                                                    EntityDto<TPrimaryKey>,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TCreateInput,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TEntityDto,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    PagedAndSortedResultRequestDto,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    int,
                                                                    TAppService>
        where TEntityDto : IEntityDto<int>
        where TAppService : ICrudBaseAppService<TEntityDto>
    {
    }
}