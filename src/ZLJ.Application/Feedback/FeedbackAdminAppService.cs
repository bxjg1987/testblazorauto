using BXJG.Utils.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Authorization.Users;

namespace ZLJ.Application.Feedback
{
    /// <summary>
    /// 后它管理留言反馈
    /// </summary>
    public class FeedbackAdminAppService : BXJG.Utils.Application.Feedback.FeedbackAdminAppService< ZLJ.Core.Authorization.Roles.Role, User>
    {
        public FeedbackAdminAppService(IRepository<FeedbackEntity, Guid> repository) : base(repository)
        {
        }
    }
}
