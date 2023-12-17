using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ZLJ.Application.Share.Auditing;
using ZLJ.Application.Share.Auditing.Dto;

namespace ZLJ.Application.Share.Auditing
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);

        //Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);
    }
}