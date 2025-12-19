using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Expressions;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.UI;
using BXJG.Common;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application
{
    /// <summary>
    /// 对abp默认的AsyncCrudAppService的增强
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    [UnitOfWork]//在blazor server中，加这个更保险
    public abstract class CrudBaseAppService<TEntity,
                                             TEntityDto,
                                             TPrimaryKey,
                                             TGetAllInput,
                                             TCreateInput,
                                             TUpdateInput,
                                             TGetInput,
                                             TDeleteInput> : AsyncCrudAppService<TEntity,
                                                                                 TEntityDto,
                                                                                 TPrimaryKey,
                                                                                 TGetAllInput,
                                                                                 TCreateInput,
                                                                                 TUpdateInput,
                                                                                 TGetInput,
                                                                                 TDeleteInput>, 
                                                             ICrudBaseAppService<TEntityDto,
                                                                                 TPrimaryKey,
                                                                                 TGetAllInput,
                                                                                 TCreateInput,
                                                                                 TUpdateInput,
                                                                                 TGetInput,
                                                                                 TDeleteInput>/*,
                                                             ICrudBaseCombinatorialAppService<TEntity,
                                                                                              TCreateInput,
                                                                                              TEntityDto>*/

        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        #region 属性和字段
        //public virtual new string UpdatePermissionName { get => base.UpdatePermissionName; set => base.UpdatePermissionName = value; }
        //public virtual new string DeletePermissionName { get => base.DeletePermissionName; set => base.DeletePermissionName = value; }
        //public virtual new string GetAllPermissionName { get => base.GetAllPermissionName; set => base.GetAllPermissionName = value; }
        //public virtual new string GetPermissionName { get => base.GetPermissionName; set => base.GetPermissionName = value; }


        public IHostEnvironment HostEnvironment { get; set; }
        /// <summary>
        /// 与当前请求关联的服务容器
        /// 通常你可以使用构造函数或属性注入，框架级别或特殊情况可以使用此对象。反正是能不用就别用
        /// 注：IocManager是全局单例，解析实现IDisposeable的服务时比较危险，此时应使用ServiceProvider，保险起见使用时最好再创建个scop
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }
        /// <summary>
        /// 分布式锁帮助器
        /// 通常在应用服务方法的中间部分用，少数情况在领域服务方法中也可以
        /// uow释放后会自动释放锁
        /// </summary>
        public DistributedLockHelper DistributedLockHelper { get; set; }
        public ICancellationTokenProvider CancellationTokenProvider { get; set; } = NullCancellationTokenProvider.Instance;

        #endregion

        #region 构造函数
        protected CrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
        #endregion

        #region 新增
        //public virtual Func<TCreateInput, ValueTask<TEntity>> MapToEntityCreateFunc { get => field ?? MapToEntityAsync; set; }
        //public virtual Func<TEntity, ValueTask> MapToEntityFunc { get => field ?? MapToEntity; set; }
        //public virtual Func<TEntity, TCreateInput, Task> CreateSaveFunc { get => field ?? CreateSave; set; }
        //public virtual Func<TEntity, Task<TEntityDto>> CreateAfterFunc { get => field ?? CreateAfter; set; }
        //Zhongjie仅用于界面，业务逻辑层任然使用abp的事件总线（它不是为界面设计的，默认也没提供多个实例），在ui提供abpk事件处理器 来连接到zhongjie实例
        //public Zhongjie Zhongjie { get; set; }
        public override async Task<TEntityDto> CreateAsync(TCreateInput input)
        {
            //新增权限检查
            CheckCreatePermission();// CheckCreatePermissionAct?.Invoke(); 
            await CheckCreateRepeate(input);
            //if (CheckCreateRepeateFunc != null)
            //    await CheckCreateRepeateFunc(input);
            //var cfjc = await GetCreateIsExistsChenker(input);
            //if (cfjc != null)
            //{
            //    //加锁注意是为了防止重复提交，数据库唯一约束局限性太大，比如软删除冲突，另外在代码层面做了，数据库见表也省得到处去建唯一索引
            //    await DistributedLockHelper.AcquireLockTenantAsync(typeof(TEntity).FullName, TimeSpan.FromMinutes(1), CancellationTokenProvider.Token);
            //    //要完美的话，前端还应该先判断，后端是最后的保障
            //    await Repository.IsExistsThrow(cfjc.Where,cfjc.DisplayNameProperty);
            //}

            //这里无需空判断，因为MapCreateToEntity是必须的
            var entity = await MapToEntityAsync(input);// MapToEntityCreateFunc(input);
            await MapToEntity(entity);
            //if (MapToEntityFunc != null)
            //    await MapToEntityFunc(entity);

            //await MapToEntity(entity);
            //await Repository.InsertAsync(entity);
            //await CreateCore(entity, input);
            //await CreateSaveFunc(entity, input);
            await CreateSave(entity, input);
            //重新查一次，以便获取关联数据
            //entity = await GetEntityByIdAsync(entity.Id, false);//.SingleAsync(c => c.Id.Equals(id));
            //return MapToEntityDto(entity);
            return await CreateAfter(entity);
            //return await CreateAfterFunc(entity);
        }

        /// <summary>
        /// 新增时的重复检查，返回null则不检查，默认情况下引用<see cref="GetUpdateIsExistsChenker(TUpdateInput)"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<ExistsExpression<TEntity>> GetCreateIsExistsChenker(TCreateInput input) => (input is TUpdateInput a) ? GetUpdateIsExistsChenker(a) : ValueTask.FromResult<ExistsExpression<TEntity>>(null);

        //public virtual new string CreatePermissionName { get => base.CreatePermissionName; set => base.CreatePermissionName = value; }
        //public virtual Action CheckCreatePermissionAct { get => field ?? CheckCreatePermission; set; }

        //public virtual Func<>

        //public virtual Func<TCreateInput, ValueTask> CheckCreateRepeateFunc { get => field ?? CheckCreateRepeate; set; }
        public virtual async ValueTask CheckCreateRepeate(TCreateInput createInput)
        {
            var cfjc = await GetCreateIsExistsChenker(createInput);
            if (cfjc != null)
            {
                //加锁注意是为了防止重复提交，数据库唯一约束局限性太大，比如软删除冲突，另外在代码层面做了，数据库见表也省得到处去建唯一索引
                await DistributedLockHelper.AcquireLockTenantAsync(typeof(TEntity).FullName, TimeSpan.FromMinutes(1), CancellationTokenProvider.Token);
                //要完美的话，前端还应该先判断，后端是最后的保障
                await Repository.IsExistsThrow(cfjc.Where, cfjc.DisplayNameProperty);
            }
        }




        /// <summary>
        /// 若你希望使用自己的manager插入，请重写此方法
        /// 通用树本身就是使用manager插入的，所以它不需要这样的设计
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async Task CreateSave(TEntity entity, TCreateInput input)
        {
            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        protected virtual Task<TEntityDto> CreateAfter(TEntity entity)
        {
            return GetDtoById(entity);
        }
        /// <summary>
        /// 新增是dto映射到实体
        /// 若主键是guid类型，则赋使用<see cref="SequentialGuidGenerator.Instance"/>赋值；
        /// 特别注意：从ef7到以前，若配置为自增时，主动为其赋值id会爆异常，但这种情况很少，所以这种特殊情况你应该重写并删除id值。
        /// </summary>
        /// <param name="createInput"></param>
        /// <returns></returns>
        protected override TEntity MapToEntity(TCreateInput createInput)
        {
            var r = base.MapToEntity(createInput);
            if (r is IEntity<Guid> et)
                et.Id =  SequentialGuidGenerator.Instance.Create();

            return r;
        }
        protected virtual ValueTask<TEntity> MapToEntityAsync(TCreateInput input) => ValueTask.FromResult(MapToEntity(input));
        #endregion

        #region 修改 
        /// <summary>
        /// 修改时的重复检查，返回null则不检查
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<ExistsExpression<TEntity>> GetUpdateIsExistsChenker(TUpdateInput input) => ValueTask.FromResult<ExistsExpression<TEntity>>(null);

        protected virtual Task<TEntityDto> UpdateAfter(TEntity entity)
        {
            return GetDtoById(entity);
        }

        //public virtual ValueTask CreateAfter(TEntity entity, ) => ValueTask.CompletedTask;
        public override async Task<TEntityDto> UpdateAsync(TUpdateInput input)
        {
            CheckUpdatePermission();
            await CheckUpdateRepeate(input);

            TEntity entity = await GetEntityByIdAsync(input.Id);
            await MapToEntityAsync(input, entity);
            await MapToEntity(entity);


            await CurrentUnitOfWork.SaveChangesAsync();
            //entity = await GetEntityByIdAsync(entity.Id, false); //.SingleAsync(c => c.Id.Equals(id));
            //return MapToEntityDto(entity);
            return await UpdateAfter(entity);
        }

        private async Task CheckUpdateRepeate(TUpdateInput input)
        {
            var cfjc = await GetUpdateIsExistsChenker(input);
            if (cfjc != null)
            {
                //加锁注意是为了防止重复提交，数据库唯一约束局限性太大，比如软删除冲突，另外在代码层面做了，数据库见表也省得到处去建唯一索引
                await DistributedLockHelper.AcquireLockTenantAsync(typeof(TEntity).FullName, TimeSpan.FromMinutes(1), CancellationTokenProvider.Token);
                //要完美的话，前端还应该先判断，后端是最后的保障
                await Repository.IsExistsThrow(cfjc.Where.And(x => !x.Id.Equals(input.Id)), cfjc.DisplayNameProperty);
            }
        }


        /// <summary>
        /// 修改时dto映射到实体
        /// </summary>
        /// <param name="updateInput"></param>
        /// <param name="entity"></param>
        protected override void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            base.MapToEntity(updateInput, entity);
        }
        protected virtual async Task MapToEntityAsync(TUpdateInput input, TEntity entity) => MapToEntity(input, entity);

        #endregion

        #region 删除
      
        public override async Task DeleteAsync(TDeleteInput input)
        {
            CheckDeletePermission();
            var entity = await GetEntityByIdAsync(input.Id);
            await DeleteCore(entity);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public virtual async Task<BatchOperationOutput<TPrimaryKey>> DeleteBatchAsync(BatchOperationInput<TPrimaryKey> input)
        {
            CheckDeletePermission();
            return await BatchHandleAsync(input.Ids, async x => await DeleteCore(x));
        }
        /// <summary>
        /// 批量删除过程中，删除每个实体时回调
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async Task DeleteCore(TEntity entity)
        {
            await Repository.DeleteAsync(entity);
        }
        #endregion

        #region 查询
        protected virtual async Task<TEntityDto> GetDtoById(TEntity entity, TPrimaryKey id = default)
        {
            var entity1 = await GetEntityByIdAsync(entity == null ? id : entity.Id, false);//.SingleAsync(c => c.Id.Equals(id));
            return MapToEntityDto(entity1);
        }
        /// <summary>
        /// 获取列表，且不跟踪实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual new async Task<IQueryable<TEntity>> CreateFilteredQuery(TGetAllInput input)
        {
            var q = await BuildQuery(false);
            if (input is IHaveFilter p)
            {
                q = q.ApplyDynamicCondtion(p.Filter);
            }
            return q;
        }

        /// <summary>
        /// 重写以关闭工作单元
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public override async Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            //return base.GetAllAsync(input);
            //从父类复制来的，为了把CreateFilteredQuery变成异步的
            CheckGetAllPermission();
            IQueryable<TEntity> query = await CreateFilteredQuery(input);
            int totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            //var sql = query.ToQueryString();
            return new PagedResultDto<TEntityDto>(totalCount, (await AsyncQueryableExecuter.ToListAsync(query)).Select(MapToEntityDto).ToList());

        }
        [UnitOfWork(false)]
        public override async Task<TEntityDto> GetAsync(TGetInput input)
        {
            CheckGetPermission();

            //var entity = await GetEntityByIdAsync(input.Id, false);//.SingleAsync(c => c.Id.Equals(id));
            //return MapToEntityDto(entity);
            return await GetDtoById(default, input.Id);
        }
        /// <summary>
        /// 根据id获取实体，且跟踪实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override async Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {
            //return base.GetEntityByIdAsync(id);
            return await GetEntityByIdAsync(id);//.SingleAsync(c => c.Id.Equals(id));
        }

        /// <summary>
        /// crud中都需要获取单个数据，有时候还需要跟踪实体，通常重写它在查询单个实体是Include更多导航属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        protected virtual async Task<TEntity> GetEntityByIdAsync(TPrimaryKey id, bool track = true)
        {
            //return base.GetEntityByIdAsync(id);
            return await AsyncQueryableExecuter.FirstOrDefaultAsync((await BuildQuery(track)).Where(c => c.Id.Equals(id)));//.SingleAsync(c => c.Id.Equals(id));
        }


        /// <summary>
        /// 获取单个和列表时都会回调，你可以重写以Include更多导航属性
        /// </summary>
        /// <param name="track">是否跟踪实体</param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> BuildQuery(bool track = true)
        {
            var q = await Repository.GetAllAsync();
            if (!track)
                return q.AsNoTrackingWithIdentityResolution();
            return q;
        }

        #endregion

        #region 辅助
        /// <summary>
        /// 新增和修改都会执行的逻辑，若需要传递额外参数，请使用当前Uow.Items
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual ValueTask MapToEntity(TEntity entity) => ValueTask.CompletedTask;
        /// <summary>
        /// 批量处理
        /// </summary>
        /// <param name="ids">前端传入的参数</param>
        /// <param name="func">处理其中每一个时回调</param>
        /// <param name="funcName">处理其中每一个时回调</param>
        /// <returns></returns>
        protected virtual async Task<BatchOperationOutput<TPrimaryKey>> BatchHandleAsync(IEnumerable<TPrimaryKey> ids, Func<TEntity, ValueTask> func, string funcName = "删除")
        {
            var r = new BatchOperationOutput<TPrimaryKey>();
            foreach (var id in ids)
            {
                try
                {
                    using var uow = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    var entity = await GetEntityByIdAsync(id);
                    await func(entity);
                    await uow.CompleteAsync();
                    r.Ids.Add(id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(id.Message500());
                    Logger.Warn($"部分{funcName}失败！{id}", ex);
                }
            }
            return r;
        }

        #endregion
    }

    /// <summary>
    /// 继承abp的AsyncCrudAppService时都需要去写那个默认的构造函数，这个类就是为了解决这个问题
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    public abstract class CrudBaseAppService<TEntity,
                                    TEntityDto,
                                    TPrimaryKey,
                                    TGetAllInput,
                                    TCreateInput,
                                    TUpdateInput,
                                    TGetInput> : CrudBaseAppService<TEntity,
                                                                    TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TUpdateInput,
                                                                    TGetInput,
                                                                    EntityDto<TPrimaryKey>>, ICrudBaseAppService<TEntityDto,
                                                                                                                 TPrimaryKey,
                                                                                                                 TGetAllInput,
                                                                                                                 TCreateInput,
                                                                                                                 TUpdateInput,
                                                                                                                 TGetInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        protected CrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// 继承abp的AsyncCrudAppService时都需要去写那个默认的构造函数，这个类就是为了解决这个问题
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    public abstract class CrudBaseAppService<TEntity,
                                    TEntityDto,
                                    TPrimaryKey,
                                    TGetAllInput,
                                    TCreateInput,
                                    TUpdateInput> : CrudBaseAppService<TEntity,
                                                                       TEntityDto,
                                                                       TPrimaryKey,
                                                                       TGetAllInput,
                                                                       TCreateInput,
                                                                       TUpdateInput,
                                                                       EntityDto<TPrimaryKey>>, ICrudBaseAppService<TEntityDto,
                                                                                                                    TPrimaryKey,
                                                                                                                    TGetAllInput,
                                                                                                                    TCreateInput,
                                                                                                                    TUpdateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected CrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 继承abp的AsyncCrudAppService时都需要去写那个默认的构造函数，这个类就是为了解决这个问题
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    public abstract class CrudBaseAppService<TEntity,
                                    TEntityDto,
                                    TPrimaryKey,
                                    TGetAllInput,
                                    TCreateInput> : CrudBaseAppService<TEntity,
                                                                       TEntityDto,
                                                                       TPrimaryKey,
                                                                       TGetAllInput,
                                                                       TCreateInput,
                                                                       TCreateInput>, ICrudBaseAppService<TEntityDto,
                                                                                                          TPrimaryKey,
                                                                                                          TGetAllInput,
                                                                                                          TCreateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected CrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 继承abp的AsyncCrudAppService时都需要去写那个默认的构造函数，这个类就是为了解决这个问题
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    public abstract class CrudBaseAppService<TEntity,
                                    TEntityDto,
                                    TPrimaryKey,
                                    TGetAllInput> : CrudBaseAppService<TEntity,
                                                                      TEntityDto,
                                                                      TPrimaryKey,
                                                                      TGetAllInput,
                                                                      TEntityDto>, ICrudBaseAppService<TEntityDto,
                                                                                                       TPrimaryKey,
                                                                                                       TGetAllInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 继承abp的AsyncCrudAppService时都需要去写那个默认的构造函数，这个类就是为了解决这个问题
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class CrudBaseAppService<TEntity,
                                    TEntityDto,
                                    TPrimaryKey> : CrudBaseAppService<TEntity,
                                                                      TEntityDto,
                                                                      TPrimaryKey,
                                                                      PagedAndSortedResultRequestDto>, ICrudBaseAppService<TEntityDto,
                                                                                                                           TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 继承abp的AsyncCrudAppService时都需要去写那个默认的构造函数，这个类就是为了解决这个问题
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    public abstract class CrudBaseAppService<TEntity,
                                    TEntityDto> : CrudBaseAppService<TEntity,
                                                                     TEntityDto,
                                                                     int>, ICrudBaseAppService<TEntityDto>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected CrudBaseAppService(IRepository<TEntity, int> repository) : base(repository)
        {
        }
    }


}