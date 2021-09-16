using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Timing;
using BXJG.Common.Dto;
using BXJG.GeneralTree;
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
using BXJG.Utils.File;
using System.Linq.Expressions;
using Abp.Linq.Expressions;
using Abp.EntityHistory;
using BXJG.GoodsInfo.Application.Common;

namespace BXJG.GoodsInfo.Application.Admin
{
    /// <summary>
    /// 后台管理物品应用服务基类。
    /// 你自定义的物品类型  的应用服务 可以继承它
    /// </summary>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入模型</typeparam>
    /// <typeparam name="TBatchDeleteInput">批量删除的输入模型</typeparam>
    /// <typeparam name="TBatchDeleteOutput">批量删除时的输出模型</typeparam>
    /// <typeparam name="TGetInput">获取单个信息的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">列表页查询时的输入模型</typeparam>
    /// <typeparam name="TGetTotalInput"></typeparam>
    /// <typeparam name="TEntityDto">列表页显示模型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TRepository">实体仓储类型</typeparam>
    /// <typeparam name="TQueryTemp">分类仓储</typeparam>
    public abstract class GoodsInfoAppService<TCreateInput,
                                              TUpdateInput,
                                              TBatchDeleteInput,
                                              TBatchDeleteOutput,
                                              TGetInput,
                                              TGetTotalInput,
                                              TGetAllInput,
                                              TEntityDto,
                                              TEntity,
                                              TRepository,
                                              TQueryTemp> : AppServiceBase
        #region MyRegion
        where TCreateInput : GoodsInfoEditDto
        where TUpdateInput : GoodsInfoEditDto
        where TBatchDeleteInput : BatchOperationInputLong
        where TBatchDeleteOutput : BatchOperationOutputLong, new()
        where TGetInput : EntityDto<long>
        where TGetTotalInput : GoodsInfoGetTotalInput, new()
        where TGetAllInput : GoodsInfoGetAllInput<TGetTotalInput>
        where TEntityDto : GoodsInfoDto, new()
        where TEntity : GoodsInfoEntity
        where TRepository : IGoodsInfoRepository<TEntity>
        where TQueryTemp : QueryTemp<TEntity>, new()
        #endregion
    {
        #region MyRegion
        protected readonly TRepository repository;
        protected readonly string goodsInfoType;
        protected readonly AttachmentManager<TEntity> attachmentManager;
        protected readonly string createPermissionName, updatePermissionName, deletePermissionName, getPermissionName;
        #endregion

        #region MyRegion
        /// <summary>
        /// 后台管理物品应用服务基类构造函数。
        /// </summary>
        /// <param name="repository">工单仓储</param>
        /// <param name="workOrderType">工单类型</param>
        /// <param name="attachmentManager">附件管理器</param>
        /// <param name="createPermissionName">新增权限名称</param>
        /// <param name="updatePermissionName">修改权限名称</param>
        /// <param name="deletePermissionName">删除权限名称</param>
        /// <param name="getPermissionName">获取权限名称</param>
        public GoodsInfoAppService(string workOrderType,
                                   TRepository repository,
                                   AttachmentManager<TEntity> attachmentManager,
                                   string createPermissionName = default,
                                   string updatePermissionName = default,
                                   string deletePermissionName = default,
                                   string getPermissionName = default)
        {
            this.repository = repository;
            this.getPermissionName = getPermissionName;
            this.createPermissionName = createPermissionName;
            this.updatePermissionName = updatePermissionName;
            this.deletePermissionName = deletePermissionName;
            this.attachmentManager = attachmentManager;
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePermissionAsync();
            var entity = await manager.Value.CreateAsync(await CreateInputToCreateDto(input));
            await BeforeCreateAsync(entity, input);
            await CurrentUnitOfWork.SaveChangesAsync();
            var cls = await clsRepository.Value.GetAsync(entity.CategoryId);
            TEntityDto r = default;
            await attachmentManager.Value.SetAttachmentsAsync(entity.Id, input.Images, async c => r = await EntityToDto(new TQueryTemp { Order = entity, Category = cls }));
            return r;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            await CheckUpdatePermissionAsync();
            var entity = await repository.GetAsync(input.Id);
            await BeforeEditAsync(entity, input);
            await CurrentUnitOfWork.SaveChangesAsync();
            var cls = await clsRepository.Value.GetAsync(entity.CategoryId);
            TEntityDto r = default;
            await attachmentManager.Value.SetAttachmentsAsync(entity.Id, input.Images, async c => r = await EntityToDto(new TQueryTemp { Order = entity, Category = cls }));
            return r;
        }
        /// <summary>
        /// 删除
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
                    await manager.Value.DeleteAsync(item);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    await attachmentManager.Value.SetAttachmentsAsync(item.Id, null);
                    r.Ids.Add(item.Id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item.Id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Id.Message500());
                    Logger.Warn("删除工单失败！".BXJGWorkOrderL(), ex);
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
            var q = await GetFilterAsync(input);
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(q);
            return await EntityToDto(entity);
        }
        /// <summary>
        /// 获取工单列表
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
            var query = await GetAllFilterAsync(input.GetTotalInput);
            var count = await AsyncQueryableExecuter.CountAsync(query);
            query = OrderBy(query, input);
            query = PageBy(query, input);
            var list = await AsyncQueryableExecuter.ToListAsync(query);



