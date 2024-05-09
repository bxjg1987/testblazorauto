using Abp.Localization.Sources;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;


namespace ZLJ.Application
{
    /// <summary>
    /// 后台管理 树形结构的数据 抽象应用服务 
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDto">查询列表或详情返回的模型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入模型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    /// <typeparam name="TManager">领域服务类型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
                                             TDto,
                                             TCreateInput,
                                             TEditDto,
                                             TGetAllInput,
                                             TManager> : CommonTreeCrudBaseAppService<TEntity,
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
    /// <typeparam name="TGetAllInput">获取列表时的输入模型</typeparam>
    public class AdminTreeCrudBaseAppService<TEntity,
                                             TDto,
                                             TCreateInput,
                                             TEditDto,
                                             TGetAllInput> : AdminTreeCrudBaseAppService<TEntity,
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
