using Abp.Localization.Sources;

namespace ZLJ.App.Common
{
    /// <summary>
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    public abstract class CommonTreeCrudBaseAppService<TDto,
                                                       TCreateInput,
                                                       TEditDto,
                                                       TDeleteInput,
                                                       TGetAllInput,
                                                       TGetInput,
                                                       TMoveInput,
                                                       TEntity,
                                                       TManager> : GeneralTreeBaseAppService<TDto,
                                                                                             TCreateInput,
                                                                                             TEditDto,
                                                                                             TDeleteInput,
                                                                                             TGetAllInput,
                                                                                             TGetInput,
                                                                                             TMoveInput,
                                                                                             TEntity,
                                                                                             TManager>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TDeleteInput : BatchOperationInputLong
        where TGetAllInput : GeneralTreeGetTreeInput
        where TGetInput : EntityDto<long>
        where TMoveInput : GeneralTreeNodeMoveInput
        where TEntity : GeneralTreeEntity<TEntity>
        where TManager : GeneralTreeManager<TEntity>
    {
        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;

        public CommonTreeCrudBaseAppService()
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
    /// 通用树形数据的crud抽象应用服务（常用）
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class CommonTreeCrudBaseAppService<TDto,
                                                       TCreateInput,
                                                       TEditDto,
                                                       TGetAllInput,
                                                       TEntity> : CommonTreeCrudBaseAppService<TDto,
                                                                                             TCreateInput,
                                                                                             TEditDto,
                                                                                             BatchOperationInputLong,
                                                                                             TGetAllInput,
                                                                                             EntityDto<long>,
                                                                                             GeneralTreeNodeMoveInput,
                                                                                             TEntity,
                                                                                             GeneralTreeManager<TEntity>>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TGetAllInput : GeneralTreeGetTreeInput
        where TEntity : GeneralTreeEntity<TEntity>
    { }
    }
