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
    /// 基于mudblazor的新增组件承载组件
    /// </summary>
    public partial class AbpMudDialogCreateCoponent
    {
        [CascadingParameter] 
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public RenderFragment Buttons { get; set; }

        [Parameter]
        public RenderFragment Children { get; set; }
    }
}
