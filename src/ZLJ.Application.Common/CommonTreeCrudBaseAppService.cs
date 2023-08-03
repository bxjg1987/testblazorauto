using Abp.Localization.Sources;

namespace ZLJ.App.Common
{
    /// <summary>
    /// 树形数据的crud抽象应用服务
    /// </summary>
    public class CommonTreeCrudBaseAppService<TDto,
                                        TCreateInput,
                                        TEditDto,
                                        TDeleteInput,
                                        TGetAllInput,
                                        TGetInput,
                                        TMoveInput,
                                        TEntity,
                                        TManager> : GeneralTreeAppServiceBase<TDto,
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
        protected ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;


        public CommonTreeCrudBaseAppService(IRepository<TEntity, long> ownRepository,
                                      TManager manager, 
                                      string createPermissionName = null,
                                      string updatePermissionName = null,
                                      string deletePermissionName = null,
                                      string getPermissionName = null, 
                                      string allTextForManager = "全部") : base(ownRepository,
                                                                                manager,
                                                                                createPermissionName,
                                                                                updatePermissionName,
                                                                                deletePermissionName, 
                                                                                getPermissionName, 
                                                                                allTextForManager)
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
