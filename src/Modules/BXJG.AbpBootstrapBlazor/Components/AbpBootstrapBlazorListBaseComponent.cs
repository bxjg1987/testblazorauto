using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using BXJG.AbpBootstrapBlazor.Interceptors;
using BXJG.Common.Dto;
using BXJG.Utils;
using BXJG.Utils.Components;
using BXJG.Utils.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.AbpBootstrapBlazor.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TCreateInput、TUpdateInput
     * 
     * 与具体的ui框架无关，建议具体项目直接集成它，不要再为了某套UI再封装一次
     * 只保留逻辑部分的抽象，因为这里的抽象是与具体项目无关的，所以应该由具体项目的子类去按自己需求做布局
     * 
     * 不做全局异常处理，因为大部分的ui框架自带了处理方式，即使木有，用肉夹馍在具体项目去做就行了，因为具体项目引用具体ui
     */

    /// <summary>
    /// 抽象的，基于bootstrap table的列表页抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    /// <typeparam name="TList">列表类型</typeparam>
    public abstract class AbpBootstrapBlazorListBaseComponent<TAppService,
                                                              TEntityDto,
                                                              TPrimaryKey,
                                                              TGetAllInput,
                                                              TCreateInput,
                                                              TUpdateInput> : AbpListBaseComponent<TAppService,
                                                                                                   TEntityDto,
                                                                                                   TPrimaryKey,
                                                                                                   TGetAllInput,
                                                                                                   TCreateInput,
                                                                                                   TUpdateInput,
                                                                                                   List<TEntityDto>>
        where TEntityDto : class, IEntityDto<TPrimaryKey>, IExtendableDto, new()
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        protected Table<TEntityDto> table;

        [Inject]
        public MessageService MessageService { get;  set; }
        //[AbpBBException]
        protected virtual async Task<bool> OnDeleteBatch(IEnumerable<TEntityDto> items)
        {
            if (SelectedItems != default && SelectedItems.Count > 1)
                await DeleteBatch();
            else
                await DeleteItem(items.Single());
            table.SelectedRows?.Clear();
            SelectedItems?.Clear();
            return true;
        }
      // [AbpBBException]
        protected virtual async Task<QueryData<TEntityDto>> OnQuery(QueryPageOptions condition)
        {
            PageSize = condition.PageItems;
            PageIndex = condition.PageIndex;
            if (condition.SortList != null && condition.SortList.Count > 0)
                Sorting = string.Join(",", condition.SortList);
            else if (condition.SortOrder != SortOrder.Unset)
                Sorting = condition.SortName + " " + condition.SortOrder.ToString();
            else
                Sorting = default;

            Keywords = condition.SearchText;

            await LoadListData();

            return new QueryData<TEntityDto>
            {
                IsAdvanceSearch = false,
                IsFiltered = true,
                IsSearch = true,
                IsSorted = true,
                Items = Items,
                TotalCount = base.TotalCount
            };
        }

        //这里也可以用肉夹馍的全局注册处理
       // [AbpBBException]
        protected override async Task Refresh()
        {
          //  await Task.Delay(2000);
            await table.QueryAsync();
        }

        //[Inject]
        //protected MessageService MessageService { get; private set; }
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

        //[AbpBBException]
        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();
        //}
        //[AbpBBException]
        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();
        //}
        //[AbpBBException]
        //public override async Task SetParametersAsync(ParameterView parameters)
        //{
        //    await base.SetParametersAsync(parameters);
        //}
        //[AbpBBException]
        //protected override void OnParametersSet()
        //{
        //    base.OnParametersSet();
        //}
        //[AbpBBException]
        //protected override async Task OnParametersSetAsync()
        //{
        //    await base.OnParametersSetAsync();
        //}
        //[AbpBBException]
        //protected override void OnAfterRender(bool firstRender)
        //{
        //    base.OnAfterRender(firstRender);
        //}
        //[AbpBBException]
        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    await base.OnAfterRenderAsync(firstRender);
        //}
        //[AbpBBException]
        //protected override async Task KeywordsChanged(string keywords)
        //{
        //    await base.KeywordsChanged(keywords);
        //}
        //[AbpBBException]
        //protected override async Task DeleteBatch()
        //{
        //    await base.DeleteBatch();
        //}
        //[AbpBBException]
        //protected override async Task DeleteItem(TEntityDto item)
        //{
        //    await base.DeleteItem(item);
        //}
        //[AbpBBException]
        //protected override async Task LoadListData()
        //{
        //    await base.LoadListData();
        //}
    }

    // 若不是使用弹窗，而是使用tab、页面等其它方式时，应提供其它子类

    ///// <summary>
    ///// 抽象的，基于MudBlazor datagrid的列表页 抽象组件
    ///// 使用弹窗弹出新增和详情窗口
    ///// </summary>
    ///// <typeparam name="TCreateDialog">新增弹窗组件</typeparam>
    ///// <typeparam name="TDetailDialog">详情弹窗组件</typeparam>
    ///// <typeparam name="TAppService">应用服务类型</typeparam>
    ///// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    ///// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    ///// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    ///// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    ///// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    //public abstract class AbpMudListDialogBaseComponent<TCreateDialog,
    //                                                    TDetailDialog,
    //                                                    TAppService,
    //                                                    TEntityDto,
    //                                                    TPrimaryKey,
    //                                                    TGetAllInput,
    //                                                    TCreateInput,
    //                                                    TUpdateInput> : AbpListBaseComponent<TAppService,
    //                                                                                            TEntityDto,
    //                                                                                            TPrimaryKey,
    //                                                                                            TGetAllInput,
    //                                                                                            TCreateInput,
    //                                                                                            TUpdateInput>
    //    where TCreateDialog : ComponentBase
    //    where TDetailDialog : ComponentBase
    //    where TEntityDto : IEntityDto<TPrimaryKey>, IExtendableDto
    //    where TGetAllInput : new()
    //    where TUpdateInput : IEntityDto<TPrimaryKey>
    //    where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    //{
    //    /// <summary>
    //    /// 弹窗服务
    //    /// </summary>
    //    [Inject]
    //    protected virtual IDialogService DialogService { get; set; }
    //    /// <summary>
    //    /// 新增时的弹窗选项对象
    //    /// </summary>
    //    protected virtual DialogOptions DialogAddOptions => new DialogOptions { CloseOnEscapeKey = true, FullWidth = true/*, DisableBackdropClick=true*/ };//DisableBackdropClick保留它，以便我们可以使用弹窗的OnBackdropClick事件
    //    /// <summary>
    //    /// 修改时的弹窗选项对象
    //    /// </summary>
    //    protected virtual DialogOptions DialogDetailOptions => DialogAddOptions;
    //    /// <summary>
    //    /// 获取新增时传入弹窗的参数
    //    /// 如：在新增商品时，把列表页当前选中的分类id传递过去
    //    /// </summary>
    //    /// <returns></returns>
    //    protected virtual DialogParameters BuildCreateParameter() => new DialogParameters();
    //    /// <summary>
    //    /// 点击新增按钮时执行
    //    /// </summary>
    //    /// <returns></returns>
    //    [ExceptionInterceptor]
    //    protected virtual async Task BtnAddClick()
    //    {

    //        var ps = BuildCreateParameter();
    //        if (!(await DialogService.Show<TCreateDialog>("新增" + FuncName, ps, DialogAddOptions).Result).Canceled)
    //        {
    //            await dataGrid.ReloadServerData();
    //        }

    //    }
    //    /// <summary>
    //    /// 行修改按钮点击时执行
    //    /// 注：不要全局修改按钮，因为木有必要
    //    /// </summary>
    //    /// <param name="dto">当前dto对象</param>
    //    /// <returns></returns>
    //    [ExceptionInterceptor]
    //    protected virtual async Task BtnEditClick(TEntityDto dto)
    //    {

    //        var ps = new DialogParameters<TDetailDialog>();
    //        FillDetailParameters(ps, dto);
    //        ps.Add("IsEdit", true);
    //        if (!(await DialogService.Show<TDetailDialog>("修改" + FuncName, ps, DialogDetailOptions).Result).Canceled)
    //        {
    //            await dataGrid.ReloadServerData();
    //        }

    //    }
    //    /// <summary>
    //    /// 行详情按钮点击时执行
    //    /// 注：不要全局详情按钮，因为木有必要
    //    /// </summary>
    //    /// <param name="dto">当前dto对象</param>
    //    /// <returns></returns>
    //    [ExceptionInterceptor]
    //    protected virtual async Task BtnDetailClick(TEntityDto dto)
    //    {

    //        var ps = new DialogParameters<TDetailDialog>();
    //        FillDetailParameters(ps, dto);
    //        ps.Add("IsEdit", false);
    //        if (!(await DialogService.Show<TDetailDialog>("查看" + FuncName + "详情", ps, DialogDetailOptions).Result).Canceled)
    //        {
    //            //说明在详情页面 又进入了修改 且修改后保存了数据
    //            await dataGrid.ReloadServerData();
    //        }

    //    }
    //    protected override async ValueTask RowDoubleClick(DataGridRowClickEventArgs<TEntityDto> arg)
    //    {
    //        // if (updateIsGranted)
    //        //  {
    //        //      await BtnEditClick(arg.Item); }
    //        //  else
    //        //   {
    //        await BtnDetailClick(arg.Item);
    //        //  }

    //    }
    //    /// <summary>
    //    /// 弹出详情弹窗时传入参数
    //    /// 通常，复杂数据时只传入id，让详情组件自己去重新查询；简单数据时传入当前选择的dto
    //    /// </summary>
    //    /// <param name="pms"></param>
    //    /// <param name="dto"></param>
    //    protected virtual void FillDetailParameters(DialogParameters<TDetailDialog> pms, TEntityDto dto)
    //    {
    //        pms.Add("Id", dto.Id);
    //    }
    //}
}