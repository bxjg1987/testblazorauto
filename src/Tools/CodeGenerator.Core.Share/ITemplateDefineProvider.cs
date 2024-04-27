using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Share
{
    /// <summary>
    /// 模板定义提供器
    /// </summary>
    public interface ITemplateDefineProvider
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="modelId"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        IEnumerable<TemplateDefine> GetList(Guid projectId, Guid? modelId=default, string keywords=default);
        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TemplateDefine GetSingle(Guid id);
    }
}
