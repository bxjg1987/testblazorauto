using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ZLJ.Web.HostBlazor.Components
{
    public partial class App
    {
        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;
        private IComponentRenderMode? RenderModeForPage => HttpContext.Request.Path.StartsWithSegments("/account")
      ? null
      : RenderMode.InteractiveServer;
    }
}
