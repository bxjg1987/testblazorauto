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

        /// <summary>
        /// true wasm已运行 falsewasm未运行
        /// </summary>
        bool xs = false;
        /// <summary>
        /// 当前用户首次访问此应用最新版本的时间存储在cookie种，此字段表示此cookie的前缀，后面跟核心程序集的md5值
        /// </summary>
        const string prefix = "dqsj_";
        /// <summary>
        /// 通过querstring参数和cookie存储菜单的展开折叠模式，此字段表示这个参数和cookie的名称
        /// </summary>
        const string mainMenuCollapsedCookieName = "mmc";

        ///// <summary>
        ///// 0折叠 1展开
        ///// </summary>
        //[SupplyParameterFromQuery]
        //public int? mmc { get; set; }
        bool mainMenuCollapsed;

        //public override async Task SetParametersAsync(ParameterView parameters)
        //{
        //    await base.SetParametersAsync(parameters);

        //}

        //protected override void OnParametersSet()
        //{
        //    base.OnParametersSet();
        //    if(mmc)
        //}
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
                var assemblyName = typeof(ZLJ.Admin.CoreRCL.Share.Routes).Assembly.GetName().Name + ".dll";
                var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var hash = Path.Combine(dir, assemblyName).GetMD5ByFilePath();
                // Console.WriteLine(Environment.CurrentDirectory + "===" + hash);

                var cookieKey = prefix + hash;
                if (HttpContext.Request.Cookies.TryGetValue(cookieKey, out var sj))
                {
                    //在网络好时，15秒左右下载完成，网络不好时设置久点，大不了首次请求时服务器多支撑一会
                    if ((DateTime.Now - DateTime.Parse(sj)).TotalSeconds > 20)
                    {
                        xs = true;
                        //别删cookie
                    }
                    //否则继续等
                }
                else
                {
                    foreach (var item in HttpContext.Request.Cookies.Keys)
                    {
                        if (item.StartsWith(prefix))
                            HttpContext.Response.Cookies.Delete(item);
                    }
                    HttpContext.Response.Cookies.Append(cookieKey, DateTime.Now.ToLongTimeString(),new CookieOptions {  Expires= DateTimeOffset.Now.AddYears(100) });
                }
            }

            string mmc = string.Empty;
            if (HttpContext.Request.Query.TryGetValue(mainMenuCollapsedCookieName, out var mmc1))
            {
                mmc = mmc1;
                //mainMenuCollapsed = mmc == "0";
                HttpContext.Response.Cookies.Append(mainMenuCollapsedCookieName, mmc.ToString());
            }
            else if (HttpContext.Request.Cookies.TryGetValue(mainMenuCollapsedCookieName, out var str))
            {
                mmc = str;
                //mainMenuCollapsed = str == "0";
            }
            mainMenuCollapsed = mmc == "0";
            //Console.WriteLine(this.GetHashCode());
        }
    }
}
