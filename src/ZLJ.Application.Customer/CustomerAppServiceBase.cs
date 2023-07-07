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

namespace ZLJ.App.Customer
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
   [AbpAuthorize(PermissionNames.Customer)]
    public abstract class CustomerAppServiceBase : ApplicationServiceBase
    {
        public CustomerSession CustomerSession { get; set; }
        public Lazy<IRepository<AssociatedCompanyEntity, long>> CompanyRepository { get; set; }
        //protected CustomerAppServiceBase(CustomerSession customerSession, Lazy<IRepository<AssociatedCompanyEntity, long>> userRepository)
        //{
        //    this.customerSession = customerSession;
        //    this.companyRepository = userRepository;

        //}
       // public long? CustomerId
        public CustomerAppServiceBase() {
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
        }

        public async Task<AssociatedCompanyEntity> GetCurrentCompanyAsync()
        {
            if (CustomerSession.CustomerId.HasValue)
                return await CompanyRepository.Value.GetAsync(CustomerSession.CustomerId.Value);
            return default;
        }

        ////public new IAbpSession AbpSession { 

        ////}
        //public TenantManager TenantManager { get; set; }
        ////public IStaffSession StaffSession { get; set; }
        //public UserManager UserManager { get; set; }
        ////public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入
        //protected CustomerAppServiceBase()
        //{
        //    LocalizationSourceName = ZLJConsts.LocalizationSourceName;
        //  //  AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        //}

        //protected virtual async Task<User> GetCurrentUserAsync()
        //{
        //    var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
        //    if (user == null)
        //    {
        //        throw new Exception("There is no current user!");
        //    }

        //    return user;
        //}

        //protected virtual Task<Tenant> GetCurrentTenantAsync()
        //{
        //    return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        //}

        //protected virtual void CheckErrors(IdentityResult identityResult)
        //{
        //    identityResult.CheckErrors(LocalizationManager);
        //}
    }

    ///// <summary>
    ///// 员工端应用服务基类  已经移植到ZLJ.Application.Employee
    ///// </summary>
    //public class StaffAppServiceBase : ZLJAppServiceBase
    //{
    //    private readonly IStaffSession staffSession;

    //    public StaffAppServiceBase(IStaffSession staffSession)
    //    {
    //        this.staffSession = staffSession;
    //    }


    //}
}
