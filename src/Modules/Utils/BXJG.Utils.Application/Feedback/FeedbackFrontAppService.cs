

using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using BXJG.Utils.Application.Share.Dtos;
using BXJG.Utils.Application.Share.Feedback;
using BXJG.Utils.Feedback;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Feedback
{
    /// <summary>
    /// 前台用户使用的留言反馈应用服务
    /// 已登录和匿名用户都可新增
    /// 已登录用户只能修改、删除自己的留言，匿名用户无法修改、删除
    /// 所有用户都可以查看所有留言
    /// </summary>
    public class FeedbackFrontAppService<TFeedbackManager, TRole, TUser> : CrudBaseAppService<FeedbackEntity, FeedbackDto, Guid, PagedAndSortedResultRequest<FeedbackCondition>, FeedbackEditDto>
 where TFeedbackManager : FeedbackManager<TRole, TUser>
        where TRole : AbpRole<TUser>, new() where TUser : AbpUser<TUser>
    {
        public TFeedbackManager FeedbackManager { get; set; }
        public FeedbackFrontAppService(IRepository<FeedbackEntity, Guid> repository) : base(repository)
        {
        }

        protected override async ValueTask MapToEntity(FeedbackEntity entity)
        {
            await FeedbackManager.SetPropertyAsync(entity);
        }
        protected override void MapToEntity(FeedbackEditDto updateInput, FeedbackEntity entity)
        {
            Check(entity);
            base.MapToEntity(updateInput, entity);
        }
        protected override Task DeleteCore(FeedbackEntity entity)
        {
            Check(entity);
            return base.DeleteCore(entity);
        }

        void Check(FeedbackEntity entity)
        {
            if (!AbpSession.UserId.HasValue)
                UserFriendlyExceptionFactory.Throw("匿名用户无法修改、删除留言");
            else if (AbpSession.UserId.Value != entity.CreatorUserId)
                UserFriendlyExceptionFactory.Throw("不能修改别人的留言");
        }
    }


    public class FeedbackFrontAppService< TRole, TUser> : FeedbackFrontAppService<FeedbackManager<TRole,TUser>, TRole, TUser>
    where TRole : AbpRole<TUser>, new() where TUser : AbpUser<TUser>
    {
        public FeedbackFrontAppService(IRepository<FeedbackEntity, Guid> repository) : base(repository)
        {
        }

    }
}
