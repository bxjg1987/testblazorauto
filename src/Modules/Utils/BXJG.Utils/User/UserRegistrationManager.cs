using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.User
{
    //需要但未实现，将来建议使用组合而不是抽象，这样具体项目的用户注册领域服务可以引用而不是继承它
    public  class UserRegistrationManager<Role, User>: BXJGBaseDomainService
        where Role : AbpRole<User>, new()
        where User : AbpUser<User>
    {
    }
}
