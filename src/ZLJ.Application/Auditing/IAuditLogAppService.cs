using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ZLJ.App.Admin.Auditing.Dto;

namespace ZLJ.App.Admin.Auditing
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);

        //Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);
    }
}