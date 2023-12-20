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

        bool xs = false;

        protected override void OnInitialized()
        {
            /*
             * 每个页面请求时 都是完整的加载页面，所以这里会执行
             * 此页面是静态的，所以基于cookie做状态存储是最合适的
             * 先考虑常规流程，一旦加载过client运行时，就永远有效
             * 
             * blazor.boot.json实现程序集刷新问题
             * 此时应该切换回server模式，简单的就是清空此cookie
             * 要么去看blazor.boot.json的处理有木有预留事件，若有则在前端清空此cookie
             * 否则将核心程序集hash写入cookie，然后这里比对，此办法相对简单
             */
            if (xs == false)
            {
                if (HttpContext.Request.Cookies.TryGetValue("dqsj", out var sj))
                {
                    if ((DateTime.Now - DateTime.Parse(sj)).TotalSeconds > 60)
                    {
                        xs = true;
                        //不要清空，
                    }
                    //否则继续等
                }
                else
                {
                    HttpContext.Response.Cookies.Append("dqsj",DateTime.Now.ToLongTimeString());
                }
            }

            //Console.WriteLine(this.GetHashCode());
        }
    }
}
