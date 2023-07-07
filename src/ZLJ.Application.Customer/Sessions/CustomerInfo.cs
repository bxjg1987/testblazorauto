using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.AssociatedCompany;
using ZLJ.Customer;

namespace ZLJ.App.Customer.Sessions
{
    [AutoMapFrom(typeof(AssociatedCompanyEntity))]
    public class CustomerInfo
    {
        public string Name { get; set; }
    }
}
