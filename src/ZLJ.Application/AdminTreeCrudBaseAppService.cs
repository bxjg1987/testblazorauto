using Abp.Localization.Sources;
using BXJG.Utils.Application.Share.GeneralTree;


namespace ZLJ.Application.Admin
{
    /// <summary>
    /// 后台管理 树形结构的数据 抽象应用服务 
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">查询列表或详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TDeleteInput">批量删除时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    /// <typeparam name="TGetInput">获取单条数据时的输入模型</typeparam>
    /// <typeparam name="TMoveInput">移动节点时的输入模型</typeparam>
    /// <typeparam name="TManager">领域服务类型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
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
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TDeleteInput : BatchOperationInputLong
        where TGetAllInput : GeneralTreeGetTreeInput
        where TGetInput : EntityDto<long>
        where TMoveInput : GeneralTreeNodeMoveInput
        where TEntity : GeneralTreeEntity<TEntity>
        where TManager : GeneralTreeManager<TEntity>
    {

        public AdminTreeCrudBaseAppService()
        {
            LocalizationSourceName = AdminConsts.Admin;
        }
    }

    /// <summary>
    /// 后台管理 树形结构的数据 抽象应用服务 
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">查询列表或详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TDeleteInput">批量删除时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    /// <typeparam name="TGetInput">获取单条数据时的输入模型</typeparam>
    /// <typeparam name="TMoveInput">移动节点时的输入模型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
                                             TDto,
                                             TCreateInput,
                                             TEditDto,
                                             TGetAllInput,
                                             TDeleteInput,
                                             TGetInput,
                                             TMoveInput> : AdminTreeCrudBaseAppService<TEntity,
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
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TDeleteInput : BatchOperationInputLong
        where TGetAllInput : GeneralTreeGetTreeInput
        where TGetInput : EntityDto<long>
        where TMoveInput : GeneralTreeNodeMoveInput
        where TEntity : GeneralTreeEntity<TEntity>
    {
    }

    /// <summary>
    /// 后台管理 树形结构的数据 抽象应用服务 
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">查询列表或详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TDeleteInput">批量删除时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    /// <typeparam name="TGetInput">获取单条数据时的输入模型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
                                             TDto,
                                             TCreateInput,
                                             TEditDto,
                                             TGetAllInput,
                                             TDeleteInput,
                                             TGetInput> : AdminTreeCrudBaseAppService<TEntity,
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
        where TEntity : GeneralTreeEntity<TEntity>
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
        where TCreateInput : GeneralTreeNodeEditBaseDto
        where TEditDto : GeneralTreeNodeEditBaseDto
        where TDeleteInput : BatchOperationInputLong
        where TGetAllInput : GeneralTreeGetTreeInput
        where TGetInput : EntityDto<long>
    {
    }


    /// <summary>
    /// 后台管理 树形结构的数据 抽象应用服务 
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">查询列表或详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TDeleteInput">批量删除时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
                                             TDto,
                                             TCreateInput,
                                             TEditDto,
                                             TGetAllInput,
                                             TDeleteInput> : AdminTreeCrudBaseAppService<TEntity,
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
            where TEntity : GeneralTreeEntity<TEntity>
            where TDto : GeneralTreeNodeBaseDto<TDto>, new()
            where TCreateInput : GeneralTreeNodeEditBaseDto
            where TEditDto : GeneralTreeNodeEditBaseDto
            where TDeleteInput : BatchOperationInputLong
            where TGetAllInput : GeneralTreeGetTreeInput
    {
    }

    /// <summary>
    /// 后台管理 树形结构的数据 抽象应用服务 
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">查询列表或详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">批量删除时的输入模型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
                                             TDto,
                                             TCreateInput,
                                             TEditDto,
                                             TGetAllInput> : AdminTreeCrudBaseAppService<TEntity,
                                                                                         TDto,
                                                                                         TCreateInput,
                                                                                         TEditDto,
                                                                                         TGetAllInput,
                                                                                       BatchOperationInputLong>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                      TCreateInput,
                                                                                                                                      TEditDto,
                                                                                                                                      TGetAllInput>
            where TEntity : GeneralTreeEntity<TEntity>
            where TDto : GeneralTreeNodeBaseDto<TDto>, new()
            where TCreateInput : GeneralTreeNodeEditBaseDto
            where TEditDto : GeneralTreeNodeEditBaseDto
            where TGetAllInput : GeneralTreeGetTreeInput
    {
    }

    /// <summary>
    /// 后台管理 树形结构的数据 抽象应用服务 
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">查询列表或详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
                                             TDto,
                                             TCreateInput,
                                             TEditDto> : AdminTreeCrudBaseAppService<TEntity,
                                                                                         TDto,
                                                                                         TCreateInput,
                                                                                         TEditDto,
                                                                                           GeneralTreeGetTreeInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                      TCreateInput,
                                                                                                                                      TEditDto>
            where TEntity : GeneralTreeEntity<TEntity>
            where TDto : GeneralTreeNodeBaseDto<TDto>, new()
            where TCreateInput : GeneralTreeNodeEditBaseDto
            where TEditDto : GeneralTreeNodeEditBaseDto
    {
    }
    /// <summary>
    /// 后台管理 树形结构的数据 抽象应用服务 
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">查询列表或详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
                                             TDto,
                                             TCreateInput> : AdminTreeCrudBaseAppService<TEntity,
                                                                                         TDto,
                                                                                         TCreateInput,
                                                                                         TCreateInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                      TCreateInput>
            where TEntity : GeneralTreeEntity<TEntity>
            where TDto : GeneralTreeNodeBaseDto<TDto>, new()
            where TCreateInput : GeneralTreeNodeEditBaseDto
    {
    }


    ///// <summary>
    ///// 通用树形数据的crud抽象应用服务（常用）
    ///// </summary>
    ///// <typeparam name="TDto"></typeparam>
    ///// <typeparam name="TCreateInput"></typeparam>
    ///// <typeparam name="TEditDto"></typeparam>
    ///// <typeparam name="TGetAllInput"></typeparam>
    ///// <typeparam name="TEntity"></typeparam>
    //public class AdminTreeCrudBaseAppService<
    //                                                  TEntity, TDto,
    //                                                  TCreateInput,
    //                                                  TEditDto,
    //                                                  TGetAllInput> : AdminTreeCrudBaseAppService<TEntity,
    //                                                                                           TDto,
    //                                                                                           TCreateInput,
    //                                                                                           TEditDto,
    //                                                                                           BatchOperationInputLong,
    //                                                                                           TGetAllInput,
    //                                                                                           EntityDto<long>,
    //                                                                                           GeneralTreeNodeMoveInput,
    //                                                                                          GeneralTreeManager<TEntity>>
    //    where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
    //    where TCreateInput : GeneralTreeNodeEditBaseDto
    //    where TEditDto : GeneralTreeNodeEditBaseDto
    //    where TGetAllInput : GeneralTreeGetTreeInput
    //    where TEntity : GeneralTreeEntity<TEntity>
    //{ }
}
