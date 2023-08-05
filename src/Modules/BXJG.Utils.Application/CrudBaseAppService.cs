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
}
