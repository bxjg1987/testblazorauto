using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BXJG.Shop.Customer.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Customer
{
    /*
     * 由于大部分功能都需要处理主程序中的用户和商城会员之间的关系，因此这里不继承IAsyncCrudAppService接口
     * 新增和修改暂时都使用CustomerUpdateDto，将来可能分开
     * 
     * 基本能用了，还需要仔细斟酌，CRUD操作 因为涉及到主程序的用户关联，因此应该将核心处理移动到CustomerManager中
     */

    /// <summary>
    /// 后台管理员对商城会员进行管理的应用服务
    /// </summary>
    public interface IBXJGShopCustomerAppService : IApplicationService
    {
        Task<PagedResultDto<CustomerDto>> GetListAsync(GetAllCustomersInput input);
        Task<CustomerDto> GetAsync(EntityDto<long> input);
        Task<CustomerDto> CreateAsync(CustomerUpdateDto input);
        Task<CustomerDto> UpdateAsync(CustomerUpdateDto input);
        Task<long[]> DeleteAsync(params long[] ids);
    }
}
