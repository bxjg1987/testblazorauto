using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.ClientProxy;
using ZLJ.Application.Share.Auditing;
using ZLJ.Application.Share.Auditing.Dto;

namespace ZLJ.Admin.ClientProxy
{
    public class AuditingAppService :  IAuditLogAppService
    {
        HttpClient _httpClient;
        public AuditingAppService(IHttpClientFactory httpClientFactory) 
        {
            _httpClient = httpClientFactory.CreateHttpClientAdmin();
        }

        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        {
            if (input.MaxResultCount <= 0)
                input.MaxResultCount = 20;
            //最好把这个方法变成post的，传参更简单，或把api整体配置为post
            return await _httpClient.Post<PagedResultDto<AuditLogListDto>>("AuditLog/GetAuditLogs", input);
        }
    }
}
