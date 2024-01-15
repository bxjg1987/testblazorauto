



using AntDesign.TableModels;
using ZLJ.Application.Common.Share.OU;
using ZLJ.Application.Share.Post;

namespace ZLJ.Admin.CoreRCL.Post
{
    public partial class List
    {
        bool sj;

        // string currOu;

        protected override string FuncName => "角色岗位";
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            if ((await base.AppService.GetAllAsync(new PagedAndSortedResultRequest<PagedPostResultRequestDto> { SkipCount = 0, MaxResultCount = 1, Sorting = "role.Id", Filter = new PagedPostResultRequestDto() })).TotalCount >= 500)
            {
                sj = true;
            }
            await base.OnInitializedAsync();
            await base.InitPermission(PermissionNames.AdministratorBaseInfoPostCreate, PermissionNames.AdministratorBaseInfoPostUpdate, PermissionNames.AdministratorBaseInfoPostDelete);

        }

        [AbpExceptionInterceptor]
        protected async Task AddRandomData()
        {


            for (int i = 0; i < 500; i++)
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
        protected override async Task ReLoad()
        {
            GetAllInput.Filter.IsStatic = default;
            GetAllInput.Filter.Permission = default;
            GetAllInput.Filter.OuCode = default;
            if(ou!=default)
            ou.Value= default;
            await base.ReLoad();
        }
        TsOu ou;
        //protected override Task OnQuery(QueryModel condition)
        //{
        //    if (currOu.IsNotNullOrWhiteSpaceBXJG())
        //        base.GetAllInput.Filter.OuCode = currOu.Split(',')[1];
        //    return base.OnQuery(condition);
        //}

        async Task OnOuChanged(string ou)
        {
            //OnSelectedItemChanged 会触发两次
            //ValueChanged 选择时只会触发一次，但清空时又会触发两次

            // Console.WriteLine(   System.Text.Json.JsonSerializer.Serialize(ou));
            GetAllInput.Filter.OuCode = ou.IsNullOrWhiteSpaceBXJG() ? string.Empty : ou.Split(',')[1];
            await base.Search();
        }
        // AbpCreateDialog<IPostAppService, PostDto, int, PagedAndSortedResultRequest<PagedPostResultRequestDto>, CreatePostDto, PostEditDto, Create> dalRef;

        /// <summary>
        /// 是否需要刷新列表页面
        /// </summary>
        bool isChanged;  
        /// <summary>
        /// 关闭新增弹窗的核心逻辑
        /// </summary>
        /// <returns></returns>
        async Task CloseDialogCore()
        {
            isCreateDialogVisible = false;
            if (isChanged)
                await Search();
        }
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
            isChanged = true;
            if (sr.End)
            {
                await CloseDialogCore();
            }
        }

        #endregion

        #region 详情和修改
        /// <summary>
        /// 是否显示详情和修改弹窗
        /// </summary>
        bool isShowDetailUpdate;
        /// <summary>
        /// 当前详情弹窗是否是修改模式
        /// false查看模式 true修改模式
        /// </summary>
        bool isEdit;
        /// <summary>
        /// 
        /// </summary>
        string detailUpdateText => isEdit ? "修改" : "查看";

        string detailUpdateIcon => isEdit ? IconType.Outline.Edit : IconType.Outline.File;
       /// <summary>
       /// 当前详情或修改的实体的id
       /// </summary>
        int detailUpdateId = 0;

        /// <summary>
        /// 新增后回调
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        async Task OnDetailUpdate(PostDto sr)
        {
            isChanged = true;
            
                await CloseDialogCore();
           
        }

        void OnEdit(PostDto sr) {
            isEdit = true;
            detailUpdateId = sr.Id;
            isShowDetailUpdate = true;
        }

        void OnDetail(PostDto sr) {
            isEdit = false;
            detailUpdateId = sr.Id;
            isShowDetailUpdate = true;
        }
        #endregion
    }
}