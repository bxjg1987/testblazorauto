using Abp.Application.Services.Dto;
using Abp.Localization.Sources;
using BXJG.AbpBlazor.Components;
using BXJG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.Shared
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class AdminCreateBaseComponent<TAppService,
                                                   TEntityDto,
                                                   TPrimaryKey,
                                                   TGetAllInput,
                                                   TCreateInput,
                                                   TUpdateInput> : AbpCreateBaseComponent<TAppService,
                                                                                          TEntityDto,
                                                                                          TPrimaryKey,
                                                                                          TGetAllInput,
                                                                                          TCreateInput,
                                                                                          TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        // where TGetAllInput : new()
        where TCreateInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        #region 本地化
        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;
        /// <summary>
        /// 获取App.Common中的本地化源
        /// </summary>
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
        /// <summary>
        /// 获取App.Admin中的本地化源
        /// </summary>
        protected override string LocalizationSourceName => ZLJ.App.Admin.AdminConsts.Admin;
        #endregion
    }
}