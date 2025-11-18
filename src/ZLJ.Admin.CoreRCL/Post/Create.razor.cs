using Abp.Authorization;
using ZLJ.RCL.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;
using System.Net.Http;

namespace ZLJ.Admin.CoreRCL.Post
{
    public partial class Create
    {
        protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientAdmin();

        //ZLJ.Application.Common.OU.IOuAppService ouProviderAppService;

        //protected IOuAppService OuProviderAppService => ScopedServices.GetRequiredService<IOuAppService>();

        public override string FuncName => "岗位角色";

      

        //[AbpExceptionInterceptor]
        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();

        //    //var list = await OuProviderAppService.GetListAsync(new GetListInput { Code = string.Empty, IsOnlyLoadChild = false });
        //}
    }
}