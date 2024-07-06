using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.Castle.Logging.Log4Net;
using ZLJ.Core.Configuration;
using ZLJ.Core.Identity;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Json;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.SqlServer;
using Abp.Hangfire;
//using BXJG.WorkOrder.EmployeeApplication;
//using ZLJ.App.Employee;
using ZLJ.Application;
using ZLJ.EntityFrameworkCore;
//using Orleans.Configuration;
//using Orleans.Hosting;

using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;
//using Savorboard.CAP.InMemoryMessageQueue;
using ZLJ.Application.Authorization.Permissions;
using Medallion.Threading;
//using AntDesign.ProLayout;
//using ZLJ.Web.Host.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using static OpenXmlPowerTools.RevisionProcessor;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using ZLJ.Application.Share.Authorization.Permissions;
using BXJG.Utils.Application;
using ZLJ.Web.Core.Configuration;

namespace ZLJ.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";
        private const string _apiVersion = "v1";

        private readonly IConfiguration _appConfiguration;
        private readonly IWebHostEnvironment webHostEnvironment;
        public Startup(IWebHostEnvironment env, IConfiguration _appConfiguration)
        {
            this.webHostEnvironment = env;
        //    _appConfiguration = env.GetAppConfiguration();
        this._appConfiguration = _appConfiguration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string defaultConnectionString = _appConfiguration.GetConnectionString(ZLJ.Core.ZLJConsts.ConnectionStringName)!;
            //services.AddLettuceEncrypt();
            //MVC
            var mvcBuilder = services.AddControllersWithViews(
                 options =>
                 {
                     options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());

                 }
            )
            //https://learn.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json/configure-options?pivots=dotnet-8-0#web-defaults-for-jsonserializeroptions
            //.AddJsonOptions(options =>
            //{
                //options.JsonSerializerOptions.PropertyNamingPolicy =  JsonNamingPolicy.CamelCase;
                //options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString ;
                //options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            //})
            ;
            //    .AddNewtonsoftJson(options =>
            //{
            //    //options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //    options.SerializerSettings.ContractResolver = new AbpMvcContractResolver()
            //    {
            //        NamingStrategy = new CamelCaseNamingStrategy()
            //    };
            //})

            //services.AddControllers();


            //经过测试不加ApplicationParts，直接引用的项目中的视图和控制器也可以用，注意是控制器必须有自己的路由，通常是特征路由
            //加不加ApplicationParts，主项目中的路由好像都无法路由到rcl中的控制器
            //https://www.codeproject.com/Articles/5296270/Asp-Net-Core-3-x-dynamically-loadable-plugins-with
            //mvcBuilder = mvcBuilder.ConfigureApplicationPartManager((PartManager) =>
            //{
            //    Assembly Assembly = typeof(ZLJ.Web.Customer.App).Assembly;
            //    ApplicationPart ApplicationPart = new AssemblyPart(Assembly);

            //    PartManager.ApplicationParts.Add(ApplicationPart);
            //});
            //  services.AddRazorPages(); //目前只是为了承载blazor才加这个，若使用mvc承载，则不需要它，注意：AddControllersWithViews并不会注册RazorPages
            //services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/ZLJ.Blazor.Admin/Pages");
            //services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/Pages");//这是默认值


            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            //services.AddWeChatPayment<WeChatPaymentNoticeHandler>(opt =>
            //{
            //    opt.mch_id = "商户id";
            //    opt.key = "商户平台秘钥";
            //});
            //services.AddTransient<WeatherForecastService>();
            //services.AddTableDemoDataService();
            //services.AddServerSideBlazor();
            //services.AddRazorComponents().AddInteractiveServerComponents();

            //等同于在根组件外面加了个CascadingAuthenticationState
            // services.AddCascadingAuthenticationState();

            //services.AddSingleton<TrackingCircuitHandler>();

            //services.AddSingleton<CircuitHandler, TrackingCircuitHandler>(p => p.GetRequiredService<TrackingCircuitHandler>());
            //services.AddBootstrapBlazor();
            services.AddSignalR(); //启用blazor时，已经包含这个

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                    //正事发布时取消注释
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        //这俩冲突，但老版本的SinglR好像又必须启用AllowCredentials
                        .AllowCredentials() //正事发布时取消注释
                                            //.AllowAnyOrigin() //正事发布时注释
                )
            );

            #region Swagger
            if (this.webHostEnvironment.IsDevelopment())
            {
                // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(_apiVersion, new OpenApiInfo
                    {
                        Title = "ZLJ API",
                        Version = _apiVersion,
                        Description = "abp831",
                        // uncomment if needed TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "abp831",
                            Email = string.Empty,
                            Url = new Uri("https://twitter.com/aspboilerplate"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/LICENSE"),
                        }
                    });
                    options.DocInclusionPredicate((docName, description) => true);

                    //解决不同命名空间中，同名模型导致冲突的问题
                    options.CustomSchemaIds(type => type.ToString());

                    //options.IncludeXmlComments(AppContext.BaseDirectory + typeof(GeneralTreeModule).Assembly.GetName().Name + ".XML");

                    //options.IncludeXmlComments(AppContext.BaseDirectory + typeof(BXJGUtilsModule).Assembly.GetName().Name + ".XML");
                    //options.IncludeXmlComments(AppContext.BaseDirectory + typeof(BXJGUtilsApplicationModule).Assembly.GetName().Name + ".XML");

                    //options.IncludeXmlComments(AppContext.BaseDirectory + typeof(BXJG.WorkOrder.ApplicationModule).Assembly.GetName().Name + ".XML");
                    //options.IncludeXmlComments(AppContext.BaseDirectory + typeof(BXJG.WorkOrder.BXJGCommonApplicationModule).Assembly.GetName().Name + ".XML");
                    //options.IncludeXmlComments(AppContext.BaseDirectory + typeof(BXJGWorkOrderEmployeeApplicationModule).Assembly.GetName().Name + ".XML");


                    options.IncludeXmlComments(AppContext.BaseDirectory + typeof(ZLJApplicationModule).Assembly.GetName().Name + ".XML");
                    options.IncludeXmlComments(AppContext.BaseDirectory + typeof(CommonApplicationModule).Assembly.GetName().Name + ".XML");
                    options.IncludeXmlComments(AppContext.BaseDirectory + typeof(BXJGUtilsApplicationModule).Assembly.GetName().Name + ".XML");

                    // options.IncludeXmlComments(AppContext.BaseDirectory + typeof(EmployeeApplicationModule).Assembly.GetName().Name + ".XML");
                    //options.IncludeXmlComments(AppContext.BaseDirectory + typeof(WebCustomerModule).Assembly.GetName().Name + ".XML");


                    // Define the BearerAuth scheme that's in use
                    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                    {
                        Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                });
            }
            #endregion


            #region Hangfire
            //var hangfireOpt = new SqlServerStorageOptions
            //{
            //SqlServerStorageOptions构造函数中都已经设置了默认值 
            //CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //QueuePollInterval = TimeSpan.Zero,

            //不加这个，在首次启动创建表时会报错（但表任然会创建，而且后续启动时不会报错了，因为表已经存在了）,
            //可以手动JobStorage.Current = new SqlServerStorage(_appConfiguration.GetConnectionString("Default"));
            //参考
            //https://github.com/HangfireIO/Hangfire/issues/1586
            //https://github.com/HangfireIO/Hangfire/issues/1390
            //但设置如下参数，即使数据库中不存在hangfire表，也不会迁移，真TM扯淡
            //PrepareSchemaIfNecessary = false,
            //怀疑是我们的项目中在模块启动阶段调用了hangfire，太快了hangfire还没初始化完成


            //UseRecommendedIsolationLevel = true,
            //DisableGlobalLocks = true
            //};
            string hangfireConnectionString = defaultConnectionString;// _appConfiguration.GetConnectionString("HangfireSqlServer")!;

            services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    //.UseDashboardMetric( ),
                    .UseSqlServerStorage(hangfireConnectionString, new SqlServerStorageOptions
                    {
                        //默认值已满足需求
                    }));

            //参考 https://docs.hangfire.io/en/latest/configuration/using-sql-server.html
            //GlobalConfiguration.Configuration.UseSqlServerStorage(defaultConnectionString, hangfireOpt);
            //首次迁移后abp的表会生成，hangfire表不会生成，然后启动主项目hangfire会报错，但表会生成，然后再次启动主项目就好了
            //下面的配置可以解决这个问题，或者考虑在迁移项目中初始化hangfire，让其生成表
            JobStorage.Current = new SqlServerStorage(hangfireConnectionString);

            //services.AddSingleton<IDistributedLockProvider>(_ => new SqlDistributedSynchronizationProvider(this._appConfiguration.GetConnectionString("Default")));
            //services.AddSingleton<IDistributedLockProvider>(_=>new MySqlDistributedSynchronizationProvider(defaultConnectionString));

            //services.AddHangfireServer(opt => {
            //    opt.Queues = new[] { "abp" };
            //});
            //单库多队列方案根本行不通，官方文档害人，参考 https://github.com/HangfireIO/Hangfire/issues/1837
            services.AddHangfireServer();//这句是执行器，不加的话，任务不会被执行
            #endregion



            //#region CAP 依赖ef选项，所以放abp配置下面


            //services.AddCap(config =>
            //{
            //    config.UseInMemoryMessageQueue();
            //    //config.UseRedis();
            //    config.UseEntityFramework<ZLJDbContext>()
            //          .RegisterExtension(new zhuce());//cap在注册阶段会去通过dbcontext拿连接字符串，但abp中dbcontext的连接字符串是动态解析来的，所以这扩展替换cap的逻辑
            //                                          //若要改善这一点，要替换cap ef中的option对象
            //    config.UseDashboard();
            //})
            //    //abp的模块是在Configure方法中启动的，cap是在asp.net core上启动的，cap启动时abp的模块还没执行，所以相关服务还没注册完成，导致默认情况下cap启动时无法获取所有订阅服务
            //    .AddSubscriberAssembly(typeof(CustomerApplicationModule),/* typeof(EmployeeApplicationModule),*/ typeof(ZLJApplicationModule), typeof(CommonApplicationModule));


            //#endregion

            #region abp
            //老的
            //Configure Abp and Dependency Injection
            //return services.AddAbp<ZLJWebHostModule>(
            //    // Configure Log4Net logging
            //    options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
            //        f => f.UseAbpLog4Net().WithConfig("log4net.config")
            //    )
            //);

            //abp下载的模板项目8.x中的方式
            services.AddAbpWithoutCreatingServiceProvider<ZLJWebHostModule>(
                // Configure Log4Net logging
                //options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseAbpLog4Net().WithConfig(webHostEnvironment.IsDevelopment() ? "log4net.config" : "log4net.Production.config"))
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseAbpLog4Net().WithConfig("log4net.config"))
            , true);
            //注意这里的第2个参数，abp模板项目没有这个参数，使用的默认true
            //改为false是希望保留按约定拦截，具体看文档中的blazor集成

            #endregion
            // var sdf = services.Where(c =>( c.ServiceType.FullName.Contains("Abp", StringComparison.OrdinalIgnoreCase)|| c.ServiceType.FullName.Contains("zlj", StringComparison.OrdinalIgnoreCase))).ToList();

            //var sdf = services.Where(c =>c.Lifetime== ServiceLifetime.Singleton&&( c.ServiceType.FullName.Contains("Abp", StringComparison.OrdinalIgnoreCase)|| c.ServiceType.FullName.Contains("zlj", StringComparison.OrdinalIgnoreCase))).ToList();

            //services.Configure<JwtBearerOptions>(opt =>
            //{

            //    opt.Events.OnAuthenticationFailed = x =>
            //    {
            //        return Task.CompletedTask;
            //    };

            //    opt.Events.OnChallenge = x =>
            //    {
            //        return Task.CompletedTask;
            //    };
            //    //     opt.Events.OnRedirectToLogin = c => {

            //    //         var request = c.HttpContext.Request;
            //    //        var sdf =  string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
            //    //string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);

            //    //         c.HttpContext.Response.Redirect(opt.LoginPath);
            //    //         return Task.CompletedTask;
            //    //     };
            //});

            //services.Configure<CookieAuthenticationOptions>(opt =>
            //{
            //    opt.Events.OnValidatePrincipal = x => 
            //    {
            //        return Task.CompletedTask;
            //    };
            //    opt.Events.OnRedirectToLogin = c =>
            //    {

            //        var request = c.HttpContext.Request;
            //        var sdf = string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
            //        string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);

            //        c.HttpContext.Response.Redirect(opt.LoginPath);
            //        return Task.CompletedTask;
            //    };
            //});
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {

            app.Use((ctx, next) =>
            {
                ctx.Request.Headers["Accept-Language"] = ctx.Request.Headers["Accept-Language"].ToString()
                                                                                               .Replace("zh-CN,", "zh-Hans,")
                                                                                               .Replace("zh-CN;", "zh-Hans;")
                                                                                               .Replace("zh,", "zh-Hans,")
                                                                                               .Replace("zh;", "zh-Hans;");
                return next();
            });


            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseRouting();

            //app.Use(async (HttpContext a, Func<Task> b) =>
            //{
            //    //  var sdfsdf = app.ApplicationServices.GetRequiredService<IAuthorizationMiddlewareResultHandler>();
            //    //       a.ChallengeAsync
            //    // var sdfsdf = a.RequestServices.GetService<IAuthorizationPolicyProvider>();
            //    // var sdfsfd = await sdfsdf.GetDefaultPolicyAsync();

            //    //  var sdfs = sdfsfd.AuthenticationSchemes;

            //    // a.ChallengeAsync();

            //    var zz = a.RequestServices.GetService<IAuthenticationService>();
            //    await zz.AuthenticateAsync(a,default);



            //    var zc = a.RequestServices.GetService<IAuthenticationSchemeProvider>();
            //    var q = await zc.GetDefaultChallengeSchemeAsync();
            //    var q1 = await zc.GetDefaultForbidSchemeAsync();
            //    var q3 = await zc.GetDefaultAuthenticateSchemeAsync();

            //    //    await b(a);
            //}
            //);

            //若存在IAuthenticationRequestHandler，则执行它并放弃其它的身份验证
            //否则使用默认身份验证方案
            app.UseAuthentication();

            ////身份验证会走默认的jwt身份验证方案，它使httpcontext.user不为空，但里面没有claim，所以这里需要替换下
            ////参考：https://github.com/dotnet/aspnetcore/blob/b034a7da67b5f81161e82b19b3ade458139f9c2b/src/Security/Authentication/Core/src/AuthAppBuilderExtensions.cs
            ////https://github.com/dotnet/aspnetcore/blob/c85baf8db0c72ae8e68643029d514b2e737c9fae/src/Security/Authentication/Core/src/AuthenticationMiddleware.cs
            ////最好是自定义中间件，在jwt之后判断，若没有则尝试cookie登陆
            //app.Use(async (context, next) =>
            //{
            //    if (context.User != default&& context.User.Identity!=default)
            //    {
            //        if (!context.User.Identity.IsAuthenticated)
            //        {
            //            context.User = (await context.AuthenticateAsync(IdentityConstants.ApplicationScheme))?.Principal;
            //        }
            //    }

            //    // Do work that can write to the Response.
            //    await next.Invoke();
            //    // Do logging or other work that doesn't write to the Response.
            //});


            ////app.UseWeChatPayment();
            //app.UseHangfireDashboard("/hangfire", new Hangfire.DashboardOptions
            //{
            //    //Authorization=null 
            //    //swagger那里的登陆是基于token的，而hangfire自己的
            //    //Authorization = new[] { new aaa() }
            //    //由于hangfire请求时不携带token，因此是基于cookie的身份验证，且不太好改造
            //    //所以我们需要单独为它提供一个登陆页面
            //    Authorization = new[] { new AbpHangfireAuthorizationFilter(PermissionNames.HangFireDashboard) }
            //    //Authorization = new[] { new aaa(PermissionNames.HangFireDashboard) }
            //});


            app.UseAuthorization();


            app.UseAbpRequestLocalization();

            //app.UseAntiforgery();

            //  var opt = app.ApplicationServices.GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>().CurrentValue;

            // var opt = app.ApplicationServices.GetRequiredService<IEnumerable< IOptionsMonitor<CookieAuthenticationOptions>>>();
            // opt.Events.OnRedirectToLogin = c =>
            //{

            //    var request = c.HttpContext.Request;
            //    var sdf = string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
            //    string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);

            //    c.HttpContext.Response.Redirect(opt.LoginPath);
            //    return Task.CompletedTask;
            //};

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapRazorComponents<Shared.App>().AddInteractiveServerRenderMode();
                endpoints.MapHub<AbpCommonHub>("/signalr");
                //endpoints.MapHangfireDashboardWithAuthorizationPolicy(PermissionNames.HangFireDashboard);
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapHangfireDashboard(PermissionNames.HangFireDashboard, "/hangfire", new DashboardOptions
                //{
                //    Authorization = new[] { new AbpHangfireAuthorizationFilter(PermissionNames.HangFireDashboard) }
                //});
                // endpoints.MapBlazorHub();
                //endpoints.MapDefaultControllerRoute(); //mvc路由
                //endpoints.MapControllers();

                //endpoints.MapHangfireDashboard();

                //找不到rcl中的controller，怀疑：要么用application part把rcl加入进去， 要么使用特征路由（导致controller被加入）
                //若使用razor页面就没这个问题了，因为rcl本来就可以放页面
                //参考：https://learn.microsoft.com/zh-cn/aspnet/core/mvc/advanced/app-parts?view=aspnetcore-7.0#load-aspnet-core-features


                //endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                //  endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapFallbackToAreaPage("_Host1", "customer");
                //endpoints.MapFallbackToPage("/customer/{*all}", "/_Host1");
                //endpoints.MapFallbackToPage("/", "/pages/_Host");
                // endpoints.controller
                //  endpoints.MapDefaultControllerRoute();

                //endpoints.MapFallbackToPage("/_Host");//AddControllersWithViews并不会加services.AddRazorPages(); 所以若基于razorPage，则必须加services.AddRazorPages()

                //endpoints.MapSwagger();

                //我们这里是以统一的Home控制权来分发的，也可以每个app单独自己的controller


                //endpoints.MapControllerRoute("account", "{appKey=main}/account/{action}");
                //endpoints.MapFallbackToFile("cap", "");



                //endpoints.MapFallbackToPage("custApp/{*p}", "_CustApp");
                //endpoints.MapDynamicControllerRoute()

            });

            if (this.webHostEnvironment.IsDevelopment())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });
                // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
                app.UseSwaggerUI(options =>
                {
                    //options.SwaggerEndpoint(
                    //    _appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json",
                    //    "ZLJ API V1");
                    options.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", $"ZLJ API {_apiVersion}");
                    options.IndexStream = () => Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("ZLJ.Web.Host.wwwroot.swagger.ui.index.html");
                    options.DisplayRequestDuration(); // Controls the display of the request duration (in milliseconds) for "Try it out" requests.  

                });
            }


            //不晓得为啥，放上面无法身份验证
            //app.UseHangfireServer();
            //app.UseHangfireDashboard();


            //本来这个该放core module去注册，但是abp的 backgroundjobManager未提供，不方便实现
            //定时任务，自动抄表（租赁单生成结算单）
            //RecurringJob.AddOrUpdate<ScanWaitingSettleOrderJob>(nameof(ScanWaitingSettleOrderJob), x => x.ExecuteAsync(null), Cron.Daily(1, 0), TimeZoneInfo.Local);
        }
    }
}