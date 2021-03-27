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
using Abp.UI;
using BXJG.Utils;
using Abp.Localization;
using BXJG.Utils.Localization;
using Abp.Authorization;

namespace BXJG.WorkOrder.WorkOrder
{
    /*
     * 很多操作都会限制工单状态，比如只有已确认的工单才可以 领取、 只有执行中的工单才可以执行完成操作
     * 可以在查询时直接加限制条件，比如执行操作中限制只查询待执行状态的工单
     * 但不应该这样坐，业务领域层对应的操作已经做了状态限制，正常情况前端传递过来的id（集合）中是不包含意外情况的工单
     * 比如执行操作参数中有id集合，这些id正常情况都是待执行的。这些业务本就应该下层到领域层，若应用层再加判断就失去了领域层业务的意义
     */

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
        where TGetAllInput : GetAllWorkOrderEmployeeBaseInput
        where TEntityDto : WorkOrderEmployeeBaseDto, new()
        where TBatchChangeStatusInput : WorkOrderEmployeeBatchChangeStatusBaseInput
        where TBatchChangeStatusOutput : WorkOrderEmployeeBatchChangeStatusBaseOutput, new()
        where TBatchAllocateInput : WorkOrderEmployeeBatchAllocateBaseInput
        where TBatchAllocateOutput : WorkOrderEmployeeBatchAllocateOutput, new()
        where TEntity : OrderBaseEntity
        where TRepository : IRepository<TEntity, long>
        where TCategoryRepository : IRepository<CategoryEntity, long>
        #endregion
    {
        protected readonly TRepository repository;
        protected readonly TCategoryRepository categoryRepository;
        protected readonly IEmployeeAppService employeeAppService;

        protected readonly string getPermissionName, allocatePermissionName , executePermissionName, completionPermissionName, rejectPermissionName;

        public WorkOrderEmployeeAppServiceBase(TRepository repository,
                                               TCategoryRepository categoryRepository,
                                               IEmployeeAppService employeeAppService,
                                               IEmployeeSession employeeSession,
                                               string getPermissionName= default,
                                               string allocatePermissionName = default,
                                               string executePermissionName = default,
                                               string completionPermissionName = default,
                                               string rejectPermissionName = default) : base(employeeSession)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.employeeAppService = employeeAppService;
            this.getPermissionName = getPermissionName;
            this.allocatePermissionName = allocatePermissionName;
            this.executePermissionName = executePermissionName;
            this.completionPermissionName = completionPermissionName;
            this.rejectPermissionName = rejectPermissionName;
        }

