using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using BXJG.Utils;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Interceptor;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common;
using ZLJ.Application.Customer.Share;

using ZLJ.Core.Authorization.Users;
using ZLJ.Core.Customer;
using ZLJ.Core.MultiTenancy;

namespace ZLJ.Application.Customer
{
    /// <summary>
    /// 后台管理 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数的类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时输入参数的类型</typeparam>
    /// <typeparam name="TGetInput">获取单个数据时输入参数的类型</typeparam>
    /// <typeparam name="TDeleteInput">删除单个数据时输入参数的类型</typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity,
                                         TEntityDto,
                                         TPrimaryKey,
                                         TGetAllInput,
                                         TCreateInput,
                                         TUpdateInput,
                                         TGetInput,
                                         TDeleteInput> : CommonCrudBaseAppService<TEntity,
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
        public CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
            LocalizationSourceName = CustomerConsts.Customer;//.Admin;
        }
    }
    /// <summary>
    /// 后台管理 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数的类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时输入参数的类型</typeparam>
    /// <typeparam name="TGetInput">获取单个数据时输入参数的类型</typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity,
                                         TEntityDto,
                                         TPrimaryKey,
                                         TGetAllInput,
                                         TCreateInput,
                                         TUpdateInput,
                                         TGetInput> : CustomerCrudBaseAppService<TEntity,
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
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
        //dbcontext中处理了
        //protected override TEntity MapToEntity(TCreateInput createInput)
        //{
        //    var entity = base.MapToEntity(createInput);
        //    if (entity is IMustHaveCustomer a)
        //        a.CustomerId = AbpSession.GetCustomerId()!.Value;
        //    return entity;
        //}
    }
    /// <summary>
    /// 后台管理 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数的类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时输入参数的类型</typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity,
                                         TEntityDto,
                                         TPrimaryKey,
                                         TGetAllInput,
                                         TCreateInput,
                                         TUpdateInput> : CustomerCrudBaseAppService<TEntity,
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
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 后台管理 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数的类型</typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity,
                                         TEntityDto,
                                         TPrimaryKey,
                                         TGetAllInput,
                                         TCreateInput> : CustomerCrudBaseAppService<TEntity,
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
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// 后台管理 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity,
                                         TEntityDto,
                                         TPrimaryKey,
                                         TGetAllInput> : CustomerCrudBaseAppService<TEntity,
                                                                                 TEntityDto,
                                                                                 TPrimaryKey,
                                                                                 TGetAllInput,
                                                                                 TEntityDto>, ICrudBaseAppService<TEntityDto,
                                                                                                                  TPrimaryKey,
                                                                                                                  TGetAllInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 后台管理 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity,
                                         TEntityDto,
                                         TPrimaryKey> : CustomerCrudBaseAppService<TEntity,
                                                                                TEntityDto,
                                                                                TPrimaryKey,
                                                                                PagedAndSortedResultRequestDto>, ICrudBaseAppService<TEntityDto,
                                                                                                                                     TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CustomerCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// 后台管理 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    public abstract class CustomerCrudBaseAppService<TEntity, TEntityDto>
                        : CustomerCrudBaseAppService<TEntity, TEntityDto, int>
     , ICrudBaseAppService<TEntityDto>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected CustomerCrudBaseAppService(IRepository<TEntity, int> repository) : base(repository)
        {
        }
    }
}
