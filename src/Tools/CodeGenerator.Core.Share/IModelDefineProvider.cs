using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Share
{
    /// <summary>
    /// 模型定义提供器
    /// </summary>
    public interface IModelDefineProvider
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="modelId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        IEnumerable<ModelDefine> GetList(Guid projectId, Guid? modelId = default, string keywords = default);
        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ModelDefine GetSingle(Guid id);
    }
}
