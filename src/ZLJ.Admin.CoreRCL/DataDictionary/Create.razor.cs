
namespace ZLJ.Admin.CoreRCL.DataDictionary
{
    /// <summary>
    /// admin应用 数据字典 UI 新增 逻辑
    /// </summary>
    public partial class Create
    {
        ///// <summary>
        ///// 用于实现Section布局
        ///// </summary>
        //[Parameter]
        //public object Master {  get; set; }
        protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientUtils();
        /// <summary>
        /// 此功能的名称
        /// </summary>
        public override string FuncName =>"数据字典";
    }
}
