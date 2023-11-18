using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Abp.Runtime.Security;
//using BXJG.WeChat.MiniProgram;
using Microsoft.AspNetCore.Authentication;
using ZLJ.App.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using ZLJ.App.Common;
using Azure.Core;
using Microsoft.Net.Http.Headers;
//using ZLJ.Authentication.WeChatMiniProgram;

namespace ZLJ.Web.Host.Startup
{
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var authBuilder = services.AddAuthentication(options =>
            {
                //  options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
               
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";

                //options.DefaultScheme = IdentityConstants.ApplicationScheme;
                //options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                //options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
                // options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                //options.SchemeMap["Identity.Application"]
                // var sdfsd =   options.Schemes.SingleOrDefault(c => c.HandlerType == typeof(CookieAuthenticationHandler));
                //var sdf =    options.SchemeMap;

                // sdfsd.eve
                //  options.DefaultChallengeScheme= "Identity.Application";
                //  options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                //options.DefaultSignInScheme = "Identity.Application";
            });
         
          //  Microsoft.AspNetCore.Authentication.SuppressAutoDefaultScheme
            // services.AddCookie("Identity.Application"); //提示已经注册了
            // CookieAuthenticationHandler



            //authBuilder.AddCookie();
            if (bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                authBuilder.AddJwtBearer("JwtBearer", options =>
                {
                    options.Audience = configuration["Authentication:JwtBearer:Audience"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // The signing key must match!
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                        // Validate the JWT Issuer (iss) claim
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                        // Validate the JWT Audience (aud) claim
                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                        // Validate the token expiry
                        ValidateLifetime = true,

                        // If you want to allow a certain amount of clock drift, set that here
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = QueryStringTokenResolver
                    };
                });
            }

            //if (bool.Parse(configuration["Authentication:WeChartMiniProgram:IsEnabled"]))
            //{
            //    authBuilder.AddWeChartMiniProgram<WeChatMiniProgramLoginHandler>(opt =>
            //    {
            //        opt.AppId = configuration["Authentication:WeChartMiniProgram:AppId"];
            //        opt.Secret = configuration["Authentication:WeChartMiniProgram:Secret"];
            //    });
            //}

            //使用自定义的提供器来根据app决定默认身份验证方案
            services.AddSingleton<IAuthenticationSchemeProvider, CustAuthenticationSchemeProvider>();
        }

        /* This method is needed to authorize SignalR javascript client.
         * SignalR can not send authorization header. So, we are getting it from query string as an encrypted text. */
        private static Task QueryStringTokenResolver(MessageReceivedContext context)
        {
            if (!context.HttpContext.Request.Path.HasValue ||
                !context.HttpContext.Request.Path.Value.StartsWith("/signalr"))
            {
                // We are just looking for signalr clients
                return Task.CompletedTask;
            }

            var qsAuthToken = context.HttpContext.Request.Query["enc_auth_token"].FirstOrDefault();
            if (qsAuthToken == null)
            {
                // Cookie value does not matches to querystring value
                return Task.CompletedTask;
            }

            //abp模板项目代码是这样的，看样子是传encryptedAccessToken，但解密会失败，也许前端要先base64编码，这里先base64解码，然后再解密
            //目前直接改为传递未加密的token
            //前端代码类似这样
            /*
             *         abp.signalr = abp.signalr || {};
             *         //abp.signalr.qs = 'enc_auth_token=' + abp.getCurrentEncryptedJWTToken();
             *         abp.signalr.qs = 'enc_auth_token=' + abp.getCurrentJWTToken();
             */

            // Set auth token from cookie
            context.Token = SimpleStringCipher.Instance.Decrypt(qsAuthToken, AdminConsts.DefaultPassPhrase);
            //context.Token = qsAuthToken;
            return Task.CompletedTask;
        }


    }
}
