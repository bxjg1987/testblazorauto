using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.GeneralTree
{
    /// <summary>
    /// 提供可选的树形或扁平化数据，通常作为下拉框和弹窗选择提供数据
    /// </summary>
    /// <typeparam name="TGetTreeForSelectInput">获取树形下拉框数据时的输入模型</typeparam>
    /// <typeparam name="TGetTreeForSelectOutput">获取树形下拉框数据时的输出模型</typeparam>
    /// <typeparam name="TGetNodesForSelectInput">获取扁平下拉框数据时的输入模型</typeparam>
    /// <typeparam name="TGetNodesForSelectOutput">获取扁平下拉框数据时的输出模型</typeparam>
    public interface IGeneralTreeProviderBaseAppService<in TGetTreeForSelectInput,
                                                        TGetTreeForSelectOutput,
                                                        in TGetNodesForSelectInput,
                                                        TGetNodesForSelectOutput> : IApplicationService
    {
        /// <summary>
        /// 获取简洁的树形数据，通常引用此数据的页面调用
        /// </summary>
        /// <param name="input">指定父节点，是否显示全部</param>
        /// <returns></returns>
        Task<IList<TGetTreeForSelectOutput>> GetTreeForSelectAsync(TGetTreeForSelectInput input);
        /// <summary>
        /// 获取指定父节点的子节点，以扁平结构返回
        /// </summary>
        /// <returns></returns>
        Task<IList<TGetNodesForSelectOutput>> GetNodesForSelectAsync(TGetNodesForSelectInput input);
    }

    /// <summary>
    /// 提供可选的树形或扁平化数据，通常作为下拉框和弹窗选择提供数据
    /// </summary>
    /// <typeparam name="TGetTreeForSelectInput">获取树形下拉框数据时的输入模型</typeparam>
    /// <typeparam name="TGetTreeForSelectOutput">获取树形下拉框数据时的输出模型</typeparam>
    /// <typeparam name="TGetNodesForSelectInput">获取扁平下拉框数据时的输入模型</typeparam>
    public interface IGeneralTreeProviderBaseAppService<in TGetTreeForSelectInput,
                                                        TGetTreeForSelectOutput,
                                                        in TGetNodesForSelectInput> : IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput,
                                                                                                                         TGetTreeForSelectOutput,
                                                                                                                         TGetNodesForSelectInput,
                                                                                                                         GeneralTreeComboboxDto>
    {

    }

    /// <summary>
    /// 提供可选的树形或扁平化数据，通常作为下拉框和弹窗选择提供数据
    /// </summary>
    /// <typeparam name="TGetTreeForSelectInput">获取树形下拉框数据时的输入模型</typeparam>
    /// <typeparam name="TGetTreeForSelectOutput">获取树形下拉框数据时的输出模型</typeparam>
    public interface IGeneralTreeProviderBaseAppService<in TGetTreeForSelectInput,
                                                        TGetTreeForSelectOutput> : IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput,
                                                                                                                      TGetTreeForSelectOutput,
                                                                                                                      TGetTreeForSelectInput>
    {

    }
    /// <summary>
    /// 提供可选的树形或扁平化数据，通常作为下拉框和弹窗选择提供数据
    /// </summary>
    /// <typeparam name="TGetTreeForSelectInput">获取树形下拉框数据时的输入模型</typeparam>
    public interface IGeneralTreeProviderBaseAppService<in TGetTreeForSelectInput> : IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput,
                                                                                                                     GeneralTreeNodeDto>
    {

    }
    /// <summary>
    /// 提供可选的树形或扁平化数据，通常作为下拉框和弹窗选择提供数据
    /// </summary>
    public interface IGeneralTreeProviderBaseAppService : IGeneralTreeProviderBaseAppService<GeneralTreeGetForSelectInput>
    {

    }


    /// <summary>
    /// 通用树形结构服务接口，其它树形接口应该继承此接口以获得树形结构数据的通用功能
    /// </summary>
    /// <typeparam name="TDto">管理页面显示的Dto类型</typeparam>
    /// <typeparam name="TCreateInput">新增模型</typeparam>
    /// <typeparam name="TEditDto">编辑模型</typeparam>
    /// <typeparam name="TDeleteInput">删除时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">管理页面获取所有树形数据时的输入模型</typeparam>
    /// <typeparam name="TGetInput">获取单个信息的输入模型</typeparam>
    /// <typeparam name="TMoveInput">移动节点时的输入模型</typeparam>
    public interface IGeneralTreeBaseAppService<TDto,
                                                in TCreateInput,
                                                in TEditDto,
                                                in TGetAllInput,
                                                in TDeleteInput,
                                                in TGetInput,
                                                in TMoveInput> : IApplicationService
    {
        /*
         * 返回列表都是IList abp官网的一般是IReadOnlyList，为了方便调用方进一步做处理 我们这里返回IList
         */

        /// <summary>
        /// 创建树形结构数据的节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TDto> CreateAsync(TCreateInput input);
        /// <summary>
        /// 更新指定节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TDto> UpdateAsync(TEditDto input);
        /// <summary>
        /// 删除指定节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<BatchOperationOutputLong> DeleteAsync(TDeleteInput input);
        /// <summary>
        /// 移动节点，服务端将自动重新生成所有兄弟节点的code
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TDto> MoveAsync(TMoveInput input);
        /// <summary>
        /// 获取所有数据，通常由展示的列表页面调用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IList<TDto>> GetAllAsync(TGetAllInput input);
        /// <summary>
        /// 获取指定节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TDto> GetAsync(TGetInput input);
    }


    /// <summary>
    /// 通用树形结构服务接口，其它树形接口应该继承此接口以获得树形结构数据的通用功能
    /// </summary>
    /// <typeparam name="TDto">管理页面显示的Dto类型</typeparam>
    /// <typeparam name="TCreateInput">新增模型</typeparam>
    /// <typeparam name="TEditDto">编辑模型</typeparam>
    /// <typeparam name="TDeleteInput">删除时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">管理页面获取所有树形数据时的输入模型</typeparam>
    /// <typeparam name="TGetInput">获取单个信息的输入模型</typeparam>
    public interface IGeneralTreeBaseAppService<TDto,
                                                in TCreateInput,
                                                in TEditDto,
                                                in TGetAllInput,
                                                in TDeleteInput,
                                                in TGetInput> : IGeneralTreeBaseAppService<TDto,
                                                                                           TCreateInput,
                                                                                           TEditDto,
                                                                                           TGetAllInput,
                                                                                           TDeleteInput,
                                                                                           TGetInput,
                                                                                           GeneralTreeNodeMoveInput>
    { }

    /// <summary>
    /// 通用树形结构服务接口，其它树形接口应该继承此接口以获得树形结构数据的通用功能
    /// </summary>
    /// <typeparam name="TDto">管理页面显示的Dto类型</typeparam>
    /// <typeparam name="TCreateInput">新增模型</typeparam>
    /// <typeparam name="TEditDto">编辑模型</typeparam>
    /// <typeparam name="TDeleteInput">删除时的输入模型</typeparam>
    /// <typeparam name="TGetAllInput">管理页面获取所有树形数据时的输入模型</typeparam>
    public interface IGeneralTreeBaseAppService<TDto,
                                                in TCreateInput,
                                                in TEditDto,
                                                in TGetAllInput,
                                                in TDeleteInput> : IGeneralTreeBaseAppService<TDto,
                                                                                              TCreateInput,
                                                                                              TEditDto,
                                                                                              TGetAllInput,
                                                                                              TDeleteInput,
                                                                                              EntityDto<long>>
    { }


    /// <summary>
    /// 通用树形结构服务接口，其它树形接口应该继承此接口以获得树形结构数据的通用功能
    /// </summary>
    /// <typeparam name="TDto">管理页面显示的Dto类型</typeparam>
    /// <typeparam name="TCreateInput">新增模型</typeparam>
    /// <typeparam name="TEditDto">编辑模型</typeparam>
    /// <typeparam name="TGetAllInput">删除时的输入模型</typeparam>
    public interface IGeneralTreeBaseAppService<TDto,
                                                in TCreateInput,
                                                in TEditDto,
                                                in TGetAllInput> : IGeneralTreeBaseAppService<TDto,
                                                                                              TCreateInput,
                                                                                              TEditDto,
                                                                                              TGetAllInput,
                                                                                              BatchOperationInputLong>
    { }
    /// <summary>
    /// 通用树形结构服务接口，其它树形接口应该继承此接口以获得树形结构数据的通用功能
    /// </summary>
    /// <typeparam name="TDto">管理页面显示的Dto类型</typeparam>
    /// <typeparam name="TCreateInput">新增模型</typeparam>
    /// <typeparam name="TEditDto">编辑模型</typeparam>
    public interface IGeneralTreeBaseAppService<TDto,
                                                in TCreateInput,
                                                in TEditDto> : IGeneralTreeBaseAppService<TDto,
                                                                                          TCreateInput,
                                                                                          TEditDto,
                                                                                          GeneralTreeGetTreeInput>
    { }
    /// <summary>
    /// 通用树形结构服务接口，其它树形接口应该继承此接口以获得树形结构数据的通用功能
    /// </summary>
    /// <typeparam name="TDto">管理页面显示的Dto类型</typeparam>
    /// <typeparam name="TCreateInput">新增模型</typeparam>
    public interface IGeneralTreeBaseAppService<TDto,
                                                in TCreateInput> : IGeneralTreeBaseAppService<TDto,
                                                                                              TCreateInput,
                                                                                              TCreateInput>
    { }
    /// <summary>
    /// 通用树形结构服务接口，其它树形接口应该继承此接口以获得树形结构数据的通用功能
    /// </summary>
    /// <typeparam name="TDto">管理页面显示的Dto类型</typeparam>
    public interface IGeneralTreeBaseAppService<TDto> : IGeneralTreeBaseAppService<TDto,
                                                                                   TDto>
    { }
    ///// <summary>
    ///// 通用树形结构服务接口，其它树形接口应该继承此接口以获得树形结构数据的通用功能
    ///// </summary>
    //public interface IGeneralTreeBaseAppService : IGeneralTreeBaseAppService<GeneralTreeGetTreeNodeBaseDto<GeneralTreeGetTreeNodeBaseDto>>
    //{ }

    ///// <summary>
    ///// 通用树形结构服务接口，其它树形接口应该继承此接口以获得树形结构数据的通用功能
    ///// </summary>
    ///// <typeparam name="TDto">管理页面显示的Dto类型</typeparam>
    ///// <typeparam name="TEditDto">编辑模型</typeparam>
    ///// <typeparam name="TGetAllInput">管理页面获取所有树形数据时的输入模型</typeparam>
    ///// <typeparam name="TGetTreeForSelectInput">获取树形下拉框数据时的输入模型</typeparam>
    ///// <typeparam name="TGetTreeForSelectOutput">获取树形下拉框数据时的输出模型</typeparam>
    ///// <typeparam name="TGetNodesForSelectInput">获取扁平下拉框数据时的输入模型</typeparam>
    ///// <typeparam name="TGetNodesForSelectOutput">获取扁平下拉框数据时的输出模型</typeparam>
    ///// <typeparam name="TMoveInput">移动节点时的输入模型</typeparam>
    //[Obsolete(message:"请使用另一个泛型版本")]
    //public interface IGeneralTreeAppServiceBase<TDto,
    //                                            TEditDto,
    //                                            TGetAllInput,
    //                                            TGetTreeForSelectInput,
    //                                            TGetTreeForSelectOutput,
    //                                            TGetNodesForSelectInput,
    //                                            TGetNodesForSelectOutput,
    //                                            TMoveInput> : IUnAuthGeneralTreeAppServiceBase<TGetTreeForSelectInput,
    //                                                                                           TGetTreeForSelectOutput,
    //                                                                                           TGetNodesForSelectInput,
    //                                                                                           TGetNodesForSelectOutput>, IGeneralTreeAppServiceBase<TDto,
    //                                                                                                                                                 TEditDto,
    //                                                                                                                                                 EntityDto<long>,
    //                                                                                                                                                 TEditDto,
    //                                                                                                                                                 TGetAllInput,
    //                                                                                                                                                 EntityDto<long>,
    //                                                                                                                                                 TMoveInput>
    //{}
}
