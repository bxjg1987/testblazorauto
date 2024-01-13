using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.BaseInfo.AssociatedCompany;

namespace ZLJ.Core.Customer
{
    public class CustomerOUEntity : OrganizationUnit, IMustHaveCustomer,IPassivable
    {
        public long CustomerId { get; set; }
        public virtual AssociatedCompanyEntity Customer { get; set; }
        public bool IsActive { get; set; }
    }
}
