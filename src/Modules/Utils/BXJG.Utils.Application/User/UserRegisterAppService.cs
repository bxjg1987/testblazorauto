using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using BXJG.Utils.Application.Share.User;
using BXJG.Utils.Role;
using BXJG.Utils.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.User
{
    /// <summary>
    /// 抽象的用户注册应用服务
    /// </summary>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TUserManager"></typeparam>
    /// <typeparam name="TRoleManager"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class UserRegisterAppService<TRole,
                                        TUser,
                                        TUserManager,
                                        TRoleManager,
                                        TInput,
                                        TOutput> : BXJGUtilsBaseAppService//, IUserRegisterAppService<TInput, TOutput>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
        where TUserManager : BXJGUtilsUserManager<TRole, TUser>
        where TRoleManager : BXJGUtilsRoleManager<TRole, TUser>
    {
        public TRoleManager RoleManager { get; set; }
        public IPasswordHasher<TUser> PasswordHasher { get; set; }
        public TUserManager UserManager { get; set; }
        //public IScopedIocResolver ScopedIocResolver { get; set; }

        public virtual Func<TInput, ValueTask> Check => input =>
        {
           // this.sco
            return ValueTask.CompletedTask;
        };
        public virtual Func<TInput, ValueTask<TUser>> Map1 => (input) =>
        {
            //var obj = scopedIocResolver.Resolve<Abp.ObjectMapping.IObjectMapper>();
            //var user = obj.Map<TUser>(input);
            var user = ObjectMapper.Map<TUser>(input);
            //ScopedIocResolver.Resolve
            return ValueTask.FromResult(user);
        };
        public virtual async Task<TOutput> RegisterAsync(TInput input,
                                                         Func<TInput, ValueTask> check = default,
                                                         Func<TInput, ValueTask<TUser>> map = default)
        {
            //前置检查
            check ??= this.Check;
            await check(input);

            map ??= this.Map1;
            var user = await map( input);



            var r = await UserManager.CreateAsync(user);
            r.CheckErrors(LocalizationManager);

            await CurrentUnitOfWork.SaveChangesAsync();

            await UnitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                user = await UserManager.GetUserByIdAsync(user.Id);
            });
            return await Map(user);
        }

        protected virtual ValueTask<TUser> Map(TInput input)
        {
            return ValueTask.FromResult(ObjectMapper.Map<TUser>(input));
        }
        protected virtual ValueTask<TOutput> Map(TUser input)
        {
            return ValueTask.FromResult(ObjectMapper.Map<TOutput>(input));
        }
    }
}
