using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using BXJG.Utils.Application.Share.User;
using BXJG.Utils.User;
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
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class UserRegisterAppService<TRole, TUser,TUserManager, TInput, TOutput> : BXJGUtilsBaseAppService, IUserRegisterAppService<TInput, TOutput>
   where TRole : AbpRole<TUser>, new() where TUser : AbpUser<TUser>
        where TUserManager: UserManager<TRole, TUser>
    {
        protected TUserManager UserManager { get; set; }
        public virtual async Task<TOutput> RegisterAsync(TInput input)
        {
            //前置检查

            var user = await Map(input);

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
