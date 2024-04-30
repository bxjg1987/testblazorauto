using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    /// <summary>
    /// 包含解决方案、所有模型和当前模型的数据
    /// </summary>
    public class ExecuteContext
    {
        /// <summary>
        /// 项目名称，如：ZLJ
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目显示名称，如：基础框架
        /// </summary>
        public string DisplayName { get; set; }
        //public bool IsDefault { get; set; }
        /// <summary>
        /// 所有模型
        /// </summary>
        public List<ModelDefine> Models { get; set; }
        /// <summary>
        /// 源码绝对路径，如：D:\xxxx\src
        /// </summary>
        public string SrcDir {  get; set; }
        /// <summary>
        /// ioc容器
        /// </summary>
        public IServiceProvider Services { get; set; }
        /// <summary>
        /// 当前模型
        /// </summary>
        public ModelDefine Model { get; set; }
        //public List<TemplateDefine> Templates { get; set; }
        /// <summary>
        /// Core.Share项目名称，如：ZLJ.Core.Share
        /// </summary>
        public string CoreShareProjectName => $"{Name}.Core.Share";
        /// <summary>
        /// Core项目名称，如：ZLJ.Core
        /// </summary>
        public string CoreProjectName => $"{Name}.Core";
        /// <summary>
        /// EF项目名称，如：ZLJ.EntityFrameworkCore
        /// </summary>
        public string EFCoreProjectName => $"{Name}.EntityFrameworkCore";
        /// <summary>
        /// CoreShare常量类名，如：ZLJConsts
        /// </summary>
        public string CoreShareConstName => $"{Name}Consts";
        /// <summary>
        /// 代码生成器占位符
        /// </summary>
        public const string CodeGeneratorReplace = "//--codegenerator==";
    }
}