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
        int pageIndex=1;
        protected override int PageIndex => pageIndex;

        int pageSize=20;
        protected override int PageSize => pageSize;

        private async Task<QueryData<PostDto>> Load(QueryPageOptions condition)
        {
            pageSize = condition.PageItems;
            pageIndex = condition.PageIndex;
           
            await Refresh();
            return new QueryData<PostDto>
            {
                IsAdvanceSearch = false,
                IsFiltered = true,
                IsSearch = true,
                IsSorted = true,
                Items = Items,
                TotalCount = base.TotalCount
            };
        }

        protected override Task Refresh()
        {
            return base.Refresh();
        }

        private async Task AddRandomData()
        {
            for (int i = 0; i < 10; i++)
            {
                await base.AppService.CreateAsync(new CreatePostDto
                {
                    Description = "演示数据" + Random.Shared.Next(),
                    DisplayName = "测试名称" + Random.Shared.Next(), Name= "test"+Random.Shared.Next(), 
                   // GrantedPermissions = new List<string> { }
                });
            }
        }
    }
}
