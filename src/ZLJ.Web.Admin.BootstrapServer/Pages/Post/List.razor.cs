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
        Table<PostDto> table;



        protected override async Task OnInitializedAsync()
        {
            await base.InitPermission(PermissionNames.AdministratorBaseInfoPostCreate, PermissionNames.AdministratorBaseInfoPostUpdate, PermissionNames.AdministratorBaseInfoPostDelete);
        }
        private async Task<bool> DeleteOwn(IEnumerable<PostDto> items)
        {
            await base.Delete();
            return true;
        }
        private async Task<QueryData<PostDto>> Load(QueryPageOptions condition)
        {
            PageSize = condition.PageItems;
            PageIndex = condition.PageIndex;
            if (condition.SortList != null && condition.SortList.Count > 0)
                Sorting = string.Join(",", condition.SortList);
            else if (condition.SortOrder != SortOrder.Unset)
                Sorting = condition.SortName + " " + condition.SortOrder.ToString();
            else
                Sorting = default;

            Keywords = condition.SearchText;

            await LoadListData();

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

        //这里也可以用肉夹馍的全局注册处理
        protected override async Task Refresh()
        {
            await table.QueryAsync();
        }

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

        #region 下面的可以用肉夹馍的全局注册处理




        [Inject]
        public MessageService MessageService { get; set; }
        protected override async ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            await MessageService.Show(new MessageOption()
            {
                Content = msg,
                Color = Color.Danger,
                ShowShadow = true,
                ShowBorder = true
            });
        }

        protected override async ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            await MessageService.Show(new MessageOption()
            {
                Content = msg,
                Color = Color.Success,
                ShowShadow = true,
                ShowBorder = true
            });
        }

        #endregion
    }
}
