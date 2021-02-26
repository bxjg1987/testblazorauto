using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using ZLJ.Configuration;
using ZLJ.Identity;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Dependency;
using Abp.Json;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using ZLJ.Web.Host.Controllers;
using System.IO;
using BXJG.Common;
using BXJG.Shop;
using BXJG.Utils;
using BXJG.GeneralTree;
using BXJG.CMS;
using BXJG.Equipment;
using BXJG.BaseInfo;
using BXJG.WeChat.Pay;
using BXJG.WeChat.MiniProgram;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

namespace ZLJ.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // services.AddLettuceEncrypt();
            //MVC
            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
                }
            ).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            });

            //这里abp引入asp.net identity，
            IdentityRegistrar.Register(services);

            //asp.net userManager有个Option属性，abp的userManager.InitializeOptionsAsync会使用设置系统覆盖此设置 
            //参考：https://github.com/aspnetboilerplate/aspnetboilerplate/blob/c0604b9b1347a3b9581bf97b4cae22db5b6bab1b/src/Abp.ZeroCore/Authorization/Users/AbpUserManager.cs
            //参考：https://docs.microsoft.com/zh-cn/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-5.0#no-locidentity-options
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequireDigit = false;//要求密码中的数字介于0-9 之间。默认true
            //    options.Password.RequireLowercase = false;//密码中需要小写字符。默认true
            //    options.Password.RequireNonAlphanumeric = false;//密码中需要一个非字母数字字符.。默认true
            //    options.Password.RequireUppercase = false;//必须包含大写字母，默认true
            //    options.Password.RequiredLength = 1; //密码最小长度，默认6
            //    options.Password.RequiredUniqueChars = 1;  //仅适用于 ASP.NET Core 2.0 或更高版本。 需要密码中的非重复字符数。默认1
            //});

            AuthConfigurer.Configure(services, _appConfiguration);

            //微信相关服务注册使用abp模块化形式,在ZLJ.Core模块中依赖微信模块，并在PreInit中配置
            //参考：ZLJCoreModule

            services.AddSignalR();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "ZLJ API", Version = "v1" });

                //添加中文注释
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location) + @"\apixml\";
                var commentsFileName = typeof(ZLJApplicationModule).Assembly.GetName().Name + ".XML";
                var commentsFileName1 = typeof(ApplicationModule).Assembly.GetName().Name + ".XML";
                var commentsFileName2 = typeof(GeneralTreeModule).Assembly.GetName().Name + ".XML";
                var commentsFileName3 = typeof(BXJGUtilsModule).Assembly.GetName().Name + ".XML";
                var commentsFileName4 = typeof(BXJGCMSApplicationModule).Assembly.GetName().Name + ".XML";
                var commentsFileName5 = typeof(BXJGEquipmentApplicationModule).Assembly.GetName().Name + ".XML";
                var commentsFileName6 = typeof(BXJGBaseInfoApplicationModule).Assembly.GetName().Name + ".XML";
                var commentsFileName7 = typeof(BXJGUtilsApplicationModule).Assembly.GetName().Name + ".XML";

                var xmlPath = Path.Combine(basePath, commentsFileName);
                var xmlPath1 = Path.Combine(basePath, commentsFileName1);
                var xmlPath2 = Path.Combine(basePath, commentsFileName2);
                var xmlPath3 = Path.Combine(basePath, commentsFileName3);
                var xmlPath4 = Path.Combine(basePath, commentsFileName4);
                var xmlPath5 = Path.Combine(basePath, commentsFileName5);
                var xmlPath6 = Path.Combine(basePath, commentsFileName6);
                var xmlPath7 = Path.Combine(basePath, commentsFileName7);

                options.IncludeXmlComments(xmlPath);
                options.IncludeXmlComments(xmlPath1);
                options.IncludeXmlComments(xmlPath2);
                options.IncludeXmlComments(xmlPath3);
                options.IncludeXmlComments(xmlPath4);
                options.IncludeXmlComments(xmlPath5);
                options.IncludeXmlComments(xmlPath6);
                options.IncludeXmlComments(xmlPath7);

                options.DocInclusionPredicate((docName, description) => true);


                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

            // Configure Abp and Dependency Injection
            return services.AddAbp<ZLJWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                )
            );
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            #region 微信相关
            app.UseWeChatMiniProgram(); //注册微信小程序登陆中间件
            app.UseWXPay();             //注册微信支付中间件
            #endregion

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAbpRequestLocalization();
            //app.UseAbpRequestLocalization(opt =>
            //{
            //    var t = opt.RequestCultureProviders.SingleOrDefault(c => c.GetType() == typeof(AcceptLanguageHeaderRequestCultureProvider));
            //    opt.RequestCultureProviders.Remove(t);
            //});

            // app.UseWeChatPayment();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(_appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "ZLJ API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("ZLJ.Web.Host.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger
        }
    }
}
