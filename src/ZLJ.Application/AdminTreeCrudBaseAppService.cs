using Abp.Localization.Sources;
using ZLJ.App.Admin.Sessions;

namespace ZLJ.App.Admin
{
    /// <summary>
    /// 树形数据的crud抽象应用服务
    /// </summary>
    public abstract class AdminTreeCrudBaseAppService<TDto,
                                                      TCreateInput,
                                                      TEditDto,
                                                      TDeleteInput,
                                                      TGetAllInput,
                                                      TGetInput,
                                                      TMoveInput,
                                                      TEntity,
                                                      TManager> : CommonTreeCrudBaseAppService<TDto,
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
        public AdminSession AdminSession { get; set; }

        public AdminTreeCrudBaseAppService()
        {
            LocalizationSourceName = AdminConsts.Admin;
        }
    }
}
