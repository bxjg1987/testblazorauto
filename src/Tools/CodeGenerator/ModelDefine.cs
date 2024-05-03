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
        /// 是否是树形结构的数据
        /// </summary>
        public bool IsTree { get; set; }
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
        public string CoreShareConstName => $"{Name}ShareConsts";


        /// <summary>
        /// Application.Common.Shar层的命名空间，如：ZLJ.Application.Common.Share.Test
        /// </summary>
        public string ApplicationCommonShareNamespace => $"{ExecuteContext.ApplicationCommonShareProjectName}.{Name}";
        /// <summary>
        /// Application.Common层的命名空间，如：ZLJ.Application.Common.Test
        /// </summary>
        public string ApplicationCommonNamespace => $"{ExecuteContext.ApplicationCommonProjectName}.{Name}";


        /// <summary>
        /// Application.Shar层的命名空间，如：ZLJ.Application.Share.Test
        /// </summary>
        public string ApplicationShareNamespace => $"{ExecuteContext.ApplicationShareProjectName}.{Name}";
        /// <summary>
        /// Application层的命名空间，如：ZLJ.Application.Test
        /// </summary>
        public string ApplicationNamespace => $"{ExecuteContext.ApplicationProjectName}.{Name}";


        /// <summary>
        /// ProviderDto名称，如：TestProviderDto
        /// </summary>
        public string ProviderDtoName => $"{Name}ProviderDto";
        /// <summary>
        /// 各应用中的Dto名称，如：TestDto
        /// </summary>
        public string DtoName => $"{Name}Dto";
        /// <summary>
        /// 获取供选择数据列表时的条件类名称，如：TestProviderCondition
        /// </summary>
        public string ProviderCondition => $"{Name}ProviderCondition";
        /// <summary>
        /// 获取供选择数据列表的类名称，如：TestProviderAppService
        /// </summary>
        public string ProviderAppService => $"{Name}ProviderAppService";
        ///// <summary>
        ///// 对象映射类和文件名
        ///// 各应用可以通用，因为命名空间和目录不同，且用户代码中几乎不需要访问它
        ///// </summary>
        //public static readonly string ObjMapName = "AutoMapperProfile";// $"{Name}ObjMapProfile";

        /// <summary>
        /// 字符串类型的字段
        /// </summary>
        public IEnumerable<FieldDefine> StringFields => Fields.Where(x => x.CSharpType == "string");
        /// <summary>
        /// 字符串类型的条件字段
        /// </summary>
        public IEnumerable<FieldDefine> ConditionStringFields => StringFields.Where(x => x.IsCondition);
        /// <summary>
        /// 条件字段
        /// </summary>
        public IEnumerable<FieldDefine> ConditionFields => Fields.Where(x => x.IsCondition);
        /// <summary>
        /// 范围条件字段
        /// </summary>
        public IEnumerable<FieldDefine> ConditionRangeFields => ConditionFields.Where(x => x.IsConditionRange);
        /// <summary>
        /// 除了字符串、范围外的其它条件字段
        /// </summary>
        public IEnumerable<FieldDefine> NormalConditionFields => NormalFields.Where(x => x.IsCondition&& !x.IsConditionRange&& x.CSharpType!="string");

        #region 权限
        /// <summary>
        /// 权限名的常量类名，如：TestPermissionNames
        /// </summary>
        public string PermissionNames => $"{Name}PermissionNames";
        /// <summary>
        /// 查看和管理权限名称，如：Test
        /// </summary>
        public string PermissionName => $"{Name}";
        /// <summary>
        /// 新增权限名称，如：TestCreate
        /// </summary>
        public string CreatePermissionName => $"{Name}Create";
        /// <summary>
        /// 修改权限名称，如：TestUpdate
        /// </summary>
        public string UpdatePermissionName => $"{Name}Update";
        /// <summary>
        /// 删除权限名称，如：TestDelete
        /// </summary>
        public string DeletePermissionName => $"{Name}Delete";
        /// <summary>
        /// 查看和管理的常量全名，如：TestPermissionNames.Test
        /// </summary>
        public string PermissionNameConst => $"{PermissionNames}.{PermissionName}";
        /// <summary>
        /// 新增常量全名，如：TestPermissionNames.TestCreate
        /// </summary>
        public string CreatePermissionNameConst => $"{PermissionNames}.{CreatePermissionName}";
        /// <summary>
        /// 修改常量全名，如：TestPermissionNames.TestUpdate
        /// </summary>
        public string UpdatePermissionNameConst => $"{PermissionNames}.{UpdatePermissionName}";
        /// <summary>
        /// 删除常量全名，如：TestPermissionNames.TestDelete
        /// </summary>
        public string DeletePermissionNameConst => $"{PermissionNames}.{DeletePermissionName}";
        #endregion
    }
}