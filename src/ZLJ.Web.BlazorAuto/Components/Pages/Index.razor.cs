
namespace ZLJ.Web.BlazorAuto.Components.Pages
{
    
    public partial class Index
    {
        public bool IsAuthenticated { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
           
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var state = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
            //if (state?.User?.Identity!=null  &&state.User.Identity.IsAuthenticated) {
            //    NavigationManager.NavigateTo("/main",true);
            //}
            IsAuthenticated = state?.User?.Identity != null && state.User.Identity.IsAuthenticated;

        }
    }
}
