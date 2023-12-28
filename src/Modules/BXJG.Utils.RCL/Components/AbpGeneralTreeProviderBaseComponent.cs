using BXJG.Common.Dto;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Components
{
    /// <summary>
    /// 通用树下拉框抽象组件
    /// </summary>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    /// <typeparam name="TGetNodesForSelectInput"></typeparam>
    /// <typeparam name="TGetNodesForSelectOutput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public abstract class AbpGeneralTreeProviderBaseComponent<TGetTreeForSelectInput,
                                                              TGetTreeForSelectOutput,
                                                              TGetNodesForSelectInput,
                                                              TGetNodesForSelectOutput,
                                                              TAppService> : AbpBaseComponent
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput, new()
        //where TGetTreeForSelectOutput : GeneralTreeNodeDto<TGetTreeForSelectOutput>//, new()
        where TGetNodesForSelectInput : GeneralTreeGetForSelectInput, new()
        //where TGetNodesForSelectOutput : GeneralTreeComboboxDto//, new()
        where TAppService : IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput, TGetTreeForSelectOutput, TGetNodesForSelectInput, TGetNodesForSelectOutput>
    {
        /// <summary>
        /// 应用服务缓存，不要使用它
        /// </summary>
        private TAppService appService;
        /// <summary>
        /// 应用服务
        /// </summary>
        protected TAppService AppService => appService ?? ScopedServices.GetRequiredService<TAppService>();

        /// <summary>
        /// true多选；false单选（默认）
        /// </summary>
        [Parameter]
        public bool IsMultiple { get; set; } = false;

        /// <summary>
        /// 已选择的项的Id集合
        /// </summary>
        [Parameter]
        public IEnumerable<long> SelectedIds { get; set; }
        /// <summary>
        /// 已选择的项的Id集合
        /// </summary>
        [Parameter]
        public EventCallback<IEnumerable<long>> SelectedIdsChanged { get; set; }

        /// <summary>
        /// 已选择的项的Id
        /// </summary>
        [Parameter]
        public long SelectedId { get; set; }
        /// <summary>
        /// 已选择的项的Id
        /// </summary>
        [Parameter]
        public EventCallback<long> SelectedIdChanged { get; set; }

        /// <summary>
        /// 若设置，则只加载它的子集，与ParentId二选一
        /// </summary>
        [Parameter]
        public string? ParentCode { get; set; }
        /// <summary>
        /// 若设置，则只加载它的子集，与ParentCode二选一
        /// </summary>
        [Parameter]
        public long? ParentId { get; set; }
        /// <summary>
        /// 选择框的应用场景
        /// </summary>
        [Parameter]
        public GetForSelectType ForType { get; set; } = 0;
        /// <summary>
        /// 树集合
        /// </summary>
        protected IList<TGetTreeForSelectOutput> treeItems;
        /// <summary>
        /// 扁平化集合
        /// </summary>
        protected IList<TGetNodesForSelectOutput> items;

        protected string keywords = string.Empty;

        protected virtual async Task LoadData()
        {
            if (ForType < GetForSelectType.Item)
            {
                var input = new TGetTreeForSelectInput() { Code = ParentCode, ParentId = ParentId, IsOnlyLoadChild = ForType == GetForSelectType.TreeItems };
                treeItems = await AppService.GetTreeForSelectAsync(input);
            }
            else
            {
                var input = new TGetNodesForSelectInput { Code = ParentCode, IsOnlyLoadChild = true, ParentId = ParentId };
                if (input is IHaveKeywords inpu2)
                    inpu2.Keywords = keywords;
                items = await AppService.GetNodesForSelectAsync(new TGetNodesForSelectInput { Code = ParentCode, IsOnlyLoadChild = true, ParentId = ParentId });
            }
        }

        //根据父节点查询子节点列表就不封装了，子类直接调用appService吧
    }

    /// <summary>
    /// 下拉框类型
    /// </summary>
    public enum GetForSelectType
    {
        /// <summary>
        /// 仅加载当前展开节点的子节点，通常用于条件
        /// </summary>
        TreeItems = 1 << 0,
        /// <summary>
        /// 加载指定节点下所有后代节点，通常用于表单绑定
        /// </summary>
        TreeAll = 1 << 1,
        /// <summary>
        /// 加载指定节点下子节点，用于扁平化选择
        /// </summary>
        Item = 1 << 2,
    }
}