            var images = await attachmentManager.Value.GetAttachmentsAsync(list.Select(c => c.Order.Id.ToString()).ToArray());

            var state = await GetStateAsync(list);
            var items = new List<TEntityDto>();
            foreach (var item in list)
            {
                var ttt = EntityToDto(item, images, state);
                items.Add(ttt);
            }
            return new PagedResultDto<TEntityDto>(count, items);
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 获取单个信息的查询，根据id查询，内部会join分类
        /// 通常不需要重写此方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TQueryTemp>> GetFilterAsync(TGetInput input)
        {
            var q = GetQuery();
            q = q.Where(c => c.Order.Id == input.Id);
            //q = await GetAndAllFilterAsync(q);
            //var str = q.ToQueryString();
            return ValueTask.FromResult(q);
        }
        //protected virtual ValueTask<IQueryable<TQueryTemp>> GetAndAllFilterAsync(IQueryable<TQueryTemp> q)
        //{
        //    return ValueTask.FromResult(q);
        //}
        /// <summary>
        /// 应用关键字模糊搜索，默认模糊搜索标题、描述
        /// 若你的工单类型某些字段需要参与关键字模糊搜索，则可以重写此方法，通常需要调用base.ApplyKeyword后，使用Or扩展方法应用更多模糊查询字段
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        protected virtual ValueTask<Expression<Func<TQueryTemp, bool>>> ApplyKeyword(string keyword)
        {
            Expression<Func<TQueryTemp, bool>> where = c => c.Order.Title.Contains(keyword) || c.Order.Description.Contains(keyword);
            return ValueTask.FromResult(where);
        }
        /// <summary>
        /// 获取数量或列表时都会调用，它主要用来应用过滤条件
        /// keyword条件过滤比较特殊，若需要更多关键字模糊查询请重写<see cref="ApplyKeyword(string)"/>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TQueryTemp>> ApplyOther(IQueryable<TQueryTemp> query, TGetTotalInput input)
        {
            if (input.CategoryCodes != null)
            {
                Expression<Func<TQueryTemp, bool>> where = c => false;
                foreach (var item in input.CategoryCodes)
                {
                    where = where.Or(c => c.Category.Code.StartsWith(item));
                }
                query = query.Where(where);
            }
            query = query.WhereIf(input.UrgencyDegrees != null, c => input.UrgencyDegrees.Contains(c.Order.UrgencyDegree))
                         .WhereIf(input.Statuses != null, c => input.Statuses.Contains(c.Order.Status))
                         .WhereIf(!input.EmployeeId.IsNullOrWhiteSpace(), c => c.Order.EmployeeId == input.EmployeeId)
                         .WhereIf(input.EstimatedExecutionTimeStart.HasValue, c => c.Order.EstimatedExecutionTime >= input.EstimatedExecutionTimeStart)
                         .WhereIf(input.EstimatedExecutionTimeEnd.HasValue, c => c.Order.EstimatedExecutionTime < input.EstimatedExecutionTimeEnd)
                         .WhereIf(input.EstimatedCompletionTimeStart.HasValue, c => c.Order.EstimatedCompletionTime >= input.EstimatedCompletionTimeStart)
                         .WhereIf(input.EstimatedCompletionTimeEnd.HasValue, c => c.Order.EstimatedCompletionTime < input.EstimatedCompletionTimeEnd)
                         .WhereIf(input.ExecutionTimeStart.HasValue, c => c.Order.ExecutionTime >= input.ExecutionTimeStart)
                         .WhereIf(input.ExecutionTimeEnd.HasValue, c => c.Order.ExecutionTime < input.ExecutionTimeEnd)
                         .WhereIf(input.CompletionTimeStart.HasValue, c => c.Order.CompletionTime >= input.CompletionTimeStart)
                         .WhereIf(input.CompletionTimeEnd.HasValue, c => c.Order.CompletionTime < input.CompletionTimeEnd);
            return ValueTask.FromResult(query);
        }
        /// <summary>
        /// 获取查询对象，默认repository.GetAll()
        /// 通常，当你的查询需要include时可以重写此方法
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetOrderQuery()
        {
            return repository.GetAll();
        }
        /// <summary>
        /// 获取工单和分类join后的查询对象
        /// 此方法会被<see cref="GetAllAsync(TGetAllInput)"/>和<see cref="GetAllAsync(TGetAllInput)"/>调用
        /// 可以重写此方法include更多导航属性
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TQueryTemp> GetQuery()
        {
            var query = from c in GetOrderQuery().AsNoTrackingWithIdentityResolution()
                        join lb in GetClsQuery().AsNoTrackingWithIdentityResolution() on c.CategoryId equals lb.Id into g
                        from kk in g.DefaultIfEmpty()
                        select new TQueryTemp { Order = c, Category = kk };
            return query;
        }

