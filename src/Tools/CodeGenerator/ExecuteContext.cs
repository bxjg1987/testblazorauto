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
        #region 基本信息
        /// <summary>
        /// 项目名称，如：ZLJ
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目显示名称，如：基础框架
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 源码绝对路径，如：D:\xxxx\src
        /// </summary>
        public string SrcDir { get; set; }
        /// <summary>
        /// ioc容器
        /// </summary>
        public IServiceProvider Services { get; set; }
        #endregion
        #region 模型
        //public bool IsDefault { get; set; }
        /// <summary>
        /// 所有模型
        /// </summary>
        public List<ModelDefine> Models { get; set; }
        /// <summary>
        /// 当前模型
        /// </summary>
        public ModelDefine Model { get; set; }
        #endregion

        #region core
        /// <summary>
        /// Core.Share项目名称，如：ZLJ.Core.Share
        /// </summary>
        public string CoreShareProjectName => $"{Name}.Core.Share";
        /// <summary>
        /// Core项目名称，如：ZLJ.Core
        /// </summary>
        public string CoreProjectName => $"{Name}.Core";
        /// <summary>
        /// CoreShare常量类名，如：ZLJConsts
        /// </summary>
        public string CoreShareConstName => $"{Name}CoreShareConsts";
        #endregion
        //public List<TemplateDefine> Templates { get; set; }

        #region ef
        /// <summary>
        /// EF项目名称，如：ZLJ.EntityFrameworkCore
        /// </summary>
        public string EFCoreProjectName => $"{Name}.EntityFrameworkCore";
        #endregion

        #region application common
        /// <summary>
        /// ApplicationCommonShare项目名称，如：ZLJ.Application.Common.Share
        /// </summary>
        public string ApplicationCommonShareProjectName => $"{Name}.Application.Common.Share";
        /// <summary>
        /// ApplicationCommon项目名称，如：ZLJ.Application.Common
        /// </summary>
        public string ApplicationCommonProjectName => $"{Name}.Application.Common";
        #endregion

        /// <summary>
        /// 代码生成器占位符
        /// </summary>
        public const string CodeGeneratorReplace = "//--codegenerator==";

        #region application
        /// <summary>
        /// ApplicationShare项目名称，如：ZLJ.Application.Share
        /// </summary>
        public string ApplicationShareProjectName => $"{Name}.Application.Share";
        /// <summary>
        /// Application项目名称，如：ZLJ.Application
        /// </summary>
        public string ApplicationProjectName => $"{Name}.Application";
        #endregion
    }
}