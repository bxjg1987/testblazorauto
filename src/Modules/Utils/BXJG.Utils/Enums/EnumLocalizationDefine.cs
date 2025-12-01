using BXJG.Utils.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Utils.Enums
{
    //此对象不算复杂，没必要提供相应Builder对象

    /// <summary>
    /// 枚举作为下拉框时的描述对象
    /// </summary>
    public class EnumLocalizationDefine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">枚举短名称，比如bool 对应是System.Bool 更容易的调用</param>
        /// <param name="type">枚举类型对象</param>
        /// <param name="locationSourceName">此枚举使用那个本地化源</param>
        /// <param name="permissions">哪些权限可以获取此枚举列表</param>
        public EnumLocalizationDefine(Type type, string name = default, string locationSourceName = BXJGUtilsConsts.LocalizationSourceName, IEnumerable<string> permissions = default)
        {
            Name = name ?? type.FullName;
            Type = type;
            LocationSourceName = locationSourceName;
            if (permissions != null)
                Permissions = permissions.ToList().AsReadOnly();
        }
        /// <summary>
        /// 枚举短名称
        /// 比如bool 对应是System.Bool 更容易的调用
        /// </summary>
        public string Name { get; init; }
        /// <summary>
        /// 枚举类型对象
        /// </summary>
        public Type Type { get; init; }
        /// <summary>
        /// 此枚举使用那个本地化源
        /// </summary>
        public string LocationSourceName { get; init; }
        /// <summary>
        /// 哪些权限可以获取此枚举列表
        /// </summary>
        public IReadOnlyList<string> Permissions { get; init; }
    }
    /// <summary>
    /// 枚举作为下拉框时的描述对象
    /// </summary>
    public class EnumConfigItem<T> : EnumLocalizationDefine
    {
        public EnumConfigItem(T type, string name = null, string locationSourceName = BXJGUtilsConsts.LocalizationSourceName, IEnumerable<string> permissions = null) : base(typeof(T), name, locationSourceName, permissions)
        {
        }

        public new T Type { get; init; }
    }
}
