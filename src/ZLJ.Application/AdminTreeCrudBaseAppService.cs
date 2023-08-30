using Abp.Localization.Sources;
using ZLJ.App.Admin.Sessions;

namespace ZLJ.App.Admin
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
                                             TDeleteInput,
                                             TGetAllInput,
                                             TGetInput,
                                             TMoveInput,
                                             TManager> : CommonTreeCrudBaseAppService<TEntity,
                                                                                      TDto,
                                                                                      TCreateInput,
                                                                                      TEditDto,
                                                                                      TDeleteInput,
                                                                                      TGetAllInput,
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
        public AdminSession AdminSession { get; set; }

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
                                             TDeleteInput,
                                             TGetAllInput,
                                             TGetInput,
                                             TMoveInput> : AdminTreeCrudBaseAppService<TEntity,
                                                                                       TDto,
                                                                                       TCreateInput,
                                                                                       TEditDto,
                                                                                       TDeleteInput,
                                                                                       TGetAllInput,
                                                                                       TGetInput,
                                                                                       TMoveInput,
                                                                                       GeneralTreeManager<TEntity>>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                                TCreateInput,
                                                                                                                                                TEditDto,
                                                                                                                                                TDeleteInput,
                                                                                                                                                TGetAllInput,
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
                                             TDeleteInput,
                                             TGetAllInput,
                                             TGetInput> : AdminTreeCrudBaseAppService<TEntity,
                                                                                      TDto,
                                                                                      TCreateInput,
                                                                                      TEditDto,
                                                                                      TDeleteInput,
                                                                                      TGetAllInput,
                                                                                      TGetInput,
                                                                                      GeneralTreeNodeMoveInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                            TCreateInput,
                                                                                                                                            TEditDto,
                                                                                                                                            TDeleteInput,
                                                                                                                                            TGetAllInput,
                                                                                                                                            TGetInput>
        where TEntity : GeneralTreeEntity<TEntity>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
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
                                             TDeleteInput,
                                             TGetAllInput> : AdminTreeCrudBaseAppService<TEntity,
                                                                                         TDto,
                                                                                         TCreateInput,
                                                                                         TEditDto,
                                                                                         TDeleteInput,
                                                                                         TGetAllInput,
                                                                                         EntityDto<long>>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                      TCreateInput,
                                                                                                                                      TEditDto,
                                                                                                                                      TDeleteInput,
                                                                                                                                      TGetAllInput>
            where TEntity : GeneralTreeEntity<TEntity>
            where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
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
    /// <typeparam name="TDeleteInput">批量删除时的输入模型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
                                             TDto,
                                             TCreateInput,
                                             TEditDto,
                                             TDeleteInput> : AdminTreeCrudBaseAppService<TEntity,
                                                                                         TDto,
                                                                                         TCreateInput,
                                                                                         TEditDto,
                                                                                         TDeleteInput,
                                                                                         GeneralTreeGetTreeInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                      TCreateInput,
                                                                                                                                      TEditDto,
                                                                                                                                      TDeleteInput>
            where TEntity : GeneralTreeEntity<TEntity>
            where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
            where TCreateInput : GeneralTreeNodeEditBaseDto
            where TEditDto : GeneralTreeNodeEditBaseDto
            where TDeleteInput : BatchOperationInputLong
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
                                                                                         BatchOperationInputLong>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                      TCreateInput,
                                                                                                                                      TEditDto>
            where TEntity : GeneralTreeEntity<TEntity>
            where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
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
            where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
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
