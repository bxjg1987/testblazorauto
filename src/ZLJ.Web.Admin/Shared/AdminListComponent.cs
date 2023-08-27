using Abp.Application.Services.Dto;
using Abp.Localization.Sources;
using BXJG.MudBlazor.Components;
using BXJG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.Shared
{
    /// <summary>
    /// 后台管理 crud中的列表页
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    /// <typeparam name="TFormComponent"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class AdminListComponent<TEntityDto,
                                    TPrimaryKey,
                                    TGetAllInput,
                                    TCreateInput,
                                    TUpdateInput,
                                    TGetInput,
                                    TDeleteInput,
                                    TFormComponent,
                                    TAppService> : AbpMudBlazorListBaseComponent<TEntityDto,
                                                                                 TPrimaryKey,
                                                                                 TGetAllInput,
                                                                                 TCreateInput,
                                                                                 TUpdateInput,
                                                                                 TGetInput,
                                                                                 TDeleteInput,
                                                                                 TFormComponent,
                                                                                 TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TGetInput : IEntityDto<TPrimaryKey>
        where TFormComponent : ComponentBase
        where TDeleteInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>

    {
        #region 本地化
        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;
        protected virtual ILocalizationSource LocalizationSourceAppCommon
        {
            get
            {
                if (appCommonLocalizationSource == null || appCommonLocalizationSource.Name != ZLJ.App.Common.Consts.Common)
                {
                    appCommonLocalizationSource = LocalizationManager.GetSource(ZLJ.App.Common.Consts.Common);
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
        protected override string LocalizationSourceName => ZLJ.App.Admin.AdminConsts.Admin;
        #endregion
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
    /// <typeparam name="TFormComponent"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class AdminListComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TUpdateInput,
                                   TGetInput,
                                   TFormComponent,
                                   TAppService> : AdminListComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TUpdateInput,
                                                                    TGetInput,
                                                                    EntityDto<TPrimaryKey>,
                                                                    TFormComponent,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TFormComponent : ComponentBase
        where TGetAllInput : new()
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
    /// <typeparam name="TFormComponent"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class AdminListComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TUpdateInput,
                                   TFormComponent,
                                   TAppService> : AdminListComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TUpdateInput,
                                                                    EntityDto<TPrimaryKey>,
                                                                    TFormComponent,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TFormComponent : ComponentBase
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
    /// <typeparam name="TFormComponent"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class AdminListComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TCreateInput,
                                   TFormComponent,
                                   TAppService> : AdminListComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TCreateInput,
                                                                    TCreateInput,
                                                                    TFormComponent,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TFormComponent : ComponentBase
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TFormComponent"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class AdminListComponent<TEntityDto,
                                   TPrimaryKey,
                                   TGetAllInput,
                                   TFormComponent,
                                   TAppService> : AdminListComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    TGetAllInput,
                                                                    TEntityDto,
                                                                    TFormComponent,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TFormComponent : ComponentBase
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TFormComponent"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class AdminListComponent<TEntityDto,
                                   TPrimaryKey,
                                   TFormComponent,
                                   TAppService> : AdminListComponent<TEntityDto,
                                                                    TPrimaryKey,
                                                                    PagedAndSortedResultRequestDto,
                                                                    TFormComponent,
                                                                    TAppService>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TFormComponent : ComponentBase
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey>
    {
    }
    /// <summary>
    /// 抽象的crud组件，用于简化crud组件的开发
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TFormComponent"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class AdminListComponent<TEntityDto,
                                   TFormComponent,
                                   TAppService> : AdminListComponent<TEntityDto,
                                                                    int,
                                                                    TFormComponent,
                                                                    TAppService>
        where TEntityDto : IEntityDto<int>
        where TFormComponent : ComponentBase
        where TAppService : ICrudBaseAppService<TEntityDto>
    {
    }
}
