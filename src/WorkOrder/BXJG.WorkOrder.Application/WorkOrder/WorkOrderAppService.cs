using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Timing;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
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
        where TManager : OrderBaseManager<TEntity>
    {
        protected readonly TRepository repository;
        protected readonly TManager manager;

        public WorkOrderAppService(TRepository repository, TManager manager)
        {
            this.repository = repository;
            this.manager = manager;
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
            throw new NotImplementedException();
        }

        public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            throw new NotImplementedException();
        }

        public virtual TEntityDto EntityToDto(TEntity entity, IEnumerable<WorkOrderCategory.CategoryEntity> categories, IEnumerable<Employee.EmployeeDto> employees)
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
