using Abp.Localization.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Common.Administrative;
using ZLJ.BaseInfo.Administrative;

namespace ZLJ.App.Common
{
    /// <summary>
    /// 树形数据作为下拉框或弹窗中的可选数据的通用服务基类
    /// </summary>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    /// <typeparam name="TGetNodesForSelectInput"></typeparam>
    /// <typeparam name="TGetNodesForSelectOutput"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class CommonTreeProviderBaseAppService<TGetTreeForSelectInput,
                                                  TGetTreeForSelectOutput,
                                                  TGetNodesForSelectInput,
                                                  TGetNodesForSelectOutput,
                                                  TEntity> : UnAuthGeneralTreeAppServiceBase<TGetTreeForSelectInput,
                                                                                             TGetTreeForSelectOutput,
                                                                                             TGetNodesForSelectInput,
                                                                                             TGetNodesForSelectOutput,
                                                                                             TEntity>
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput 
        where TGetTreeForSelectOutput : GeneralTreeNodeDto<TGetTreeForSelectOutput>, new() 
        where TGetNodesForSelectInput : GeneralTreeGetForSelectInput 
        where TGetNodesForSelectOutput : GeneralTreeComboboxDto, new() 
        where TEntity : GeneralTreeEntity<TEntity>
    {
        protected ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;
        public CommonTreeProviderBaseAppService(IRepository<TEntity, long> repository, string allTextForSearch = "不限", string allTextForForm = "请选择") : base(repository, allTextForSearch, allTextForForm)
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
    }
}
