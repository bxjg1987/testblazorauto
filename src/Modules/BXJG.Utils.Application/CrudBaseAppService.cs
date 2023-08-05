using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
{
    /*
     * 为了剔除构造函数这点小需求实现这么多的抽象类值得吗？
     * 值得，这给了我们一个机会来加入与我们项目相关的抽象逻辑。
     */

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
                                                                                 TDeleteInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        protected new IRepository<TEntity, TPrimaryKey> Repository { get; set; }
        public CrudBaseAppService() : base(null)
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
                                                                             EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
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
                                                                                EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
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
                                                                                TCreateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
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
                                                                                TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
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
                                                                                PagedAndSortedResultRequestDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
    }
    /// <summary>
    /// 继承abp的AsyncCrudAppService时都需要去写那个默认的构造函数，这个类就是为了解决这个问题
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    public abstract class CrudBaseAppService<TEntity,
                                             TEntityDto> : CrudBaseAppService<TEntity,
                                                                              TEntityDto,
                                                                              int>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
    }
}