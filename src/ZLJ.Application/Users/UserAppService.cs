using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using ZLJ.Application.Authorization.Accounts;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.Authorization.Users;
using ZLJ.Application.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZLJ.Application.Common.Users;
using ZLJ.Application.Authorization.Permissions;
using ZLJ.Application.Share.Roles;
using ZLJ.Application.Share.Authorization.Permissions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ZLJ.Application.Users
{
    public class UserAppService : AdminCrudBaseAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, EditUserDto>//, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;

        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;

            CreatePermissionName = PermissionNames.AdministratorSystemUserAdd;
            UpdatePermissionName = PermissionNames.AdministratorSystemUserUpdate;
            DeletePermissionName = PermissionNames.AdministratorSystemUserDelete;
            GetAllPermissionName = PermissionNames.AdministratorSystemUser;
            GetPermissionName = PermissionNames.AdministratorSystemUser;
        }

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);
            user.Surname = user.Name;
            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;
            if (user.EmailAddress.IsNullOrWhiteSpace())
                user.EmailAddress = input.PhoneNumber + "@a.com";
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(user);
        }

        public override async Task<UserDto> UpdateAsync(EditUserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            if (!input.Password.IsNullOrWhiteSpace())
            {
                input.Password = _passwordHasher.HashPassword(user, input.Password);
            }
            else
                input.Password = user.Password;

            if (input.EmailAddress.IsNullOrWhiteSpace())
                input.EmailAddress = input.PhoneNumber + "@a.com";

            MapToEntity(input, user);
            user.Surname = user.Name;

            CheckErrors(await _userManager.UpdateAsync(user));


            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }
            return MapToEntityDto(user);
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        //public async Task<IEnumerable<long>> DeleteBatchAsync(IList<long> input)
        //{
        //    CheckDeletePermission();

        //    if (AbpSession.UserId.HasValue && input.Contains(AbpSession.UserId.Value))
        //        input.Remove(AbpSession.UserId.Value);

        //    input.Remove(1);

        //    var list = new List<long>();
        //    foreach (var item in input)
        //    {
        //        var user = await _userManager.GetUserByIdAsync(item);
        //        var r = await _userManager.DeleteAsync(user);
        //        if (r.Succeeded)
        //            list.Add(item);
        //    }
        //    return list;
        //}
        protected override async Task DeleteCore(User entity)
        {
            if (AbpSession.UserId.HasValue && AbpSession.UserId.Value == entity.Id)
                throw new UserFriendlyException("不允许删除自己");
            await _userManager.DeleteAsync(entity);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }
        public override Task<PagedResultDto<UserDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            CurrentUnitOfWork.Options.IsTransactional = false;
            return base.GetAllAsync(input);
        }
        public override Task<UserDto> GetAsync(EntityDto<long> input)
        {
            CurrentUnitOfWork.Options.IsTransactional = false;
            return base.GetAsync(input);
        }
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }


        protected override void MapToEntity(EditUserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override async Task<IQueryable<User>> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return (await Repository.GetAllIncludingAsync(x => x.Roles)).AsNoTrackingWithIdentityResolution()
                .WhereIf(input.RoleId.HasValue, c => c.Roles.Any(d => d.RoleId == input.RoleId.Value))
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id, CancellationTokenProvider.Token);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to reset password.");
            }
            long currentUserId = _abpSession.UserId.Value;
            var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }
    }
}

