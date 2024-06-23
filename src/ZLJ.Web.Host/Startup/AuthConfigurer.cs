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
using ZLJ.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using ZLJ.Application.Common;
using Azure.Core;
using Microsoft.Net.Http.Headers;
using ZLJ.Core;
using log4net.Core;
//using ZLJ.Core.Authentication.WeChatMiniProgram;

namespace ZLJ.Web.Host.Startup
{
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            if (bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                }).AddJwtBearer("JwtBearer", options =>
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
        }

        /* This method is needed to authorize SignalR javascript client.
         * SignalR can not send authorization header. So, we are getting it from query string as an encrypted text. */
        private static Task QueryStringTokenResolver(MessageReceivedContext context)
        {

            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(AuthConfigurer));
            var tmpPath = context.HttpContext.Request.Path;

            if (!tmpPath.HasValue)
            {
                // We are just looking for signalr clients
                return Task.CompletedTask;
            }

            var qsAuthToken = context.HttpContext.Request.Query["access_token"].FirstOrDefault();
            //blazor前端以server模式运行时，token在头部
            if (qsAuthToken.IsNullOrWhiteSpaceBXJG() && context.HttpContext.Request.Headers.TryGetValue("Authorization", out var sdfsdf))
                qsAuthToken = sdfsdf.ToString().Replace("Bearer","").TrimStart();
       
          // context.HttpContext.Request.Headers.Authorization.FirstOrDefault()

            if (qsAuthToken == null)
            {
                // Cookie value does not matches to querystring value
                return Task.CompletedTask;
            }



            if (tmpPath.Value.StartsWith("/signalr"))
            {
                context.Token = SimpleStringCipher.Instance.Decrypt(qsAuthToken);        // Set auth token from cookie
                logger.LogDebug($"signalr连接的assesstoken为：{context.Token}");
            }
            else if (tmpPath.Value.Contains("/bxjgfile/"))
                context.Token = SimpleStringCipher.Instance.Decrypt(qsAuthToken, ZLJConsts.DefaultPassPhrase);

            return Task.CompletedTask;
        }
    }
}
