using Abp.Authorization;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;

namespace ZLJ.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
