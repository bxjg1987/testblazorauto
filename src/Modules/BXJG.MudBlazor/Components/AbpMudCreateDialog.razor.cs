using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.MudBlazor.Components
{
    /// <summary>
    /// 基于mudblazor弹窗，专用于承载新增组件
    /// </summary>
    public partial class AbpMudCreateDialog
    {
        /// <summary>
        /// 需要传递给具体的新增组件的参数
        /// </summary>
        [Parameter]
        public IDictionary<string, object> Parameters { get; set; }
        /// <summary>
        /// 弹窗实例
        /// </summary>
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }
        /// <summary>
        /// 新增组件类型
        /// </summary>
        [Parameter]
        public Type? CreateComponentType { get; set; }
        /// <summary>
        /// 新增组件
        /// </summary>
        DynamicComponent? createComponent;
        /// <summary>
        /// 是否正在保存
        /// </summary>
        bool Saving = false;
        /// <summary>
        /// 点击保存按钮时执行
        /// </summary>
        /// <returns></returns>
        async Task SaveClick()
        {
            //dialogParameters = new DialogParameters<AbpMudBaseComponent>();
            Saving = true;
            //安全执行不要放新增组件内部，这样可以简化新增组件的开发
            //不要考虑新增组件可能在某处单独使用，那种情况也在外面包一层承载组件
            await base.SafelyExecuteAsync(async () =>
            {
                var frm = createComponent!.Instance as IForm;
                var r = await frm!.Save();
                this.MudDialog.Close(r);
                //if (SaveAndContinue)
                //    await frm!.Reset();
            });
            Saving = false;
        }
    }
}

//public interface isdf
//{ 
//   public RenderFragment ChildContent { get; }

//    public RenderFragment Buttons { get; }
//}
//}