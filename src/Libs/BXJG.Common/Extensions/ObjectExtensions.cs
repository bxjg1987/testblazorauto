using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace System
{
    public static class ObjectExtensions
    {
        public static void SetValue(this object obj, string propertyName, object value, BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)
        {
            var t = obj.GetType();
            var prop = t.GetProperty(propertyName, flag);

            //if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //{
            //  var sdf = new NullableConverter.StandardValuesCollection(prop.PropertyType.GetGenericArguments())
            //    dynamic objValue = System.Activator.CreateInstance(prop.PropertyType);
            //    objValue = value;
            //    prop.SetValue(obj, (object)objValue, null);
            //}
            //else
            //{
            //    prop.SetValue(obj, value, null);
            //}
            object convertedValue = value;
            if (value != null && value.GetType() != prop.PropertyType)
            {
                //PropertyDescriptor ss;
                // ss.Converter.string
                Type propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                var converter = TypeDescriptor.GetConverter(propertyType);
                convertedValue = converter.ConvertFromInvariantString(value.ToString());
                //  converter.ConvertFromString()
                // convertedValue =  Convert.ChangeType(value, propertyType);
            }
            prop.SetValue(obj, convertedValue, null);
            //  prop.SetValue(obj, Convert.ChangeType( value,prop.PropertyType) ,null);
        }

        /// <summary>
        /// 反射获取指定属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string propertyName, BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)
        {
            var t = obj.GetType();
            var p = t.GetProperty(propertyName, flag);
            return p.GetValue(obj, null);
        }
        /// <summary>
        /// 反射获取对象属性值并转换为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValue<T>(this object obj, string propertyName, BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)
        {
            var value = obj.GetPropertyValue(propertyName, flag);
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

        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var t = obj.GetType();
            var ps = t.GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            var dic = new Dictionary<string, object>();
            foreach (var p in ps)
            {
                dic.Add(p.Name, p.GetValue(obj, null));
            }
            return dic;
        }
    }
}