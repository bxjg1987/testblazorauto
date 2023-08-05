using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using BXJG.Utils;
using BXJG.Utils.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Customer.Sessions;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ.App.Customer
{
    /// <summary>
    /// 客户服务平台crud应用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
                         : CommonCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        public CustomerSession CustomerSession { get; set; }
        public CustomerCrudBaseAppService(IRepository<TEntity,TPrimaryKey> repository):base(repository)
        {
            LocalizationSourceName = CustConsts.Cust;
        }
    }

    /// <summary>
    /// 客户服务平台crud应用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
                        : CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 客户服务平台crud应用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
                        : CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// 客户服务平台crud应用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
                        : CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 客户服务平台crud应用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
                        : CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 客户服务平台crud应用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey>
                        : CustomerCrudBaseAppService<TEntity, TEntityDto, TPrimaryKey, PagedAndSortedResultRequestDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 客户服务平台crud应用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity, TEntityDto>
                        : CustomerCrudBaseAppService<TEntity, TEntityDto, int>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected CustomerCrudBaseAppService(IRepository<TEntity, int> repository) : base(repository)
        {
        }
    }
}
