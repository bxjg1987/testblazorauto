using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using AntDesign;
using AntDesign.TableModels;
using BXJG.Common.Contracts;
using BXJG.Utils;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Share.GeneralTree;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Rougamo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZLJ.RCL.Interceptors;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZLJ.RCL.Components
{
    /*
     * 之前我们实现过动态条件，参考BXJG.MudBlazor中的实现
     * 动态条件对于用户来讲有点复杂，所以我们暂时不考虑
     */

    /// <summary>
    /// 抽象的，基于MudBlazor treeView的列表页抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    public abstract class TreeListBaseComponent<TAppService,
                                                TEntityDto,
                                                TCreateInput,
                                                TEditDto,
                                                TGetAllInput> : BaseComponent
        //where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntityDto : IGeneralTree<TEntityDto>, IExtendableObj//, new()
        //where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TGetAllInput : new()
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
    {
        //界面部分就不要用IPermissionChecker了，不过server模式时AuthorizationService内部会使用IPermissionChecker
        //请查看自定义授权策略提供器
        //客户端部分是直接在前端内存中比对的，有区别于server模式的，自定义的授权策略提供器

        /// <summary>
        /// 请使用AuthorizationService
        /// </summary>
        IAuthorizationService authorizationService;
        /// <summary>
        /// 授权检查服务
        /// </summary>
        protected virtual IAuthorizationService AuthorizationService => authorizationService ??= ScopedServices.GetRequiredService<IAuthorizationService>();

        /// <summary>
        /// 请调用AppService
        /// </summary>
        TAppService? appService;
        /// <summary>
        /// 获取主服务
        /// </summary>
        protected virtual TAppService AppService => appService ??= ScopedServices.GetRequiredService<TAppService>();
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
        /// 身份验证状态，server、wasm的实现不同
        /// </summary>
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
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
        protected virtual async ValueTask BatchOperationMessage(BatchOperationOutputLong output, string funName = "删除")
        {
            if (output.ErrorMessage.Any())
            {
                if (output.Ids.Count == output.ErrorMessage.Count)
                    await ShowFailMessage(msg: $"批量{funName}全部失败！");
                else
                    await ShowFailMessage(msg: $"批量{funName}部分失败！成功数量：{output.Ids.Count}；失败数量：{output.ErrorMessage.Count}");
            }
            else
                await ShowSuccessMessage(msg: $"批量{funName}全部成功！");
        }
        #region 列表
        /// <summary>
        /// 搜索条件
        /// </summary>
        protected TGetAllInput GetAllInput = new TGetAllInput();

        /// <summary>
        /// 排序规则，格式："field1 aes,field2 desc"
        /// </summary>
        public virtual string Sorting
        {
            get
            {
                if (GetAllInput is ISortedResultRequest dxx)
                    return dxx.Sorting;
                else if (GetAllInput is IHaveFilter dx11 && dx11.Filter is ISortedResultRequest dx22)
                    return dx22.Sorting;
                return "Code";
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
                if (sd222 != default)
                {
                    sd222.Sorting = value;

                    if (sd222.Sorting.IsNullOrWhiteSpaceBXJG())
                        sd222.Sorting = "Code";
                }
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
        /// 父类仅仅需要读，至于是否可写由子类自己决定
        /// </summary>
        protected virtual IEnumerable<TEntityDto> SelectedItems { get; set; } = new List<TEntityDto>();
        /// <summary>
        /// 父类仅仅需要读，至于是否可写由子类自己决定
        /// </summary>
        protected virtual bool IsLoading { get; set; }

        ///// <summary>
        ///// 刷新列表
        ///// </summary>
        ///// <returns></returns>
        //protected virtual async Task Refresh()
        //{
        //    await LoadListData();
        //    if (SelectedItems != default && SelectedItems is ICollection<TEntityDto> list)
        //        list.Clear();
        //    else
        //        SelectedItems = new List<TEntityDto>() ;
        //}

        /// <summary>
        /// 加载列表数据
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadListData()
        {
            if (IsLoading)
                return;

            IsLoading = true;
            try
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
                await LoadCore();
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// 根据条件TGetAllInput加载数据的核心方法
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadCore()
        {
            var dtos = await AppService.GetAllAsync(GetAllInput);

            //_ = InvokeAsync(StateHasChanged);//让多选影响顶部按钮得以执行 包一层是因为需要加载完才执行
            //dataGrid.SelectedItems.Clear();//翻页时，已选择的好像木有清空，这里手动来下

            //给每行属性附加额外状态

            await Map(dtos);
            Items = dtos;
            TotalCount = dtos.Count;

            if (SelectedItems != default && SelectedItems is ICollection<TEntityDto> list)
                list.Clear();
            else
                SelectedItems = new List<TEntityDto>();
        }

        async ValueTask Map(IList<TEntityDto> dtos) {
            foreach (var dto in dtos)
            {
                dynamic dd = new ExpandoObject();
                dd.IsDeleting = false;
                dd.IsShowDeleteConfirmation = false;
                await AddItemExtData(dto, dd);
                dto.ExtensionData = dd;

                if (dto.Children != default && dto.Children.Any())
                   await Map(dto.Children);
            }
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
        [AbpExceptionInterceptor]
        protected virtual async Task BtnSearchClick()
        {
            //Console.WriteLine(DateTime.Now.ToString("fff"));
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

            //table.ResetData();

            //PageIndex = 1;
            //PageSize = 20;
            //Keywords = string.Empty;
            //await OnQuery(table.GetQueryModel());
            await LoadListData();
            // Keywords = keywords;
            // await LoadListData();
            //table.ReloadData();
        }
        /// <summary>
        /// 对ant表格的引用
        /// </summary>
        protected Table<TEntityDto> table;
        /// <summary>
        /// 获取指定节点的子节点的委托，ant table组件实现树时需要此委托
        /// </summary>
        protected virtual Func<TEntityDto, IEnumerable<TEntityDto>> GetTreeChildren { get; } = x => x.Children;
        /// <summary>
        /// 将ant table条件转换为TGetAllInput后加载数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [AbpExceptionInterceptor]//首次由ant触发调用，因此需要加这个
        protected virtual async Task OnQuery(QueryModel condition)
        {
            /*
             * 目前只考虑高级搜索方式，不考虑动态条件
             */



            var ls = condition.SortModel.Where(c => c.Sort.IsNotNullOrWhiteSpaceBXJG()).OrderBy(c => c.Priority).Select(c => c.FieldName + " " + c.Sort.Replace("end", ""));
            Sorting = string.Join(",", ls);
            //页码和页索引直接在table做bingd-xxx
            //但若在这里做，则子类无需再绑定了
            //  this.PageSize = condition.PageSize;
            //  this.PageIndex = condition.PageIndex;
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

        /// <summary>
        /// 清空所有条件并重新加载
        /// 若有更多条件，子类应重写此方法清空条件，并执行base.ReLoad()
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        protected virtual async Task BtnReLoadClick()
        {
            table.ResetData();
            //PageIndex = 1;
            //PageSize = 20;
            Keywords = string.Empty;
            //StateHasChanged();
            //await OnQuery(table.GetQueryModel());
            await LoadListData();
            //  await base.Reset();
            // table.ResetData();
            //table.ReloadData(); 远程加载时，根本不会执行这里

            // table.ResetData();//它仅仅是将条件复位，并不会加载数据

        }
        /// <summary>
        /// 绑定到刷新按钮的点击事件
        /// 条件分页都不变，重新加载当前数据
        /// </summary>
        [AbpExceptionInterceptor]
        protected virtual async Task BtnRefreshClick()
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
            await LoadListData();
            //table.ReloadData();
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

        #region 删除
        //没权限时不显示的，所以不加入这个判断
        /// <summary>
        /// 是否禁用批量删除按钮，出现任意情况，则为true：正在加载数据；正在删除数据；没有选择数据；
        /// </summary>
        protected virtual bool ShouldDisableDelete => IsLoading || isDeleting || SelectedItems == default || !SelectedItems.Any();
        /// <summary>
        /// 是否批量删除的确认框
        /// </summary>
        protected bool isShowDeleteConfirm = false;
        /// <summary>
        /// 是否正在执行批量删除操作
        /// </summary>
        protected bool isDeleting = false;
        /// <summary>
        /// 显示批量删除的确认框
        /// </summary>
        protected virtual void ShowDeleteConfirm()
        {
            HideDeleteConfirm();
            isShowDeleteConfirm = true;
        }
        /// <summary>
        /// 隐藏批量删除的确认框
        /// </summary>
        protected virtual void HideDeleteConfirm()
        {
            isShowDeleteConfirm = false;
            foreach (var dto in Items)
            {
                dto.ExtensionData.IsShowDeleteConfirmation = false;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        protected virtual async Task BtnDeleteClick()
        {
            await Delete();
        }
        protected virtual async Task Delete()
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            HideDeleteConfirm();
            isDeleting = true;
            try
            {
                await DeleteCore();
            }
            finally
            {
                isDeleting = false;
            }
        }
        protected virtual async Task DeleteCore()
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限

            var r = await AppService.DeleteAsync(new BatchOperationInputLong { Ids = SelectedItems.Select(x => x.Id).ToArray() });
            _ = BatchOperationMessage(r, "批量删除");//这里木有必要await
                                                 //BatchDeleteMessage(temp);
            if (r.Ids.Any())
                await LoadListData();
            //_ = InvokeAsync(dataGrid.ReloadServerData); //内部会StateChange

        }
        /// <summary>
        /// 删除单个项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        protected virtual async Task BtnDeleteItemClick(TEntityDto item)
        {
            await DeleteItem(item);
        }
        protected virtual async Task DeleteItem(TEntityDto item)
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            // var curr = dataGrid.Items.Single(c => c.Id!.Equals(input.Id));
            HideDeleteConfirm();
            item.ExtensionData.IsDeleting = true;
            try
            {
                await DeleteItemCore(item);
            }
            finally
            {
                item.ExtensionData.IsDeleting = false;
            }
        }
        protected virtual async Task DeleteItemCore(TEntityDto item)
        {

            var r = await AppService.DeleteAsync(new() { Ids = new[] { item.Id } });

            // _ = BatchOperationMessage(r);//这里木有必要await
            //BatchDeleteMessage(temp);
            if (r.Ids.Any())
            {
                _ = ShowSuccessMessage(msg: "删除成功！");
                await LoadListData();
            }
            else
            {
                _ = ShowFailMessage(title: "删除失败！", r.ErrorMessage.FirstOrDefault()?.Message);
            }

        }
        /// <summary>
        /// 显示删除明细的确认框
        /// </summary>
        /// <param name="dto"></param>
        protected virtual void ShowDeleteConfirm(TEntityDto dto)
        {
            HideDeleteConfirm();
            dto.ExtensionData.IsShowDeleteConfirmation = true;
        }
        #endregion


    }
}