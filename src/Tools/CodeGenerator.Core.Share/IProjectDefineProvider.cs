using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Share
{
    /// <summary>
    /// 项目定义提供器
    /// </summary>
    public interface IProjectDefineProvider
    {
        /// <summary>
        /// 获取项目定义列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        IEnumerable<ProjectDefine> GetList(string keywords);    
        /// <summary>
        /// 获取单个项目定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProjectDefine GetSingle(Guid id);
    }
}
