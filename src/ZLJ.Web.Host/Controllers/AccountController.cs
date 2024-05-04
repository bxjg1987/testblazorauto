using Abp.Authorization.Users;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZLJ.Application.Authorization;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.Authorization;
using ZLJ.Controllers;

using ZLJ.Core.MultiTenancy;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using ZLJ.Core.Identity;
using DocumentFormat.OpenXml.InkML;
using Abp.Extensions;
using Abp.Domain.Uow;
using ZLJ.Web.Host.Startup;
using Abp.UI;
using ZLJ.Application.Common.Share.Models.TokenAuth;

namespace ZLJ.Web.Host.Controllers
{
    /*
     * 我们的系统有多个端，后台管理、客户服务、员工后台等
     * 不同端可能使用的技术不同，比如后台管理使用基于vue的前后端分离，而其它端使用blazor，将来可能使用razorpages
     * 微软的identity框架已提供SignInManager，abp提供了继承于它的登陆器，而主项目又提供了ZLJ.SignInManager，它已经非常简化了登陆
     * abp还提供了LoginManger，通常我们是先调用LoginManager拿到用户、租户角色等信息，然后调用SignInManager拿到identity
     * SignInManager登陆时会找默认身份验证方案，比如找到cookie方案，则SignIn会写入加密cookie。我们已经重写了获取获取默认身份验证方案的逻辑，会根据app类型确定默认使用哪种身份验证方案
     * 
     * 这个登陆器只是拿到登陆后的数据，至于怎么处理，要看我们用的什么web框架，比如webapi应该根据登陆结果生成token，而mvc blazor razorpage通常响应cookie后跳转到首页
     * SignInManager是可扩展的，典型的是根据需要在UserClaimsPrincipalFactory为不同的端响应不同的cliaim
     * 
     * 不同用户可能登陆逻辑不同，有多种处理办法
     * 1、不同类型的用户定义对应的SignInManager
     * 2、所有用户使用SignInManager登陆，但上层入口各自定义，比如后台管理使用abp提供的基于token的登陆，其它的各自建立自己的登陆器控制器
     * 3、顶层登陆器controller 下层的ZLJ.SignInManager用同一个
     * 目前使用方式3，mvc blazor 使用 AccountController； webApi使用abp自带的；不同用户类型的不同处理放各自应用中，通过启动配置注入到主登陆器上
     * 
     * 通常用户相关的有Account处理登陆、注销、找回密码、邮件手机账号激活等；还有Profile操作当前登陆用户的信息
     * 各种用户Profile的操作有相似，但区别还是挺大的，可以考虑用抽象的方式
     * 
     * 本地化
     * ZLJControllerBase默认设置主项目的本地化
     * 若各应用需要访问本地化，则需要通用的主应用的则直接调用base.L
     * 若需要访问各应用自己的本地化，则应通过base.LocaxxxManager来获取各自自己的本地化
     */

    /// <summary>
    /// mvc blazor类型的端的统一登陆器
    /// </summary>
    public class AccountController : ZLJControllerBase
    {
        private readonly ITenantCache _tenantCache;
        private readonly LogInManager _logInManager;
        SignInManager signInManager1;
        // SignInManager<User> signInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        //IAuthenticationSchemeProvider authenticationSchemeProvider;
        //IAuthenticationHandlerProvider authenticationHandlerProvider;
        public AccountController(LogInManager logInManager, AbpLoginResultTypeHelper abpLoginResultTypeHelper, ITenantCache tenantCache, SignInManager signInManager1)
        //IAuthenticationSchemeProvider authenticationSchemeProvider, 
        //IAuthenticationHandlerProvider authenticationHandlerProvider
        {
            _logInManager = logInManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _tenantCache = tenantCache;
            this.signInManager1 = signInManager1;
            //this.authenticationSchemeProvider = authenticationSchemeProvider;
            //this.authenticationHandlerProvider = authenticationHandlerProvider;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            // var defaultAuthenticate = await authenticationSchemeProvider.GetDefaultAuthenticateSchemeAsync();
                return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [UnitOfWork]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] AuthenticateModel model, [FromQuery] string returnUrl = "/")
        {
            try
            {
                //var zh = 
                var loginResult = await GetLoginResultAsync(
                    model.UserNameOrEmailAddress,
                    model.Password,
                    model.TenancyName //先粗暴点就这么写吧，后期参考zero登陆租户原理调整
                                      //GetTenancyNameOrNull()
                );
                //await base.HttpContext.SignInAsync("Identity.Application",
                //                                   new System.Security.Claims.ClaimsPrincipal(loginResult.Identity), new AuthenticationProperties
                //                                   {
                //                                       IsPersistent = model.RememberClient
                //                                   });

                await signInManager1.SignInAsync(loginResult.Identity, model.RememberClient);
                return Redirect(returnUrl);
            }
            catch (UserFriendlyException ufe)
            {
                ViewBag.ErrorMsg = $"{ufe.Message}，{ufe.Details}";
                    return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = "登陆失败！服务器内部发生错误，请联系系统管理员。";
                throw;
            }
        }

        public async Task<ActionResult> Logout()
        {
            await signInManager1.SignOutAsync();
            return Redirect("/admin");
        }


        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

    }
}
