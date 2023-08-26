using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using BXJG.Common.Dto;
using BXJG.Utils.Dto;
using BXJG.Utils.Notification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
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
                                                                                 TDeleteInput>, ICrudBaseAppService<TEntityDto,
                                                                                                                    TPrimaryKey,
                                                                                                                    TGetAllInput,
                                                                                                                    TCreateInput,
                                                                                                                    TUpdateInput,
                                                                                                                    TGetInput,
                                                                                                                    TDeleteInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// 与当前请求关联的服务容器
        /// 通常你可以使用构造函数或属性注入，框架级别或特殊情况可以使用此对象。
        /// 注：IocManager是全局单例，解析实现IDisposeable的服务时比较危险，此时应使用ServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        protected CrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

        //md，行不通
        //public new IRepository<TEntity, TPrimaryKey> Repository { get; set; }

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
            if (typeof(TPrimaryKey) == typeof(Guid))
            {
                (r as IEntity<Guid>).Id = SequentialGuidGenerator.Instance.Create();
            }
            MapToEntity(r);
            return r;
        }
        /// <summary>
        /// 修改时dto映射到实体
        /// </summary>
        /// <param name="updateInput"></param>
        /// <param name="entity"></param>
        protected override void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            base.MapToEntity(updateInput, entity);
            MapToEntity(entity);
        }
        /// <summary>
        /// 新增和修改都会执行的逻辑，若需要传递额外参数，请使用当前Uow.Items
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual void MapToEntity(TEntity entity) { }

        /// <summary>
        /// 批量处理
        /// </summary>
        /// <param name="input">前端传入的参数</param>
        /// <param name="func">处理其中每一个时回调</param>
        /// <returns></returns>
        protected virtual async Task<BatchOperationOutput<TPrimaryKey>> BatchHandleAsync(BatchOperationInput<TPrimaryKey> input, Func<TEntity, ValueTask> func)
        {
            var r = new BatchOperationOutput<TPrimaryKey>();
            foreach (var id in input.Ids)
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
                    Logger.Warn($"部分操作失败！{id}", ex);
                }
            }
            return r;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public virtual async Task<BatchOperationOutput<TPrimaryKey>> BatchDeleteAsync(BatchOperationInput<TPrimaryKey> input)
        {
            CheckDeletePermission();
            return await BatchHandleAsync(input, BatchDeleteItemAsync);
        }
        /// <summary>
        /// 批量删除过程中，删除每个实体时回调
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async ValueTask BatchDeleteItemAsync(TEntity entity)
        {
            await Repository.DeleteAsync(entity);
        }
        protected override IQueryable<TEntity> CreateFilteredQuery(TGetAllInput input)
        {
            var q = GetAllInclude(base.CreateFilteredQuery(input)).AsNoTrackingWithIdentityResolution();
            if (input is IHaveFilter p)
                q = q.ApplyDynamicCondtion(p.Filter);
            return q;

        }
        /// <summary>
        /// 获取列表时回调，你可以重写以Include更多导航属性
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAllInclude(IQueryable<TEntity> q) => q;
        [UnitOfWork(false)]
        public override Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input)
        {
            return base.GetAllAsync(input);
        }
        [UnitOfWork(false)]
        public override Task<TEntityDto> GetAsync(TGetInput input)
        {
            return base.GetAsync(input);
        }

        protected override Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {
            //return base.GetEntityByIdAsync(id);
            return GetEntityByIdInclude(base.Repository.GetAll()).SingleAsync(c => c.Id.Equals(id));
        }
        /// <summary>
        /// 执行GetEntityByIdAsync将回调此方法，可重写此方法来包含导航属性，默认不包含任何导航属性。
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetEntityByIdInclude(IQueryable<TEntity> q) => q;
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