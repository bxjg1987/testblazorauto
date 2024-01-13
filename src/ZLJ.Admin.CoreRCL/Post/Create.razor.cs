using Abp.Authorization;
using ZLJ.RCL.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.OU;

namespace ZLJ.Admin.CoreRCL.Post
{
    public partial class Create
    {
        //ZLJ.Application.Common.OU.IOuAppService ouProviderAppService;

        //protected IOuAppService OuProviderAppService => ScopedServices.GetRequiredService<IOuAppService>();

        public override string FuncName => "角色岗位";

      

        //[AbpExceptionInterceptor]
        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();

        //    //var list = await OuProviderAppService.GetListAsync(new GetListInput { Code = string.Empty, IsOnlyLoadChild = false });
        //}
    }
}