using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CodeGenerator
{
    public enum MultiTenantMode { 
        Must=1<<0,Have=1<<1,No=1<<2
    }
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
        /// 多租户模式
        /// </summary>
        public MultiTenantMode MultiTenantMode { get; set; }

        #region 基本信息
        /// <summary>
        /// 名称，如：Test
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
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
        /// 主键的c#类型
        /// </summary>
        public string PrimaryFieldCSharpType => PrimaryField.CSharpType;
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
        public IEnumerable<FieldDefine> ConditionNormalFields => NormalFields.Where(x => x.IsCondition&& !x.IsConditionRange&& x.CSharpType!="string");
        ///// <summary>
        ///// 在列表中显示的，冻结在左侧的字段列表
        ///// </summary>
        //public List<FieldDefine> FieldsFixedLeft => NormalFields.Where(x => x.FixedLeft == true).ToList();
        ///// <summary>
        ///// 在列表中显示的，冻结在右侧的字段列表
        ///// </summary>
        //public List<FieldDefine> FieldsFixedRight => NormalFields.Where(x => x.FixedLeft == false).ToList();
        ///// <summary>
        ///// 在列表中显示的，不冻结的字段列表
        ///// </summary>
        //public List<FieldDefine> FieldsFixedNo => NormalFields.Where(x => x.FixedLeft == default).ToList();
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

        #region ef
        public string EFNamespace => $"{ExecuteContext.EFCoreProjectName}.{Name}";//@(Model.EFCoreProjectName).@(Model.Model.Name)
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
        public string ApplicationShareNamespace => $"{ExecuteContext.App.ApplicationShareProjectName}.{Name}";
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
        /// 各应用中的CreateDto名称，如：TestCreateDto
        /// </summary>
        public string CreateDtoName => $"{Name}CreateDto";
        /// <summary>
        /// 具体应用中crud管理时获取数据的条件类名，如：TestCondition
        /// 此条件对象可以用于获取或统计数据
        /// </summary>
        public string ConditionName => $"{Name}Condition";
        #region 权限
        /// <summary>
        /// 管理权限名称，如：Test
        /// </summary>
        public string PermissionName => $"{ExecuteContext.App.Name}.{Name}";
        /// <summary>
        /// 查看权限名称，如：TestGet
        /// </summary>
        public string PermissionNameGet => $"{ExecuteContext.App.Name}.{Name}.Get";
        /// <summary>
        /// 新增权限名称，如：TestCreate
        /// </summary>
        public string PermissionNameCreate => $"{ExecuteContext.App.Name}.{Name}.Create";
        /// <summary>
        /// 修改权限名称，如：TestUpdate
        /// </summary>
        public string PermissionNameUpdate => $"{ExecuteContext.App.Name}.{Name}.Update";
        /// <summary>
        /// 删除权限名称，如：TestDelete
        /// </summary>
        public string PermissionNameDelete => $"{ExecuteContext.App.Name}.{Name}.Delete";
        /// <summary>
        /// cud的父权限常量名，如：xxxPermissionNames.xxx
        /// </summary>
        public string PermissionNameConst => $"{ApplicationShareConstName}.{nameof(PermissionName)}";
        /// <summary>
        /// 查看权限的常量名称，如：xxxPermissionNames.xxxGet
        /// </summary>
        public string PermissionNameGetConst => $"{ApplicationShareConstName}.{nameof(PermissionNameGet)}";
        /// <summary>
        /// 新增权限的常量名
        /// </summary>
        public string PermissionNameCreateConst => $"{ApplicationShareConstName}.{nameof(PermissionNameCreate)}";
        /// <summary>
        /// 修改权限的常量名
        /// </summary>
        public string PermissionNameUpdateConst => $"{ApplicationShareConstName}.{nameof(PermissionNameUpdate)}";
        /// <summary>
        /// 删除权限的常量名
        /// </summary>
        public string PermissionNameDeleteConst => $"{ApplicationShareConstName}.{nameof(PermissionNameDelete)}";
        #endregion
        #endregion
        #region Application
        /// <summary>
        /// Application层的命名空间，如：ZLJ.Application.Test
        /// </summary>
        public string ApplicationNamespace => $"{ExecuteContext.App.ApplicationProjectName}.{Name}";
        /// <summary>
        /// 某应用的应用服务
        /// </summary>
        public string ApplicationServiceName => $"{Name}AppService";
        #endregion

        #region UI
        /// <summary>
        /// 某应用，某模块，blazor客户端中的顶级命名空间，如：ZLJ.Admin.CoreRCL.Test
        /// </summary>
        public string BlazorClientNamespace => $"{ExecuteContext.App.BlazorClientProjectName}.{Name}";
        #endregion
    }
}