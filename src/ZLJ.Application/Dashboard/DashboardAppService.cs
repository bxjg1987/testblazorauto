using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLJ.Application.Common.Dashboard;
using ZLJ.Application.Share;
using ZLJ.Core.AssociatedCompany;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.BaseInfo.StaffInfo;
using ZLJ.Core.BaseInfo.Post;
using ZLJ.Core.MultiTenancy;

namespace ZLJ.Application.Dashboard
{
    /// <summary>
    /// 仪表盘应用服务
    /// </summary>
    [AbpAuthorize]
    public class DashboardAppService : AdminBaseAppService
    {
        private readonly IRepository<StaffInfoEntity, long> _staffRepository;
        private readonly IRepository<AssociatedCompanyEntity, long> _associatedCompanyRepository;
        private readonly IRepository<OrganizationUnit, long> _ouRepository;
        private readonly IRepository<PostEntity, int> _postRepository;
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly TenantManager _tenantManager;

        public DashboardAppService(
            IRepository<StaffInfoEntity, long> staffRepository,
            IRepository<AssociatedCompanyEntity, long> associatedCompanyRepository,
            IRepository<OrganizationUnit, long> ouRepository,
            IRepository<PostEntity, int> postRepository,
            IRepository<AuditLog, long> auditLogRepository,
            IRepository<User, long> userRepository,
            TenantManager tenantManager)
        {
            _staffRepository = staffRepository;
            _associatedCompanyRepository = associatedCompanyRepository;
            _ouRepository = ouRepository;
            _postRepository = postRepository;
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            _tenantManager = tenantManager;
        }

        /// <summary>
        /// 获取概览统计数据
        /// </summary>
        public async Task<OverviewDto> GetOverviewAsync()
        {
            var now = Clock.Now;
            var monthStart = new DateTime(now.Year, now.Month, 1);

            var staffQuery = await _staffRepository.GetAllListAsync();
            var ouQuery = await _ouRepository.GetAllListAsync();
            var postQuery = await _postRepository.GetAllListAsync();
            var companyQuery = await _associatedCompanyRepository.GetAllListAsync();

            var dto = new OverviewDto
            {
                StaffCount = staffQuery.Count,
                StaffNewThisMonth = staffQuery.Count(s => s.CreationTime >= monthStart),
                OuCount = ouQuery.Count,
                PostCount = postQuery.Count,
                AssociatedCompanyCount = companyQuery.Count,
                AssociatedCompanyNewThisMonth = companyQuery.Count(c => c.CreationTime >= monthStart)
            };

            if (!AbpSession.TenantId.HasValue)
            {
                var tenants = await _tenantManager.Tenants.ToListAsync();
                dto.TenantCount = tenants.Count;
                dto.ActiveTenantCount = tenants.Count(t => t.IsActive);
            }

            return dto;
        }

        /// <summary>
        /// 获取操作趋势数据（近30天）
        /// </summary>
        [UnitOfWork(false)]
        public async Task<List<TrendItemDto>> GetOperationTrendAsync()
        {
            var startDate = Clock.Now.AddDays(-30);
            var auditLogs = await _auditLogRepository.GetAll()
                .Where(a => a.ExecutionTime >= startDate)
                .GroupBy(a => a.ExecutionTime.Date)
                .Select(g => new TrendItemDto
                {
                    Date = g.Key.ToString("MM-dd"),
                    Count = g.LongCount()
                })
                .ToListAsync();

            var result = new List<TrendItemDto>();
            for (int i = 29; i >= 0; i--)
            {
                var date = Clock.Now.AddDays(-i).Date;
                var dateStr = date.ToString("MM-dd");
                var item = auditLogs.FirstOrDefault(a => a.Date == dateStr);
                result.Add(new TrendItemDto
                {
                    Date = dateStr,
                    Count = item?.Count ?? 0
                });
            }

            return result;
        }

        /// <summary>
        /// 获取月度统计数据（近12个月）
        /// </summary>
        [UnitOfWork(false)]
        public async Task<List<TrendItemDto>> GetMonthlyStatAsync()
        {
            var now = Clock.Now;
            var startMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-11);

            var auditLogs = await _auditLogRepository.GetAll()
                .Where(a => a.ExecutionTime >= startMonth)
                .GroupBy(a => new { a.ExecutionTime.Year, a.ExecutionTime.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.LongCount()
                })
                .ToListAsync();

            var result = new List<TrendItemDto>();
            for (int i = 11; i >= 0; i--)
            {
                var d = new DateTime(now.Year, now.Month, 1).AddMonths(-i);
                var item = auditLogs.FirstOrDefault(a => a.Year == d.Year && a.Month == d.Month);
                result.Add(new TrendItemDto
                {
                    Date = d.ToString("yyyy-MM"),
                    Count = item?.Count ?? 0
                });
            }

            return result;
        }

        /// <summary>
        /// 获取操作类型分布数据
        /// </summary>
        [UnitOfWork(false)]
        public async Task<List<OperationTypeDto>> GetOperationTypeDistributionAsync()
        {
            var startDate = Clock.Now.AddDays(-30);
            var result = await _auditLogRepository.GetAll()
                .Where(a => a.ExecutionTime >= startDate)
                .GroupBy(a => a.ServiceName)
                .Select(g => new OperationTypeDto
                {
                    Name = g.Key ?? "未知",
                    Count = g.LongCount()
                })
                .OrderByDescending(a => a.Count)
                .Take(10)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 获取活跃用户排行数据
        /// </summary>
        [UnitOfWork(false)]
        public async Task<List<ActiveUserDto>> GetActiveUsersAsync()
        {
            var startDate = Clock.Now.AddDays(-30);
            var auditLogs = await _auditLogRepository.GetAll()
                .Where(a => a.ExecutionTime >= startDate && a.UserId != null)
                .GroupBy(a => a.UserId)
                .Select(g => new { UserId = g.Key, Count = g.LongCount() })
                .OrderByDescending(a => a.Count)
                .Take(10)
                .ToListAsync();

            var userIds = auditLogs.Select(a => a.UserId.Value).ToList();
            var users = await _userRepository.GetAll()
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.UserName })
                .ToListAsync();

            var result = auditLogs.Select(a => new ActiveUserDto
            {
                UserName = users.FirstOrDefault(u => u.Id == a.UserId.Value)?.UserName ?? "未知用户",
                Count = a.Count
            }).ToList();

            return result;
        }

        /// <summary>
        /// 获取待办事项列表
        /// </summary>
        public async Task<List<TodoItemDto>> GetTodoItemsAsync()
        {
            var result = new List<TodoItemDto>();
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取最近操作动态
        /// </summary>
        [UnitOfWork(false)]
        public async Task<List<RecentActivityDto>> GetRecentActivityAsync()
        {
            var auditLogs = await _auditLogRepository.GetAll()
                .Where(a => a.UserId != null)
                .OrderByDescending(a => a.ExecutionTime)
                .Take(10)
                .Select(a => new
                {
                    a.UserId,
                    a.ServiceName,
                    a.MethodName,
                    a.ExecutionTime
                })
                .ToListAsync();

            var userIds = auditLogs.Select(a => a.UserId.Value).Distinct().ToList();
            var users = await _userRepository.GetAll()
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.UserName })
                .ToListAsync();

            var result = auditLogs.Select(a => new RecentActivityDto
            {
                UserName = users.FirstOrDefault(u => u.Id == a.UserId.Value)?.UserName ?? "未知用户",
                Content = (a.ServiceName ?? "") + "." + (a.MethodName ?? ""),
                ExecutionTime = a.ExecutionTime
            }).ToList();

            return result;
        }
    }
}
