using Abp.IdentityFramework;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using BXJG.Utils.Application;
using Microsoft.AspNetCore.Identity;
using ZLJ.Application.Common.Share;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ.App.Common
{
    /// <summary>
    /// 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数的类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时输入参数的类型</typeparam>
    /// <typeparam name="TGetInput">获取单个数据时输入参数的类型</typeparam>
    /// <typeparam name="TDeleteInput">删除单个数据时输入参数的类型</typeparam>
    public class CommonCrudBaseAppService<TEntity,
                                          TEntityDto,
                                          TPrimaryKey,
                                          TGetAllInput,
                                          TCreateInput,
                                          TUpdateInput,
                                          TGetInput,
                                          TDeleteInput> : CrudBaseAppService<TEntity,
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
        /// 租户管理器
        /// </summary>
        public TenantManager TenantManager { get; set; }
        //public IStaffSession StaffSession { get; set; }
        /// <summary>
        /// 用户管理器
        /// </summary>
        public UserManager UserManager { get; set; }
        /// <summary>
        /// 实例化扁平化数据的crud应用服务
        /// </summary>
        /// <param name="repository"></param>
        protected CommonCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
            LocalizationSourceName = App.Common.Consts.Common;
        }
        //protected override TEntity MapToEntity(TCreateInput createInput)
        //{
        //    var entity = base.MapToEntity(createInput);

        //    if (entity is IEntity<Guid> et)
        //        et.Id = SequentialGuidGenerator.Instance.Create();

        //    return entity;
        //}
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }
        /// <summary>
        /// 获取当前租户
        /// </summary>
        /// <returns></returns>
        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;
        /// <summary>
        /// 获取App.Common中的本地化源
        /// </summary>
        protected virtual ILocalizationSource LocalizationSourceAppCommon
        {
            get
            {
                if (appCommonLocalizationSource == null || appCommonLocalizationSource.Name != App.Common.Consts.Common)
                {
                    appCommonLocalizationSource = LocalizationManager.GetSource(App.Common.Consts.Common);
                }

                return appCommonLocalizationSource;
            }
        }
        /// <summary>
        /// 获取ZLJ.Core中的本地化源
        /// </summary>
        protected virtual ILocalizationSource LocalizationSourceAppZLJ
        {
            get
            {

                if (zljLocalizationSource == null || zljLocalizationSource.Name != ZLJConsts.LocalizationSourceName)
                {
                    zljLocalizationSource = LocalizationManager.GetSource(ZLJConsts.LocalizationSourceName);
                }

                return zljLocalizationSource;
            }
        }
        /// <summary>
        /// 获取BXJG.Utils中的本地化源
        /// </summary>
        protected virtual ILocalizationSource LocalizationSourceUtils
        {
            get
            {

                if (utilsLocalizationSource == null || utilsLocalizationSource.Name != BXJGUtilsConsts.LocalizationSourceName)
                {
                    utilsLocalizationSource = LocalizationManager.GetSource(BXJGUtilsConsts.LocalizationSourceName);
                }

                return utilsLocalizationSource;
            }
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
    /// <summary>
    /// 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数的类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时输入参数的类型</typeparam>
    /// <typeparam name="TGetInput">获取单个数据时输入参数的类型</typeparam>
    public class CommonCrudBaseAppService<TEntity,
                                          TEntityDto,
                                          TPrimaryKey,
                                          TGetAllInput,
                                          TCreateInput,
                                          TUpdateInput,
                                          TGetInput> : CommonCrudBaseAppService<TEntity,
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
        protected CommonCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数的类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时输入参数的类型</typeparam>
    public class CommonCrudBaseAppService<TEntity,
                                          TEntityDto,
                                          TPrimaryKey,
                                          TGetAllInput,
                                          TCreateInput,
                                          TUpdateInput> : CommonCrudBaseAppService<TEntity,
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
        protected CommonCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    /// <typeparam name="TCreateInput">新增时输入参数的类型</typeparam>
    public class CommonCrudBaseAppService<TEntity,
                                          TEntityDto,
                                          TPrimaryKey,
                                          TGetAllInput,
                                          TCreateInput> : CommonCrudBaseAppService<TEntity,
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
        protected CommonCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">获取分页列表数据时的输入类型</typeparam>
    public class CommonCrudBaseAppService<TEntity,
                                          TEntityDto,
                                          TPrimaryKey,
                                          TGetAllInput> : CommonCrudBaseAppService<TEntity,
                                                                                   TEntityDto,
                                                                                   TPrimaryKey,
                                                                                   TGetAllInput,
                                                                                   TEntityDto>, ICrudBaseAppService<TEntityDto,
                                                                                                                    TPrimaryKey,
                                                                                                                    TGetAllInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CommonCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class CommonCrudBaseAppService<TEntity,
                                          TEntityDto,
                                          TPrimaryKey> : CommonCrudBaseAppService<TEntity,
                                                                                  TEntityDto,
                                                                                  TPrimaryKey,
                                                                                  PagedAndSortedResultRequestDto>, ICrudBaseAppService<TEntityDto,
                                                                                                                                       TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected CommonCrudBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// 扁平化数据的crud应用服务基类，继承它以简化扁平化数据crud应用服务的开发
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TEntityDto">详情对象类型</typeparam>
    public class CommonCrudBaseAppService<TEntity,
                                          TEntityDto> : CommonCrudBaseAppService<TEntity,
                                                                                 TEntityDto,
                                                                                 int>, ICrudBaseAppService<TEntityDto>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected CommonCrudBaseAppService(IRepository<TEntity, int> repository) : base(repository)
        {
        }
    }
}
