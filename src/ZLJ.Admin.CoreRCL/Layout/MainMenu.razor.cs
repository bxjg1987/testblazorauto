using Abp.Application.Navigation;
using Abp.Runtime.Session;
using AntDesign;
using BXJG.Common;

namespace ZLJ.Admin.CoreRCL.Layout
{
    public partial class MainMenu
    {
        IUserNavigationManager UserNavigationManager => ScopedServices.GetRequiredService<IUserNavigationManager>();

        IAbpSession abpSession => ScopedServices.GetRequiredService<IAbpSession>();

        UserMenu menu;//= new UserMenu() { Items = new List<UserMenuItem>() };

        [Inject]
        protected PersistentComponentState state { get; private set; }

        private PersistingComponentStateSubscription subscription;
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

            subscription.Dispose();
        }

        [Inject]
        public IconService iconService { get; set; }
        Task OnPersisting()
        {
            state.PersistAsJson("main", menu);
            return Task.CompletedTask;
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var zhongjie = sdfsdf.GetCurrent();
            //注意预渲染
            if (zhongjie != default)
            {
                //Console.WriteLine("客户端事件：totalmainmenu注册了");
                //sj = zhongjie.Zhuce<bool>(async (col) =>
                //{
                //    Console.WriteLine("客户但事件totalmainmenu执行了" + col);
                //    collapsed = col;
                //    await this.InvokeAsync(StateHasChanged);
                //}, "totalmainmenu");
            }
            subscription = state.RegisterOnPersisting(OnPersisting);

            if (!state.TryTakeFromJson("main", out menu))
            {
                initMenu();//下面有异步，这里初始化下
                if (abpSession.UserId.HasValue)
                {
                    menu = await UserNavigationManager.GetMenuAsync("MainMenu", new Abp.UserIdentifier(abpSession.TenantId, abpSession.UserId.Value));
                    // menu.Items.Add();
                    // await Console.Out.WriteLineAsync(   System.Text.Json.JsonSerializer.Serialize(menu));
                }
            }
        }

        void initMenu()
        {
            menu = new UserMenu() { Items = new List<UserMenuItem>() };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await iconService.CreateFromIconfontCN("//at.alicdn.com/t/font_2735473_hi62ezq5579.js");
            }
        }
    }
}
