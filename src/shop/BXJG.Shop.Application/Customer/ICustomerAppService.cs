using Abp.Application.Services;
using BXJG.Shop.Customer.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    public interface ICustomerAppService : IApplicationService
    {
        Task<IList<CustomerDto>> GetListAsync(GetAllCustomersInput input);

        Task<CustomerDto> UpdateAsync(CustomerUpdateDto input); 
    }
}
