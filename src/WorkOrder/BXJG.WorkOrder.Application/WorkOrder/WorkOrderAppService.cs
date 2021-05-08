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
using Abp.Dependency;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Abp.Application.Services;

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
        where TCreateInput : WorkOrderCreateBaseInput
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
        #region 字段和属性
        protected readonly TRepository repository;
        protected readonly TCategoryRepository categoryRepository;
        protected readonly TManager manager;
        protected readonly IEmployeeAppService employeeAppService;
        protected readonly DefaultClsManager defaultClsManager;
        protected readonly string workOrderType;
        #region 权限名称
        /// <summary>
        /// 工单管理-新增权限名称
        /// </summary>
        protected readonly string createPermissionName;
        /// <summary>
        /// 工单管理-修改权限名称
        /// </summary>
        protected readonly string updatePermissionName;
        /// <summary>
        /// 工单管理-删除权限名称
        /// </summary>
        protected readonly string deletePermissionName;
        /// <summary>
        /// 工单管理-获取权限名称
        /// </summary>
        protected readonly string getPermissionName;
        /// <summary>
        /// 工单管理-待确认权限名称
        /// </summary>
        protected readonly string toBeConfirmedPermissionName;
        /// <summary>
        /// 工单管理-确认权限名称
        /// </summary>
        protected readonly string confirmePermissionName;
        /// <summary>
        /// 工单管理-分配权限名称
        /// </summary>
        protected readonly string allocatePermissionName;
        /// <summary>
        /// 工单管理-执行权限名称
        /// </summary>
        protected readonly string executePermissionName;
        /// <summary>
        /// 工单管理-完成权限名称
        /// </summary>
        protected readonly string completionPermissionName;
        /// <summary>
        /// 工单管理-拒绝权限名称
        /// </summary>
        protected readonly string rejectPermissionName;
        #endregion

        #endregion

        #region 构造函数
        /// <summary>
        /// 工单后台管理应用服务基类构造函数
        /// </summary>
        /// <param name="repository">工单仓储</param>
        /// <param name="manager">工单领域服务</param>
        /// <param name="categoryRepository">工单类别仓储</param>
        /// <param name="employeeAppService">员工服务</param>
        /// <param name="defaultClsManager">默认分类提供器</param>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="createPermissionName">新增权限名称</param>
        /// <param name="updatePermissionName">修改权限名称</param>
        /// <param name="deletePermissionName">删除权限名称</param>
        /// <param name="getPermissionName">获取权限名称</param>
        /// <param name="toBeConfirmedPermissionName">待确认权限名称</param>
        /// <param name="confirmePermissionName">确认权限名称</param>
        /// <param name="allocatePermissionName">分配权限名称</param>
        /// <param name="executePermissionName">执行权限名称</param>
        /// <param name="completionPermissionName">完成权限名称</param>
        /// <param name="rejectPermissionName">拒绝权限名称</param>
        public WorkOrderAppServiceBase(TRepository repository,
                                       TManager manager,
                                       TCategoryRepository categoryRepository,
                                       IEmployeeAppService employeeAppService,
                                       DefaultClsManager defaultClsManager,
                                       string workOrderType,
                                       string createPermissionName = default,
                                       string updatePermissionName = default,
                                       string deletePermissionName = default,
                                       string getPermissionName = default,
                                       string toBeConfirmedPermissionName = default,
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
            this.toBeConfirmedPermissionName = toBeConfirmedPermissionName;
            this.defaultClsManager = defaultClsManager;
            this.workOrderType = workOrderType;
            //this.iocResolver = iocResolver;
        }
        #endregion

        /// <summary>
        /// 新增工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePermissionAsync();
            var entity = await manager.CreateAsync(await CreateInputToCreateDto(input));
            if (input.Status.HasValue && input.Status > entity.Status)
            {
                await entity.Skip(Clock.Now,
                                  input.Status,
                                  input.StatusChangedDescription,
                                  input.EmployeeId,
                                  input.EstimatedExecutionTime,
                                  input.EstimatedCompletionTime,
                                  excuteTime: input.ExecutionTime,
                                  completeTime: input.CompletionTime,
                                  toBeAllocated: d => CheckConfirmePermissionAsync(),
                                  toBeProcessed: d => CheckAllocatePermissionAsync(),
                                  processing: d => CheckExecutePermissionAsync(),
                                  completed: d => CheckCompletionPermissionAsync(),
                                  rejected: d => CheckRejectPermissionAsync());
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

            if (input.CategoryId.HasValue)
                entity.CategoryId = input.CategoryId.Value;
            entity.Title = input.Title;
            entity.Description = input.Description;
            entity.StatusChangedDescription = input.StatusChangedDescription;
            entity.UrgencyDegree = input.UrgencyDegree ?? OrderBaseEntity.DefaultUrgencyDegree;
            entity.EmployeeId = input.EmployeeId;
            entity.ChangeEstimatedTime(input.EstimatedExecutionTime, input.EstimatedCompletionTime);

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
        [UnitOfWork(false)]
        public virtual async Task<TEntityDto> GetAsync(TGetInput input)
        {
            await CheckGetPermissionAsync();
            var entity = await repository.GetAsync(input.Id);
            return await EntityToDto(entity);
        }
        /// <summary>
        /// 获取指定所有工单的条件
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> GetAllFilterAsync(TGetAllInput input)
        {
            var query = from c in repository.GetAll().AsNoTrackingWithIdentityResolution()
                        join lb in categoryRepository.GetAll() on c.CategoryId equals lb.Id into g
                        from kk in g.DefaultIfEmpty()
                        where (input.CategoryCode.IsNullOrWhiteSpace() || kk.Code.StartsWith(input.CategoryCode))
                        select c;

            if (!input.Keyword.IsNullOrWhiteSpace())
            {
                var empIdsQuery = await employeeAppService.GetIdsByKeywordAsync(input.Keyword);
                query = query.Where(c => empIdsQuery.Contains(c.EmployeeId) || c.Title.Contains(input.Keyword));
            }
            query = query.WhereIf(input.UrgencyDegree.HasValue, c => c.UrgencyDegree == input.UrgencyDegree)
                         .WhereIf(input.Status.HasValue, c => c.Status == input.Status)
                         .WhereIf(!input.EmployeeId.IsNullOrWhiteSpace(), c => c.EmployeeId == input.EmployeeId)
                         .WhereIf(input.EstimatedExecutionTimeStart.HasValue, c => c.EstimatedExecutionTime >= input.EstimatedExecutionTimeStart)
                         .WhereIf(input.EstimatedExecutionTimeEnd.HasValue, c => c.EstimatedExecutionTime < input.EstimatedExecutionTimeEnd)
                         .WhereIf(input.EstimatedCompletionTimeStart.HasValue, c => c.EstimatedCompletionTime >= input.EstimatedCompletionTimeStart)
                         .WhereIf(input.EstimatedCompletionTimeEnd.HasValue, c => c.EstimatedCompletionTime < input.EstimatedCompletionTimeEnd)
                         .WhereIf(input.ExecutionTimeStart.HasValue, c => c.ExecutionTime >= input.ExecutionTimeStart)
                         .WhereIf(input.ExecutionTimeEnd.HasValue, c => c.ExecutionTime < input.ExecutionTimeEnd)
                         .WhereIf(input.CompletionTimeStart.HasValue, c => c.CompletionTime >= input.CompletionTimeStart)
                         .WhereIf(input.CompletionTimeEnd.HasValue, c => c.CompletionTime < input.CompletionTimeEnd);

            return query;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            //分类、员工先查询 再用in，
            //假定员工和分类数量不会太多（太多的话考虑分配in查询），且可以使用缓存
            //in查询有索引时性能有所提升
            //按分类名称排序 倒是可用用join映射 select new { 工单实体，join的分类 } 后面再排序
            //按处理人和手机号比较麻烦，可以尝试join已经查询出来的员工列表试试
            //不过至少可用按分类id和处理人id排序
            //如果都无法满足时，可以考虑使用原始sql，毕竟这里只是查询需求，不做业务处理，可以引入dapper或ef的原始sql执行方式

            //var ss = dynamicAssociateEntityDefineManager.GroupedDefines;
            //var define = ss.First().Value.First();
            //var service = iocResolver.Resolve(define.ServiceType) as IDynamicAssociateEntityService;
            //var ss2 = await service.GetAllAsync(define, "a", "d");

            await CheckGetPermissionAsync();
            var query = await GetAllFilterAsync(input);
            var count = await AsyncQueryableExecuter.CountAsync(query);
            query = OrderBy(query, input);
            query = PageBy(query, input);
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
            var state = await GetStateAsync(list);
            var items = new List<TEntityDto>();
            foreach (var item in list)
            {
                var ttt = EntityToDto(item, cls, emps, state);
                items.Add(ttt);
            }
            return new PagedResultDto<TEntityDto>(count, items);
        }
        /// <summary>
        /// 批量调整工单状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchChangeStatusOutput> ChangeStatusAsync(TBatchChangeStatusInput input)
        {
            //await CheckConfirmePermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchChangeStatusOutput();
            foreach (var item in list)
            {
                try
                {
                    await item.ChangeStatus(input.Status,
                                            input.StatusChangedTime ?? Clock.Now,
                                            input.Description,
                                            item.EmployeeId,
                                            item.EstimatedExecutionTime,
                                            item.EstimatedCompletionTime,
                                            item.ExecutionTime,
                                            item.CompletionTime,
                                            d => CheckToBeonfirmedPermissionAsync(),
                                            d => CheckConfirmePermissionAsync(),
                                            d => CheckAllocatePermissionAsync(),
                                            d => CheckExecutePermissionAsync(),
                                            d => CheckConfirmePermissionAsync(),
                                            d => CheckRejectPermissionAsync());
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
                    Logger.Warn(L("执行失败！"), ex);
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
            //await CheckAllocatePermissionAsync();
            var query = repository.GetAll().Where(c => input.Ids.Contains(c.Id));
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchAllocateOutput();
            foreach (var item in list)
            {
                try
                {
                    if (item.Status >= Status.ToBeProcessed)
                    {
                        await item.BackOff(input.StatusChangedTime ?? Clock.Now,
                                           status: Status.ToBeAllocated,
                                           toBeConfirmed: d => CheckToBeonfirmedPermissionAsync(),
                                           toBeAllocated: d => CheckConfirmePermissionAsync(),
                                           toBeProcessed: d => CheckAllocatePermissionAsync(),
                                           processing: d => CheckExecutePermissionAsync());
                    }
                    //item.AllocateRetain(Clock.Now, input.EmployeeId, input.EstimatedExecutionTime, input.EstimatedCompletionTime);
                    await item.Skip(input.StatusChangedTime ?? Clock.Now,
                                    status: Status.ToBeProcessed,
                                    empId: input.EmployeeId,
                                    estimatedExecutionTime: input.EstimatedExecutionTime,
                                    estimatedCompletionTime: input.EstimatedCompletionTime,
                                    toBeAllocated: d => CheckConfirmePermissionAsync(),
                                    toBeProcessed: d => CheckAllocatePermissionAsync());

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
        /// GetAll的分页
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> PageBy(IQueryable<TEntity> query, TGetAllInput input)
        {
            return query.PageBy(input);
        }
        /// <summary>
        /// GetAll的排序
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> OrderBy(IQueryable<TEntity> query, TGetAllInput input)
        {
            return query.OrderBy(input.Sorting);
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
            var state = await GetStateAsync(new TEntity[] { entity });
            return EntityToDto(entity, new CategoryEntity[] { category }, emps, state);
        }
        /// <summary>
        /// 新增时的映射
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<TCreateDto> CreateInputToCreateDto(TCreateInput input)
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
            if (!input.UrgencyDegree.HasValue)
                dto.UrgencyDegree = OrderBaseEntity.DefaultUrgencyDegree;
            return ValueTask.FromResult(dto);
        }
        /// <summary>
        /// 新增前回调
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask BeforeCreateAsync(TEntity entity, TCreateInput input)
        {
            return ValueTask.CompletedTask;
        }
        /// <summary>
        /// 新增或更新到数据库前执行此方法<br />
        /// 默认不做任何操作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask BeforeEditAsync(TEntity entity, TUpdateInput input)
        {
            return ValueTask.CompletedTask;
        }
        /// <summary>
        /// 子类可能需要聚合更多外键
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected virtual ValueTask<object> GetStateAsync(IEnumerable<TEntity> entities)
        {
            return ValueTask.FromResult<object>(null);
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
        protected virtual Task CheckToBeonfirmedPermissionAsync()
        {
            return CheckPermissionAsync(toBeConfirmedPermissionName);
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
                                                               GetAllWorkOrderInput,
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
                                   BXJGWorkOrderConfig cfg,
                                   DefaultClsManager defaultClsManager,
                                   IRepository<CategoryEntity, long> categoryRepository,
                                   IEmployeeAppService employeeAppService) : base(repository,
                                                                                  manager,
                                                                                  categoryRepository,
                                                                                  employeeAppService,
                                                                                  defaultClsManager,
                                                                                  CoreConsts.DefaultWorkOrderTypeName,
                                                                                  CoreConsts.WorkOrderCreate,
                                                                                  CoreConsts.WorkOrderUpdate,
                                                                                  CoreConsts.WorkOrderDelete,
                                                                                  CoreConsts.WorkOrderManager,
                                                                                  CoreConsts.WorkOrderConfirme,
                                                                                  CoreConsts.WorkOrderAllocate,
                                                                                  CoreConsts.WorkOrderExecute,
                                                                                  CoreConsts.WorkOrderCompletion,
                                                                                  CoreConsts.WorkOrderReject)
        {
            if (!cfg.EnableDefaultWorkOrder)
                throw new ApplicationException("BXJGWorkOrderConfig.EnableDefaultWorkOrder=false");
        }

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

        protected override async ValueTask BeforeCreateAsync(OrderEntity entity, WorkOrderCreateInput input)
        {
            await base.BeforeCreateAsync(entity, input);

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
