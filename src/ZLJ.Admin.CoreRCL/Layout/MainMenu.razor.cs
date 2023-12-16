using Abp.Application.Navigation;
using Abp.Runtime.Session;

namespace ZLJ.Admin.CoreRCL.Layout
{
    public partial class MainMenu
    {
        IUserNavigationManager UserNavigationManager => ScopedServices.GetRequiredService<IUserNavigationManager>();

        IAbpSession abpSession => ScopedServices.GetRequiredService<IAbpSession>();

        UserMenu menu = new UserMenu() { Items=new List<UserMenuItem>() };

        [Inject]
        public IconService iconService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (abpSession.UserId.HasValue)
            {
                menu = await UserNavigationManager.GetMenuAsync("MainMenu", new Abp.UserIdentifier(abpSession.TenantId, abpSession.UserId.Value));
                await Console.Out.WriteLineAsync(   System.Text.Json.JsonSerializer.Serialize(menu));
            }
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
