using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    /// <summary>
    /// 模型
    /// </summary>
    public class ModelDefine
    {
        /// <summary>
        /// 上下文
        /// </summary>
        public ExecuteContext ExecuteContext { get; set; }
        /// <summary>
        /// 名称，如：Test
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名，如：测试
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<FieldDefine> Fields { get; set; }
        /// <summary>
        /// 实体类名称，如：TestEntity
        /// </summary>
        public string EntityName => $"{Name}Entity";
        /// <summary>
        /// 模型Core层的命名空间，如：ZLJ.Core.Test
        /// </summary>
        public string CoreNamespace => $"{ExecuteContext.CoreProjectName}.{Name}";
        /// <summary>
        /// 主键字段
        /// </summary>
        public FieldDefine PrimaryField => Fields.First(x => x.IsPrimary);
        /// <summary>
        /// 非主键字段
        /// </summary>
        public IEnumerable<FieldDefine> NormalFields => Fields.Where(x => !x.IsPrimary);
        ///// <summary>
        ///// 模型Core层的命名空间，如：ZLJ.Core.Share
        ///// </summary>
        //public string CoreShareNamespace => $"{ExecuteContext.CoreProjectName}.{Name}";
    }
}
