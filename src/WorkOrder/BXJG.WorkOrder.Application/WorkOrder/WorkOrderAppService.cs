using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
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
    public class WorkOrderAppService<TEntityDto,
                                     TGetAllInput,
                                     TCreateInput,
                                     TUpdateInput,
                                     TGetInput,
                                     TBatchDeleteInput,
                                     TBatchDeleteOutput,
                                     TBatchChangeStatusInput,
                                     TBatchChangeStatusOutput,
                                     TBatchAllocateInput,
                                     TBatchAllocateOutput,
                                     TEntity,
                                     TRepository,
                                     TCategoryRepository,
                                     TManager> : AppServiceBase, IWorkOrderAppService<TEntityDto,
                                                                                      TGetAllInput,
                                                                                      TCreateInput,
                                                                                      TUpdateInput,
                                                                                      TGetInput,
                                                                                      TBatchDeleteInput,
                                                                                      TBatchDeleteOutput,
                                                                                      TBatchChangeStatusInput,
                                                                                      TBatchChangeStatusOutput,
                                                                                      TBatchAllocateInput,
                                                                                      TBatchAllocateOutput>

        where TEntityDto : OrderDto, new()
        where TCreateInput : CreateInput
        where TUpdateInput : UpdateInput
        where TGetInput : GetInput
        where TGetAllInput : GetAllInput
        where TBatchDeleteInput : BatchOperationInput
        where TBatchDeleteOutput : BatchOperationResult
        where TBatchChangeStatusInput : BatchChangeStatusInput
        where TBatchChangeStatusOutput : BatchChangeStatusOutput, new()
        where TBatchAllocateInput : BatchAllocateInput
        where TBatchAllocateOutput : BatchAllocateOutput, new()
        where TEntity : OrderBaseEntity
        where TRepository : IRepository<TEntity, long>
        where TCategoryRepository : IRepository<CategoryEntity, long>
        where TManager : OrderBaseManager<TEntity>
    {
        protected readonly TRepository repository;
        protected readonly TCategoryRepository categoryRepository;
        protected readonly TManager manager;
        protected readonly IEmployeeAppService employeeAppService;

        public WorkOrderAppService(TRepository repository, TManager manager, TCategoryRepository categoryRepository, IEmployeeAppService employeeAppService)
        {
            this.repository = repository;
            this.manager = manager;
            this.categoryRepository = categoryRepository;
            this.employeeAppService = employeeAppService;
        }

        public virtual async Task<TBatchAllocateOutput> AllocateAsync(TBatchAllocateInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            list.ForEach(c => c.Allocate(Clock.Now, input.EmployeeId, input.Start, input.End));
            await CurrentUnitOfWork.SaveChangesAsync();
            return new TBatchAllocateOutput();
        }

        public virtual async Task<TBatchChangeStatusOutput> CompletionAsync(TBatchChangeStatusInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            list.ForEach(c => c.Completion(Clock.Now, input.Description));
            await CurrentUnitOfWork.SaveChangesAsync();
            return new TBatchChangeStatusOutput();
        }

        public virtual async Task<TBatchChangeStatusOutput> ConfirmeAsync(TBatchChangeStatusInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            list.ForEach(c => c.Confirme(Clock.Now));
            await CurrentUnitOfWork.SaveChangesAsync();
            return new TBatchChangeStatusOutput();
        }

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
            await repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return null;
        }

        public virtual async Task<TBatchDeleteOutput> DeleteAsync(TBatchDeleteInput input)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TBatchChangeStatusOutput> ExecuteAsync(TBatchChangeStatusInput input)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TEntityDto> GetAsync(TGetInput input)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<TBatchChangeStatusOutput> RejectAsync(TBatchChangeStatusInput input)
        {
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            list.ForEach(c => c.Reject(Clock.Now, input.Description));
            await CurrentUnitOfWork.SaveChangesAsync();
            return new TBatchChangeStatusOutput();
        }

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
            var category = await categoryRepository.GetAsync(input.CategoryId);
            var emp = await employeeAppService.GetByIdsAsync(input.EmployeeId);
            return EntityToDto(entity, new CategoryEntity[] { category }, emp);
        }

        public virtual TEntityDto EntityToDto(TEntity entity, IEnumerable<CategoryEntity> categories, IEnumerable<Employee.EmployeeDto> employees)
        {
            var dto = new TEntityDto();
            dto.CategoryDisplayName = categories.Single(c => c.Id == entity.CategoryId).DisplayName;
            dto.CategoryId = entity.CategoryId;
            dto.CompletionTime = entity.CompletionTime;
            dto.CreationTime = entity.CreationTime;
            dto.CreatorUserId = entity.CreatorUserId;
            dto.DeleterUserId = entity.DeleterUserId;
            dto.DeletionTime = entity.DeletionTime;
            dto.Description = entity.Description;
            dto.EmployeeId = entity.EmployeeId;
            var emp = employees.SingleOrDefault(c => c.Id == entity.EmployeeId);
            dto.EmployeeName = emp?.Name;
            dto.EmployeePhone = emp.Phone;
            dto.EstimatedCompletionTime = entity.EstimatedCompletionTime;
            dto.EstimatedExecutionTime = entity.EstimatedExecutionTime;
            dto.ExecutionTime = entity.ExecutionTime;
            dto.ExtendedField1 = entity.ExtendedField1;
            dto.ExtendedField2 = entity.ExtendedField2;
            dto.ExtendedField3 = entity.ExtendedField3;
            dto.ExtendedField4 = entity.ExtendedField4;
            dto.ExtendedField5 = entity.ExtendedField5;
            dto.ExtensionData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);
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
    }
}
