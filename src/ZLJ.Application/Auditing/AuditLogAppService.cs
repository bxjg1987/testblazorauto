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
using ZLJ.Core.Authorization.Users;
using System;
using Abp.Linq;
using Microsoft.EntityFrameworkCore;
using ZLJ.Application.Admin.Authorization.Permissions;
using ZLJ.Application.Share.Auditing;
using ZLJ.Application.Share.Auditing.Dto;
using ZLJ.Application.Share.Authorization.Permissions;

namespace ZLJ.Application.Admin.Auditing
{
    [Abp.Domain.Uow.UnitOfWork(false)]
    [DisableAuditing]//禁用它的审计日志功能
    [AbpAuthorize(PermissionNames.AdministratorSystemLog)]
    public class AuditLogAppService : AdminBaseAppService, IAuditLogAppService
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly IRepository<User, long> _userRepository;
        //private readonly IAuditLogListExcelExporter _auditLogListExcelExporter;
        private readonly INamespaceStripper _namespaceStripper;

        public AuditLogAppService(
            IRepository<AuditLog, long> auditLogRepository,
            IRepository<User, long> userRepository,
            //IAuditLogListExcelExporter auditLogListExcelExporter, 
            INamespaceStripper namespaceStripper)
        {
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            //_auditLogListExcelExporter = auditLogListExcelExporter;
            _namespaceStripper = namespaceStripper;

        }

        public async Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input)
        {
            //if (input.StartDate == null)
            //    input.StartDate = DateTime.Now.Date;
            //if (input.EndDate == null)
            //    input.EndDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            //else
            //    input.EndDate = input.EndDate.Value.AddDays(1).AddSeconds(-1);

            if (input.Sorting.ToLower().StartsWith("user") && !input.Sorting.StartsWith("User."))
            {
                input.Sorting = input.Sorting.Replace("user", "User.", StringComparison.OrdinalIgnoreCase);// "User." + input.Sorting;
            }
            else if (!input.Sorting.StartsWith("AuditLog."))
            {
                input.Sorting = "AuditLog." + input.Sorting.Replace(",", ",AuditLog."); //"AuditLog." + input.Sorting;
            }

            var query = CreateAuditLogAndUsersQuery(input);

            var resultCount = await AsyncQueryableExecuter.CountAsync(query);
            var results = await query
                .OrderBy(input.Sorting)
                .PageBy(input).ToListAsync(CancellationTokenProvider.Token);

            var auditLogListDtos = ConvertToAuditLogListDtos(results);

            return new PagedResultDto<AuditLogListDto>(resultCount, auditLogListDtos);
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
                    var auditLogListDto = base.ObjectMapper.Map<AuditLogListDto>(result.AuditLog);// result.AuditLog.MapTo<AuditLogListDto>();
                    auditLogListDto.UserName = result.User == null ? null : result.User.UserName;
                    auditLogListDto.ServiceName = _namespaceStripper.StripNameSpace(auditLogListDto.ServiceName);
                    return auditLogListDto;
                }).ToList();
        }

        private IQueryable<AuditLogAndUser> CreateAuditLogAndUsersQuery(GetAuditLogsInput input)
        {
            var query = from auditLog in _auditLogRepository.GetAll().AsNoTrackingWithIdentityResolution()
                        join user in _userRepository.GetAll().AsNoTrackingWithIdentityResolution() on auditLog.UserId equals user.Id into userJoin
                        from joinedUser in userJoin.DefaultIfEmpty()
                            //where auditLog.ExecutionTime >= input.StartDate && auditLog.ExecutionTime <= input.EndDate
                        select new AuditLogAndUser { AuditLog = auditLog, User = joinedUser };
            //这里很诡异， User = new User 然后所有字段赋值查询就快，直接赋值joinedUser 到了后面几页就慢
            //生成的语句在数据库中直接执行也快
            //换高版本数据库试试

            query = query.WhereIf(input.StartDate.HasValue, c => c.AuditLog.ExecutionTime >= input.StartDate.Value)
                         .WhereIf(input.EndDate.HasValue, c => c.AuditLog.ExecutionTime < input.EndDate.Value)
                         .WhereIf(input.Keywords.IsNotNullOrWhiteSpaceBXJG(), item => item.User.UserName.Contains(input.Keywords) ||
                                                                                      item.AuditLog.ServiceName.Contains(input.Keywords) ||
                                                                                      item.AuditLog.MethodName.Contains(input.Keywords)||
                                                                                      item.AuditLog.BrowserInfo.Contains(input.Keywords))
                         .WhereIf(input.MinExecutionDuration.HasValue && input.MinExecutionDuration > 0, item => item.AuditLog.ExecutionDuration >= input.MinExecutionDuration.Value)
                         .WhereIf(input.MaxExecutionDuration.HasValue && input.MaxExecutionDuration < int.MaxValue, item => item.AuditLog.ExecutionDuration <= input.MaxExecutionDuration.Value)
                         .WhereIf(input.HasException == true, item => item.AuditLog.Exception != null && item.AuditLog.Exception != "")
                         .WhereIf(input.HasException == false, item => item.AuditLog.Exception == null || item.AuditLog.Exception == "");
            return query;
        }
    }
}