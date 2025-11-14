using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using BXJG.Utils.Application.Share.Dtos;
using BXJG.Utils.Application.Share.User;
using BXJG.Utils.Role;
using BXJG.Utils.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.User
{
    /// <summary>
    /// 后台管理 用户 接口
    /// </summary>
    //[RemoteService(IsEnabled =false, IsMetadataEnabled =false)]//泛型的，本来就不暴露
    public class UserAppService<TUser,
                                TRole,
                                TUserManager,
                                TRoleManager,
                                TDto,
                                TCondition,
                                TCreateInput,
                                TEditInput> : CrudBaseAppService<TUser,
                                                                 TDto,
                                                                 long,
                                                                 PagedAndSortedResultRequest<TCondition>,
                                                                 TCreateInput,
                                                                 TEditInput>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>//, IEntity<long>
        where TUserManager : BXJGUtilsUserManager<TRole, TUser>
        where TRoleManager : BXJGUtilsRoleManager<TRole, TUser>
        where TDto : IUserDto
        where TCondition : class, new()
        where TEditInput : IUserEditDto
        where TCreateInput:IUserCreateDto
    {
        public TRoleManager RoleManager { get; set; }
        public IPasswordHasher<TUser> PasswordHasher { get; set; }
        public TUserManager UserManager { get; set; }
        public IEnumerable<IPasswordValidator<TUser>> _passwordValidators { get; set; }
        public INotificationSubscriptionManager _notificationSubscriptionManager { get; set; }
        //private  IAppNotifier _appNotifier { get; set; }
        public UserAppService(IRepository<TUser, long> repository) : base(repository)
        {
        }
        
     public virtual Func<TUser, TCreateInput,Task> SetPwdFunc { get=>field??SetPwd; set; }
        protected override async Task CreateSave( TUser user, TCreateInput input)
        {
            //return base.InsertCore(entity);

            //参考zero的实现

            //if (AbpSession.TenantId.HasValue)
            //{
            //  默认实现从租户特征获取最大用户数量进行判断
            //    await _userPolicy.CheckMaxUserCountAsync(AbpSession.GetTenantId());
            //}

            // var user = MapToEntity(input);//  ObjectMapper.Map<TUser>(input.User); //Passwords is not mapped (see mapping configuration)
            user.TenantId = AbpSession.TenantId;

            //设置随机密码，下次登录必须更改
            //Set password
            //if (input.SetRandomPassword)
            //{
            //    var randomPassword = await _userManager.CreateRandomPassword();
            //    user.Password = _passwordHasher.HashPassword(user, randomPassword);
            //    input.User.Password = randomPassword;
            //}
            //else if (!input.User.Password.IsNullOrEmpty())
            //{
            await SetPwdFunc(user, input);
            //}

            //user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

            //Assign roles
            await SetRolesFunc(user, input);
            //user.SetNormalizedNames();
            user.Surname = user.Name;
            await CreateSaveFunc(user);

            await SetOUFunc(user, input);

            await CreateNoticeFunc(user);
            //await _appNotifier.WelcomeToTheApplicationAsync(user);


            //user = await GetEntityByIdAsync(user.Id,false);
            //return MapToEntityDto(user);
            //后台管理用户，简单的话不需要发送激活邮件
            //Send activation email
            //if (input.SendActivationEmail)
            //{
            //    user.SetNewEmailConfirmationCode();
            //    await _userEmailer.SendEmailActivationLinkAsync(
            //        user,
            //        AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
            //        input.User.Password
            //    );
            //}
            // base.CreateAsync(user);
        }

        public virtual async Task CreateNotice(TUser user)
        {
            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
        }
        public virtual Func<TUser, Task> CreateNoticeFunc { get=>field??CreateNotice; set; }

        public virtual async Task SetOU(TUser user, TCreateInput input)
        {
            //Organization Units
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());
        }
        public virtual Func<TUser, TCreateInput, Task> SetOUFunc { get=>field??SetOU; set; }

        public virtual async Task CreateSave1(TUser user)
        {
            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.
        }
        public virtual Func<TUser, Task> CreateSaveFunc { get=>field??CreateSave1; set; }

        public virtual async Task SetRoles(TUser user, TCreateInput input)
        {
            user.Roles = new Collection<UserRole>();
            foreach (var roleName in input.RoleNames)
            {
                var role = await RoleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
            }
        }
        public virtual Func<TUser, TCreateInput, Task> SetRolesFunc { get=>field??SetRoles; set; }
         

        public virtual async Task SetPwd(TUser user, TCreateInput input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
            foreach (var validator in _passwordValidators)
            {
                CheckErrors(await validator.ValidateAsync(UserManager, user, input.Password));
            }

            user.Password = PasswordHasher.HashPassword(user, input.Password);
        }
      

        //public override Task<TDto> UpdateAsync(TEditInput input)
        //{
        //    return base.UpdateAsync(input);
        //}
        protected override async Task MapToEntityAsync(TEditInput input, TUser user)
        {
            await base.MapToEntityAsync(input, user);
            //Debug.Assert(input.User.Id != null, "input.User.Id should be set.");

            //var user = await UserManager.FindByIdAsync(input.User.Id.Value.ToString());

            ////Update user properties
            //ObjectMapper.Map(input.User, user); //Passwords is not mapped (see mapping configuration)

            CheckErrors(await UserManager.UpdateAsync(user));

            //if (input.SetRandomPassword)
            //{
            //    var randomPassword = await _userManager.CreateRandomPassword();
            //    user.Password = _passwordHasher.HashPassword(user, randomPassword);
            //    input.User.Password = randomPassword;
            //}
            //else if (!input.User.Password.IsNullOrEmpty())
            //{
            if (input.ChangePassword)
            {
                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.Password));
            }
            //}

            //单独定义接口去做角色和组织机构的更新

            //Update roles
            CheckErrors(await UserManager.SetRolesAsync(user, input.RoleNames.ToArray()));

            //update organization units
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            //if (input.SendActivationEmail)
            //{
            //    user.SetNewEmailConfirmationCode();
            //    await _userEmailer.SendEmailActivationLinkAsync(
            //        user,
            //        AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
            //        input.User.Password
            //    );
            //}
        }


        protected override ValueTask MapToEntity(TUser entity)
        {
            entity.Surname??= entity.Name;
            return base.MapToEntity(entity);
        }


        protected override async Task DeleteCore(TUser entity)
        {
            if (entity.Id == AbpSession.UserId)
                UserFriendlyExceptionFactory.Throw("不能删除自己！", LocalizationSource);

            CheckErrors(await UserManager.DeleteAsync(entity));
            // return base.DeleteCore(entity);
        }
        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
