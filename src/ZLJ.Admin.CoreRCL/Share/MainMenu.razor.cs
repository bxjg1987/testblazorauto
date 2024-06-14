using Abp.Application.Navigation;
using Abp.Runtime.Session;
using AntDesign;
using BXJG.Common.Events;

namespace ZLJ.Admin.CoreRCL.Share
{
    public partial class MainMenu
    {
        IUserNavigationManager UserNavigationManager => ScopedServices.GetRequiredService<IUserNavigationManager>();

        IAbpSession abpSession => ScopedServices.GetRequiredService<IAbpSession>();

        UserMenu menu = new UserMenu() { Items = new List<UserMenuItem>() };

        //[Inject]
        //protected PersistentComponentState state { get; private set; }

        //private PersistingComponentStateSubscription subscription;
        //[SupplyParameterFromQuery]
        //public int mmc { get; set; }
        ///// <summary>
        ///// true折叠菜单，false折叠
        ///// </summary>
        //bool collapsed => mmc > 0;
        //MenuMode MenuMode => collapsed ? AntDesign.MenuMode.Inline : AntDesign.MenuMode.Vertical;
        IDisposable sj;

        [Inject]
        IZhongjieProvider sdfsdf { get; set; }
        public void Dispose()
        {
            sj?.Dispose();

            // subscription.Dispose();
        }

        //静态传入auto，不能用级联，或者可以试试全局级联
        //https://learn.microsoft.com/zh-cn/aspnet/core/blazor/components/cascading-values-and-parameters?view=aspnetcore-8.0#cascading-valuesparameters-and-render-mode-boundaries
        //  [CascadingParameter(Name = "mmc")]
        [Parameter]
        public bool Collapsed { get; set; }
        //antblazor菜单有问题，所以单独定义这个字段，在首次渲染后将其赋值为Collapsed
        // bool collapsed;

        [Inject]
        public IconService iconService { get; set; }
        //Task OnPersisting()
        //{
        //    Console.WriteLine(  "菜单持久化执行了"+System.Text.Json.JsonSerializer.Serialize(menu));
        //    state.PersistAsJson("main", menu);
        //    return Task.CompletedTask;
        //}
        [Inject]
        public IMessageService MessageService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var zhongjie = sdfsdf.GetCurrent();
            //注意预渲染
            if (zhongjie != default)
            {
                //Console.WriteLine("客户端事件：totalmainmenu注册了");
                sj = zhongjie.Zhuce(async () =>
                {
                    await MessageService.Warning("若看到此消息，说明基于事件总线的跨组件通信成功");
                }, "aaa");
            }
            //  subscription = state.RegisterOnPersisting(OnPersisting);

            //   if (!state.TryTakeFromJson("main", out menu))
            //   {
            //      await Console.Out.WriteLineAsync(   "没有从缓存中获取到菜单");
            //     menu = new UserMenu() { Items = new List<UserMenuItem>() };//这里初始化下，否则界面渲染为空估计要报错
            if (abpSession.UserId.HasValue)
            {
                _ = UserNavigationManager.GetMenuAsync("MainMenu", new Abp.UserIdentifier(abpSession.TenantId, abpSession.UserId.Value)).ContinueWith(t =>
                {
                    menu = t.Result;
                    //if (menu != null)
                    //{ 
                     
                    //    //menu.
                    //    menu.Items = menu.Items.OrderBy(c=>c.Order).ToList();
                    //}
                    InvokeAsync(StateHasChanged);
                });
            }
            //  }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await iconService.CreateFromIconfontCN("https://at.alicdn.com/t/font_2735473_hi62ezq5579.js");
                //   collapsed = Collapsed;
                StateHasChanged();
            }
        }
    }
}
