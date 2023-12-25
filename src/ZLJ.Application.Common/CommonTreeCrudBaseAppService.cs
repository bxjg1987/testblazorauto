using Abp.Localization.Sources;

namespace ZLJ.App.Common
{
    /// <summary>
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TMoveInput"></typeparam>
    /// <typeparam name="TManager"></typeparam>
    public class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput,
                                              TEditDto,
                                              TGetAllInput,
                                              TDeleteInput,
                                              TGetInput,
                                              TMoveInput,
                                              TManager> : GeneralTreeBaseAppService<TEntity,
                                                                                    TDto,
                                                                                    TCreateInput,
                                                                                    TEditDto,
                                                                                    TGetAllInput,
                                                                                    TDeleteInput,
                                                                                    TGetInput,
                                                                                    TMoveInput,
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
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TMoveInput"></typeparam>
    public class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput,
                                              TEditDto,
                                              TGetAllInput,
                                              TDeleteInput,
                                              TGetInput,
                                              TMoveInput> : CommonTreeCrudBaseAppService<TEntity,
                                                                                         TDto,
                                                                                         TCreateInput,
                                                                                         TEditDto,
                                                                                         TGetAllInput,
                                                                                         TDeleteInput,
                                                                                         TGetInput,
                                                                                         TMoveInput,
                                                                                         GeneralTreeManager<TEntity>>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                                  TCreateInput,
                                                                                                                                                  TEditDto,
                                                                                                                                                  TGetAllInput,
                                                                                                                                                  TDeleteInput,
                                                                                                                                                  TGetInput,
                                                                                                                                                  TMoveInput>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TDeleteInput : BatchOperationInputLong
        where TGetAllInput : GeneralTreeGetTreeInput
        where TGetInput : EntityDto<long>
        where TMoveInput : GeneralTreeNodeMoveInput
        where TEntity : GeneralTreeEntity<TEntity>
    { }


    /// <summary>
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    public class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput,
                                              TEditDto,
                                              TGetAllInput,
                                              TDeleteInput,
                                              TGetInput> : CommonTreeCrudBaseAppService<TEntity,
                                                                                        TDto,
                                                                                        TCreateInput,
                                                                                        TEditDto,
                                                                                        TGetAllInput,
                                                                                        TDeleteInput,
                                                                                        TGetInput,
                                                                                        GeneralTreeNodeMoveInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                              TCreateInput,
                                                                                                                                              TEditDto,
                                                                                                                                              TGetAllInput,
                                                                                                                                              TDeleteInput,
                                                                                                                                              TGetInput>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TDeleteInput : BatchOperationInputLong
        where TGetAllInput : GeneralTreeGetTreeInput
        where TGetInput : EntityDto<long>
        where TEntity : GeneralTreeEntity<TEntity>
    { }

    /// <summary>
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TDto">列表和详情的显示模型</typeparam>
    /// <typeparam name="TCreateInput">新增模型</typeparam>
    /// <typeparam name="TEditDto">修改模型</typeparam>
    /// <typeparam name="TDeleteInput">删除时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    public class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput,
                                              TEditDto,
                                              TGetAllInput,
                                              TDeleteInput> : CommonTreeCrudBaseAppService<TEntity,
                                                                                           TDto,
                                                                                           TCreateInput,
                                                                                           TEditDto,
                                                                                           TGetAllInput,
                                                                                           TDeleteInput,
                                                                                           EntityDto<long>>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                        TCreateInput,
                                                                                                                                        TEditDto,
                                                                                                                                        TGetAllInput,
                                                                                                                                        TDeleteInput>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TDeleteInput : BatchOperationInputLong
        where TGetAllInput : GeneralTreeGetTreeInput
        where TEntity : GeneralTreeEntity<TEntity>
    { }

    /// <summary>
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TDto">列表和详情的显示模型</typeparam>
    /// <typeparam name="TCreateInput">新增模型</typeparam>
    /// <typeparam name="TEditDto">修改模型</typeparam>
    /// <typeparam name="TGetAllInput">删除时的输入模型</typeparam>
    public class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput,
                                              TEditDto,
                                              TGetAllInput> : CommonTreeCrudBaseAppService<TEntity,
                                                                                           TDto,
                                                                                           TCreateInput,
                                                                                           TEditDto,
                                                                                           TGetAllInput,
                                                                                         BatchOperationInputLong>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                                TCreateInput,
                                                                                                                                                TEditDto,
                                                                                                                                                TGetAllInput>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
          where TGetAllInput : GeneralTreeGetTreeInput
        where TEntity : GeneralTreeEntity<TEntity>
    { }
    /// <summary>
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TDto">列表和详情的显示模型</typeparam>
    /// <typeparam name="TCreateInput">新增模型</typeparam>
    /// <typeparam name="TEditDto">修改模型</typeparam>
    public class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput,
                                              TEditDto> : CommonTreeCrudBaseAppService<TEntity,
                                                                                       TDto,
                                                                                       TCreateInput,
                                                                                       TEditDto,
                                                                                         GeneralTreeGetTreeInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                            TCreateInput,
                                                                                                                                            TEditDto>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TEntity : GeneralTreeEntity<TEntity>
    { }

    /// <summary>
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TDto">列表和详情的显示模型</typeparam>
    /// <typeparam name="TCreateInput">新增或修改的模型</typeparam>
    public class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput> : CommonTreeCrudBaseAppService<TEntity,
                                                                                           TDto,
                                                                                           TCreateInput,
                                                                                           TCreateInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                     TCreateInput>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEntity : GeneralTreeEntity<TEntity>
    { }

    ///// <summary>
    ///// 通用树形数据的crud抽象应用服务（常用）
    ///// </summary>
    ///// <typeparam name="TDto"></typeparam>
    ///// <typeparam name="TCreateInput"></typeparam>
    ///// <typeparam name="TEditDto"></typeparam>
    ///// <typeparam name="TGetAllInput"></typeparam>
    ///// <typeparam name="TEntity"></typeparam>
    //public abstract class CommonTreeCrudBaseAppService<TEntity,
    //                                                   TDto,
    //                                                   TCreateInput,
    //                                                   TEditDto,
    //                                                   TGetAllInput> : CommonTreeCrudBaseAppService<TEntity,
    //                                                                                         TDto,
    //                                                                                         TCreateInput,
    //                                                                                         TEditDto,
    //                                                                                         BatchOperationInputLong,
    //                                                                                         TGetAllInput,
    //                                                                                         EntityDto<long>,
    //                                                                                         GeneralTreeNodeMoveInput,
    //                                                                                        GeneralTreeManager<TEntity>>
    //    where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
    //    where TCreateInput : GeneralTreeNodeEditBaseDto
    //    where TEditDto : GeneralTreeNodeEditBaseDto
    //    where TGetAllInput : GeneralTreeGetTreeInput
    //    where TEntity : GeneralTreeEntity<TEntity>
    //{ }
}
