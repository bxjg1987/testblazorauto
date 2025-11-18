using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.User
{
    /*
     * 由于blazor项目间接依赖UtilsCore，IAbpUserManager<TRole,TUser>依赖Abp.ZeroCore，它是具体实现，依赖很多服务端的库
     * 所以IBXJGUtilsUserManager被迫定义在这里
     * 
     * 这个可能是abp9 还是 10加的，起码asp.net zero12是没有相关接口的，如： IAbpUserManager<TRole,TUser>
     * 
     * 也可以换个思路IBXJGUtilsUserManager不继承IAbpUserManager<TRole,TUser>
     */

    public interface IBXJGUtilsUserManager<TRole, TUser> //: IAbpUserManager<TRole,TUser>
        //where TRole : AbpRole<TUser>,new()
        //where TUser : AbpUser<TUser>
    {
    }
}
