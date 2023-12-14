using Abp.Authorization;
using ZLJ.Web.Blazor.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Admin.Post.Dto;

namespace ZLJ.Web.Admin.Pages.Post
{
    public partial class Create
    {
        //ZLJ.App.Common.OU.IOuAppService ouProviderAppService;

        protected ZLJ.App.Common.OU.IOuAppService OuProviderAppService => ScopedServices.GetRequiredService<ZLJ.App.Common.OU.IOuAppService>();

        public override string FuncName => "角色岗位";

        protected override async Task CheckPermission()
        {
            await base.PermissionChecker.AuthorizeAsync(PermissionNames.AdministratorBaseInfoPostCreate);
        }

        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var list = await OuProviderAppService.GetListAsync(new App.Common.OU.GetListInput { Code = string.Empty, IsOnlyLoadChild = false });
        }
    }
}