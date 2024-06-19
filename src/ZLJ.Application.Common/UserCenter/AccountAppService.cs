using Abp.Authorization;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZLJ.Application.Common.Users;
using ZLJ.Core.Authorization;
using ZLJ.Core.Authorization.Users;

namespace ZLJ.Application.Common.UserCenter
{
    //从abp开原本代码复制过来的

    public class AccountAppService : CommonBaseAppService
    {
        //public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly IAbpSession _abpSession;
        private readonly UserManager _userManager;
        private readonly LogInManager _logInManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountAppService(IAbpSession abpSession, UserManager userManager, LogInManager logInManager, IPasswordHasher<User> passwordHasher)
        {
            _abpSession = abpSession;
            _userManager = userManager;
            _logInManager = logInManager;
            _passwordHasher = passwordHasher;
          
        }

        public async Task ChangePassword(ChangePasswordDto input)
        {
            //if (_abpSession.UserId == null)
            //{
            //    throw new UserFriendlyException("Please log in before attemping to change password.");
            //}
            await _userManager.InitializeOptionsAsync(_abpSession.TenantId);

            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);

            //若为空后续本身就会报错，这是不该出现的错误
            //if (user == default)
            //      throw new Exception("不该出现的错误");

            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                var sdfsd = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
                sdfsd.CheckErrors(base.LocalizationManager);
            }
            else
                IdentityResult.Failed(new IdentityError {  Description="旧密码错误！"}).CheckErrors(LocalizationManager);
         
            //  var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword,  shouldLockout: false);
          //  if (loginAsync.Result != AbpLoginResultType.Success)
          //  {
          //      throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
          //  }
          //  //if (!new Regex(PasswordRegex).IsMatch(input.NewPassword))
          //  //{
          //  //    throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
          //  //}
          //  user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
          //await  CurrentUnitOfWork.SaveChangesAsync();
          //  return true;
        }

    }
}
