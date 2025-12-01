using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Auth
{
    public partial class PermissionNames
    {
        #region 留言
        public const string FeedbackAdmin = Utils.Share.BXJGUtilsConsts.Feedback + "Admin";
    //留言的crud权限常量字符串
    public const string FeedbackCreatePermissionName = FeedbackAdmin + ".Create";
        public const string FeedbackUpdatePermissionName = FeedbackAdmin + ".Update";
        public const string FeedbackDeletePermissionName = FeedbackAdmin + ".Delete";
        public const string FeedbackGetPermissionName = FeedbackAdmin + ".Get";
        #endregion
    }
}
