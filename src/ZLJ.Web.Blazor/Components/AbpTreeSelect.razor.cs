using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Blazor.Components
{
    public partial class AbpTreeSelect< TGetTreeForSelectInput,
                                                        TGetTreeForSelectOutput,
                                                         TGetNodesForSelectInput,
                                                        TGetNodesForSelectOutput>
         where TGetTreeForSelectOutput : GeneralTreeNodeDto<TGetTreeForSelectOutput>
        where TGetNodesForSelectOutput : ComboboxItemDto
    {
        /// <summary>
        /// true下拉树
        /// false从树中取某个节点的子集作为下拉框数据
        /// </summary>
        [Parameter]
        public bool IsTree { get; set; }

        IEnumerable<TGetTreeForSelectOutput> treeData;

        [Parameter]
        public string Value { get; set; }
        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        [Parameter]
        public IEnumerable<string> Values { get; set; }
        [Parameter]
        public EventCallback<IEnumerable<string>> ValuesChanged { get; set; }


        IEnumerable<TGetNodesForSelectOutput> dataSource; [Parameter]
        public string NodeValue { get; set; }
        [Parameter]
        public EventCallback<string> NodeValueChanged { get; set; }

        [Parameter]
        public IEnumerable<string> NodeValues { get; set; }
        [Parameter]
        public EventCallback<IEnumerable<string>> NodeValuesChanged { get; set; }

        protected virtual async Task OnSelectedItemChangedHandler(string sss) {
         await   NodeValueChanged.InvokeAsync(sss);
        }


        [Parameter]
        public string Placeholder { get; set; } = "请选择";
        [Parameter]
        public bool Multiple {  get; set; }
    }
}
