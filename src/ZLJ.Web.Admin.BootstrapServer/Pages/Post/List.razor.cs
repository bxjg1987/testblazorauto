using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Admin.Post.Dto;

namespace ZLJ.Web.Admin.BootstrapServer.Pages.Post
{
    public partial class List
    {
        protected override string FuncName => "角色岗位";

        protected override async Task OnInitializedAsync()
        {
            await base.InitPermission(PermissionNames.AdministratorBaseInfoPostCreate, PermissionNames.AdministratorBaseInfoPostUpdate, PermissionNames.AdministratorBaseInfoPostDelete);
        }
      
        //这里也可以用肉夹馍的全局注册处理
       
        private async Task AddRandomData()
        {
           
            for (int i = 0; i < 10; i++)
            {
                await base.AppService.CreateAsync(new CreatePostDto
                {
                    Description = "演示数据" + Random.Shared.Next(),
                    DisplayName = "测试名称" + Random.Shared.Next(),
                    Name = "test" + Random.Shared.Next(),
                    // GrantedPermissions = new List<string> { }
                });
            }
        }

      
    }
}
