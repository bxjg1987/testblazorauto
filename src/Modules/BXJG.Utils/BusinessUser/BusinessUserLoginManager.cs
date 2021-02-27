using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.BusinessUser
{
    public interface IBusinessUserLoginManager<TUser>
    {
        Task<Claim> GetBusinessUserClaim(TUser user);
    }
    
    public class BusinessUserLoginManager<TEntity, TKey, TTenant, TRole, TUser, TUserManager> : IBusinessUserLoginManager<TUser>
        where TEntity : class, IEntity<TKey>, IBusinessUserEntity
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        protected readonly IRepository<TEntity, TKey> repository;
        protected readonly TUserManager userManager;
        protected readonly string roleName;
        protected readonly string claimKey;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        public BusinessUserLoginManager(IRepository<TEntity, TKey> repository,
                                        TUserManager userManager,
                                        string roleName,
                                        string claimKey)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.roleName = roleName;
            this.claimKey = claimKey;
        }

        public async Task<Claim> GetBusinessUserClaim(TUser user)
        {
            var isCustomer = await userManager.IsInRoleAsync(user, roleName);
            if (!isCustomer)
                return null;
            var custId = await AsyncQueryableExecuter.FirstOrDefaultAsync(repository.GetAll().Where(c => c.UserId == user.Id).Select(c => c.Id));
            return new Claim(claimKey, custId.ToString());
        }
    }
}
