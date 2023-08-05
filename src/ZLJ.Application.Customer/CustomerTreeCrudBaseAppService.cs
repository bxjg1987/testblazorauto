using Abp.Localization.Sources;
using ZLJ.App.Customer.Sessions;

namespace ZLJ.App.Customer
{
    /// <summary>
    /// 树形数据的crud抽象应用服务
    /// </summary>
    public abstract class CustomerTreeCrudBaseAppService<TDto,
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
        public CustomerSession CustomerSession { get; set; }

        public CustomerTreeCrudBaseAppService()
        {
            LocalizationSourceName = CustConsts.Cust;
        }
    }
}
