using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using Abp.Linq;
using ZLJ.Customer;
using ZLJ.BaseInfo.AssociatedCompany;
using ZLJ.App.Customer.Sessions;
using Abp.Localization.Sources;
using BXJG.Utils;

namespace ZLJ.App.Customer
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    [AbpAuthorize(PermissionNames.Customer)]
    public abstract class CustomerBaseAppService : CommonBaseAppService
    {
        public CustomerSession CustomerSession { get; set; }
        public Lazy<IRepository<AssociatedCompanyEntity, long>> CompanyRepository { get; set; }
        //protected CustomerAppServiceBase(CustomerSession customerSession, Lazy<IRepository<AssociatedCompanyEntity, long>> userRepository)
        //{
        //    this.customerSession = customerSession;
        //    this.companyRepository = userRepository;

        //}
        // public long? CustomerId
        public CustomerBaseAppService()
        {
            LocalizationSourceName = CustConsts.Cust;
        }

        public async Task<AssociatedCompanyEntity> GetCurrentCompanyAsync()
        {
            if (CustomerSession.CustomerId.HasValue)
                return await CompanyRepository.Value.GetAsync(CustomerSession.CustomerId.Value);
            return default;
        }



    }
}
