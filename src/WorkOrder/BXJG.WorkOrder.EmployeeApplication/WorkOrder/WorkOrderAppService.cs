using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Timing;
using BXJG.Common.Dto;
using BXJG.GeneralTree;
using BXJG.WorkOrder.Employee;
using BXJG.WorkOrder.WorkOrderCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Domain.Entities;
using BXJG.WorkOrder.Session;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 员工管理工单应用服务基类
    /// </summary>
    /// <typeparam name="TGetInput">获取单个信息的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">列表页查询时的输入模型</typeparam>
    /// <typeparam name="TEntityDto">列表页显示模型</typeparam>
    /// <typeparam name="TBatchChangeStatusInput">批量状态修改时的输入模型</typeparam>
    /// <typeparam name="TBatchChangeStatusOutput">批量状态修改时的输出模型</typeparam>
    /// <typeparam name="TBatchAllocateInput">批量分配时的输入模型</typeparam>
    /// <typeparam name="TBatchAllocateOutput">批量分配时的输出模型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TRepository">实体仓储类型</typeparam>
    /// <typeparam name="TManager">领域服务类型</typeparam>
    /// <typeparam name="TCategoryRepository">分类仓储</typeparam>
    public abstract class WorkOrderEmployeeAppServiceBase<TGetInput,
                                                          TGetAllInput,
                                                          TEntityDto,
                                                          TBatchChangeStatusInput,
                                                          TBatchChangeStatusOutput,
                                                          TBatchAllocateInput,
                                                          TBatchAllocateOutput,
                                                          TEntity,
                                                          TRepository,
                                                          TCategoryRepository> : EmployeeAppServiceBase
        #region 泛型约束
        where TGetInput : EntityDto<long>
        where TGetAllInput : GetAllWorkOrderEmployeeInputBase
        where TEntityDto : WorkOrderEmployeeDtoBase, new()
        where TBatchChangeStatusInput : WorkOrderEmployeeBatchChangeStatusInputBase
        where TBatchChangeStatusOutput : WorkOrderEmployeeBatchChangeStatusOutputBase, new()
        where TBatchAllocateInput : WorkOrderEmployeeBatchAllocateInputBase
        where TBatchAllocateOutput : WorkOrderEmployeeBatchAllocateOutput, new()
        where TEntity : OrderBaseEntity
        where TRepository : IRepository<TEntity, long>
        where TCategoryRepository : IRepository<CategoryEntity, long>
        #endregion
    {
        protected readonly TRepository repository;
        protected readonly TCategoryRepository categoryRepository;
        protected readonly IEmployeeAppService employeeAppService;

        public WorkOrderEmployeeAppServiceBase(TRepository repository,
                                               TCategoryRepository categoryRepository,
                                               IEmployeeAppService employeeAppService,
                                               IEmployeeSession employeeSession) : base(employeeSession)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.employeeAppService = employeeAppService;
        }
        /// <summary>
        /// 获取指定工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> GetAsync(TGetInput input)
        {
            var entity = await repository.GetAsync(input.Id);
            return await EntityToDto(entity);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            //分类、员工先查询 再用in，
            //假定员工和分类数量不会太多（太多的话考虑分配in查询），且可以使用缓存
            //in查询有索引时性能有所提升
            var query = from c in repository.GetAll()
                        join lb in categoryRepository.GetAll() on c.CategoryId equals lb.Id into g
                        from kk in g.DefaultIfEmpty()
                        where (input.CategoryCode.IsNullOrWhiteSpace() || kk.Code.StartsWith(input.CategoryCode))
                        select c;

            if (!input.Keyword.IsNullOrWhiteSpace())
            {
                var empIdsQuery = await employeeAppService.GetIdsByKeywordAsync(input.Keyword);
                query = query.Where(c => empIdsQuery.Contains(c.EmployeeId) || c.Title.Contains(input.Keyword));
            }
            if (!input.Keyword.IsNullOrWhiteSpace())
            {
                var empIdsQuery = await employeeAppService.GetIdsByKeywordAsync(input.Keyword);
                query = query.Where(c => empIdsQuery.Contains(c.EmployeeId) || c.Title.Contains(input.Keyword));
            }
            query = query.WhereIf(input.UrgencyDegree.HasValue, c => c.UrgencyDegree == input.UrgencyDegree)
                         .WhereIf(input.EstimatedExecutionTimeStart.HasValue, c => c.EstimatedExecutionTime >= input.EstimatedExecutionTimeStart)
                         .WhereIf(input.EstimatedExecutionTimeEnd.HasValue, c => c.EstimatedExecutionTime < input.EstimatedExecutionTimeEnd)
                         .WhereIf(input.EstimatedCompletionTimeStart.HasValue, c => c.EstimatedCompletionTime >= input.EstimatedCompletionTimeStart)
                         .WhereIf(input.EstimatedCompletionTimeEnd.HasValue, c => c.EstimatedCompletionTime < input.EstimatedCompletionTimeEnd)
                         .WhereIf(input.ExecutionTimeStart.HasValue, c => c.ExecutionTime >= input.ExecutionTimeStart)
                         .WhereIf(input.ExecutionTimeEnd.HasValue, c => c.ExecutionTime < input.ExecutionTimeEnd)
                         .WhereIf(input.CompletionTimeStart.HasValue, c => c.CompletionTime >= input.CompletionTimeStart)
                         .WhereIf(input.CompletionTimeEnd.HasValue, c => c.CompletionTime < input.CompletionTimeEnd);

            var count = await AsyncQueryableExecuter.CountAsync(query);
            query = query.OrderBy(input.Sorting).PageBy(input);
            var list = await AsyncQueryableExecuter.ToListAsync(query);

            var cIds = list.Select(c => c.CategoryId);
            var cQuery = categoryRepository.GetAll().Where(c => cIds.Contains(c.Id));
            var cls = await AsyncQueryableExecuter.ToListAsync(cQuery);

            var empIds = list.Where(c => !c.EmployeeId.IsNullOrWhiteSpace()).Select(c => c.EmployeeId);

            IEnumerable<EmployeeDto> emps = null;
            if (empIds != null && empIds.Count() > 0)
            {
                emps = await employeeAppService.GetByIdsAsync(empIds.ToArray());
            }
            var state = await GetStateAsync(list.ToArray());
            var items = new List<TEntityDto>();
            foreach (var item in list)
            {
                var ttt = EntityToDto(item, cls, emps, state);
                items.Add(ttt);
            }
            return new PagedResultDto<TEntityDto>(count, items);
        }
        /// <summary>
        /// 批量领取工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchAllocateOutput> AllocateAsync(TBatchAllocateInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchAllocateOutput();
            foreach (var item in list)
            {
                try
                {
                    item.Allocate(Clock.Now, employeeSession.CurrentEmployeeId, input.Start, input.End);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (Exception ex)
                {
                    Logger.Warn(L("分配工单失败！"), ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 批量执行工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchChangeStatusOutput> ExecuteAsync(TBatchChangeStatusInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchChangeStatusOutput();
            foreach (var item in list)
            {
                try
                {
                    item.Execute(Clock.Now);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (Exception ex)
                {
                    Logger.Warn(L("执行工单失败！"), ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 批量完成工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchChangeStatusOutput> CompletionAsync(TBatchChangeStatusInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchChangeStatusOutput();
            foreach (var item in list)
            {
                try
                {
                    item.Completion(Clock.Now, input.Description);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (Exception ex)
                {
                    Logger.Warn(L("完成工单失败！"), ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 实体映射到dto
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="categories"></param>
        /// <param name="employees"></param>
        /// <param name="state">子类可能需要聚合更多外键</param>
        /// <returns></returns>
        protected virtual TEntityDto EntityToDto(TEntity entity, IEnumerable<CategoryEntity> categories, IEnumerable<EmployeeDto> employees, object state = null)
        {
            var dto = new TEntityDto();
            dto.CategoryId = entity.CategoryId;
            if (categories != null)
            {
                dto.CategoryDisplayName = categories.SingleOrDefault(c => c.Id == entity.CategoryId)?.DisplayName;
            }
            dto.CompletionTime = entity.CompletionTime;
            dto.Description = entity.Description;
            dto.EmployeeId = entity.EmployeeId;
            if (employees != null)
            {
                var emp = employees.SingleOrDefault(c => c.Id == entity.EmployeeId);
                dto.EmployeeName = emp?.Name;
                dto.EmployeePhone = emp?.Phone;
            }
            dto.EstimatedCompletionTime = entity.EstimatedCompletionTime;
            dto.EstimatedExecutionTime = entity.EstimatedExecutionTime;
            dto.ExecutionTime = entity.ExecutionTime;

            dto.Id = entity.Id;
            dto.Status = entity.Status;
            dto.StatusChangedDescription = entity.StatusChangedDescription;
            dto.StatusChangedTime = entity.StatusChangedTime;
            dto.Title = entity.Title;
            dto.UrgencyDegree = entity.UrgencyDegree;
            return dto;
        }
        /// <summary>
        /// 将单个实体映射为dto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async Task<TEntityDto> EntityToDto(TEntity entity)
        {
            var category = await categoryRepository.GetAsync(entity.CategoryId);
            IEnumerable<EmployeeDto> emps = null;
            if (!entity.EmployeeId.IsNullOrWhiteSpace())
            {
                emps = await employeeAppService.GetByIdsAsync(entity.EmployeeId);
            }
            var state = await GetStateAsync(entity);
            return EntityToDto(entity, new CategoryEntity[] { category }, emps, state);
        }
        /// <summary>
        /// 子类可能需要聚合更多外键
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async ValueTask<object> GetStateAsync(params TEntity[] entity)
        {
            return null;
        }
    }

    /// <summary>
    /// 后台管理默认工单应用服务接口
    /// </summary>
    public class WorkOrderEmployeeAppService : WorkOrderEmployeeAppServiceBase<
                                                               EntityDto<long>,
                                                               GetAllWorkOrderEmployeeInputBase,
                                                               WorkOrderEmployeeDto,
                                                               WorkOrderEmployeeBatchChangeStatusInputBase,
                                                               WorkOrderEmployeeBatchChangeStatusOutputBase,
                                                               WorkOrderEmployeeBatchAllocateInputBase,
                                                               WorkOrderEmployeeBatchAllocateOutput,
                                                               OrderEntity,
                                                               IRepository<OrderEntity, long>,
                                                               IRepository<CategoryEntity, long>>

    {
        public WorkOrderEmployeeAppService(IRepository<OrderEntity, long> repository, IRepository<CategoryEntity, long> categoryRepository, IEmployeeAppService employeeAppService, IEmployeeSession employeeSession) : base(repository, categoryRepository, employeeAppService, employeeSession)
        {
        }

        protected override WorkOrderEmployeeDto EntityToDto(OrderEntity entity, IEnumerable<CategoryEntity> categories, IEnumerable<EmployeeDto> employees, object state = default)
        {
            var dto = base.EntityToDto(entity, categories, employees, state);
            dto.ExtendedField1 = entity.ExtendedField1;
            dto.ExtendedField2 = entity.ExtendedField2;
            dto.ExtendedField3 = entity.ExtendedField3;
            dto.ExtendedField4 = entity.ExtendedField4;
            dto.ExtendedField5 = entity.ExtendedField5;
            if (!entity.ExtensionData.IsNullOrWhiteSpace())
            {
                dto.ExtensionData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);
            }
            return dto;
        }
    }
}
