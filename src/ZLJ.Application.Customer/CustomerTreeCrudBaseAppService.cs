using Abp.Localization.Sources;
using ZLJ.App.Customer.Sessions;

namespace ZLJ.App.Customer
{
    /// <summary>
    /// 客户 管理 树形数据 的crud应用服务 抽象类
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TDto">列表和详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TDeleteInput">批量删除输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    /// <typeparam name="TGetInput">获取单个数据的输入模型</typeparam>
    /// <typeparam name="TMoveInput">移动节点的输入模型</typeparam>
    /// <typeparam name="TManager">领域服务模型</typeparam>
    public class CustomerTreeCrudBaseAppService<TEntity,
                                                TDto,
                                                TCreateInput,
                                                TEditDto,
                                                TGetAllInput,
                                                TDeleteInput,
                                                TGetInput,
                                                TMoveInput,
                                                TManager> : CommonTreeCrudBaseAppService<TEntity,
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
        

        public CustomerTreeCrudBaseAppService()
        {
            LocalizationSourceName = CustConsts.Cust;
        }
    }

    /// <summary>
    /// 客户 管理 树形数据 的crud应用服务 抽象类
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TDto">列表和详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TDeleteInput">批量删除输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    /// <typeparam name="TGetInput">获取单个数据的输入模型</typeparam>
    /// <typeparam name="TMoveInput">移动节点的输入模型</typeparam>
    public class CustomerTreeCrudBaseAppService<TEntity,
                                                TDto,
                                                TCreateInput,
                                                TEditDto,
                                                TGetAllInput,
                                                TDeleteInput,
                                                TGetInput,
                                                TMoveInput> : CustomerTreeCrudBaseAppService<TEntity,
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
    /// 客户 管理 树形数据 的crud应用服务 抽象类
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TDto">列表和详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TDeleteInput">批量删除输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    /// <typeparam name="TGetInput">获取单个数据的输入模型</typeparam>
    public class CustomerTreeCrudBaseAppService<TEntity,
                                                TDto,
                                                TCreateInput,
                                                TEditDto,
                                                TGetAllInput,
                                                TDeleteInput,
                                                TGetInput> : CustomerTreeCrudBaseAppService<TEntity,
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
    /// 客户 管理 树形数据 的crud应用服务 抽象类
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TDto">列表和详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TDeleteInput">批量删除输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    public class CustomerTreeCrudBaseAppService<TEntity,
                                                TDto,
                                                TCreateInput,
                                                TEditDto,
                                                TGetAllInput,
                                                TDeleteInput> : CustomerTreeCrudBaseAppService<TEntity,
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
    /// 客户 管理 树形数据 的crud应用服务 抽象类
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TDto">列表和详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">批量删除输入模型</typeparam>
    public class CustomerTreeCrudBaseAppService<TEntity,
                                                TDto,
                                                TCreateInput,
                                                TEditDto,
                                                TGetAllInput> : CustomerTreeCrudBaseAppService<TEntity,
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
    /// 客户 管理 树形数据 的crud应用服务 抽象类
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TDto">列表和详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    public class CustomerTreeCrudBaseAppService<TEntity,
                                                TDto,
                                                TCreateInput,
                                                TEditDto> : CustomerTreeCrudBaseAppService<TEntity,
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
    /// 客户 管理 树形数据 的crud应用服务 抽象类
    /// </summary>
    /// <typeparam name="TEntity">实体类</typeparam>
    /// <typeparam name="TDto">列表和详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    public class CustomerTreeCrudBaseAppService<TEntity,
                                                TDto,
                                                TCreateInput> : CustomerTreeCrudBaseAppService<TEntity,
                                                                                               TDto,
                                                                                               TCreateInput,
                                                                                               TCreateInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                         TCreateInput>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEntity : GeneralTreeEntity<TEntity>
    { }
}
