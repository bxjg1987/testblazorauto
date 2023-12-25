using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.AssociatedCompany;

namespace ZLJ.Customer
{
    public class CustomerOUEntity : OrganizationUnit, IMustHaveCustomer,IPassivable
    {
        public long CustomerId { get; set; }
        public virtual AssociatedCompanyEntity Customer { get; set; }
        public bool IsActive { get; set; }
    }
}
