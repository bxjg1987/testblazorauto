using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
using Microsoft.AspNetCore.Identity;
namespace BXJG.Shop
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class BXJGShopAppServiceBase: ApplicationService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        //参考BXJGShopDomainServiceBase中的注释
        //public ICancellationTokenProvider CancellationToken { get; set; } = NullCancellationTokenProvider.Instance;
        protected BXJGShopAppServiceBase()
        {
            LocalizationSourceName = BXJGShopConsts.LocalizationSourceName;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
    }
}
