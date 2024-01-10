using BXJG.Utils.Application.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share
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
                                                        TGetNodesForSelectOutput> : BXJG.Utils.Application.Share.GeneralTree.IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput,
                                                                                                                                                                TGetTreeForSelectOutput,
                                                                                                                                                                TGetNodesForSelectInput,
                                                                                                                                                                TGetNodesForSelectOutput>
    {
        ///// <summary>
        ///// 获取简洁的树形数据，通常引用此数据的页面调用
        ///// </summary>
        ///// <param name="input">指定父节点，是否显示全部</param>
        ///// <returns></returns>
        //Task<IList<TGetTreeForSelectOutput>> GetTreeForSelectAsync(TGetTreeForSelectInput input);
        ///// <summary>
        ///// 获取指定父节点的子节点，以扁平结构返回
        ///// </summary>
        ///// <returns></returns>
        //Task<IList<TGetNodesForSelectOutput>> GetNodesForSelectAsync(TGetNodesForSelectInput input);
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
}
