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

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 工单后台管理应用服务
    /// </summary>
    /// <typeparam name="TEntityDto">列表页显示模型</typeparam>
    /// <typeparam name="TGetAllInput">列表页查询时的输入模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入模型</typeparam>
    /// <typeparam name="TGetInput">获取单个信息的输入模型</typeparam>
    /// <typeparam name="TBatchDeleteInput">批量删除的输入模型</typeparam>
    /// <typeparam name="TBatchDeleteOutput">批量删除时的输出模型</typeparam>
    /// <typeparam name="TBatchChangeStatusInput">批量状态修改时的输入模型</typeparam>
    /// <typeparam name="TBatchChangeStatusOutput">批量状态修改时的输出模型</typeparam>
    /// <typeparam name="TBatchAllocateInput">批量分配时的输入模型</typeparam>
    /// <typeparam name="TBatchAllocateOutput">批量分配时的输出模型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TRepository">实体仓储类型</typeparam>
    /// <typeparam name="TCategoryRepository">分类仓储</typeparam>
    /// <typeparam name="TManager">领域服务类型</typeparam>
    public abstract class WorkOrderAppService<TCreateInput,
                                              TUpdateInput,
                                              TBatchDeleteInput,
                                              TBatchDeleteOutput,
                                              TGetInput,
                                              TGetAllInput,
                                              TEntityDto,
                                              TBatchChangeStatusInput,
                                              TBatchChangeStatusOutput,
                                              TBatchAllocateInput,
                                              TBatchAllocateOutput,
                                              TEntity,
                                              TRepository,
                                              TManager,
                                              TCategoryRepository> : AppServiceBase, IWorkOrderAppService<TCreateInput,
                                                                                                          TUpdateInput,
                                                                                                          TBatchDeleteInput,
                                                                                                          TBatchDeleteOutput,
                                                                                                          TGetInput,
                                                                                                          TGetAllInput,
                                                                                                          TEntityDto,
                                                                                                          TBatchChangeStatusInput,
                                                                                                          TBatchChangeStatusOutput,
                                                                                                          TBatchAllocateInput,
                                                                                                          TBatchAllocateOutput>
        #region 泛型约束
        where TCreateInput : CreateInput
        where TUpdateInput : UpdateInput
        where TBatchDeleteInput : BatchOperationInputLong
        where TBatchDeleteOutput : BatchOperationOutputLong, new()
        where TGetInput : EntityDto<long>
        where TGetAllInput : GetAllInput
        where TEntityDto : WorkOrderDto, new()
        where TBatchChangeStatusInput : BatchChangeStatusInput
        where TBatchChangeStatusOutput : BatchChangeStatusOutput, new()
        where TBatchAllocateInput : BatchAllocateInput
        where TBatchAllocateOutput : BatchAllocateOutput, new()
        where TEntity : OrderBaseEntity
        where TRepository : IRepository<TEntity, long>
        where TManager : OrderBaseManager<TEntity>
        where TCategoryRepository : IRepository<CategoryEntity, long>
        #endregion
    {
        protected readonly TRepository repository;
        protected readonly TCategoryRepository categoryRepository;
        protected readonly TManager manager;
        protected readonly IEmployeeAppService employeeAppService;

        public WorkOrderAppService(TRepository repository,
                                   TManager manager,
                                   TCategoryRepository categoryRepository,
                                   IEmployeeAppService employeeAppService)
        {
            this.repository = repository;
            this.manager = manager;
            this.categoryRepository = categoryRepository;
            this.employeeAppService = employeeAppService;
        }
        /// <summary>
        /// 新增工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            var entity = await manager.CreateAsync(input.CategoryId,
                                                   input.UrgencyDegree,
                                                   input.Title,
                                                   input.Description,
                                                   input.EstimatedExecutionTime,
                                                   input.EstimatedCompletionTime,
                                                   input.ExtendedField1,
                                                   input.ExtendedField2,
                                                   input.ExtendedField3,
                                                   input.ExtendedField4,
                                                   input.ExtendedField5);

            if (input.ExtensionData != null)
            {
                foreach (var item in input.ExtensionData)
                {
                    entity.SetData(item.Key, item.Value);
                }
            }
            Skip(entity, input as TUpdateInput);
            await CurrentUnitOfWork.SaveChangesAsync();
            var category = await categoryRepository.GetAsync(input.CategoryId);
            IEnumerable<EmployeeDto> emps = null;
            if (!entity.EmployeeId.IsNullOrWhiteSpace())
            {
                emps = await employeeAppService.GetByIdsAsync(entity.EmployeeId);
            }
            return EntityToDto(entity, new CategoryEntity[] { category }, emps);
        }
        /// <summary>
        /// 修改工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            var entity = await repository.GetAsync(input.Id);
            entity.CategoryId = input.CategoryId;
            entity.ChangeEstimatedTime(input.EstimatedExecutionTime, input.EstimatedCompletionTime);
            entity.ChangePracticalTime(input.ExecutionTime, input.CompletionTime);
            entity.Description = input.Description;
            entity.Title = input.Title;
            entity.Description = input.Description;
            entity.UrgencyDegree = input.UrgencyDegree;
            entity.ExtendedField1 = input.ExtendedField1;
            entity.ExtendedField2 = input.ExtendedField2;
            entity.ExtendedField3 = input.ExtendedField3;
            entity.ExtendedField4 = input.ExtendedField4;
            entity.ExtendedField5 = input.ExtendedField5;
            if (input.ExtensionData != null)
            {
                foreach (var item in input.ExtensionData)
                {
                    entity.SetData(item.Key, item.Value);
                }
            }
            if (input.Status > entity.Status)
            {
                Skip(entity, input);
            }
            else if (input.Status < entity.Status)
            {
                entity.BackOff(Clock.Now, input.Status);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            var category = await categoryRepository.GetAsync(input.CategoryId);
            IEnumerable<EmployeeDto> emps = null;
            if (!entity.EmployeeId.IsNullOrWhiteSpace())
            {
                emps = await employeeAppService.GetByIdsAsync(entity.EmployeeId);
            }
            return EntityToDto(entity, new CategoryEntity[] { category }, emps);
        }
        /// <summary>
        /// 删除工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchDeleteOutput> DeleteAsync(TBatchDeleteInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchDeleteOutput();
            foreach (var item in list)
            {
                try
                {
                    await manager.DeleteAsync(item);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (Exception ex)
                {
                    Logger.Warn(L("删除工单失败！"), ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 获取指定工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> GetAsync(TGetInput input)
        {
            var entity = await repository.GetAsync(input.Id);
            var category = await categoryRepository.GetAsync(entity.CategoryId);
            IEnumerable<EmployeeDto> emps = null;
            if (!entity.EmployeeId.IsNullOrWhiteSpace())
            {
                emps = await employeeAppService.GetByIdsAsync(entity.EmployeeId);
            }
            return EntityToDto(entity, new CategoryEntity[] { category }, emps);
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
            var query = repository.GetAll();

            //IEnumerable<CategoryEntity> categoryEntities;
            if (!input.CategoryCode.IsNullOrWhiteSpace())
            {
                var clsIdsQuery = categoryRepository.GetAll().Where(c => c.Code.StartsWith(input.CategoryCode)).Select(c => c.Id);
                var clsIds = await AsyncQueryableExecuter.ToListAsync(clsIdsQuery);
                query = query.Where(c => clsIds.Contains(c.CategoryId));
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

            var empIds = list.Select(c => c.EmployeeId);

            IEnumerable<EmployeeDto> emps = null;
            if (empIds != null && empIds.Count() > 0)
            {
                emps = await employeeAppService.GetByIdsAsync(empIds.ToArray());
            }

            return new PagedResultDto<TEntityDto>(count, list.Select(c => EntityToDto(c, cls, emps)).ToList());
        }
        /// <summary>
        /// 批量分配工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchChangeStatusOutput> ConfirmeAsync(TBatchChangeStatusInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchChangeStatusOutput();
            foreach (var item in list)
            {
                try
                {
                    item.Confirme(Clock.Now);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (Exception ex)
                {
                    Logger.Warn("确认工单失败！", ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 批量分配工单
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
                    item.Allocate(Clock.Now, input.EmployeeId, input.Start, input.End);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (Exception ex)
                {
                    Logger.Warn("分配工单失败！", ex);
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
                    Logger.Warn("执行工单失败！", ex);
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
                    Logger.Warn("完成工单失败！", ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 批量拒绝工单
        /// </summary>
        /// <param name="input">包含id集合和拒绝原因</param>
        /// <returns></returns>
        public virtual async Task<TBatchChangeStatusOutput> RejectAsync(TBatchChangeStatusInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchChangeStatusOutput();
            foreach (var item in list)
            {
                try
                {
                    item.Reject(Clock.Now, input.Description);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (Exception ex)
                {
                    Logger.Warn("拒绝工单失败！", ex);
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
        /// <returns></returns>
        protected virtual TEntityDto EntityToDto(TEntity entity, IEnumerable<CategoryEntity> categories, IEnumerable<EmployeeDto> employees)
        {
            var dto = new TEntityDto();
            dto.CategoryId = entity.CategoryId;
            if (employees != null)
            {
                dto.CategoryDisplayName = categories.SingleOrDefault(c => c.Id == entity.CategoryId)?.DisplayName;
            }
            dto.CompletionTime = entity.CompletionTime;
            dto.CreationTime = entity.CreationTime;
            dto.CreatorUserId = entity.CreatorUserId;
            dto.DeleterUserId = entity.DeleterUserId;
            dto.DeletionTime = entity.DeletionTime;
            dto.Description = entity.Description;
            dto.EmployeeId = entity.EmployeeId;
            if (employees != null)
            {
                var emp = employees.SingleOrDefault(c => c.Id == entity.EmployeeId);
                dto.EmployeeName = emp?.Name;
                dto.EmployeePhone = emp.Phone;
            }
            dto.EstimatedCompletionTime = entity.EstimatedCompletionTime;
            dto.EstimatedExecutionTime = entity.EstimatedExecutionTime;
            dto.ExecutionTime = entity.ExecutionTime;
            dto.ExtendedField1 = entity.ExtendedField1;
            dto.ExtendedField2 = entity.ExtendedField2;
            dto.ExtendedField3 = entity.ExtendedField3;
            dto.ExtendedField4 = entity.ExtendedField4;
            dto.ExtendedField5 = entity.ExtendedField5;
            if (!entity.ExtensionData.IsNullOrWhiteSpace())
            {
                dto.ExtensionData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);
            }
            dto.Id = entity.Id;
            dto.IsDeleted = entity.IsDeleted;
            dto.LastModificationTime = entity.LastModificationTime;
            dto.LastModifierUserId = entity.LastModifierUserId;
            dto.Status = entity.Status;
            dto.StatusChangedDescription = entity.StatusChangedDescription;
            dto.StatusChangedTime = entity.StatusChangedTime;
            dto.Title = entity.Title;
            dto.UrgencyDegree = entity.UrgencyDegree;
            return dto;
        }
        /// <summary>
        /// 根据用户提交数据处理订单<br />
        /// 一次性执行多个工单操作
        /// </summary>
        /// <param name="entity">工单</param>
        /// <param name="input">用户提交的数据</param>
        protected virtual void Skip(TEntity entity, TUpdateInput input)
        {
            if (input.Status > Status.ToBeConfirmed)
                entity.Confirme(Clock.Now);
            if (input.Status > Status.ToBeAllocated)
                entity.Allocate(Clock.Now, input.EmployeeId, input.EstimatedExecutionTime, input.EstimatedCompletionTime);
            if (input.Status > Status.ToBeProcessed)
                entity.Execute(Clock.Now);
            if (input.Status > Status.Processing)
            {
                if (input.Status == Status.Rejected)
                {
                    entity.Reject(Clock.Now, input.Description);
                }
                else
                {
                    entity.Completion(Clock.Now, input.Description);
                }
            }
        }
    }


    /// <summary>
    /// 默认工单应用服务接口
    /// </summary>
    public class WorkOrderAppService : WorkOrderAppService<CreateInput,
                                                           UpdateInput,
                                                           BatchOperationInputLong,
                                                           BatchOperationOutputLong,
                                                           EntityDto<long>,
                                                           GetAllInput,
                                                           WorkOrderDto,
                                                           BatchChangeStatusInput,
                                                           BatchChangeStatusOutput,
                                                           BatchAllocateInput,
                                                           BatchAllocateOutput,
                                                           OrderEntity,
                                                           IRepository<OrderEntity, long>,
                                                           OrderManager,
                                                           IRepository<CategoryEntity, long>>, IWorkOrderAppService

    {
        public WorkOrderAppService(IRepository<OrderEntity, long> repository,
                                   OrderManager manager,
                                   IRepository<CategoryEntity, long> categoryRepository,
                                   IEmployeeAppService employeeAppService) : base(repository,
                                                                                  manager,
                                                                                  categoryRepository,
                                                                                  employeeAppService)
        {
        }
    }
}
