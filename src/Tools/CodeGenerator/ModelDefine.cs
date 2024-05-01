using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        /// 普通非主键字段
        /// </summary>
        public IEnumerable<FieldDefine> NormalFields => Fields.Where(x => !x.IsPrimary);
        /// <summary>
        /// 模型Core.Share层的命名空间，如：ZLJ.Core.Share.Test
        /// </summary>
        public string CoreShareNamespace => $"{ExecuteContext.CoreShareProjectName}.{Name}";
        /// <summary>
        /// Core.Share中的常量名，如：TestShareConsts
        /// </summary>
        public string CoreShareConst => $"{Name}ShareConsts";
        /// <summary>
        /// 模型Application.Common.Shar层的命名空间，如：ZLJ.Application.Common.Share.Test
        /// </summary>
        public string ApplicationCommonShareNamespace => $"{ExecuteContext.ApplicationCommonShareProjectName}.{Name}";
        /// <summary>
        /// ProviderDto名称，如：TestProviderDto
        /// </summary>
        public string ProviderDto => $"{Name}ProviderDto";
        /// <summary>
        /// 获取供选择数据列表时的条件类名称，如：TestProviderCondition
        /// </summary>
        public string ProviderCondition => $"{Name}ProviderCondition";
        /// <summary>
        /// 字符串类型的字段
        /// </summary>
        public IEnumerable<FieldDefine> StringFields => Fields.Where(x => x.CSharpType=="string");
        /// <summary>
        /// 条件字段
        /// </summary>
        public IEnumerable<FieldDefine> ConditionFields => Fields.Where(x => x.IsCondition);

    }
}
