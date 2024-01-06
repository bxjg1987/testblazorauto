



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
            GetAllInput.Sorting = $"role.{GetAllInput.Sorting.Replace("role.", "")}";//目前值考虑单列排序
            return base.LoadListData();
        }

        //[AbpExceptionInterceptor]
        protected override async Task Reset()
        {
            GetAllInput.Filter.IsStatic = default;
            GetAllInput.Filter.OuCode = default;
            GetAllInput.Filter.Permission = default;
            await base.Reset();
        }

        // AbpCreateDialog<IPostAppService, PostDto, int, PagedAndSortedResultRequest<PagedPostResultRequestDto>, CreatePostDto, PostEditDto, Create> dalRef;

        #region 新增

        /*
         * 结合blazor8的section时，新增弹窗太简单，不用单独封装弹窗组件，也便于传参到新增表单中
         * 也不要放抽象组件中，因为抽象组件是多个应用共享的，它们可能不用弹窗
         */

        /// <summary>
        /// 新增弹窗是否显示
        /// </summary>
        bool isCreateDialogVisible;
        // Guid addToolId = Guid.NewGuid(),addBodyId=Guid.NewGuid();
        //[AbpExceptionInterceptor]
        /// <summary>
        /// 显示弹窗
        /// </summary>
        public void ShowCreateDialog()
        {
            isCreateDialogVisible = true;
            //  var r = await dalRef.Show();
            //   if (r)
            //       await Reset();
        }
        /// <summary>
        /// 弹窗是否新增成功过
        /// </summary>
        bool isAdded;
        /// <summary>
        /// 点击关闭新增弹窗时执行
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        private async Task CloseDialog()
        {
            await CloseDialogCore();
        }
        /// <summary>
        /// 新增后回调
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        async Task OnAddEnd(SaveResult<PostDto> sr)
        {
            isAdded = true;
            if (sr.End)
            {
                await CloseDialogCore();
            }
        }
        /// <summary>
        /// 关闭新增弹窗的核心逻辑
        /// </summary>
        /// <returns></returns>
        async Task CloseDialogCore()
        {
            isCreateDialogVisible = false;
            if (isAdded)
                await Refresh();
        }
        #endregion
    }
}