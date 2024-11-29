using Abp.Localization.Sources;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.GeneralTree;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Share;

namespace ZLJ.Application.Common
{
    /// <summary>
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TManager"></typeparam>
    public abstract class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput,
                                              TEditDto,
                                              TGetAllInput,
                                              TManager> : GeneralTreeBaseAppService<TEntity,
                                                                                    TDto,
                                                                                    TCreateInput,
                                                                                    TEditDto,
                                                                                    TGetAllInput,
                                                                                    TManager>
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TGetAllInput : GeneralTreeGetTreeInput
        where TEntity : GeneralTreeEntity<TEntity>
        where TManager : GeneralTreeManager<TEntity>
    {
        private ILocalizationSource appCommonLocalizationSource, zljLocalizationSource, utilsLocalizationSource;

        public CommonTreeCrudBaseAppService()
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

    /// <summary>
    /// 树形数据的crud抽象应用服务（完整）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    public abstract class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput,
                                              TEditDto,
                                              TGetAllInput> : CommonTreeCrudBaseAppService<TEntity,
                                                                                         TDto,
                                                                                         TCreateInput,
                                                                                         TEditDto,
                                                                                         TGetAllInput,
                                                                                         GeneralTreeManager<TEntity>>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                                  TCreateInput,
                                                                                                                                                  TEditDto,
                                                                                                                                                  TGetAllInput>
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
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
    public abstract class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput,
                                              TEditDto> : CommonTreeCrudBaseAppService<TEntity,
                                                                                       TDto,
                                                                                       TCreateInput,
                                                                                       TEditDto,
                                                                                         GeneralTreeGetTreeInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                            TCreateInput,
                                                                                                                                            TEditDto>
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
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
    public abstract class CommonTreeCrudBaseAppService<TEntity,
                                              TDto,
                                              TCreateInput> : CommonTreeCrudBaseAppService<TEntity,
                                                                                           TDto,
                                                                                           TCreateInput,
                                                                                           TCreateInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                     TCreateInput>
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
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
