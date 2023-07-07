using Abp.AspNetCore.Mvc.Controllers;
using Abp.Authorization.Users;
using Abp.Authorization;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ZLJ.App.Admin.Authorization;
using ZLJ.Authorization.Users;
using ZLJ.Authorization;
using ZLJ.Models.TokenAuth;
using ZLJ.MultiTenancy;
using ZLJ.Identity;

namespace ZLJ.Web.Customer.Controllers
{
    /*
     * 由于使用了主项目统一的登陆，这个controller暂时不用了
     * 
     * 能进到这里，内部代码可以硬编码使用appKey
     * 所以请求识别appKey没啥意义
     * 但有时候公共代码处理，可能需要appKey
     * 但我们希望用二级域名来标识appKey，让各app的url变短
     * 所以需要根据配置和别的机制识别到底是用子路径还是二级域名识别appKey
     * 若是前者，需要动态注册路由增加子路径
     * 可以考虑使用queryString的方式，貌似切换模式时都能用
     * 
     * 这里从路由传递appKey参数的目的是 主项目有中间件会来识别它
     * 之后的请求可能有些共享代码要用到appKey
     * 当前app中的用户代码其实不需要它，因为在自己的app代码中肯定晓得自己是哪个app
     * 
     * blazor项目中用到mvc情况不多，若没特殊情况，尽量将action定义在这个cotroller中，方便在CustAppController顶部做一些全局的操作。
     */


    [Microsoft.AspNetCore.Mvc.Route("cust/account")]
    //[AppKey( "custApp" )]  //这个跟area一样，必须要求路由中包含这个，最终又导致我们的子应用路径变长了
    public class CustAccountController : AbpController //继承它，abp会把它注入到容器，就不需要asp.net application part了，否则也许需要使用application part
    {
        private readonly ITenantCache _tenantCache;
        private readonly LogInManager _logInManager;
        private readonly SignInManager signInManager1;
        // SignInManager<User> signInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        //IAuthenticationSchemeProvider authenticationSchemeProvider;
        //IAuthenticationHandlerProvider authenticationHandlerProvider;
        public CustAccountController(LogInManager logInManager, AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            ITenantCache tenantCache, SignInManager signInManager1)
        //,
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
        [HttpGet]
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
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] AuthenticateModel model, [FromQuery]string returnUrl ="/")
        {
            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );
            //await base.HttpContext.SignInAsync("Identity.Application",
            //                                   new System.Security.Claims.ClaimsPrincipal(loginResult.Identity), new AuthenticationProperties
            //                                   {
            //                                       IsPersistent = model.RememberClient
            //                                   });
            await signInManager1.SignInAsync(loginResult.Identity, model.RememberClient);
            return Redirect(returnUrl);
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
