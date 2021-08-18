using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.EntityHistory;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Application.Services.Dto;
using Abp.Linq;
using Abp.Authorization;
using System.ComponentModel.DataAnnotations;

namespace BXJG.Utils.OperationLog
{
    /// <summary>
    /// 操作日志dto
    /// </summary>
    public class Dto
    {
        /// <summary>
        /// 操作时的浏览器信息
        /// </summary>
        public virtual string BrowserInfo { get; set; }
        /// <summary>
        /// 客户端id
        /// </summary>
        public virtual string ClientIpAddress { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public virtual string ClientName { get; set; }
        /// <summary>
        /// 操作员id
        /// </summary>
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 被操作的实体类型
        /// </summary>
        public virtual string EntityTypeFullName { get; set; }
        /// <summary>
        /// 被操作的实体的id
        /// </summary>
        public virtual string EntityId { get; set; }
        ///// <summary>
        ///// 工单处理人
        ///// </summary>
        //public string EmployeeId { get; set; }
        ///// <summary>
        ///// 处理人姓名
        ///// </summary>
        //public string EmployeeName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTimeOffset OperationTime { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public virtual string Reason { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public virtual dynamic ExtensionData { get; set; }
        /// <summary>
        /// 被改过的字段
        /// </summary>
        public virtual List<FieldDto> Fields { get; set; }
    }
    /// <summary>
    /// 操作日志字段dto
    /// </summary>
    public class FieldDto
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public virtual string FileldName { get; set; }
        /// <summary>
        /// 字段显示名
        /// </summary>
        public virtual string FieldDisplayName { get; set; }
        /// <summary>
        /// 修改前的值
        /// </summary>
        public virtual string NewValue { get; set; }
        /// <summary>
        /// 修改后的值
        /// </summary>
        public virtual string OriginalValue { get; set; }
    }
    public class GetAllInput
    {
        [Required]
        public string EntityId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public string Sorting { get; set; }
    }

    //目前只是站在实体的角度
    //可以类似的设计站在操作员的角度
    public class OperationLogAppService : ApplicationService
    {
        protected IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        protected readonly IRepository<EntityChange, long> repository;

        protected string permissionName;

        public OperationLogAppService(IRepository<EntityChange, long> repository, string permissionName = default)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<long> GetTotalAsync(GetAllInput input)
        {
            await CheckPermissionAsync();
            var query = await CreateFilterAsync(input);
            return await AsyncQueryableExecuter.CountAsync(query);
        }

        public async Task<PagedResultDto<Dto>> GetAllAsync(GetAllInput input)
        {
            await CheckPermissionAsync();

            var query = await CreateFilterAsync(input);
            var totle = await AsyncQueryableExecuter.CountAsync(query);
            query = this.Sort(query, input);
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var dots = new List<Dto>();
            foreach (var item in list)
            {
                dots.Add(await Map2DtoAsync(item));
            }
            return new PagedResultDto<Dto>(totle, dots);
        }

        protected virtual async ValueTask CheckPermissionAsync()
        {
            if (!string.IsNullOrEmpty(permissionName))
                await PermissionChecker.AuthorizeAsync(permissionName);
        }

        protected virtual ValueTask<IQueryable<EntityChange>> CreateFilterAsync(GetAllInput input)
        {
            return ValueTask.FromResult(repository.GetAll());
        }

        protected virtual IOrderedQueryable<EntityChange> Sort(IQueryable<EntityChange> query, GetAllInput input)
        {
            IOrderedQueryable<EntityChange> q;
            if (!input.Sorting.IsNullOrWhiteSpace())
                q = query.OrderBy(input.Sorting);
            else
                q = query.OrderByDescending(c => c.ChangeTime);
            return q;
        }

        protected virtual ValueTask MapBeforeAsync(IList<EntityChange> entityChanges)
        {
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask<Dto> Map2DtoAsync(EntityChange entityChange)
        {
            return ValueTask.FromResult(base.ObjectMapper.Map<Dto>(entityChange));
        }
    }
}
