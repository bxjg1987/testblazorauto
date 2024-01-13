using Abp.Authorization;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;

namespace ZLJ.Core.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
