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
using Abp.UI;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 工单后台管理应用服务基类
    /// </summary>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入模型</typeparam>
    /// <typeparam name="TBatchDeleteInput">批量删除的输入模型</typeparam>
    /// <typeparam name="TBatchDeleteOutput">批量删除时的输出模型</typeparam>
    /// <typeparam name="TGetInput">获取单个信息的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">列表页查询时的输入模型</typeparam>
    /// <typeparam name="TEntityDto">列表页显示模型</typeparam>
    /// <typeparam name="TBatchChangeStatusInput">批量状态修改时的输入模型</typeparam>
    /// <typeparam name="TBatchChangeStatusOutput">批量状态修改时的输出模型</typeparam>
    /// <typeparam name="TBatchAllocateInput">批量分配时的输入模型</typeparam>
    /// <typeparam name="TBatchAllocateOutput">批量分配时的输出模型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TRepository">实体仓储类型</typeparam>
    /// <typeparam name="TCreateDto"></typeparam>
    /// <typeparam name="TManager">领域服务类型</typeparam>
    /// <typeparam name="TCategoryRepository">分类仓储</typeparam>
    public abstract class WorkOrderAppServiceBase<TCreateInput,
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
                                                  TCreateDto,
                                                  TManager,
                                                  TCategoryRepository> : AppServiceBase
        #region 泛型约束
        where TCreateInput : TUpdateInput
        where TUpdateInput : WorkOrderUpdateBaseInput
        where TBatchDeleteInput : BatchOperationInputLong
        where TBatchDeleteOutput : BatchOperationOutputLong, new()
        where TGetInput : EntityDto<long>
        where TGetAllInput : GetAllWorkOrderBaseInput
        where TEntityDto : WorkOrderDtoBase, new()
        where TBatchChangeStatusInput : WorkOrderBatchChangeStatusInputBase
        where TBatchChangeStatusOutput : WorkOrderBatchChangeStatusOutputBase, new()
        where TBatchAllocateInput : WorkOrderBatchAllocateInputBase
        where TBatchAllocateOutput : WorkOrderBatchAllocateOutputBase, new()
        where TEntity : OrderBaseEntity
        where TRepository : IRepository<TEntity, long>
        where TCreateDto : WorkOrderCreateDtoBase, new()
        where TManager : OrderBaseManager<TEntity>
        where TCategoryRepository : IRepository<CategoryEntity, long>
        #endregion
    {
        protected readonly TRepository repository;
        protected readonly TCategoryRepository categoryRepository;
        protected readonly TManager manager;
        protected readonly IEmployeeAppService employeeAppService;
        protected readonly string getPermissionName,
                                  allocatePermissionName,
                                  executePermissionName,
                                  completionPermissionName,
                                  rejectPermissionName,
                                  createPermissionName,
                                  updatePermissionName,
                                  deletePermissionName,
                                  confirmePermissionName;

        public WorkOrderAppServiceBase(TRepository repository,
                                       TManager manager,
                                       TCategoryRepository categoryRepository,
                                       IEmployeeAppService employeeAppService,
                                       string createPermissionName = default,
                                       string updatePermissionName = default,
                                       string deletePermissionName = default,
                                       string getPermissionName = default,
                                       string confirmePermissionName = default,
                                       string allocatePermissionName = default,
                                       string executePermissionName = default,
                                       string completionPermissionName = default,
                                       string rejectPermissionName = default)
        {
            this.repository = repository;
            this.manager = manager;
            this.categoryRepository = categoryRepository;
            this.employeeAppService = employeeAppService;

            this.getPermissionName = getPermissionName;
            this.allocatePermissionName = allocatePermissionName;
            this.executePermissionName = executePermissionName;
            this.completionPermissionName = completionPermissionName;
            this.rejectPermissionName = rejectPermissionName;
            this.createPermissionName = createPermissionName;
            this.updatePermissionName = updatePermissionName;
            this.deletePermissionName = deletePermissionName;
            this.confirmePermissionName = confirmePermissionName;
        }
        /// <summary>
        /// 新增工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePermissionAsync();
            var entity = await manager.CreateAsync(await CreateInputToCreateDto(input));
            if (input.Status.HasValue && entity.Status < input.Status.Value)
            {
                //缺乏精细的权限控制
                entity.SkipRetain(Clock.Now,
                                  input.Status,
                                  input.StatusChangedDescription,
                                  //以下三个属性在CreateInputToCreateDto已复制
                                  //input.EmployeeId,
                                  //input.EstimatedExecutionTime,
                                  //input.EstimatedCompletionTime,
                                  excuteTime: input.ExecutionTime,
                                  completeTime: input.CompletionTime);
            }
            await BeforeCreateAsync(entity, input);
            await CurrentUnitOfWork.SaveChangesAsync();
            return await EntityToDto(entity);
        }
        /// <summary>
        /// 修改工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            await CheckUpdatePermissionAsync();
            var entity = await repository.GetAsync(input.Id);
            //需要先执行这里，让工单处于希望的状态，后续赋值才可正常执行
            if (input.Status.HasValue && entity.Status != input.Status.Value)
            {
                //缺乏精细的权限控制
                entity.ChangeStateRetain(Clock.Now,
                                         input.Status,
                                         input.StatusChangedDescription,
                                         input.EmployeeId,
                                         input.EstimatedExecutionTime,
                                         input.EstimatedCompletionTime,
                                         input.ExecutionTime,
                                         input.CompletionTime);
            }
            entity.CategoryId = input.CategoryId;
            entity.Description = input.Description;
            entity.Title = input.Title;
            entity.StatusChangedDescription = input.StatusChangedDescription;


            entity.SetUrgencyDegreeRetain(input.UrgencyDegree);//涉及到状态判断的操作，放ChangeStateRetain前后都不合适


            //因为上面能赋哪几个值是由状态确定的，我们这里有没判断，所以不知道是哪个状态，所以下面还需要赋值下
            entity.EmployeeId = input.EmployeeId;//涉及到状态判断的操作，放ChangeStateRetain前后都不合适
            entity.ChangeEstimatedTimeRetain(input.EstimatedExecutionTime, input.EstimatedCompletionTime);//涉及到状态判断的操作，放ChangeStateRetain前后都不合适


            await BeforeEditAsync(entity, input);
            await CurrentUnitOfWork.SaveChangesAsync();
            return await EntityToDto(entity);
        }
        /// <summary>
        /// 删除工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchDeleteOutput> DeleteAsync(TBatchDeleteInput input)
        {
            await CheckDeletePermissionAsync();
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
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
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
            await CheckGetPermissionAsync();
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
            //按分类名称排序 倒是可用用join映射 select new { 工单实体，join的分类 } 后面再排序
            //按处理人和手机号比较麻烦，可以尝试join已经查询出来的员工列表试试
            //不过至少可用按分类id和处理人id排序
            //如果都无法满足时，可以考虑使用原始sql，毕竟这里只是查询需求，不做业务处理，可以引入dapper或ef的原始sql执行方式
            await CheckGetPermissionAsync();
            var query = from c in repository.GetAll()
                        join lb in categoryRepository.GetAll() on c.CategoryId equals lb.Id into g
                        from kk in g.DefaultIfEmpty()
                        where (input.CategoryCode.IsNullOrWhiteSpace() || kk.Code.StartsWith(input.CategoryCode))
                        select c;

            if (!input.Keywords.IsNullOrWhiteSpace())
            {
                var empIdsQuery = await employeeAppService.GetIdsByKeywordAsync(input.Keywords);
                query = query.Where(c => empIdsQuery.Contains(c.EmployeeId) || c.Title.Contains(input.Keywords));
            }
            query = query.WhereIf(input.UrgencyDegree.HasValue, c => c.UrgencyDegree == input.UrgencyDegree)
                         .WhereIf(input.Status.HasValue, c => c.Status == input.Status)
                         //.WhereIf(!input.Keyword.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Keyword)) 上面已经写了
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
        /// 批量确认工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchChangeStatusOutput> ConfirmeAsync(TBatchChangeStatusInput input)
        {
            await CheckConfirmePermissionAsync();
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
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn(L("确认工单失败！"), ex);
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
            await CheckAllocatePermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchAllocateOutput();
            foreach (var item in list)
            {
                try
                {
                    item.AllocateRetain(Clock.Now, input.EmployeeId, input.EstimatedExecutionTime, input.EstimatedCompletionTime);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
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
            await CheckExecutePermissionAsync();
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
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
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
            await CheckCompletionPermissionAsync();
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
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn(L("完成工单失败！"), ex);
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
            await CheckRejectPermissionAsync();
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
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn(L("拒绝工单失败！"), ex);
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
                dto.EmployeePhone = emp?.Phone;
            }
            dto.EstimatedCompletionTime = entity.EstimatedCompletionTime;
            dto.EstimatedExecutionTime = entity.EstimatedExecutionTime;
            dto.ExecutionTime = entity.ExecutionTime;

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
        /// 新增时的映射
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async ValueTask<TCreateDto> CreateInputToCreateDto(TCreateInput input)
        {
            var dto = new TCreateDto
            {
                CategoryId = input.CategoryId,
                Description = input.Description,
                EstimatedCompletionTime = input.EstimatedCompletionTime,
                EstimatedExecutionTime = input.EstimatedExecutionTime,
                Title = input.Title,
                EmployeeId = input.EmployeeId,
                UrgencyDegree = input.UrgencyDegree
                //Time = Clock.Now
            };
            //if (input.UrgencyDegree.HasValue)
            //    dto.UrgencyDegree = input.UrgencyDegree.Value;
            return dto;
        }

        protected virtual async ValueTask BeforeCreateAsync(TEntity entity, TCreateInput input)
        {

        }
        /// <summary>
        /// 新增或更新到数据库前执行此方法<br />
        /// 默认不做任何操作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async ValueTask BeforeEditAsync(TEntity entity, TUpdateInput input)
        {

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

        #region 权限判断
        protected virtual Task CheckCreatePermissionAsync()
        {
            return CheckPermissionAsync(createPermissionName);
        }
        protected virtual Task CheckUpdatePermissionAsync()
        {
            return CheckPermissionAsync(updatePermissionName);
        }
        protected virtual Task CheckDeletePermissionAsync()
        {
            return CheckPermissionAsync(deletePermissionName);
        }
        protected virtual Task CheckRejectPermissionAsync()
        {
            return CheckPermissionAsync(rejectPermissionName);
        }
        protected virtual Task CheckConfirmePermissionAsync()
        {
            return CheckPermissionAsync(confirmePermissionName);
        }
        protected virtual Task CheckCompletionPermissionAsync()
        {
            return CheckPermissionAsync(completionPermissionName);
        }
        protected virtual Task CheckExecutePermissionAsync()
        {
            return CheckPermissionAsync(executePermissionName);
        }
        protected virtual Task CheckAllocatePermissionAsync()
        {
            return CheckPermissionAsync(allocatePermissionName);
        }
        protected virtual Task CheckGetPermissionAsync()
        {
            return CheckPermissionAsync(getPermissionName);
        }
        #endregion
    }

    /// <summary>
    /// 后台管理默认工单应用服务接口
    /// </summary>
    public class WorkOrderAppService : WorkOrderAppServiceBase<WorkOrderCreateInput,
                                                               WorkOrderUpdateInput,
                                                               BatchOperationInputLong,
                                                               BatchOperationOutputLong,
                                                               EntityDto<long>,
                                                               GetAllWorkOrderBaseInput,
                                                               WorkOrderDto,
                                                               WorkOrderBatchChangeStatusInputBase,
                                                               WorkOrderBatchChangeStatusOutputBase,
                                                               WorkOrderBatchAllocateInputBase,
                                                               WorkOrderBatchAllocateOutputBase,
                                                               OrderEntity,
                                                               IRepository<OrderEntity, long>,
                                                               WorkOrderCreateDto,
                                                               OrderManager,
                                                               IRepository<CategoryEntity, long>>

    {
        public WorkOrderAppService(IRepository<OrderEntity, long> repository,
                                   OrderManager manager,
                                   IRepository<CategoryEntity, long> categoryRepository,
                                   IEmployeeAppService employeeAppService) : base(repository,
                                                                                  manager,
                                                                                  categoryRepository,
                                                                                  employeeAppService,
                                                                                  CoreConsts.WorkOrderCreate,
                                                                                  CoreConsts.WorkOrderUpdate,
                                                                                  CoreConsts.WorkOrderDelete,
                                                                                  CoreConsts.WorkOrderManager,
                                                                                  CoreConsts.WorkOrderConfirme,
                                                                                  CoreConsts.WorkOrderAllocate,
                                                                                  CoreConsts.WorkOrderExecute,
                                                                                  CoreConsts.WorkOrderCompletion,
                                                                                  CoreConsts.WorkOrderReject)
        { }

        protected override async ValueTask BeforeEditAsync(OrderEntity entity, WorkOrderUpdateInput input)
        {
            await base.BeforeEditAsync(entity, input);
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
        }
        protected override WorkOrderDto EntityToDto(OrderEntity entity, IEnumerable<CategoryEntity> categories, IEnumerable<EmployeeDto> employees, object state = default)
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
