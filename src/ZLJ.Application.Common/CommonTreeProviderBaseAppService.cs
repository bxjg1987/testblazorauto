using Abp.Localization.Sources;
using BXJG.Utils.Application.Share.GeneralTree;
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
    public class CommonTreeProviderBaseAppService<TEntity,
                                                           TGetTreeForSelectInput,
                                                           TGetTreeForSelectOutput,
                                                           TGetNodesForSelectInput,
                                                           TGetNodesForSelectOutput> : GeneralTreeProviderBaseAppService<TEntity,
                                                                                                                         TGetTreeForSelectInput,
                                                                                                                         TGetTreeForSelectOutput,
                                                                                                                         TGetNodesForSelectInput,
                                                                                                                         TGetNodesForSelectOutput>
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
        where TGetTreeForSelectOutput : GeneralTreeNodeDto<TGetTreeForSelectOutput>, new()
        where TGetNodesForSelectInput : GeneralTreeGetForSelectInput
        where TGetNodesForSelectOutput : GeneralTreeComboboxDto, new()
        where TEntity : GeneralTreeEntity<TEntity>
    {
        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;

        public CommonTreeProviderBaseAppService()
        {
            LocalizationSourceName = App.Common.Consts.Common;

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

    /// <summary>
    /// 树形数据作为下拉框或弹窗中的可选数据的通用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    /// <typeparam name="TGetNodesForSelectInput"></typeparam>
    public class CommonTreeProviderBaseAppService<TEntity,
                                                           TGetTreeForSelectInput,
                                                           TGetTreeForSelectOutput,
                                                           TGetNodesForSelectInput> : CommonTreeProviderBaseAppService<TEntity,
                                                                                                                       TGetTreeForSelectInput,
                                                                                                                       TGetTreeForSelectOutput,
                                                                                                                       TGetNodesForSelectInput,
                                                                                                                       GeneralTreeComboboxDto>
        , IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput, TGetTreeForSelectOutput, TGetNodesForSelectInput>
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
        where TGetTreeForSelectOutput : GeneralTreeNodeDto<TGetTreeForSelectOutput>, new()
        where TGetNodesForSelectInput : GeneralTreeGetForSelectInput
        where TEntity : GeneralTreeEntity<TEntity>
    {
    }

    /// <summary>
    /// 树形数据作为下拉框或弹窗中的可选数据的通用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    public abstract class CommonTreeProviderBaseAppService<TEntity,
                                                           TGetTreeForSelectInput,
                                                           TGetTreeForSelectOutput> : CommonTreeProviderBaseAppService<TEntity,
                                                                                                                       TGetTreeForSelectInput,
                                                                                                                       TGetTreeForSelectOutput,
                                                                                                                       TGetTreeForSelectInput>
          , IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput, TGetTreeForSelectOutput>
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
        where TGetTreeForSelectOutput : GeneralTreeNodeDto<TGetTreeForSelectOutput>, new()
        where TEntity : GeneralTreeEntity<TEntity>
    {
    }

    /// <summary>
    /// 树形数据作为下拉框或弹窗中的可选数据的通用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    public abstract class CommonTreeProviderBaseAppService<TEntity,
                                                           TGetTreeForSelectInput> : CommonTreeProviderBaseAppService<TEntity,
                                                                                                                       TGetTreeForSelectInput,
                                                                                                                       GeneralTreeNodeDto>
            , IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput>
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
        where TEntity : GeneralTreeEntity<TEntity>
    {
    }
    /// <summary>
    /// 树形数据作为下拉框或弹窗中的可选数据的通用服务基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class CommonTreeProviderBaseAppService<TEntity> : CommonTreeProviderBaseAppService<TEntity,
                                                                                                       GeneralTreeGetForSelectInput>
         , IGeneralTreeProviderBaseAppService
        where TEntity : GeneralTreeEntity<TEntity>
    {
    }
}