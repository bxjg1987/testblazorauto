using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;

using ZLJ.Authorization;
using ZLJ.Authorization.Users;
using ZLJ.Auditing.Dto;
using System;
using Abp.Linq;
namespace ZLJ.Auditing
{
    [DisableAuditing]//禁用它的审计日志功能
    [AbpAuthorize(PermissionNames.AdministratorSystemLog)]
    public class AuditLogAppService : ZLJAppServiceBase, IAuditLogAppService
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly IRepository<User, long> _userRepository;
        //private readonly IAuditLogListExcelExporter _auditLogListExcelExporter;
        private readonly INamespaceStripper _namespaceStripper;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入

        public AuditLogAppService(
            IRepository<AuditLog, long> auditLogRepository, 
            IRepository<User, long> userRepository, 
            //IAuditLogListExcelExporter auditLogListExcelExporter, 
            INamespaceStripper namespaceStripper )
        {
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            //_auditLogListExcelExporter = auditLogListExcelExporter;
            _namespaceStripper = namespaceStripper;

            this.AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }
        
        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        {
            if (input.StartDate == null)
                input.StartDate = DateTime.Now.Date;
            if (input.EndDate == null)
                input.EndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            //else
            //    input.EndDate = input.EndDate.Value.AddDays(1).AddSeconds(-1);

            if (input.Sorting.IsNullOrWhiteSpace())
                input.Sorting = "AuditLog.ExecutionTime desc";
            else if (input.Sorting.ToLower().StartsWith( "username"))
            {
                input.Sorting = "User." + input.Sorting;
            }
            else
            {
                input.Sorting = "AuditLog." + input.Sorting;
            }


            var query = CreateAuditLogAndUsersQuery(input);
           
            var resultCount = await AsyncQueryableExecuter.CountAsync(query);
            var results = await AsyncQueryableExecuter.ToListAsync( query
                .OrderBy(input.Sorting)
                .PageBy( input.SkipCount,input.MaxResultCount));

            var auditLogListDtos = ConvertToAuditLogListDtos(results);

            return new PagedResultDto<AuditLogListDto>  (resultCount, auditLogListDtos);
        }

        //public async Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input)
        //{
        //    var auditLogs = await CreateAuditLogAndUsersQuery(input)
        //                .AsNoTracking()
        //                .OrderByDescending(al => al.AuditLog.ExecutionTime)
        //                .ToListAsync();

        //    var auditLogListDtos = ConvertToAuditLogListDtos(auditLogs);

        //    return _auditLogListExcelExporter.ExportToFile(auditLogListDtos);
        //}

        private List<AuditLogListDto> ConvertToAuditLogListDtos(List<AuditLogAndUser> results)
        {
            return results.Select(
                result =>
                {
                    var auditLogListDto = result.AuditLog.MapTo<AuditLogListDto>();
                    auditLogListDto.UserName = result.User == null ? null : result.User.UserName;
                    auditLogListDto.ServiceName = _namespaceStripper.StripNameSpace(auditLogListDto.ServiceName);
                    return auditLogListDto;
                }).ToList();
        }

        private IQueryable<AuditLogAndUser> CreateAuditLogAndUsersQuery(GetAuditLogsInput input)
        {
            var query = from auditLog in _auditLogRepository.GetAll()
                join user in _userRepository.GetAll() on auditLog.UserId equals user.Id into userJoin
                from joinedUser in userJoin.DefaultIfEmpty()
                where auditLog.ExecutionTime >= input.StartDate && auditLog.ExecutionTime <= input.EndDate
                select new AuditLogAndUser {AuditLog = auditLog, User = joinedUser};

            query = query
                .WhereIf(!input.UserName.IsNullOrWhiteSpace(), item => item.User.UserName.Contains(input.UserName))
                .WhereIf(!input.ServiceName.IsNullOrWhiteSpace(), item => item.AuditLog.ServiceName.Contains(input.ServiceName))
                .WhereIf(!input.MethodName.IsNullOrWhiteSpace(), item => item.AuditLog.MethodName.Contains(input.MethodName))
                .WhereIf(!input.BrowserInfo.IsNullOrWhiteSpace(), item => item.AuditLog.BrowserInfo.Contains(input.BrowserInfo))
                .WhereIf(input.MinExecutionDuration.HasValue && input.MinExecutionDuration > 0, item => item.AuditLog.ExecutionDuration >= input.MinExecutionDuration.Value)
                .WhereIf(input.MaxExecutionDuration.HasValue && input.MaxExecutionDuration < int.MaxValue, item => item.AuditLog.ExecutionDuration <= input.MaxExecutionDuration.Value)
                .WhereIf(input.HasException == true, item => item.AuditLog.Exception != null && item.AuditLog.Exception != "")
                .WhereIf(input.HasException == false, item => item.AuditLog.Exception == null || item.AuditLog.Exception == "");
            return query;
        }
    }
}
