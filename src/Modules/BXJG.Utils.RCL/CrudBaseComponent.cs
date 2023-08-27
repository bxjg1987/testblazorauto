using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils
{
    //这个封装不是特别有必要，因为大部分逻辑都在应用服务中
    //而这里也没有跟具体ui框架相关
    //所以此封装直接定义在BXJG.UtiLS.MudBlazor中更合适。

    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TUpdateInput,
                                   TGetInput,
                                   TDeleteInput,
                                   TAppService> : AbpBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
    {
        /// <summary>
        /// 缓存当前主服务对象
        /// </summary>
        private TAppService appService;
        /// <summary>
        /// 获取主服务
        /// </summary>
        protected TAppService AppService => appService ??= ScopedServices.GetRequiredService<TAppService>();
    }

    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TUpdateInput,
                                   TGetInput,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TUpdateInput,
                                                                    TGetInput,
                                                                    EntityDto<TPrimaryKey>,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TUpdateInput,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TUpdateInput,
                                                                    EntityDto<TPrimaryKey>,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TCreateInput,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TEntityDto,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TPrimaryKey,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    PagedAndSortedResultRequestDto,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class CrudBaseComponent<TEntityDto,
                                   TAppService> : CrudBaseComponent<TEntityDto,
                                                                    int,
                                                                    TAppService>
        where TEntityDto : IEntityDto<int>
        where TAppService : ICrudBaseAppService<TEntityDto>
    {
    }
}