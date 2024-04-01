using System;
using System.ComponentModel;
using System.Reflection;
using BXJG.Utils.Share.Attributes;

namespace BXJG.Utils.Extensions
{

    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtension
    {
       

        /// <summary>
        /// 获取枚举的颜色描述信息
        /// </summary>
        public static string GetColor(this Enum em)
        {
            Type type = em.GetType();
            FieldInfo fd = type.GetField(em.ToString());
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(ColorAttribute), false);
            string name = string.Empty;
            foreach (ColorAttribute attr in attrs)
            {
                name = attr.ColorHexStr;
            }
            return name;
        }
    }
}
