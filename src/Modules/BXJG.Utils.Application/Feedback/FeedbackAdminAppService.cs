using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.UI;
using BXJG.Utils.Application.Share.Auth;
using BXJG.Utils.Application.Share.Dtos;
using BXJG.Utils.Application.Share.Feedback;
using BXJG.Utils.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Feedback
{
    /// <summary>
    /// 后台管理端 管理留言的应用服务
    /// 权限仅使用一个字符串
    /// </summary>
    public class FeedbackAdminAppService<TFeedbackManager, TRole, TUser> : CrudBaseAppService<FeedbackEntity, FeedbackDto, Guid, PagedAndSortedResultRequest<FeedbackCondition>, FeedbackEditDto>
   where TFeedbackManager : FeedbackManager<TRole, TUser>

        where TRole : AbpRole<TUser>, new() where TUser : AbpUser<TUser>
    {
        //protected override string GetPermissionName { get => base.GetPermissionName; set => base.GetPermissionName = value; }
        public FeedbackAdminAppService(IRepository<FeedbackEntity, Guid> repository) : base(repository)
        {
            GetPermissionName = Share.Auth.PermissionNames.FeedbackGetPermissionName;
            UpdatePermissionName = Share.Auth.PermissionNames.FeedbackUpdatePermissionName;
            DeletePermissionName = Share.Auth.PermissionNames.FeedbackDeletePermissionName;
            CreatePermissionName = Share.Auth.PermissionNames.FeedbackCreatePermissionName;
        }
        [Obsolete]
        public override Task<FeedbackDto> CreateAsync(FeedbackEditDto input)
        {
            throw UserFriendlyExceptionFactory.GetException("不能创建留言");
        }
    }
    public class FeedbackAdminAppService<TRole, TUser> : FeedbackAdminAppService<FeedbackManager<TRole, TUser>, TRole, TUser>

    where TRole : AbpRole<TUser>, new() where TUser : AbpUser<TUser>
    {
        public FeedbackAdminAppService(IRepository<FeedbackEntity, Guid> repository) : base(repository)
        {
        }
    }
}
