



using ZLJ.Application.Share.Post;

namespace ZLJ.Admin.CoreRCL.Post
{
    public partial class List
    {
        protected override string FuncName => "角色岗位";
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await base.InitPermission(PermissionNames.AdministratorBaseInfoPostCreate, PermissionNames.AdministratorBaseInfoPostUpdate, PermissionNames.AdministratorBaseInfoPostDelete);
        }

        [AbpExceptionInterceptor]
        protected async Task AddRandomData()
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
        protected override Task LoadListData()
        {
            GetAllInput.Sorting = $"role.{GetAllInput.Sorting}";//目前值考虑单列排序
            return base.LoadListData();
        }

        //[AbpExceptionInterceptor]
        protected override async Task Reset()
        {
            GetAllInput.Filter.IsStatic = default;
            await base.Reset();
        }

        //AbpCreateDialog<IPostAppService, PostDto, int, PagedAndSortedResultRequest<PagedPostResultRequestDto>, CreatePostDto, PostEditDto, Create> dalRef;


        bool isCreateDialogVisible;
        public async Task ShowCreateDialog()
        {
            //var r = await dalRef.Show();
            //if (r)
            //    await Reset();
        }

    }
}
