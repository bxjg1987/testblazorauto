using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Host.Shared
{
    public class AdminGeneralTreeDetailOrUpdateBaseComponent<TAppService,
                                                             TEntityDto,
                                                             TUpdateInput,
                                                             TCreateInput,
                                                             TGetAllInput> : AbpMudGeneralTreeDetailOrUpdateDialogBaseComponent<TAppService,
                                                                                                                                TEntityDto,
                                                                                                                                TUpdateInput,
                                                                                                                                TCreateInput,
                                                                                                                                TGetAllInput>
        where TEntityDto : IGeneralTree<TEntityDto>//, IExtendableDto
        where TUpdateInput : IHaveParentId<long> //IGeneralTree<TEntityDto>
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TUpdateInput, TGetAllInput>
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