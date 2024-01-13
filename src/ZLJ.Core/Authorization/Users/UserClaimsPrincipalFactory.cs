using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using ZLJ.Core.Authorization.Roles;
using ZLJ.Core.BaseInfo.StaffInfo;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Http;
using ZLJ.Core.Customer;

namespace ZLJ.Core.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IRepository<User, long> userRepository;

        public UserClaimsPrincipalFactory(
            IHttpContextAccessor contextAccessor,
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor, IUnitOfWorkManager unitOfWorkManager, IRepository<User, long> userRepository)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor, unitOfWorkManager)
        {
            this.contextAccessor = contextAccessor;
            this.userRepository = userRepository;
        }
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var claim = await base.CreateAsync(user);

            //TenantId
           // claim.Identities.First().AddClaim(new Claim("TenantId", user.TenantId.HasValue?user.TenantId.Value.ToString():"0"));

            if (user is CustomerStaffInfoEntity cs)
            {
                claim.Identities.First().AddClaim(new Claim("customerId", cs.CustomerId.ToString()));
            }
            // var user = await userRepository.GetAsync(claim);
            //if (contextAccessor.HttpContext.Request.Query.TryGetValue("userType", out var longUserType))
            //{
            //    if (longUserType.First().Equals(_staffLoginManager.claimKey, System.StringComparison.OrdinalIgnoreCase))
            //    {
            //        var c = await _staffLoginManager.GetBusinessUserClaim(user);
            //        claim.Identities.First().AddClaim(c);
            //    }
            //}
            //先默认员工登陆，顾客登陆时需要重构这里
            //try
            //{
            //    var c = await _staffLoginManager.GetBusinessUserClaim(user);
            //    claim.Identities.First().AddClaim(c);
            //}
            //catch
            //{

            //}

            return claim;
        }
    }
}
