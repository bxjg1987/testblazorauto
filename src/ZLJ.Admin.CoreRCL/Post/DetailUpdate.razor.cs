using System.Net.Http;

namespace ZLJ.Admin.CoreRCL.Post
{
    public partial class DetailUpdate
    {
        protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientAdmin();

        protected override string FuncName => "岗位角色";
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;
    }
}
