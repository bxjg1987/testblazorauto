using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Extensions;
using Abp.UI;
using BXJG.Common.Contracts;
using BXJG.Utils;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BXJG.Utils.RCL.Components
{
    /*
     * 之前我们实现过动态条件，参考BXJG.MudBlazor中的实现
     * 动态条件对于用户来讲有点复杂，所以我们暂时不考虑
     * 
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TCreateInput、TUpdateInput
     * 
     * 虽然当前项目直接依赖antblazor，且此当前抽象列表组件也依赖它
     * 但我们实现逻辑是尽量考虑标准的抽象列表组件，因此在组件中直接包含PageSize、PageIndex等属性，
     * 而不是完全依赖antblazor的套路
     * 这样，将来我们需要抽象一个标准的列表组件时，这里的大部分代码是可以复制到抽象中的。
     * 
     * 它仅仅定义列表相关功能，并不包含新增、修改等弹窗相关内容，那个交给子类去实现，因为有列表不一定需要弹窗
     * 
     * 异步操作要界面流畅，请查看删除明细的注释
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
    public abstract class ListBaseComponent<TEntityDto,
                                            TPrimaryKey,
                                            TGetAllInput> : BaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>, IExtendableObj//, new()
        where TGetAllInput : new()
    {
        //界面部分就不要用IPermissionChecker了，不过server模式时AuthorizationService内部会使用IPermissionChecker
        //请查看自定义授权策略提供器
        //客户端部分是直接在前端内存中比对的，有区别于server模式的，自定义的授权策略提供器

      

        /// <summary>
        /// 此功能的名称
        /// </summary>
        protected abstract string FuncName { get; }
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
        /// 我们只需要最终是否有某个状态，不需要保留原本的权限字符串，所以使用方法定义，而非虚属性
        /// </summary>
        /// <param name="createPermissionName"></param>
        /// <param name="updatePermissionName"></param>
        /// <param name="deletePermissionName"></param>
        /// <returns></returns>
        protected virtual async Task InitPermission(string createPermissionName = default, string updatePermissionName = default,
            string deletePermissionName = default/*, string getPermissionName = default*/, IDictionary<string, bool> others = default)
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            if (createPermissionName.IsNotNullOrWhiteSpaceBXJG())
                createIsGranted = (await AuthorizationService.AuthorizeAsync(authState.User, createPermissionName)).Succeeded;//await PermissionChecker.IsGrantedAsync(createPermissionName);
            if (updatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                updateIsGranted = (await AuthorizationService.AuthorizeAsync(authState.User, updatePermissionName)).Succeeded;// await PermissionChecker.IsGrantedAsync(updatePermissionName);
            if (deletePermissionName.IsNotNullOrWhiteSpaceBXJG())
                deleteIsGranted = (await AuthorizationService.AuthorizeAsync(authState.User, deletePermissionName)).Succeeded;//await PermissionChecker.IsGrantedAsync(deletePermissionName);
            if (others != default)
            {
                foreach (var item in others)
                {
                    others[item.Key] = (await AuthorizationService.AuthorizeAsync(authState.User, item.Key)).Succeeded;
                }
            }
            //if (getPermissionName.IsNotNullOrWhiteSpaceBXJG())
            //    getIsGranted = await PermissionChecker.IsGrantedAsync(getPermissionName);
        }
        /// <summary>
        /// 批量操作消息提醒
        /// 如：批量删除、批量审核时消息提醒
        /// </summary>
        /// <param name="output">批量操作结果</param>
        /// <param name="funName">操作名</param>
        protected virtual async Task BatchOperationMessage(BatchOperationOutputBase output, string funName = "删除")
        {
            if (output.ErrorMessage.Any())
            {
                //http请求拦截器全部失败时会抛出异常；部分失败时直接提示，然后返回数据；全部成功时仅返回数据，所以不需要再调用这里了
                //string errMsg = string.Empty;
                //foreach (var item in output.ErrorMessage)
                //    errMsg += item.Message+ Environment.NewLine;

                //if (output.Ids.Count == 0)
                //    await ShowFailMessage(msg: $"{funName}失败！"+ errMsg);
                //else
                //    await ShowFailMessage(msg: $"{funName}部分失败！成功数量：{output.Ids.Count}；{errMsg}");
            }
            else
                await ShowSuccessMessage(msg: $"{funName}成功！");
        }
        #region 列表
        /// <summary>
        /// 搜索条件
        /// </summary>
        protected TGetAllInput GetAllInput = new TGetAllInput();
        /// <summary>
        /// 是否是分页模式
        /// </summary>
        public bool IsPage => GetAllInput is IPagedResultRequest;

        ///// <summary>
        ///// 填充动态条件
        ///// </summary>
        ///// <returns></returns>
        //protected virtual ValueTask FillDynamicConditions(ICollection<ConditionFieldDefine> conditions) => ValueTask.CompletedTask;

        /// <summary>
        /// 获取每页行数，若不做分页请返回0
        /// 它本质上是对GetAllInput的读写，由于GetAllInput是或不是分页条件，所以内部做了特殊处理（由于dto是应用层的，所以在这里而不是dto上定义此逻辑）
        /// </summary>
        protected virtual int PageSize
        {
            get
            {
                if (GetAllInput is ILimitedResultRequest dx)
                {
                    return dx.MaxResultCount;
                }
                else if (GetAllInput is IHaveFilter dx1 && dx1.Filter is ILimitedResultRequest dx2)
                {
                    return dx2.MaxResultCount;
                }
                return int.MaxValue;
            }
            set
            {
                if (GetAllInput is ILimitedResultRequest dx)
                {
                    dx.MaxResultCount = value;
                }
                else if (GetAllInput is IHaveFilter dx1 && dx1.Filter is ILimitedResultRequest dx2)
                {
                    dx2.MaxResultCount = value;
                }
            }
        }
        /// <summary>
        /// 排序规则，格式："field1 aes,field2 desc"
        /// 它本质上是对GetAllInput的读写，由于GetAllInput是或不是分页条件，所以内部做了特殊处理（由于dto是应用层的，所以在这里而不是dto上定义此逻辑）
        /// </summary>
        public virtual string Sorting
        {
            get
            {
                var str = string.Empty;
                if (GetAllInput is ISortedResultRequest dxx)
                    str = dxx.Sorting;
                else if (GetAllInput is IHaveFilter dx11 && dx11.Filter is ISortedResultRequest dx22)
                    str = dx22.Sorting;
                if (str.IsNullOrWhiteSpace())
                    return "Id";
                return str;
            }
            set
            {
                ISortedResultRequest sd222 = null;
                if (GetAllInput is ISortedResultRequest dxx)
                    sd222 = dxx;
                else if (GetAllInput is IHaveFilter dx11 && dx11.Filter is ISortedResultRequest dx22)
                    sd222 = dx22;

                // var ls = condition.SortModel.Where(c => c.Sort.IsNotNullOrWhiteSpaceBXJG()).OrderBy(c => c.Priority).Select(c => c.FieldName + " " + c.Sort.Replace("end", ""));
                //  sd222.Sorting = string.Join(",", ls);
                sd222.Sorting = value;

                if (sd222 == default || sd222.Sorting.IsNullOrWhiteSpaceBXJG())
                    sd222.Sorting = "Id";
            }
        }
        /// <summary>
        /// 当前列表数据
        /// 通常是当前页的数据
        /// </summary>
        protected virtual List<TEntityDto> Items { get; set; } = new List<TEntityDto>();
        /// <summary>
        /// 当前条件下的总数据数量
        /// </summary>
        protected virtual int TotalCount { get; set; }
        /// <summary>
        /// 当前是第几页
        /// 它本质上是对GetAllInput的读写，由于GetAllInput是或不是分页条件，所以内部做了特殊处理（由于dto是应用层的，所以在这里而不是dto上定义此逻辑）
        /// </summary>
        protected virtual int PageIndex
        {
            get
            {
                //  int pageIndex = 1;
                //  if (GetAllInput is IPagedResultRequest dx)
                return (GetAllInput as IPagedResultRequest).SkipCount / PageSize + 1;

                //若是纯条件，也就是不分页，就木有必要

                //if (pageIndex <= 0)
                //    pageIndex = 1;


            }
            set
            {
                if (GetAllInput is IPagedResultRequest dx)
                {
                    dx.SkipCount = (value - 1) * PageSize;
                }
            }
        }
        /// <summary>
        /// 父类仅仅需要读，至于是否可写由子类自己决定
        /// </summary>
        protected virtual IEnumerable<TEntityDto> SelectedItems { get; set; } = new List<TEntityDto>();
        /// <summary>
        /// 父类仅仅需要读，至于是否可写由子类自己决定
        /// </summary>
        protected virtual bool IsLoading { get; set; }
        /// <summary>
        /// 异步加载列表数据
        /// </summary>
        /// <returns></returns>
        protected virtual void LoadListData()
        {
            /*
             * 某些操作比如删除时，删除后提示，然后加载数据
             * 此过程不用等到加载完成后才人为整个删除过程才完成，而是删除后提示就算完成，后续的刷新完全可以是异步的
             * 
             * 外部可能并不是事件触发时调用这里，所以需要主动调用StateChange
             * 
             * 很多处理都需要重写加载，所以把这个加载做成异步
             */

            if (IsLoading)
                return;

            IsLoading = true;


            if (SelectedItems != default && SelectedItems is ICollection<TEntityDto> tempList)
                tempList.Clear();
            else
                SelectedItems = new List<TEntityDto>();




            StateHasChanged();

            InvokeAsync(async () =>
            {
                try
                {
                    await LoadCore();
                }
                finally
                {
                    IsLoading = false;
                    StateHasChanged();
                }
            });

        }
        /// <summary>
        /// 根据条件TGetAllInput加载数据的核心方法
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadCore()
        {
            //var cd = GetAllInput;
            //if (cd is IDynamicCondition cdd)
            //{
            //    cdd.Conditions = new List<ConditionFieldDefine>();// await BuildDynamicCondition();// state.FilterDefinitions.MapToDynamicCondition().ToList();
            //    await FillDynamicConditions(cdd.Conditions as List<ConditionFieldDefine>);
            //}
            //else if (cd is IHaveFilter cddq && cddq.Filter is IDynamicCondition cddqq)
            //{
            //    cddqq.Conditions = new List<ConditionFieldDefine>();// await BuildDynamicCondition();//state.FilterDefinitions.MapToDynamicCondition().ToList();
            //    await FillDynamicConditions(cddqq.Conditions as List<ConditionFieldDefine>);
            //}
            //if (cd is IPagedAndSortedResultRequest cd2)
            //{
            //    cd2.MaxResultCount = PageSize; //state.PageSize;
            //    cd2.SkipCount = (PageIndex - 1) * PageSize;
            //}
            //if (cd is ISortedResultRequest cd3)
            //{
            //    cd3.Sorting = Sorting;// state.SortDefinitions.ToLinqDynamicCore();
            //}

            //if (cd is IHaveKeywords cd4)
            //{
            //    cd4.Keywords = Keywords;
            //}
            //else if (cd is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
            //{
            //    cddqq.Keywords = Keywords;// state.FilterDefinitions.MapToDynamicCondition().ToList();
            //}
            //await FillCondtion(cd);
            await Task.Yield();//某些ui，如antblazor的下拉框 选择事件触发时，绑定的属性还没更新，所以这里让出下线程
            var dtos = await HttpClient.GetAll<TEntityDto>(GetAllInput);// AppService.GetAllAsync(GetAllInput);

            //_ = InvokeAsync(StateHasChanged);//让多选影响顶部按钮得以执行 包一层是因为需要加载完才执行
            //dataGrid.SelectedItems.Clear();//翻页时，已选择的好像木有清空，这里手动来下

            //给每行属性附加额外状态

            foreach (var dto in dtos.Items)
            {
                dynamic dd = new ExpandoObject();
                dd.IsDeleting = false;
                dd.IsShowDeleteConfirmation = false;
                await AddItemExtData(dto, dd);
                dto.ExtensionData = dd;
            }
            Items = dtos.Items.ToList();
            TotalCount = dtos.TotalCount;

        }
        /// <summary>
        /// 获取列表时，为其中的每项添加额外的数据
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual ValueTask AddItemExtData(TEntityDto dto, dynamic data) => ValueTask.CompletedTask;
        /// <summary>
        /// 搜索关键字
        /// </summary>
        protected virtual string Keywords
        {
            get
            {
                if (GetAllInput is IHaveKeywords cd4)
                {
                    return cd4.Keywords;
                }
                else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
                {
                    return cddqq.Keywords;// state.FilterDefinitions.MapToDynamicCondition().ToList();
                }
                return string.Empty;
            }
            set
            {
                if (GetAllInput is IHaveKeywords cd4)
                {
                    cd4.Keywords = value;
                }
                else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
                {
                    cddqq.Keywords = value;// state.FilterDefinitions.MapToDynamicCondition().ToList();
                }
            }
        }
        /// <summary>
        /// 条件变化时回调
        /// </summary>
        /// <returns></returns>
        protected virtual void BtnSearchClick()
        {
            //  Console.WriteLine(DateTime.Now.ToString("fff"));
            //await Task.Delay(1);
            //if (GetAllInput is IHaveKeywords cd4)
            //{
            //    cd4.Keywords = keywords;
            //}
            //else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
            //{
            //    cddqq.Keywords = keywords;// state.FilterDefinitions.MapToDynamicCondition().ToList();
            //}
            //PageIndex = 1;

            // table.ResetData();
            //PageIndex = 1;
            //PageSize = 20;
            //Keywords = string.Empty;
            // await OnQuery(table.GetQueryModel());
            PageIndex = 1;
            LoadListData();
            // Keywords = keywords;
            // await LoadListData();
            //table.ReloadData();
        }
        /// <summary>
        /// 条件分页都不变，重新加载当前数据
        /// </summary>
        protected virtual void BtnRefreshClick()
        {
            //if (GetAllInput is IHaveKeywords cd4)
            //{
            //    cd4.Keywords = keywords;
            //}
            //else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
            //{
            //    cddqq.Keywords = keywords;// state.FilterDefinitions.MapToDynamicCondition().ToList();
            //}
            //PageIndex = 1;
            //table.ReloadData(PageIndex);
            // Keywords = keywords;
            //table.cac
            LoadListData();
            //table.ReloadData();
        }
        /// <summary>
        /// 清空所有条件并重新加载
        /// 若有更多条件，子类应重写此方法清空条件，并执行base.ReLoad()
        /// </summary>
        /// <returns></returns>
        protected virtual void BtnClearFilterClick()
        {
            if (GetAllInput is IReset t)
                t.Reset();
            else if (GetAllInput is IHaveFilter p && p.Filter is IReset qq)
                qq.Reset();
            else
                GetAllInput = new TGetAllInput();

            PageIndex = 1;
            PageSize = 20;
            Keywords = string.Empty;



            //StateHasChanged();
            //await OnQuery(table.GetQueryModel());
            LoadListData();

            //  await base.Reset();
            // table.ResetData();
            //table.ReloadData(); 远程加载时，根本不会执行这里

            // table.ResetData();//它仅仅是将条件复位，并不会加载数据

        }
        ///// <summary>
        ///// 重置搜索条件后刷新
        ///// </summary>
        ///// <returns></returns>
        //protected virtual async Task Reset()
        //{
        //    PageIndex = 1;
        //    PageSize = 20;
        //    Keywords = string.Empty;
        //    await Refresh();
        //}
        #endregion
        /// <summary>
        /// 是否正在加载、删除...
        /// </summary>
        protected virtual bool IsBusy => IsLoading || isDeleting;
        #region 删除
        //没权限时不显示的，所以不加入这个判断
        /// <summary>
        /// 是否禁用批量删除按钮，出现任意情况，则为true：正在加载数据；正在删除数据；没有选择数据；
        /// </summary>
        protected virtual bool ShouldDisableDelete => IsBusy || SelectedItems == default || !SelectedItems.Any();
        /// <summary>
        /// 是否正在执行批量删除操作
        /// </summary>
        protected bool isDeleting = false;
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        protected virtual async Task BtnDeleteClick()
        {
            await Delete();
        }
        /// <summary>
        /// 批量删除的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Delete()
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            if (isDeleting) return;
            isDeleting = true;


            try
            {
                var r = await DeleteCore();
                isDeleting = false;


                //后续逻辑都是辅助性的，因此放到异步中，加快主操作速度
                _ = InvokeAsync(async () =>
                {
                   
                    await BatchOperationMessage(r);//await表示显示因此后才结束，所以这里不要等待
                   
                    
                    if (r.Ids.Any())
                        LoadListData();
                }).ConfigureAwait(false);



            }
            finally
            {
                isDeleting = false;
            }
        }
        /// <summary>
        /// 批量删除已选择的项的核心逻辑
        /// </summary>
        /// <returns></returns>
        public virtual Task<BatchOperationOutput<TPrimaryKey>> DeleteCore()
        {
            return HttpClient.DeleteBatch<TPrimaryKey, TEntityDto>(new BatchOperationInput<TPrimaryKey> { Ids = SelectedItems.Select(x => x.Id).ToArray() });//  AppService.BatchDeleteAsync(new BatchOperationInput<TPrimaryKey> { Ids = SelectedItems.Select(x => x.Id).ToArray() });
        }
        /// <summary>
        /// 删除单个项
        /// 这里是同步的，确认框可以很快隐藏掉
        /// 这里可以去实施aop拦截器
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual async Task BtnDeleteItemClick(TEntityDto item)
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            // var curr = dataGrid.Items.Single(c => c.Id!.Equals(input.Id));
            await DeleteItem(item);
        }
        /// <summary>
        /// 删除界面逻辑，通常不需要重写，多个地方需要删除单个项目时通常调用这里
        /// </summary>
        /// <param name="item"></param>
        protected virtual async Task DeleteItem(TEntityDto item)
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            // var curr = dataGrid.Items.Single(c => c.Id!.Equals(input.Id));
            if (item.ExtensionData.IsDeleting)
                return;
            item.ExtensionData.IsDeleting = true;

            //异步来，便于删除确认框快速隐藏

            try
            {
                await DeleteItemCore(item);
                item.ExtensionData.IsDeleting = false;

                //后续逻辑都是辅助性的，因此放到异步中，加快主操作速度
                _ = InvokeAsync(async () =>
                {
                    await ShowSuccessMessage("删除提示", "删除成功！");
                    LoadListData();
                }).ConfigureAwait(false);

            }
            finally
            {
                item.ExtensionData.IsDeleting = false;
            }
        }
        /// <summary>
        /// 删除单条数据的核心逻辑
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual async Task DeleteItemCore(TEntityDto item)
        {
            await HttpClient.Delete<TEntityDto>(new EntityDto<TPrimaryKey>(item.Id));
        }
        #endregion
    }
}