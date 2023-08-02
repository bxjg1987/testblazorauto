using Abp;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Linq;
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
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ.App.Common
{
    /// <summary>
    /// crud的后台管理基类
    /// </summary>
    public abstract class AsyncCrudCommonBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
                        : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        public TenantManager TenantManager { get; set; }
        //public IStaffSession StaffSession { get; set; }
        public UserManager UserManager { get; set; }

        protected override TEntity MapToEntity(TCreateInput createInput)
        {
            var entity = base.MapToEntity(createInput);

            if (entity is IEntity<Guid> et)
                et.Id = SequentialGuidGenerator.Instance.Create();

            return entity;
        }
        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;

        protected AsyncCrudCommonBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }

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
    /// crud的后台管理基类
    /// </summary>
    public abstract class AsyncCrudCommonBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
               : AsyncCrudCommonBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        public AsyncCrudCommonBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// crud的后台管理基类
    /// </summary>
    public abstract class AsyncCrudCommonBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
               : AsyncCrudCommonBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        public AsyncCrudCommonBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// crud的后台管理基类
    /// </summary>
    public abstract class AsyncCrudCommonBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
               : AsyncCrudCommonBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
        // : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TUpdateInput>
        public AsyncCrudCommonBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
}
