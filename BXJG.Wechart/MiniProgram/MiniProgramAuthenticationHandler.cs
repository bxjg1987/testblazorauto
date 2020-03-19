using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace BXJG.Wechart.MiniProgram
{
    //可以参考TwitterHandler和OAuthHandler的设计

    public class MiniProgramAuthenticationHandler : RemoteAuthenticationHandler<MiniProgramAuthenticationOptions>
    {
        private HttpClient Backchannel => Options.Backchannel;
        public MiniProgramAuthenticationHandler(IOptionsMonitor<MiniProgramAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            var parameters = new Dictionary<string, string>
            {
                { "appid", Options.AppId },
                { "secret", Options.Secret },
                { "js_code",Request.Query["code"] },
                { "grant_type", "authorization_code" },
            };
            var requestUrl = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, parameters);

            var response = await Backchannel.GetAsync(requestUrl, Context.RequestAborted);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"An error occurred when retrieving wechar mini program user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Microsoft Account API is enabled.");

            var rt = await JsonSerializer.DeserializeAsync<dynamic>(await response.Content.ReadAsStreamAsync());
            //var rt = new
            //{
            //    openid = "testopenid",
            //    session_key = "testsession_key",
            //    unionid = "test unionid",
            //    errcode = 0,
            //    errmsg = ""
            //};

            if (rt.errcode != 0)
                throw new HttpRequestException($"errcode:{rt.errcode}, errmsg:{rt.errmsg}");


            var identity = new ClaimsIdentity(ClaimsIssuer);

            identity.AddClaim(new Claim("openid", rt.openid));
            identity.AddClaim(new Claim("session_key", rt.session_key));
            identity.AddClaim(new Claim("unionid", rt.unionid));

            var authenticationProperties = new AuthenticationProperties
            {
                ExpiresUtc = base.Clock.UtcNow.AddMinutes(5),
                IsPersistent = true,
                IssuedUtc = Clock.UtcNow
            };

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), authenticationProperties, Scheme.Name);

            return HandleRequestResult.Success(ticket);
        }

        //protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        //{
        //    var result = await Context.AuthenticateAsync(SignInScheme);
        //    return await base.HandleAuthenticateAsync();
        //}
    }
}
