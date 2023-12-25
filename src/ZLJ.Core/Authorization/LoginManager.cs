using Microsoft.AspNetCore.Identity;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Zero.Configuration;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace ZLJ.Authorization
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        //private readonly IHttpContextAccessor _httpContext;


        //private readonly CustomerLoginManager<Tenant, Role, User, UserManager> customerLoginManager;
        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            IPasswordHasher<User> passwordHasher,
            RoleManager roleManager,
            UserClaimsPrincipalFactory claimsPrincipalFactory/*, IHttpContextAccessor httpContextAccessor, CustomerLoginManager<Tenant, Role, User, UserManager> customerLoginManager*/)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  passwordHasher,
                  roleManager,
                  claimsPrincipalFactory)
        {
            //this.customerLoginManager = customerLoginManager;
            //this._httpContext = httpContextAccessor;
        }

        //public Task<AbpLoginResult<Tenant, User>> WechartMiniProgramLoginAsync() {
        //    this._httpContext.AuthenticateAsync
        //}

        ///// <summary>
        ///// 重写以处理不同类型用户的登陆逻辑
        ///// 若不这样做请考虑为不同的用户类型单独定义登陆器
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="tenant"></param>
        ///// <returns></returns>
        //protected override async Task<AbpLoginResult<Tenant, User>> CreateLoginResultAsync(User user, Tenant tenant = null)
        //{
        //    var r = await base.CreateLoginResultAsync(user, tenant);

        //  //  await customerLoginManager.TryLoginAsync(r);//尝试以顾客身份登陆，

        //    return r;
        //}
    }
}
