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
    public interface IBusinessUserLoginManager<in TUser>
    {
        public string claimKey { get; }
        Task<Claim> GetBusinessUserClaim(TUser user);
    }

    public class BusinessUserLoginManager<TEntity, TKey, TUser> : IBusinessUserLoginManager<TUser>
        where TEntity : class, IEntity<TKey>, IBusinessUserEntity
        where TUser : AbpUserBase
    {
        protected readonly IRepository<TEntity, TKey> repository;
        public string claimKey { get; private set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        public BusinessUserLoginManager(IRepository<TEntity, TKey> repository, string claimKey)
        {
            this.repository = repository;
            this.claimKey = claimKey;
        }

        public async Task<Claim> GetBusinessUserClaim(TUser user)
        {
            //var isCustomer = await userManager.IsInRoleAsync(user, roleName);
            //if (!isCustomer)
            //    return null;
            var custId = await AsyncQueryableExecuter.FirstOrDefaultAsync(repository.GetAll().Where(c => c.UserId == user.Id).Select(c => c.Id));
            return new Claim(claimKey, custId.ToString());
        }
    }
}
