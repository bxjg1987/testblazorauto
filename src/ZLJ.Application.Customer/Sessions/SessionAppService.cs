using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Auditing;
using ZLJ.BaseInfo.AssociatedCompany;
using ZLJ.Customer;

namespace ZLJ.App.Customer.Sessions
{
    //public class SessionAppService : Common.Sessions.SessionAppService
    //{
    //    private readonly IRepository<AssociatedCompanyEntity, long> repository;
    //    public SessionAppService( IRepository<AssociatedCompanyEntity, long> repository)
    //    {
    //        this.repository = repository;
    //    }
    //    [UnitOfWork(false)]
    //    protected override async ValueTask<ZLJ.App.Common.Sessions.Dto.GetCurrentLoginInformationsOutput> Create()
    //    {
    //        var ss = new GetCurrentLoginInformationsOutput();
    //        ss.Customer = new CustomerInfo();

    //        ss.Customer.Name = await repository.GetAll().Where(c => c.Id == customerSession.CustomerId).Select(c => c.Name).SingleOrDefaultAsync();

    //        return ss;
    //    }
    //}
}
