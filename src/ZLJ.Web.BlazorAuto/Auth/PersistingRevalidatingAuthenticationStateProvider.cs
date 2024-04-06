using Abp.Runtime.Security;
using BXJG.Common.Http;
using BXJG.Common.RCL.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;
using ZLJ.Admin.CoreRCL.Auth;
using ZLJ.Application.Common.Share.Models.TokenAuth;


namespace ZLJ.Web.BlazorAuto.Auth
{
    // This is a server-side AuthenticationStateProvider that revalidates the security stamp for the connected user
    // every 30 minutes an interactive circuit is connected. It also uses PersistentComponentState to flow the
    // authentication state to the client which is then fixed for the lifetime of the WebAssembly application.
    internal sealed class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider, IAccessTokenProvider
    {
        //  private readonly IServiceScopeFactory scopeFactory;
        private readonly PersistentComponentState state;
        //  private readonly IdentityOptions options;

        private readonly PersistingComponentStateSubscription subscription;
        private readonly IHttpContextAccessor httpContextAccessor;
        private Task<AuthenticationState>? authenticationStateTask;

        public PersistingRevalidatingAuthenticationStateProvider(ILoggerFactory loggerFactory,
                                                                 //IServiceScopeFactory serviceScopeFactory,
                                                                 PersistentComponentState persistentComponentState,
                                                                 IHttpContextAccessor httpContextAccessor) : base(loggerFactory)
        {
            // scopeFactory = serviceScopeFactory;
            state = persistentComponentState;
            //   options = optionsAccessor.Value;

            AuthenticationStateChanged += OnAuthenticationStateChanged;
            subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
            this.httpContextAccessor = httpContextAccessor;
        }

        //30分验证下用户是否过期
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        //验证用户的userStamp是否变化了，若变化了说明需要重新登录
        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {

            return true;
            // Get the user manager from a new scope to ensure it fetches fresh data
            //await using var scope = scopeFactory.CreateAsyncScope();
            //var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
            //return await ValidateSecurityStampAsync(userManager, authenticationState.User);
        }

        //private async Task<bool> ValidateSecurityStampAsync(UserManager userManager, ClaimsPrincipal principal)
        //{
        //    var user = await userManager.GetUserAsync(principal);
        //    if (user is null)
        //    {
        //        return false;
        //    }
        //    else if (!userManager.SupportsUserSecurityStamp)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        var principalStamp = principal.FindFirstValue(options.ClaimsIdentity.SecurityStampClaimType);
        //        var userStamp = await userManager.GetSecurityStampAsync(user);
        //        return principalStamp == userStamp;
        //    }
        //}

        private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            authenticationStateTask = task;
        }

        private async Task OnPersistingAsync()
        {
            if (authenticationStateTask is null)
            {
                throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
            }

            var authenticationState = await authenticationStateTask;
            var principal = authenticationState.User;

            if (principal.Identity?.IsAuthenticated == true)
            {
                state.PersistAsJson(nameof(UserInfo), new UserInfo
                {
                    Id = long.Parse(principal.FindFirstValue(nameof(AuthenticateResultModel.UserId)  ) ),
                    AccessToken = GetAccessToken(),
                    EncryptedAccessToken = GetEncryptedAccessToken(),
                    //UserId = userId,
                    //Email = email,
                });

                //var userId = principal.FindFirst(options.ClaimsIdentity.UserIdClaimType)?.Value;
                ////var email = principal.FindFirst(options.ClaimsIdentity.EmailClaimType)?.Value;

                ////var accessToken = CreateAccessToken(CreateJwtClaims(principal.Identity as ClaimsIdentity));

                //if (userId != null /*&& email != null*/)
                //{
                //    state.PersistAsJson(nameof(UserInfo), new UserInfo
                //    {
                //        Id = long.Parse(userId),
                //        AccessToken = GetAccessToken(),
                //        EncryptedAccessToken = GetEncryptedAccessToken(),
                //        //UserId = userId,
                //        //Email = email,
                //    });
                //}
            }
        }

        protected override void Dispose(bool disposing)
        {
            subscription.Dispose();
            AuthenticationStateChanged -= OnAuthenticationStateChanged;
            base.Dispose(disposing);
        }

       

        public string GetAccessToken()
        {
           // var r =  AsyncHelper.RunSync(()=> GetAuthenticationStateAsync());
     
         
            //if (authenticationStateTask!=default &&authenticationStateTask.IsCompleted)
                return httpContextAccessor.HttpContext.User.FindFirstValue(nameof(AuthenticateResultModel.AccessToken));

          //  return default;
            //    sdfsdf();
            //return accessToken;
        }

        public string GetEncryptedAccessToken()
        {
            return httpContextAccessor.HttpContext.User.FindFirstValue(nameof(AuthenticateResultModel.EncryptedAccessToken));
        }
    }
}
