namespace ZLJ.Admin.CoreRCL.Administrative
{
    public partial class Create
    {
        // public override string FuncName => "数据字典";
        public override string FuncName => "省市区";

        protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientAdmin();
    }
}