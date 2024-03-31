using BXJG.Common.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace BXJG.Common.RCL.Auth
{
    // This is a client-side AuthenticationStateProvider that determines the user's authentication state by
    // looking for data persisted in the page when it was rendered on the server. This authentication state will
    // be fixed for the lifetime of the WebAssembly application. So, if the user needs to log in or out, a full
    // page reload is required.
    //
    // This only provides a user name and email for display purposes. It does not actually include any tokens
    // that authenticate to the server when making subsequent requests. That works separately using a
    // cookie that will be included on HttpClient requests to the server.
    public class PersistentAuthenticationStateProvider : AuthenticationStateProvider, IAccessTokenProvider
    {
        // private readonly AccessTokenProvider _tokenProvider;

        private static readonly Task<AuthenticationState> defaultUnauthenticatedTask =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        private readonly Task<AuthenticationState> authenticationStateTask = defaultUnauthenticatedTask;

        public PersistentAuthenticationStateProvider(PersistentComponentState state)
        {
            if (!state.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
            {
                //Console.WriteLine($"PersistentAuthenticationStateProvider构造函数跳过了");
                return;
            }

            //Console.WriteLine($"PersistentAuthenticationStateProvider构造函数执行了：{JsonSerializer.Serialize(userInfo)}");

            //好像不太有必要存储到claims中
            accessToken = userInfo.AccessToken;
            encryptedAccessToken = userInfo.EncryptedAccessToken;


            Claim[] claims = [new Claim("AccessToken", userInfo.AccessToken),
              //  new Claim("TenantId", userInfo.TenantId.HasValue? userInfo.TenantId.Value.ToString():""),
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString())];

            authenticationStateTask = Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                    authenticationType: nameof(PersistentAuthenticationStateProvider)))));
        }
        string accessToken = string.Empty;
        string encryptedAccessToken = string.Empty;
        public string GetAccessToken()
        {
            return accessToken;
        }
        public string GetEncryptedAccessToken() {
            return encryptedAccessToken;
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync() => authenticationStateTask;
    }
}
