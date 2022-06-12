using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace System
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 反射获取指定属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetValue(this object obj, string propertyName)
        {
            var t = obj.GetType();
            var p = t.GetProperty(propertyName);
            return p.GetValue(obj, null);
        }
        /// <summary>
        /// 反射获取对象属性值并转换为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValue<T>(this object obj, string propertyName)
        {
            var value = obj.GetValue(propertyName);
            return (T)Convert.ChangeType(value, typeof(T));
        }
        /// <summary>
        /// 反射获取对象属性并转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetValueString(this object obj, string propertyName)
        {
            return obj.GetValue<string>(propertyName);
        }
        /// <summary>
        /// 尝试做减法运算
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj2"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool TrySubtract(this object obj, object obj2, out object val)
        {
            try
            {
                var p = Convert.ToDecimal(obj);
                var p1 = Convert.ToDecimal(obj2);
                val = p - p1;
                return true;
            }
            catch
            {
                val = default;
                return false;
            }
        }
        /// <summary>
        /// 尝试做减法运算
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj2"></param>
        /// <param name="val">失败时的默认值</param>
        /// <returns></returns>
        public static object TrySubtract(this object obj, object obj2, object val = default)
        {
            if (obj.TrySubtract(obj2, out var p))
                return p;

            return val;
        }
    }
}