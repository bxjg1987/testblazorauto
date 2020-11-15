using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using Abp.Organizations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ZLJ.Authorization.Roles;

namespace ZLJ.Authorization.Users
{
    public class UserStore : AbpUserStore<Role, User>
    {
        public UserStore(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            
            IRepository<UserRole, long> userRoleRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserClaim, long> userClaimRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
            : base(
                unitOfWorkManager,
                userRepository,
                roleRepository,
                
                userRoleRepository,
                userLoginRepository,
                userClaimRepository,
                userPermissionSettingRepository,
                userOrganizationUnitRepository,
                organizationUnitRoleRepository)
        {
        }
        //public override async Task ReplaceClaimAsync(User user, Claim claim,  Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    Check.NotNull(user, nameof(user));
        //    Check.NotNull(claim, nameof(claim));
        //    Check.NotNull(newClaim, nameof(newClaim));

        //    await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Claims, cancellationToken);

        //    var userClaims = user.Claims.Where(uc => uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type);
        //    foreach (var userClaim in userClaims)
        //    {
        //        userClaim.ClaimType = claim.Type;
        //        userClaim.ClaimValue = claim.Value;
        //    }
        //}
    }
}
