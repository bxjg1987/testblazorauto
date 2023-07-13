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
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ.App.Customer
{
    /// <summary>
    /// crud的后台管理基类
    /// </summary>
    public class AsyncCrudCustomerBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
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

        protected ILocalizationSource LocalizationSourceAppCommon
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
        protected ILocalizationSource LocalizationSourceAppZLJ
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
        protected ILocalizationSource LocalizationSourceUtils
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

        public AsyncCrudCustomerBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
            LocalizationSourceName = CustConsts.Cust;

        }
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }

    /// <summary>
    /// crud的后台管理基类
    /// </summary>
    public class AsyncCrudCustomerBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
               : AsyncCrudCustomerBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        public AsyncCrudCustomerBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
    /// <summary>
    /// crud的后台管理基类
    /// </summary>
    public class AsyncCrudCustomerBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
               : AsyncCrudCustomerBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        public AsyncCrudCustomerBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// crud的后台管理基类
    /// </summary>
    public class AsyncCrudCustomerBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
               : AsyncCrudCustomerBaseAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
        // : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TUpdateInput>
        public AsyncCrudCustomerBaseAppService(IRepository<TEntity, TPrimaryKey> repository) : base(repository)
        {
        }
    }
}
