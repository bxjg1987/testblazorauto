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

       // bool isAjax => HttpContext.Request.IsAjaxRequestBXJG();

        private IComponentRenderMode? RenderModeForPage {
            get {

                if (HttpContext.Request.Path.StartsWithSegments("/Account"))
                    return default;
                if (HttpContext.Request.Path.StartsWithSegments("/404"))
                    return default;

                if (HttpContext.Request.Path.StartsWithSegments("/error"))
                    return default;

               // return new InteractiveAutoRenderMode(false);
                return new InteractiveServerRenderMode(false);
            }
        } 
    }
}
