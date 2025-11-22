namespace ZLJ.Admin.CoreRCL.Tenant
{
    public partial class Create
    {
        //[Parameter]
        //public object Master { get; set; } = new object();

        //protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientAdmin();

        //ZLJ.Application.Common.OU.IOuAppService ouProviderAppService;

        //protected IOuAppService OuProviderAppService => ScopedServices.GetRequiredService<IOuAppService>();

        public override string FuncName => "租户";



        //[AbpExceptionInterceptor]
        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();

        //    //var list = await OuProviderAppService.GetListAsync(new GetListInput { Code = string.Empty, IsOnlyLoadChild = false });
        //}
    }
}
