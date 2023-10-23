using BootstrapBlazor.Components;
using BXJG.AbpBootstrapBlazor;
using BXJG.AbpBootstrapBlazor.Interceptors;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using Rougamo;
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
            throw new UserFriendlyException("用户友好异常，不会记录日志，仅仅提示用户。");
            await base.InitPermission(PermissionNames.AdministratorBaseInfoPostCreate, PermissionNames.AdministratorBaseInfoPostUpdate, PermissionNames.AdministratorBaseInfoPostDelete);
        }
        //protected override Task LoadListData()
        //{
        //    throw new Exception("ttttttt");
        //    return base.LoadListData();
        //}

        //protected override void OnAfterRender(bool firstRender)
        //{  
        //    throw new Exception("ttttttt");
        //    base.OnAfterRender(firstRender);
        //}
        //[AbpBBException]
        //protected override Task OnAfterRenderAsync(bool firstRender)
        //{
        //    throw new Exception("ttttttt");
        //    return base.OnAfterRenderAsync(firstRender);
        //}


        //这里也可以用肉夹馍的全局注册处理
       // [AbpBBException]
        protected async Task AddRandomData()
        {
            sfsdf();
            //  throw new Exception("xxxxxx");
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

      //  [IgnoreMo]
        private void sfsdf()
        {
            throw new Exception("系统异常，会记录日志后提示用户");
        }
    }
}
