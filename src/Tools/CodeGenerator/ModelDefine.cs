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

        #region 基本信息
        /// <summary>
        /// 名称，如：Test
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名，如：测试
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 是否是树形结构的数据
        /// </summary>
        public bool IsTree { get; set; }
        /// <summary>
        /// 父级权限名称
        /// </summary>
        public string? ParentPermissionName { get; set; }
        /// <summary>
        /// 父级菜单名称
        /// </summary>
        public string? ParentMenuName { get; set; }
        #endregion

        #region 字段
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<FieldDefine> Fields { get; set; }
        /// <summary>
        /// 主键字段
        /// </summary>
        public FieldDefine PrimaryField => Fields.First(x => x.IsPrimary);
        /// <summary>
        /// 普通非主键字段
        /// </summary>
        public IEnumerable<FieldDefine> NormalFields => Fields.Where(x => !x.IsPrimary);
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
        #endregion

        #region CoreShare
        /// <summary>
        /// 模型Core.Share层的命名空间，如：ZLJ.Core.Share.Test
        /// </summary>
        public string CoreShareNamespace => $"{ExecuteContext.CoreShareProjectName}.{Name}";
        /// <summary>
        /// Core.Share中的常量名，如：TestShareConsts
        /// </summary>
        public string CoreShareConstName => $"{Name}ShareConsts";
        #endregion
        #region Core
        /// <summary>
        /// 实体类名称，如：TestEntity
        /// </summary>
        public string EntityName => $"{Name}Entity";
        /// <summary>
        /// 模型Core层的命名空间，如：ZLJ.Core.Test
        /// </summary>
        public string CoreNamespace => $"{ExecuteContext.CoreProjectName}.{Name}";
        #endregion

        #region ApplicationCommonShare
        /// <summary>
        /// Application.Common.Shar层的命名空间，如：ZLJ.Application.Common.Share.Test
        /// </summary>
        public string ApplicationCommonShareNamespace => $"{ExecuteContext.ApplicationCommonShareProjectName}.{Name}";
        /// <summary>
        /// 获取供选择数据列表时的条件类名称，如：TestProviderCondition
        /// </summary>
        public string ProviderCondition => $"{Name}ProviderCondition";
        /// <summary>
        /// ProviderDto名称，如：TestProviderDto
        /// </summary>
        public string ProviderDtoName => $"{Name}ProviderDto";
        /// <summary>
        /// 获取供选择数据列表的类名称，如：TestProviderAppService
        /// </summary>
        public string ProviderAppService => $"{Name}ProviderAppService";
        #endregion
        #region ApplicationCommon
        /// <summary>
        /// Application.Common层的命名空间，如：ZLJ.Application.Common.Test
        /// </summary>
        public string ApplicationCommonNamespace => $"{ExecuteContext.ApplicationCommonProjectName}.{Name}";
        #endregion

        #region ApplicationShare
        /// <summary>
        /// Application.Shar层的命名空间，如：ZLJ.Application.Share.Test
        /// </summary>
        public string ApplicationShareNamespace => $"{ExecuteContext.ApplicationShareProjectName}.{Name}";
        /// <summary>
        /// ApplicationShare常量类名，如：TestApplicationShareConsts
        /// </summary>
        public string ApplicationShareConstName => $"{Name}ApplicationShareConsts";
        /// <summary>
        /// 各应用中的Dto名称，如：TestDto
        /// </summary>
        public string DtoName => $"{Name}Dto";
        /// <summary>
        /// 各应用中的EditDto名称，如：TestEditDto
        /// </summary>
        public string EditDtoName => $"{Name}EditDto";
        /// <summary>
        /// 具体应用中crud管理时获取数据的条件类名，如：TestCondition
        /// 此条件对象可以用于获取或统计数据
        /// </summary>
        public string ConditionName => $"{Name}Condition";
        #region 权限
        /// <summary>
        /// 查看和管理权限名称，如：TestGet
        /// </summary>
        public string PermissionNameGet => $"{Name}Get";
        /// <summary>
        /// 新增权限名称，如：TestCreate
        /// </summary>
        public string PermissionNameCreate => $"{Name}Create";
        /// <summary>
        /// 修改权限名称，如：TestUpdate
        /// </summary>
        public string PermissionNameUpdate => $"{Name}Update";
        /// <summary>
        /// 删除权限名称，如：TestDelete
        /// </summary>
        public string PermissionNameDelete => $"{Name}Delete";
        /// <summary>
        /// 查看和管理的常量全名，如：TestApplicationShareConsts.TestGet
        /// </summary>
        public string PermissionNameGetConst => $"{ApplicationShareConstName}.{nameof(PermissionNameGet)}";
        /// <summary>
        /// 新增常量全名，如：TestApplicationShareConsts.TestCreate
        /// </summary>
        public string PermissionNameCreateConst => $"{ApplicationShareConstName}.{nameof(PermissionNameCreate)}";
        /// <summary>
        /// 修改常量全名，如：TestApplicationShareConsts.TestUpdate
        /// </summary>
        public string PermissionNameUpdateConst => $"{ApplicationShareConstName}.{nameof(PermissionNameUpdate)}";
        /// <summary>
        /// 删除常量全名，如：TestApplicationShareConsts.TestDelete
        /// </summary>
        public string PermissionNameDeleteConst => $"{ApplicationShareConstName}.{nameof(PermissionNameDelete)}";
        #endregion
        #endregion
        #region Application
        /// <summary>
        /// Application层的命名空间，如：ZLJ.Application.Test
        /// </summary>
        public string ApplicationNamespace => $"{ExecuteContext.ApplicationProjectName}.{Name}";
        #endregion
    }
}