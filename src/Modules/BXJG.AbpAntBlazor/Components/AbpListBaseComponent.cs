using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using AntDesign.TableModels;
using BXJG.Common.Dto;
using BXJG.Utils;
using BXJG.Utils.Components;
using BXJG.Utils.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Rougamo;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BXJG.AbpBlazor.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TCreateInput、TUpdateInput
     * 
     * 只保留逻辑部分的抽象，因为这里的抽象是与具体项目无关的，所以应该由具体项目的子类去按自己需求做布局
     * 
     * 之前我们实现过动态条件，参考BXJG.MudBlazor中的实现
     * 动态条件对于用户来讲有点复杂，所以我们暂时不考虑
     * 
     * 这样有个好处，我们不用在应用层的查询dto定义一堆条件了，也不需要应用层写一堆查询逻辑了
     * 但还是要保留，防止有高级处理
     * 但这里需要转两次，有点浪费，我们决定直接将表单值转换为动态条件，这样bb的CustomerSearchModel就不需要了
     */

    /// <summary>
    /// 抽象的，基于ant table的列表页抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    /// <typeparam name="TCreateComponent">新增组件类型</typeparam>
    /// <typeparam name="TEditOrDetailComponent">修改和详情组件类型</typeparam>
    public abstract class AbpListBaseComponent<TAppService,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput,
                                               TCreateInput,
                                               TUpdateInput,
                                               TCreateComponent,
                                               TEditOrDetailComponent> : AbpListBaseComponent<TAppService,
                                                                                              TEntityDto,
                                                                                              TPrimaryKey,
                                                                                              TGetAllInput,
                                                                                              TCreateInput,
                                                                                              TUpdateInput,
                                                                                              IEnumerable<TEntityDto>>
        where TCreateComponent : AbpCreateBaseComponent<TAppService,
                                                        TEntityDto,
                                                        TPrimaryKey,
                                                        TGetAllInput,
                                                        TCreateInput,
                                                        TUpdateInput>
        where TCreateInput : new()
        where TEntityDto : IEntityDto<TPrimaryKey>, IExtendableDto//, new()
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {


        /// <summary>
        /// 必须的，统一异常处理拦截器要用
        /// </summary>
        [Inject]
        public IMessageService MessageService { get; set; }
        /// <summary>
        /// 对ant表格的引用
        /// </summary>
        protected Table<TEntityDto> table;
        [AbpExceptionInterceptor]
        protected override async Task Delete()
        {
            await base.Delete();
        }
        [AbpExceptionInterceptor]
        protected override async Task Delete(TEntityDto item)
        {
            await base.Delete(item);
        }
        [AbpExceptionInterceptor]
        protected override async Task KeywordsChanged(string keywords = default)
        {
            await base.KeywordsChanged(keywords);
        }
        //[AbpExceptionInterceptor]
        //protected virtual async Task<bool> OnDeleteBatch(IEnumerable<TEntityDto> items)
        //{
        //    if (SelectedItems != default && SelectedItems.Count > 1)
        //        await DeleteBatch();
        //    else
        //        await DeleteItem(items.Single());
        //    //table.SelectedRows?.Clear();
        //    SelectedItems?.Clear();
        //    return true;
        //}
        [AbpExceptionInterceptor]
        protected virtual async Task OnQuery(QueryModel condition)
        {
            /*
             * 目前只考虑高级搜索方式，不考虑动态条件
             */



            var ls = condition.SortModel.Where(c => c.Sort.IsNotNullOrWhiteSpaceBXJG()).OrderBy(c => c.Priority).Select(c => c.FieldName + " " + c.Sort.Replace("end", ""));
            Sorting = string.Join(",", ls);



            // var r =await AppService.GetAllAsync(GetAllInput);
            //Items = r.Items;
            // TotalCount = r.TotalCount;
            //if (condition.SortList != null && condition.SortList.Count > 0)
            //    sd222.Sorting = string.Join(",", condition.SortList);
            //else if (condition.SortOrder != SortOrder.Unset)
            //    sd222.Sorting = condition.SortName + " " + condition.SortOrder.ToString();
            //else
            //    sd222.Sorting = "Id";

            //#region 关键字
            //IHaveKeywords gjz = null;
            //if (GetAllInput is IHaveKeywords cd4)
            //{
            //    gjz = cd4;
            //}
            //else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
            //{
            //    gjz = cddqq;
            //}
            //gjz.Keywords = condition.SearchText;
            //#endregion

            ////动态条件的填充请已在父类中定义

            await LoadListData();

            //return new QueryData<TEntityDto>
            //{
            //    IsAdvanceSearch = true,
            //    IsFiltered = true,
            //    IsSearch = true,
            //    IsSorted = true,
            //    Items = Items,
            //    TotalCount = base.TotalCount
            //};
        }
        [AbpExceptionInterceptor]
        protected override async Task Refresh()
        {
            var qm = table.GetQueryModel();
            var nqm = new QueryModel(1, qm.PageSize, qm.StartIndex, qm.SortModel, qm.FilterModel);
            table.ReloadData(nqm);

            SelectedItems = new List<TEntityDto>();
        }
        [AbpExceptionInterceptor]
        protected override async Task Reset()
        {
            // table.ResetData();//不晓得为啥不行

            //  PageIndex = 1;
            //  PageSize = 20;
            //  Keywords = string.Empty;
            //await Refresh();
            //  await base.Reset();
            // table.ResetData();
            // table.ReloadData();
            
            table.ResetData();
           await base.Reset();

        }
        protected override async ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            await MessageService.Error(msg);
        }
        protected override async ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            await MessageService.Success(msg);
        }


        ///// <summary>
        ///// 搜索条件重置操作
        ///// </summary>
        //[AbpExceptionInterceptor]
        //public virtual void Reset()
        //{
        //    //GetAllInput = new TGetAllInput();
        //    table.ResetData();
        //}

        /// <summary>
        /// 对新增组件的引用
        /// </summary>
        protected TCreateComponent createComponent;

        ///// <summary>
        ///// 点击列表中弹出新增框底部的保存按钮时执行
        ///// </summary>
        ///// <param name="dto"></param>
        ///// <param name="itemChangedType"></param>
        ///// <returns></returns>
        //protected virtual async Task<bool> OnSaveAsync(TEntityDto dto, ItemChangedType itemChangedType)
        //{
        //    return await createComponent.BtnSaveClick();
        //}

        #region 生命周期方法增加统一异常处理拦截器
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
        #endregion
    }
}