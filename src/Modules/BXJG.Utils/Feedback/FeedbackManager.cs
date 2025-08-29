using Abp;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Feedback
{
    /// <summary>
    /// 留言反馈的领域服务
    /// 以后可能增加tag、图像等处理
    /// 目前没有回复功能，将来通过实体评论功能来实现
    /// </summary>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    public class FeedbackManager<TRole, TUser> : BXJGBaseDomainService where TRole : AbpRole<TUser>, new() where TUser : AbpUser<TUser>
    {
        public IRepository<FeedbackEntity, Guid> Repository { get; set; }

        public IAbpUserManager<TRole, TUser> UserManager { get; set; }

        public IAbpSession Session { get; set; }
        public IGuidGenerator GuidGenerator { get; set; }


        public async ValueTask SetPropertyAsync(FeedbackEntity entity)
        {
            TUser user = null;
            if (Session.UserId.HasValue && (entity.ConnectName.IsNullOrWhiteSpaceBXJG() || entity.ConnectInfo.IsNullOrWhiteSpaceBXJG()))
            {
                //比起直接从仓储拿，这里应该是有缓存的
                user = await UserManager.GetUserByIdAsync(Session.UserId.Value);

                if (entity.ConnectName.IsNullOrWhiteSpaceBXJG())
                    entity.ConnectName = user.FullName;
                if (entity.ConnectInfo.IsNullOrWhiteSpaceBXJG())
                    entity.ConnectInfo = "手机号：" + user.PhoneNumber;
            }
            entity.TenantId = Session.TenantId;
            //entity.Content = content;

            //entity.Title = title; //使用ai根据内容生成，但速度慢，应该用后台作业

            //entity.ConnectName = connectName;
            //entity.ConnectInfo = connectInfo;
        }
    }
}