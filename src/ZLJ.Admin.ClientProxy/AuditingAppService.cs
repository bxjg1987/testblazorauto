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
    public class AuditingAppService : BaseAppServiceClient, IAuditLogAppService
    {
        public AuditingAppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        {
            //最好把这个方法变成post的，传参更简单，或把api整体配置为post
            return await Post<PagedResultDto<AuditLogListDto>>("api/services/app/AuditLog/GetAuditLogs", input);
        }
    }
}
