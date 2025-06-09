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
using ZLJ.Application.Share.Post;
using ZLJ.Admin.CoreRCL.Share;

namespace ZLJ.Admin.CoreRCL.Post
{
    public partial class Create
    {
        //TreePermission tp;
        //string[] ps = new string[] { "Admin.TestSimple.Create" };
        protected override HttpClient HttpClient => httpClient ??= ScopedServices.GetRequiredService<IHttpClientFactory>().CreateHttpClientAdmin();

        //ZLJ.Application.Common.OU.IOuAppService ouProviderAppService;

        //protected IOuAppService OuProviderAppService => ScopedServices.GetRequiredService<IOuAppService>();

        public override string FuncName => "角色岗位";

        //protected override Task<PostDto> SaveCore()
        //{
        //    createDto.GrantedPermissions = tp.CheckedKeys;
        //    return base.SaveCore();
        //}

        //[AbpExceptionInterceptor]
        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();

        //    //var list = await OuProviderAppService.GetListAsync(new GetListInput { Code = string.Empty, IsOnlyLoadChild = false });
        //}
    }
}