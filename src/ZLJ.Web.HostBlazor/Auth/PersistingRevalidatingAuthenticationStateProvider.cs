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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ZLJ.Admin.CoreRCL.Auth;
using ZLJ.Application.Common.Share.Models.TokenAuth;

using ZLJ.Core.Authorization.Users;
using ZLJ.Web.Core.Authentication.JwtBearer;

namespace ZLJ.Web.HostBlazor.Auth
{
    // This is a server-side AuthenticationStateProvider that revalidates the security stamp for the connected user
    // every 30 minutes an interactive circuit is connected. It also uses PersistentComponentState to flow the
    // authentication state to the client which is then fixed for the lifetime of the WebAssembly application.
    internal sealed class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider, IAccessTokenProvider
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly PersistentComponentState state;
        private readonly IdentityOptions options;

        private readonly PersistingComponentStateSubscription subscription;

        private Task<AuthenticationState>? authenticationStateTask;

        public PersistingRevalidatingAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory serviceScopeFactory,
            PersistentComponentState persistentComponentState,
            IOptions<IdentityOptions> optionsAccessor,
            TokenAuthConfiguration configuration)
            : base(loggerFactory)
        {
            scopeFactory = serviceScopeFactory;
            state = persistentComponentState;
            options = optionsAccessor.Value;

            AuthenticationStateChanged += OnAuthenticationStateChanged;
            subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
            _configuration = configuration;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            // Get the user manager from a new scope to ensure it fetches fresh data
            await using var scope = scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager>();
            return await ValidateSecurityStampAsync(userManager, authenticationState.User);
        }

        private async Task<bool> ValidateSecurityStampAsync(UserManager userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            if (user is null)
            {
                return false;
            }
            else if (!userManager.SupportsUserSecurityStamp)
            {
                return true;
            }
            else
            {
                var principalStamp = principal.FindFirstValue(options.ClaimsIdentity.SecurityStampClaimType);
                var userStamp = await userManager.GetSecurityStampAsync(user);
                return principalStamp == userStamp;
            }
        }

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
                var userId = principal.FindFirst(options.ClaimsIdentity.UserIdClaimType)?.Value;
                //var email = principal.FindFirst(options.ClaimsIdentity.EmailClaimType)?.Value;

                //var accessToken = CreateAccessToken(CreateJwtClaims(principal.Identity as ClaimsIdentity));

                if (userId != null /*&& email != null*/)
                {
                    state.PersistAsJson(nameof(UserInfo), new UserInfo
                    {
                        Id = long.Parse(userId),
                        AccessToken = GetAccessToken(),
                        EncryptedAccessToken = GetEncryptedAccessToken(),
                        //UserId = userId,
                        //Email = email,
                    });
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            subscription.Dispose();
            AuthenticationStateChanged -= OnAuthenticationStateChanged;
            base.Dispose(disposing);
        }

        #region 以下代码从ZLJ.Web.Host/controllers/TokenAuthController中抄过来的
        private readonly TokenAuthConfiguration _configuration;
        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }
        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AdminConsts.DefaultPassPhrase);
        }

        //当前对象是在server模式运行的，且是scope生命周期的
        string accessToken = string.Empty;
        string encryptedAccessToken = string.Empty;
        public string GetAccessToken()
        {
            sdfsdf();
            return accessToken;
        }

        public string GetEncryptedAccessToken()
        {
            sdfsdf();
            return encryptedAccessToken;
        }

        void sdfsdf()
        {
            if (accessToken.IsNotNullOrWhiteSpaceBXJG())
            {
                return;
            }
            lock (this)
            {
                if (accessToken.IsNotNullOrWhiteSpaceBXJG())
                {
                    return;
                }
                if (authenticationStateTask != default)
                {
                    var authenticationState = AsyncHelper.RunSync(() => authenticationStateTask);
                    var principal = authenticationState.User;
                    accessToken = CreateAccessToken(CreateJwtClaims(principal.Identity as ClaimsIdentity));
                    encryptedAccessToken = GetEncryptedAccessToken(accessToken);
                }
            }
        }
        #endregion
    }
}
