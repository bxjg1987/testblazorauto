using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ZLJ.Application.Common.ClientProxy;
using ZLJ.RCL;
namespace ZLJ.Web.BlazorAuto.Components
{
    public partial class App
    {
        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;


        private IComponentRenderMode? RenderModeForPage => HttpContext.Request.Path.StartsWithSegments("/Account") ? null : RenderMode.InteractiveAuto;
    }
}