        /// <summary>
        /// 获取指定工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> GetAsync(TGetInput input)
        {
            await CheckGetPermissionAsync();
            var query = repository.GetAll().Where(c => c.Id == input.Id);
            query = ApplyGetDataRights(query);
            var e = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
            //除非非法请求，否则e始终有值，所以这里不用判断是否为空
            return await EntityToDto(e);
            //throw new UserFriendlyException(LocalizationManager.BXJGUtilsL("Insufficient data rights"));
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            await CheckGetPermissionAsync();
            //分类、员工先查询 再用in，
            //假定员工和分类数量不会太多（太多的话考虑分配in查询），且可以使用缓存
            //in查询有索引时性能有所提升
            var query = await GetAllFilterAsync(input);
            query = ApplyGetDataRights(query);
            var count = await AsyncQueryableExecuter.CountAsync(query);
            query = OrderPageBy(query, input);
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
        /// GetAll的排序和分页
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> OrderPageBy(IQueryable<TEntity> query, TGetAllInput input)
        {
            return query.OrderBy(input.Sorting).PageBy(input);
        }
        /// <summary>
        /// 获取指定所有工单的条件
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> GetAllFilterAsync(TGetAllInput input)
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
            return query;
        }
        /// <summary>
        /// 查询1个或多个工单时的数据权限条件<br />
        /// 默认 （大厅的且待分配的）  或  （已分配给自己的且不是待确认的）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ApplyGetDataRights(IQueryable<TEntity> query)
        {
            return query.Where(c => (c.EmployeeId == null && c.Status == Status.ToBeAllocated) || (c.EmployeeId == CurrentEmployeeId && c.Status != Status.ToBeConfirmed));
        }
        /// <summary>
        /// 批量领取工单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchAllocateOutput> AllocateAsync(TBatchAllocateInput input)
        {
            await CheckAllocatePermissionAsync();
            var query = repository.GetAll()
                                  //.Where(c => c.Status == Status.ToBeAllocated)领域层的逻辑不应在应用层来限制。传递来的ids正常来说都是待分配的
                                  .Where(c => input.Ids.Contains(c.Id));

            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var r = new TBatchAllocateOutput();
            //var msg = $"部分或全部失败！{Environment.NewLine}";
            foreach (var item in list)
            {
                try
                {
                    item.AllocateRetain(Clock.Now, CurrentEmployeeId, input.EstimatedExecutionTime, input.EstimatedCompletionTime);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                    //msg += $"编号{item.Id}的处理失败！{ex.Message}{Environment.NewLine}";
                }
                catch (Exception ex)
                {
                    //msg += $"编号{item.Id}的处理失败！服务端内部异常。{Environment.NewLine}";
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn(L("分配工单失败！"), ex);
                }
            }
            //if (!msg.IsNullOrWhiteSpace())
            //    throw new UserFriendlyException(msg);
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
            var query = repository.GetAll()
                                  .Where(c => c.EmployeeId == CurrentEmployeeId)
                                  //.Where(c => c.Status == Status.ToBeProcessed)
                                  .Where(c => input.Ids.Contains(c.Id));
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
            var query = repository.GetAll()
                                  .Where(c => c.EmployeeId == CurrentEmployeeId)
                                  //.Where(c => c.Status == Status.Processing)
                                  .Where(c => input.Ids.Contains(c.Id));
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
        /// 拒绝
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TBatchChangeStatusOutput> RejectAsync(TBatchChangeStatusInput input)
        {
            await CheckRejectPermissionAsync();
            var query = repository.GetAll()
                                  .Where(c => c.EmployeeId == CurrentEmployeeId)
                                  //.Where(c => c.Status != Status.Rejected) //正常情况前端是不应该提交已拒绝的，领域层已有规则限制，这里不应再判断，否则将来重构领域层可能还得修改应用服务
                                  .Where(c => input.Ids.Contains(c.Id));
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

        #region 权限判断
        protected virtual Task CheckRejectPermissionAsync()
        {
            return CheckPermissionAsync(rejectPermissionName);
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
    public class WorkOrderEmployeeAppService : WorkOrderEmployeeAppServiceBase<EntityDto<long>,
                                                                               GetAllWorkOrderEmployeeBaseInput,
                                                                               WorkOrderEmployeeDto,
                                                                               WorkOrderEmployeeBatchChangeStatusBaseInput,
                                                                               WorkOrderEmployeeBatchChangeStatusBaseOutput,
                                                                               WorkOrderEmployeeBatchAllocateBaseInput,
                                                                               WorkOrderEmployeeBatchAllocateOutput,
                                                                               OrderEntity,
                                                                               IRepository<OrderEntity, long>,
                                                                               IRepository<CategoryEntity, long>>

    {
        public WorkOrderEmployeeAppService(IRepository<OrderEntity, long> repository,
                                           IRepository<CategoryEntity, long> categoryRepository, 
                                           IEmployeeAppService employeeAppService, 
                                           IEmployeeSession employeeSession) : base(repository, 
                                                                                    categoryRepository, 
                                                                                    employeeAppService,
                                                                                    employeeSession,
                                                                                    CoreConsts.EmployeeWorkOrderManager,
                                                                                    CoreConsts.EmployeeWorkOrderManager,
                                                                                    CoreConsts.EmployeeWorkOrderManager,
                                                                                    CoreConsts.EmployeeWorkOrderManager,
                                                                                    CoreConsts.EmployeeWorkOrderManager)
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
