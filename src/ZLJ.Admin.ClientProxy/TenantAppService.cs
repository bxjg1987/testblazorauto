using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.ClientProxy;
using ZLJ.Application.Share.MultiTenancy;

namespace ZLJ.Admin.ClientProxy
{
    public class TenantAppService : BaseAppServiceClient, ITenantAppService
    {
        public TenantAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<BatchOperationOutput<int>> BatchDeleteAsync(BatchOperationInput<int> input)
        {
            return await Post<BatchOperationOutput<int>>("api/services/app/tenant/BatchDelete", input);
        }

        public async Task<TenantDto> CreateAsync(EditTenantDto input)
        {
            return await Post<TenantDto>("api/services/app/tenant/Create", input);
        }

        public async Task DeleteAsync(EntityDto<int> input)
        {
            await Post("api/services/app/tenant/Delete", input);
        }

        public async Task<PagedResultDto<TenantDto>> GetAllAsync(PagedAndSortedResultRequest<Condition> input)
        {
            return await Post<PagedResultDto<TenantDto>>("api/services/app/tenant/getall", input);
        }

        public async Task<TenantDto> GetAsync(EntityDto<int> input)
        {
            return await Post<TenantDto>("api/services/app/tenant/Get", input);
        }

        //public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        //{
        //    if (input.MaxResultCount <= 0)
        //        input.MaxResultCount = 20;
        //    //最好把这个方法变成post的，传参更简单，或把api整体配置为post
        //    return await Post<PagedResultDto<AuditLogListDto>>("api/services/app/AuditLog/GetAuditLogs", input);
        //}

        public async Task<TenantDto> UpdateAsync(EditTenantDto input)
        {
            return await Post<TenantDto>("api/services/app/tenant/Update", input);
        }
    }
}