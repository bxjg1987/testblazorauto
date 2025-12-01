using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.User
{
    /*
     * 由于blazor项目间接依赖UtilsCore，IAbpUserManager<TRole,TUser>依赖Abp.ZeroCore，它是具体实现，依赖很多服务端的库
     * 所以IBXJGUtilsUserManager被迫定义在这里
     * 
     * 这个可能是abp9 还是 10加的，起码asp.net zero12是没有相关接口的，如： IAbpUserManager<TRole,TUser>
     * 
     * 也可以换个思路IBXJGUtilsUserManager不继承IAbpUserManager<TRole,TUser>
     * 在使用时注入IAbpUserManager<TRole,TUser> 内部手动as成IBXJGUtilsUserManager<TRole,TUser>
     * 这是一种折中的办法
     * 
     * https://github.com/aspnetboilerplate/aspnetboilerplate/issues/7092
     * 参考abp源码，开源版虽然从10.2开始增加了IAbpUserManager<TRole,TUser>
     * 但并没有被切实使用
     * 所以暂时也不处理，
     * 参考源码：ServicesCollectionDependencyRegistrar
     */

    //public interface IBXJGUtilsUserManager<TRole, TUser> //: IAbpUserManager<TRole,TUser>
    //                                                     where TRole : AbpRoleBase, new()
    //                                                     where TUser : AbpUserBase
    //{
    //}
}
