using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using ZLJ.Authorization.Roles;
using BXJG.Utils.BusinessUser;
using BXJG.Shop.Customer;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using ZLJ.MultiTenancy;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Http;

namespace ZLJ.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ICustomerLoginManager<User> customerLoginManager;

        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor, ICustomerLoginManager<User> customerLoginManager, IUnitOfWorkManager unitOfWorkManager, IHttpContextAccessor contextAccessor)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor, unitOfWorkManager)
        {
            this.customerLoginManager = customerLoginManager;
            this.contextAccessor = contextAccessor;
        }
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var claim = await base.CreateAsync(user);
            if (contextAccessor.HttpContext.Request.Query.TryGetValue("userType", out var longUserType))
            {
                if (longUserType.First().Equals(customerLoginManager.claimKey, System.StringComparison.OrdinalIgnoreCase))
                {
                    var c = await customerLoginManager.GetBusinessUserClaim(user);
                    claim.Identities.First().AddClaim(c);
                }
            }
            return claim;
        }
    }
}
