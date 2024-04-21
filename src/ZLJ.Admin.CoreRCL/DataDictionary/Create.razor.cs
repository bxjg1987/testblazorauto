
namespace ZLJ.Admin.CoreRCL.DataDictionary
{
    public partial class Create
    {
        protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientUtils();

        public override string FuncName =>"数据字典";


    }
}
