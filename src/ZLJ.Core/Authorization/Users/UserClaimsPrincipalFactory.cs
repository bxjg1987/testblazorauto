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

namespace ZLJ.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        private readonly ICustomerLoginManager<User> customerLoginManager;

        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor, ICustomerLoginManager<User> customerLoginManager)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor)
        {
            this.customerLoginManager = customerLoginManager;
        }
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var claim = await base.CreateAsync(user);
            var c = await customerLoginManager.GetBusinessUserClaim(user);
            if(c!=null)
            claim.Identities.First().AddClaim(c);

            return claim;
        }
    }
}