        protected virtual async Task<IQueryable<TQueryTemp>> GetAllFilterAsync(TGetTotalInput input)
        {
            var query = GetQuery();
            //if (input.CategoryCodes != null)
            //{
            //    //Expression<Func<TQueryTemp, bool>> where = c => false;
            //    //foreach (var item in input.CategoryCodes)
            //    //{
            //    //    where = where.Or(c => c.Category.Code.StartsWith(item));
            //    //}
            //    query = query.Where(await ApplyCls(input.CategoryCodes));
            //}
            //var query = query1.Select(c => c.Order);


            //var empIdsQuery = await GetEmployeeIdsAsync(input);
            //if (empIdsQuery != null && empIdsQuery.Count() > 0)
            //    query = query.Where(c => empIdsQuery.Contains(c.Order.EmployeeId));

            //应用基本条件过滤
            query = await ApplyOther(query, input);
            //关键字条件过滤比较特殊，定义在单独的方法中，允许重写
            query = query.WhereIf(!input.Keyword.IsNullOrWhiteSpace(), await ApplyKeyword(input.Keyword));
            //query = await GetAndAllFilterAsync(query);
            return query;
        }
        protected virtual IQueryable<TQueryTemp> PageBy(IQueryable<TQueryTemp> query, TGetAllInput input)
        {
            return query.PageBy(input);
        }
        protected virtual IQueryable<TQueryTemp> OrderBy(IQueryable<TQueryTemp> query, TGetAllInput input)
        {
            return query.OrderBy(input.Sorting);
        }
        protected virtual TEntityDto EntityToDto(TQueryTemp entity, IDictionary<string, List<AttachmentEntity>> images, object state = null)
        {
            var dto = base.ObjectMapper.Map<TEntityDto>(entity.Order);
            //dto.CategoryId = entity.CategoryId;
            dto.CategoryDisplayName = entity.Category?.DisplayName;
            //dto.CompletionTime = entity.CompletionTime;
            //dto.CreationTime = entity.CreationTime;
            //dto.CreatorUserId = entity.CreatorUserId;
            //dto.DeleterUserId = entity.DeleterUserId;
            //dto.DeletionTime = entity.DeletionTime;
            //dto.Description = entity.Description;
            //dto.EmployeeId = entity.EmployeeId;
            //if (employees != null)
            //{
            //    var emp = employees.SingleOrDefault(c => c.Id == entity.EmployeeId);
            //    dto.EmployeeName = emp?.Name;
            //    dto.EmployeePhone = emp?.Phone;
            //}
            //dto.EstimatedCompletionTime = entity.EstimatedCompletionTime;
            //dto.EstimatedExecutionTime = entity.EstimatedExecutionTime;
            //dto.ExecutionTime = entity.ExecutionTime;

            //dto.Id = entity.Id;
            //dto.IsDeleted = entity.IsDeleted;
            //dto.LastModificationTime = entity.LastModificationTime;
            //dto.LastModifierUserId = entity.LastModifierUserId;
            //dto.Status = entity.Status;
            //dto.StatusChangedDescription = entity.StatusChangedDescription;
            //dto.StatusChangedTime = entity.StatusChangedTime;
            //dto.Title = entity.Title;
            //dto.UrgencyDegree = entity.UrgencyDegree;
            if (images.ContainsKey(entity.Order.Id.ToString()))
                dto.Images = ObjectMapper.Map<List<AttachmentDto>>(images[entity.Order.Id.ToString()]);
            return dto;
        }
        /// <summary>
        /// 实体映射为dto。
        /// 默认使用ObjectMapper.Map(entity)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual ValueTask<TEntityDto> Entity2DtoAsync(TQueryTemp item)
        {
            var dto = ObjectMapper.Map<TEntityDto>(item.Entity);
            return ValueTask.FromResult(dto);
        }
        /// <summary>
        /// 新增时保存到数据库之前回调
        /// 默认将输入dto映射为entity
        /// </summary>
        /// <param name="entity">数据库查询来的实体</param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<TEntity> BeforeCreateAsync(TCreateInput input)
        {
            var entity = ObjectMapper.Map<TEntity>(input);
            return ValueTask.FromResult(entity);
        }
        /// <summary>
        /// 修改时保存到数据库之前回调
        /// 默认将输入dto映射为entity
        /// </summary>
        /// <param name="entity">数据库查询来的实体</param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask BeforeEditAsync(TEntity entity, TUpdateInput input)
        {
            ObjectMapper.Map(input, entity);
            return ValueTask.CompletedTask;
        }
        #endregion

        #region MyRegion
        /// <summary>
        /// 检查新增权限，若失败则抛出未授权异常
        /// </summary>
        /// <returns></returns>
        protected virtual Task CheckCreatePermissionAsync()
        {
            return CheckPermissionAsync(createPermissionName);
        }
        /// <summary>
        /// 检查修改权限，若失败则抛出未授权异常
        /// </summary>
        /// <returns></returns>
        protected virtual Task CheckUpdatePermissionAsync()
        {
            return CheckPermissionAsync(updatePermissionName);
        }
        /// <summary>
        /// 检查删除权限，若失败则抛出未授权异常
        /// </summary>
        /// <returns></returns>
        protected virtual Task CheckDeletePermissionAsync()
        {
            return CheckPermissionAsync(deletePermissionName);
        }
        /// <summary>
        /// 检查查询权限，若失败则抛出未授权异常
        /// </summary>
        /// <returns></returns>
        protected virtual Task CheckGetPermissionAsync()
        {
            return CheckPermissionAsync(getPermissionName);
        }
        #endregion
    }
}
