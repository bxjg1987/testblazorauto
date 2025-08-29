using BXJG.Utils.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;

namespace ZLJ.Application.Common.Feedback
{
    /// <summary>
    /// 前台用户需要调用的留言反馈接口
    /// </summary>
    public class FeedbackFrontAppService : BXJG.Utils.Application.Feedback.FeedbackFrontAppService< ZLJ.Core.Authorization.Roles.Role, User>
    {
        public FeedbackFrontAppService(IRepository<FeedbackEntity, Guid> repository) : base(repository)
        {
        }
    }
}
