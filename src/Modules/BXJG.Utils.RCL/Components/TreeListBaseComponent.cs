using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.UI;
using BXJG.Common.Contracts;
using BXJG.Utils;
using BXJG.Utils.Application.Share.Dtos;
using BXJG.Utils.Application.Share.GeneralTree;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NUglify.Html;
using System;
using System.Collections;
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
    public abstract class TreeListBaseComponent<TEntityDto,  TGetAllInput> : BaseComponent
        //where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntityDto : IGeneralTree<TEntityDto>, IExtendableObj//, new()
        //where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TGetAllInput : new()
    {
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
                //    errMsg += item.Message + Environment.NewLine;

                //if (output.Ids.Count == 0)
                //    await ShowFailMessage(msg: $"{funName}失败！" + errMsg);
                //else
                //    await ShowFailMessage(msg: $"{funName}部分失败！成功数量：{output.Ids.Count}；{errMsg}");
            }
            else
                await  ShowSuccessMessage(msg: $"{funName}成功！");
        }

        /// <summary>
        /// 是否正在加载、删除...
        /// </summary>
        protected virtual bool IsBusy => IsLoading || isDeleting;

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
        protected virtual void LoadListData()
        {
            if (IsLoading)
                return;

            IsLoading = true;
            //Items.Clear();

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
            await Task.Yield();//某些ui，如antblazor的下拉框 选择事件触发时，绑定的属性还没更新，所以这里让出下线程
            var dtos = await HttpClient.GetList<TEntityDto>(GetAllInput);

            //_ = InvokeAsync(StateHasChanged);//让多选影响顶部按钮得以执行 包一层是因为需要加载完才执行
            //dataGrid.SelectedItems.Clear();//翻页时，已选择的好像木有清空，这里手动来下

            //给每行属性附加额外状态

            await Map(dtos);
            Items = dtos;
            TotalCount = dtos.Count;


        }

        async ValueTask Map(IList<TEntityDto> dtos)
        {
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
        protected virtual void BtnSearchClick()
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
            LoadListData();
            // Keywords = keywords;
            // await LoadListData();
            //table.ReloadData();
        }
        /// <summary>
        /// 获取指定节点的子节点的委托，ant table组件实现树时需要此委托
        /// </summary>
        protected virtual Func<TEntityDto, IEnumerable<TEntityDto>> GetTreeChildren { get; } = x => x.Children;


        /// <summary>
        /// 清空所有条件并重新加载
        /// 若有更多条件，子类应重写此方法清空条件，并执行base.ReLoad()
        /// </summary>
        /// <returns></returns>



        protected virtual void BtnClearFilterClick()
        {
            // table.ResetData();
            //PageIndex = 1;
            // PageSize = 20;
            if (GetAllInput is IReset reset)
                reset.Reset();
      else
                GetAllInput = new TGetAllInput();
            Keywords = string.Empty;
            //StateHasChanged();
            //await OnQuery(table.GetQueryModel());
            LoadListData();
            //  await base.Reset();
            // table.ResetData();
            //table.ReloadData(); 远程加载时，根本不会执行这里

            // table.ResetData();//它仅仅是将条件复位，并不会加载数据

        }
        /// <summary>
        /// 绑定到刷新按钮的点击事件
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
        protected virtual bool ShouldDisableDelete => IsBusy || SelectedItems == default || !SelectedItems.Any();
        /// <summary>
        /// 是否批量删除的确认框
        /// </summary>
        protected bool isShowDeleteConfirm = false;
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
                _ = InvokeAsync(async () => {
                    _= BatchOperationMessage(r).ConfigureAwait(false);
                    if (r.Ids.Any())
                        LoadListData();
                }).ConfigureAwait(false);

              
            }
            finally
            {
                isDeleting = false;
            }
        }
        protected virtual Task<BatchOperationOutput<long>> DeleteCore()
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限

            return HttpClient.DeleteBatch<long,TEntityDto>(new BatchOperationInputLong { Ids = SelectedItems.Select(x => x.Id).ToArray() });


        }
        /// <summary>
        /// 删除单个项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual async Task BtnDeleteItemClick(TEntityDto item)
        {
            await DeleteItem(item);
        }
        protected virtual async Task DeleteItem(TEntityDto item)
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            // var curr = dataGrid.Items.Single(c => c.Id!.Equals(input.Id));
            if (item.ExtensionData.IsDeleting) return;
            item.ExtensionData.IsDeleting = true;

            try
            {
                await DeleteItemCore(item);
                item.ExtensionData.IsDeleting = false;

                //后续逻辑都是辅助性的，因此放到异步中，加快主操作速度
                _ = InvokeAsync(async () => {
                    //Core靠全局异常去提示
                await    ShowSuccessMessage("删除提示", "删除成功！");

                    LoadListData();
                }).ConfigureAwait(false);


               




            }
            finally
            {
                item.ExtensionData.IsDeleting = false;
            }
        }
        protected virtual async Task DeleteItemCore(TEntityDto item)
        {
            await HttpClient.Delete<TEntityDto>(new{   item.Id });
        }
        #endregion


    }
}