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
using Rougamo;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BXJG.AbpBootstrapBlazor.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TCreateInput、TUpdateInput
     * 
     * 与具体的ui框架无关，建议具体项目直接集成它，不要再为了某套UI再封装一次
     * 只保留逻辑部分的抽象，因为这里的抽象是与具体项目无关的，所以应该由具体项目的子类去按自己需求做布局
     * 
     * 不做全局异常处理，因为大部分的ui框架自带了处理方式，即使木有，用肉夹馍在具体项目去做就行了，因为具体项目引用具体ui
     * 
     * 之前我们实现过动态条件，参考BXJG.MudBlazor中的实现
     * 动态条件对于用户来讲有点复杂，所以我们暂时不考虑
     * 
     * bb支持弹出和顶部的搜索，最终决定选额顶部的方式，这样用户查看列表时可以看到自己的条件，而且顶部的方式本身可以收缩的
     * 
     * bb里支持动态条件，且高级搜索依然转化为动态条件，下面说说高级中的流程
     * 我们的条件dto应该实现ITableSearchModel，然后赋值给CustomerSearchModel，但我们的dto是定义在应用层的，不可能引入bb的东东
     * 所以我们可以让页面本身来实现这个接口，将this赋值给CustomerSearchModel
     * 
     * 然后重新动态条件方法
     * 搜索按钮点击后，OnQuery执行前会回调动态条件构造，然后才执行OnQuery
     * 我们在OnQuery中通过参数拿到动态条件，然后转换为我们查询dto的值，或查询dto的动态条件
     * 
     * 这样有个好处，我们不用在应用层的查询dto定义一堆条件了，也不需要应用层写一堆查询逻辑了
     * 但还是要保留，防止有高级处理
     * 但这里需要转两次，有点浪费，我们决定直接将表单值转换为动态条件，这样bb的CustomerSearchModel就不需要了
     * 但为了能显示出自定义搜索框，还行需要实现ITableSearchModel，只不过实现为空
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
    public abstract class AbpBBListBaseComponent<TAppService,
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
                                                                                      List<TEntityDto>>, ITableSearchModel
        where TEntityDto : class, IEntityDto<TPrimaryKey>, IExtendableDto, new()
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        protected Table<TEntityDto> table;
        [AbpBBException]
        protected override async Task DeleteBatch()
        {
            await base.DeleteBatch();
        }
        [AbpBBException]
        protected override async Task DeleteItem(TEntityDto item)
        {
            await base.DeleteItem(item);
        }
        [AbpBBException]
        protected override async Task KeywordsChanged(string keywords)
        {
            await base.KeywordsChanged(keywords);
        }
        [Inject]
        public MessageService MessageService { get; set; }
        [AbpBBException]
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
        [AbpBBException]
        protected virtual async Task<QueryData<TEntityDto>> OnQuery(QueryPageOptions condition)
        {
            /*
             * 由于bootstrap关于分页和关键字，木有双休绑定，因此这里手动赋值
             * 目前只考虑高级搜索方式，不考虑动态条件
             */

            if (GetAllInput is ILimitedResultRequest dx)
            {
                dx.MaxResultCount = condition.PageItems;
            }
            if (GetAllInput is IHaveFilter dx1 && dx1.Filter is ILimitedResultRequest dx2)
            {
                dx2.MaxResultCount = condition.PageItems;
            }


            if (GetAllInput is IPagedResultRequest tj1)
                tj1.SkipCount = (condition.PageIndex - 1) * condition.PageItems;
            else if (GetAllInput is IHaveFilter tj2 && tj2.Filter is IPagedResultRequest tj3)
                tj3.SkipCount = (condition.PageIndex - 1) * condition.PageItems;


            //if (pageIndex <= 0)
            //    pageIndex = 1;

            //return pageIndex;

            ISortedResultRequest sd222 = null;
            if (GetAllInput is ISortedResultRequest dxx)
                sd222 = dxx;
            else if (GetAllInput is IHaveFilter dx11 && dx11.Filter is ISortedResultRequest dx22)
                sd222 = dx22;
            if (condition.SortList != null && condition.SortList.Count > 0)
                sd222.Sorting = string.Join(",", condition.SortList);
            else if (condition.SortOrder != SortOrder.Unset)
                sd222.Sorting = condition.SortName + " " + condition.SortOrder.ToString();
            else
                sd222.Sorting = "Id";

            #region 关键字
            IHaveKeywords gjz = null;
            if (GetAllInput is IHaveKeywords cd4)
            {
                gjz = cd4;
            }
            else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
            {
                gjz = cddqq;
            }
            gjz.Keywords = condition.SearchText;
            #endregion

            //动态条件的填充请已在父类中定义

            await LoadListData();

            return new QueryData<TEntityDto>
            {
                IsAdvanceSearch = true,
                IsFiltered = true,
                IsSearch = true,
                IsSorted = true,
                Items = Items,
                TotalCount = base.TotalCount
            };
        }
        [AbpBBException]
        protected override async Task Refresh()
        {
            await table.QueryAsync();
        }
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

        /// <summary>
        /// 获得 搜索条件集合
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<IFilterAction> GetSearches()
        {
            //看顶部注释

            var ret = new List<IFilterAction>();

            //if (!string.IsNullOrEmpty(Name))
            //{
            //    ret.Add(new SearchFilterAction(nameof(Foo.Name), Name));
            //}

            //if (!string.IsNullOrEmpty(Count))
            //{
            //    if (Count == "1")
            //    {
            //        ret.Add(new SearchFilterAction(nameof(Foo.Count), 30, FilterAction.LessThan));
            //    }
            //    else if (Count == "2")
            //    {
            //        ret.Add(new SearchFilterAction(nameof(Foo.Count), 30, FilterAction.GreaterThanOrEqual));
            //        ret.Add(new SearchFilterAction(nameof(Foo.Count), 70, FilterAction.LessThan));
            //    }
            //    else if (Count == "3")
            //    {
            //        ret.Add(new SearchFilterAction(nameof(Foo.Count), 70, FilterAction.GreaterThanOrEqual));
            //        ret.Add(new SearchFilterAction(nameof(Foo.Count), 100, FilterAction.LessThan));
            //    }
            //}

            //if (SearchDate != null)
            //{
            //    ret.Add(new SearchFilterAction(nameof(Foo.DateTime), SearchDate.Start, FilterAction.GreaterThanOrEqual));
            //    ret.Add(new SearchFilterAction(nameof(Foo.DateTime), SearchDate.End, FilterAction.LessThanOrEqual));
            //}

            //if (Education != null)
            //{
            //    ret.Add(new SearchFilterAction(nameof(Foo.Education), Education, FilterAction.Equal));
            //}
            return ret;
        }

        /// <summary>
        /// 重置操作
        /// </summary>
        [AbpBBException]
        public virtual void Reset()
        {
            GetAllInput = new TGetAllInput();
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
}