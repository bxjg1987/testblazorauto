using Abp.Localization.Sources;
using BXJG.Utils.Application;
using BXJG.Utils.Share;
using ZLJ.Application.Common.Share;
using ZLJ.Core;

namespace ZLJ.Application.Common
{
    /// <summary>
    /// 抽象的为其它功能提供可选数据的接口
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    /// <typeparam name="TGetAllInput">查询时输入参数的类型</typeparam>
    /// <typeparam name="TEntityDto">可选数据的dto</typeparam>
    public class CommonProviderBaseAppService<TEntity,
                                              TGetAllInput,
                                              TEntityDto,
                                              TKey> : ProviderBaseAppService<TEntity,
                                                                             TGetAllInput,
                                                                             TEntityDto,
                                                                             TKey>, IProviderBaseAppService<TGetAllInput,
                                                                                                            TEntityDto,
                                                                                                            TKey>
      where TEntity : class, IEntity<TKey>
    {
        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;

        public CommonProviderBaseAppService()
        {
            LocalizationSourceName = Application.Common.Consts.Common;
        }

        protected virtual ILocalizationSource LocalizationSourceAppCommon
        {
            get
            {
                if (appCommonLocalizationSource == null || appCommonLocalizationSource.Name != Application.Common.Consts.Common)
                {
                    appCommonLocalizationSource = LocalizationManager.GetSource(Application.Common.Consts.Common);
                }

                return appCommonLocalizationSource;
            }
        }
        protected virtual ILocalizationSource LocalizationSourceAppZLJ
        {
            get
            {

                if (zljLocalizationSource == null || zljLocalizationSource.Name != ZLJ.Core.Share.ZLJConsts.LocalizationSourceName)
                {
                    zljLocalizationSource = LocalizationManager.GetSource(ZLJ.Core.Share.ZLJConsts.LocalizationSourceName);
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
    }
}
